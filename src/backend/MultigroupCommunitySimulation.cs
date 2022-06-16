using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace EpidemicSimulation.src.backend
{
    class MultigroupCommunitySimulation : Simulation, ISimulation
    {
        private List<Point> CentralPoints = new List<Point>();
        private List<Rectangle> Obsticles = new List<Rectangle>();
        private int PointCooldown = 450;

        public MultigroupCommunitySimulation(uint population = 20, uint infected = 3) :base(population, infected) { 
            CentralPoints.Add(new Point(250,250));
            CentralPoints.Add(new Point(750,250));
            CentralPoints.Add(new Point(250,750));
            CentralPoints.Add(new Point(750,750));
            VisitingProbability = 0.0001f;

            Obsticles.Add(new Rectangle(SimulationRect.Location.X+SimulationRect.Width/2-4, 0, 8, SimulationRect.Height));
            Obsticles.Add(new Rectangle(0, SimulationRect.Location.Y+SimulationRect.Height/2-4 , SimulationRect.Width, 8));
            Person.Obsticles = Obsticles;
            foreach (Person person in this._people) 
            if (Obsticles[0].Contains(person.Rect.Location) || Obsticles[1].Contains(person.Rect.Location)) person.GoToPoint(this.CentralPoints[Simulation.s_randomizer.Next(0,3)]);
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