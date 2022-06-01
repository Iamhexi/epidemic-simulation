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
        public Texture2D Infecious;
        public Texture2D InfeciousRadius;
        public Texture2D Recovered;
        public Texture2D Dead;

        //game class setup
        public Point? CenterPoint;
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        private static System.Random s_randomizer = new System.Random();
        private bool _Pause = false;
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
        protected uint _infeciousAmount;
        private List<Person> _people = new List<Person>();

        public Simulation(uint susceptible = 10, uint infecious = 5)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../content";
            IsMouseVisible = true;

            // the parameters set from menu
            _susceptibleAmount = susceptible;
            _infeciousAmount = infecious;
            Person.s_MovementSpeed = 2;
            SimulationSpeed = SimulationSpeedValues.x2;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_SimulationWidth;
            _graphics.PreferredBackBufferHeight = s_SimulationHeight;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds((double)this.SimulationSpeed);

            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible());}
            for (int i = 0; i<_infeciousAmount; i++) { this._people.Add(new Infecious()); }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Susceptible = Content.Load<Texture2D>("suscetible");
            SusceptibleRadius = Content.Load<Texture2D>("suscetible-radius");
            Infecious = Content.Load<Texture2D>("infected");
            InfeciousRadius = Content.Load<Texture2D>("infected-radius");
            Recovered = Content.Load<Texture2D>("recovered");
            Dead = Content.Load<Texture2D>("dead");

            _chartManager = new ChartManager(
                new Vector2(10f, 400f),
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
            if (!this._Pause)
            {
                foreach(Person person in this._people)
                {
                    foreach(Person secondPerson in this._people)
                    {
                        if (person.Type() == "Infecious" ^ secondPerson.Type() == "Infecious" && person.Type() != "Dead" && secondPerson.Type() != "Dead") // xor gate
                        {
                            if (Person.s_CheckCollision(person.RadiusRect, secondPerson.RadiusRect))
                            {
                                float overlappingArea = Person.s_FieldIntersectionPrecentege(person.RadiusRect, secondPerson.RadiusRect);
                                double temp_random = s_randomizer.NextDouble();
                                if (overlappingArea > Disease.RequiredFieldIntersetion)
                                {
                                        if (person.Type() == "Susceptible" || person.Type() == "Recovered" && overlappingArea * Disease.Communicability - 3*person.ImmunityRate > temp_random) 
                                        { this.SusceptibleToInfecious(person); return; }
                                        else if (secondPerson.Type() == "Susceptible" || person.Type() == "Recovered" && overlappingArea * Disease.Communicability - 3*secondPerson.ImmunityRate > temp_random) 
                                        { this.SusceptibleToInfecious(secondPerson); return; }
                                }
                            }
                        }
                        if(person.Type() != "Dead" && secondPerson.Type() != "Dead" && !person.IgnoreColision && !secondPerson.IgnoreColision)
                        if (Person.s_CheckCollision(person.AnticipadedPositon, secondPerson.AnticipadedPositon) || Person.s_CheckCollision(person.Rect, secondPerson.Rect) )
                        person.IsColliding = true;
                    }
                    if (person.Type() == "Infecious")
                    {
                        person.InfectionDuration += 1;
                        if (Disease.Lethality-person.ImmunityRate > s_randomizer.NextDouble()) { this.InfeciousToDead(person); return; }
                        if (person.InfectionDuration > Disease.Duration) { this.InfeciousToRecovered(person); return; }
                    }
                    ActivateCenterPoint(person);
                    person.UpdateSelf();
                }
            }
            base.Update(gameTime);
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

                    case "Infecious":
                        _spriteBatch.Draw(InfeciousRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Infecious, person.Rect, Color.White);
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

        private void SusceptibleToInfecious(Person susceptible)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].Equals(susceptible))
                {
                    Console.WriteLine("Found susceptible! Replacing with infected...");
                    this._people[i] = new Infecious(susceptible.Position, susceptible.MovementVector, susceptible.ImmunityRate, 35);
                    return;
                }
            }
        }

        private void InfeciousToRecovered(Person infecious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == infecious.GetHashCode())
                {
                    this._people[i] = new Recovered(infecious.Position, infecious.MovementVector, infecious.ImmunityRate*20, 35);
                    return;
                }
            }
        }

        private void InfeciousToDead(Person infecious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == infecious.GetHashCode())
                {
                    this._people[i] = new Dead(infecious.Position, new Vector2(0,0), 0, 0);
                    return;
                }
            }
        }

       public Dictionary<string, int> GenerateOutputLists()
        {
            Dictionary<string, int> result_dict = new Dictionary<string, int>();
            result_dict.Add("Susceptible", 0);
            result_dict.Add("Infecious", 0);
            result_dict.Add("Recovered", 0);
            result_dict.Add("Dead", 0);
            foreach (Person person in _people) {
                switch (person.Type())
                {
                    case "Susceptible": result_dict["Susceptible"] += 1; break;
                    case "Infecious": result_dict["Infecious"] += 1; break;
                    case "Recovered": result_dict["Recovered"] += 1; break;
                    case "Dead": result_dict["Dead"] += 1; break;
                    default: System.Console.WriteLine(" unknown type found "); break;
                }
            }
            return result_dict;
        }

        public void Pause() { this._Pause = !this._Pause; }

        private void logInfection(Person person, Person secondPerson)
        {
            System.Console.WriteLine($"\n\nInfected!\n\nPerson1 : {person.Type()}\tHashCode : {person.GetHashCode()}\n\nPerson2 : {secondPerson.Type()}\t{secondPerson.GetHashCode()}");
        }
    }
}
