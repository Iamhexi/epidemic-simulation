using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Random;

namespace EpidemicSimulation
{
    public class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infectious;
        public Texture2D InfectiousRadius;
        public Texture2D Recovered;
        public Texture2D Dead;

        //game class setup
        public Point? CenterPoint;
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        private static System.Random s_randomizer = new System.Random();
        private bool _pause = false;
        private ChartManager _chartManager;

        // enviroment variables
        public static int s_SimulationWidth = 800;
        public static int s_SimulationHeight = 800;
        protected enum SimulationSpeedValues: ushort {
            half = 32,
            x1 = 16,
            x2 = 8,
            x4 = 4,
            x8 = 2
        };
        protected SimulationSpeedValues SimulationSpeed;
        protected uint _susceptibleAmount;
        protected uint _InfectiousAmount;
        private List<Person> _people = new List<Person>();

        public Simulation(uint susceptible = 10, uint Infectious = 5)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../content";
            IsMouseVisible = true;

            // the parameters set from menu
            _susceptibleAmount = susceptible;
            _InfectiousAmount = Infectious;
            Person.s_MovementSpeed = 2;
            SimulationSpeed = SimulationSpeedValues.x2;

            for (int i = 0; i<_susceptibleAmount; i++)
                this._people.Add(new Susceptible());
            for (int i = 0; i<_InfectiousAmount; i++)
                this._people.Add(new Infectious());
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_SimulationWidth;
            _graphics.PreferredBackBufferHeight = s_SimulationHeight;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds((double)this.SimulationSpeed);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Susceptible = Content.Load<Texture2D>("suscetible");
            SusceptibleRadius = Content.Load<Texture2D>("suscetible-radius");
            Infectious = Content.Load<Texture2D>("infected");
            InfectiousRadius = Content.Load<Texture2D>("infected-radius");
            Recovered = Content.Load<Texture2D>("recovered");
            Dead = Content.Load<Texture2D>("dead");

            _chartManager = new ChartManager(
                new Vector2(10f, 250f),
                new Point(200, 200),
                this,
                GraphicsDevice
            );
            _chartManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) Exit();
            if (!this._pause)
            {
                foreach(Person person in this._people)
                {
                    foreach(Person secondPerson in this._people)
                    {
                        if (person.Type() == "Infectious" ^ secondPerson.Type() == "Infectious" && person.Type() != "Dead" && secondPerson.Type() != "Dead") // xor gate
                        {
                            if (Person.s_CheckCollision(person.RadiusRect, secondPerson.RadiusRect))
                            {
                                float overlappingArea = Person.s_FieldIntersectionPrecentege(person.RadiusRect, secondPerson.RadiusRect);
                                double temp_random = s_randomizer.NextDouble();
                                if (overlappingArea > Disease.RequiredFieldIntersetion)
                                {
                                    if ( (person.Type() == "Susceptible" || person.Type() == "Recovered") &&
                                        overlappingArea * Disease.Communicability - 3*person.ImmunityRate > temp_random)
                                    {
                                        this.SusceptibleToInfectious(person);
                                        return;
                                    }

                                }
                            }
                        }
                        if(person.Type() != "Dead" && secondPerson.Type() != "Dead" && !person.IgnoreColision && !secondPerson.IgnoreColision)
                        if (Person.s_CheckCollision(person.AnticipadedPositon, secondPerson.AnticipadedPositon) || Person.s_CheckCollision(person.Rect, secondPerson.Rect) )
                        person.IsColliding = true;
                    }
                    if (person.Type() == "Infectious")
                    {
                        person.InfectionDuration += 1;
                        if (Disease.Lethality-person.ImmunityRate > s_randomizer.NextDouble()) { this.InfectiousToDead(person); return; }
                        if (person.InfectionDuration > Disease.Duration) { this.InfectiousToRecovered(person); return; }
                    }
                    ActivateCenterPoint(person);
                    person.UpdateSelf();
                }
                _chartManager.Update();
            }
            base.Update(gameTime);

            if (GenerateOutputLists()["Infectious"] == 0)
                Pause();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            _spriteBatch.Begin();
            foreach(Person person in this._people)
            {
                switch (person.Type())
                {
                    case "Susceptible":
                        _spriteBatch.Draw(SusceptibleRadius, person.RadiusRect, Microsoft.Xna.Framework.Color.White);
                        _spriteBatch.Draw(Susceptible, person.Rect, Microsoft.Xna.Framework.Color.White);
                        break;

                    case "Infectious":
                        _spriteBatch.Draw(InfectiousRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Infectious, person.Rect, Color.White);
                        break;
                    case "Recovered":
                        _spriteBatch.Draw(Recovered, person.Rect, Color.White);
                        break;
                    case "Dead":
                        _spriteBatch.Draw(Dead, person.Rect, Color.White);
                        break;
                    default: System.Console.WriteLine($" unknown type found, { person.Type() }"); break;
                }
            }

            _chartManager.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ActivateCenterPoint(Person person, float visitingProbability = 0.0008f)
        {
            if (CenterPoint.HasValue) { person.GoToPoint(CenterPoint, visitingProbability); }
        }

        private void SusceptibleToInfectious(Person susceptible)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].Equals(susceptible))
                {
                    this._people[i] = new Infectious(susceptible.Position, susceptible.MovementVector, susceptible.ImmunityRate, 35);
                    return;
                }
            }
        }

        private void InfectiousToRecovered(Person Infectious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == Infectious.GetHashCode())
                {
                    this._people[i] = new Recovered(Infectious.Position, Infectious.MovementVector, Infectious.ImmunityRate*20, 35);
                    return;
                }
            }
        }

        private void InfectiousToDead(Person Infectious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == Infectious.GetHashCode())
                {
                    this._people[i] = new Dead(Infectious.Position, new Vector2(0,0), 0, 0);
                    return;
                }
            }
        }

       public Dictionary<string, int> GenerateOutputLists()
        {
            Dictionary<string, int> result_dict = new Dictionary<string, int>();
            result_dict.Add("Susceptible", 0);
            result_dict.Add("Infectious", 0);
            result_dict.Add("Recovered", 0);
            result_dict.Add("Dead", 0);

            foreach (Person person in _people) {
                switch (person.Type())
                {
                    case "Susceptible": result_dict["Susceptible"] += 1; break;
                    case "Infectious": result_dict["Infectious"] += 1; break;
                    case "Recovered": result_dict["Recovered"] += 1; break;
                    case "Dead": result_dict["Dead"] += 1; break;
                    default: System.Console.WriteLine(" unknown type found "); break;
                }
            }

            return result_dict;
        }

        public void Pause() { this._pause = !this._pause; }
    }
}
