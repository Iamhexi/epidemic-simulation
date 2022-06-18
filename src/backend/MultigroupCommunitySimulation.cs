using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EpidemicSimulation
{
    /**
        This class constitues the multigroup community scenario, handling high-level
        events of simulation such as pausing, closing, starting a simulation,
        providing data and providing a simplified constructor.
    */
    class MultigroupCommunitySimulation : Simulation, ISimulation
    {
        private List<Point> CentralPoints = new List<Point>();
        private List<Rectangle> Obsticles = new List<Rectangle>();
        private int PointCooldown = 400;

        public MultigroupCommunitySimulation(uint population, uint infected) :base(population, infected) {
            CentralPoints.Add(new Point(250,250));
            CentralPoints.Add(new Point(750,250));
            CentralPoints.Add(new Point(250,750));
            CentralPoints.Add(new Point(750,750));
            VisitingProbability = 0.0003f;

            Obsticles.Add(new Rectangle(SimulationRect.Location.X+SimulationRect.Width/2-4, 0, 8, SimulationRect.Height));
            Obsticles.Add(new Rectangle(0, SimulationRect.Location.Y+SimulationRect.Height/2-4 , SimulationRect.Width, 8));
            Person.Obsticles = Obsticles;
            foreach (Person person in this._people)
            if (Obsticles[0].Contains(person.Rect.Location) || Obsticles[1].Contains(person.Rect.Location)) person.GoToPoint(new Point(s_randomizer.Next(100,900), s_randomizer.Next(100,900)));
        }

        public Dictionary<string, int> GetSimulationData()
        {
            return GenerateOutputLists();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PointCooldown < 0) {CenterPoint = this.CentralPoints[Simulation.s_randomizer.Next(0,3)]; PointCooldown = 300; }
            else PointCooldown -=1;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(Wall, Obsticles[0], Color.White);
            _spriteBatch.Draw(Wall, Obsticles[1], Color.White);
            _spriteBatch.End();
        }
        public void Start(){ Run(); }
        public void Close() { Exit(); }
     }
    }
