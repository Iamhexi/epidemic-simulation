using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Random;

namespace EpidemicSimulation
{
    internal class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infecious;
        public Texture2D InfeciousRadius;
        public Texture2D Recovered;
        public Texture2D Dead;
        public Texture2D Wall;

        //game class setup
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        protected static System.Random s_randomizer = new System.Random();
        protected float VisitingProbability = 0.008f;
        private bool _Pause = false;
        public Point? CenterPoint;

        // enviroment variables
        public Rectangle SimulationRect;
        public static int s_SimulationWidth = 1000;
        public static int s_SimulationHeight = 1000;
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
        protected List<Person> _people = new List<Person>();

        public Simulation(uint susceptible = 20, uint infecious = 2, Rectangle? frame = null)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../content";
            IsMouseVisible = true;

            if (frame.HasValue) { 
                s_SimulationWidth = frame.Value.Width;
                s_SimulationHeight = frame.Value.Height;
                SimulationRect = frame.Value;
                }
            else { SimulationRect = new Rectangle(0, 0, s_SimulationWidth, s_SimulationHeight); }
            
            // the parameters set from menu
            _susceptibleAmount = susceptible;
            _infeciousAmount = infecious;
            Person.s_MovementSpeed = 2;
            Disease.s_SetUpParams();
            SimulationSpeed = SimulationSpeedValues.x2;

            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible(SimulationRect));}
            for (int i = 0; i<_infeciousAmount; i++) { this._people.Add(new Infecious(SimulationRect)); }
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds((double)this.SimulationSpeed);
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
            Wall = new Texture2D(GraphicsDevice, 1, 1); Wall.SetData(new Color[] {Color.Cyan});
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
                                        if (person.Type() == "Susceptible" || person.Type() == "Recovered" && overlappingArea * Disease.Communicability - person.ImmunityRate > temp_random) 
                                        { this.SusceptibleToInfecious(person); return; }
                                        else if (secondPerson.Type() == "Susceptible" || person.Type() == "Recovered" && overlappingArea * Disease.Communicability - secondPerson.ImmunityRate > temp_random) 
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
                _chartManager.Update();
                base.Update(gameTime);      
            }
            if (GenerateOutputLists()["Infectious"] == 0) Pause();
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
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location, new Point(s_SimulationWidth, 2)), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location, new Point(1, s_SimulationHeight)), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X, SimulationRect.Location.Y+SimulationRect.Height-2, s_SimulationWidth, 2), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X+SimulationRect.Width-1, SimulationRect.Location.Y , 1, SimulationRect.Height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ActivateCenterPoint(Person person) 
        { 
            if (CenterPoint.HasValue) { person.GoToPoint(CenterPoint, VisitingProbability); }
        }
        private void SusceptibleToInfecious(Person susceptible)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == susceptible.GetHashCode())
                {
                this._people[i] = new Infecious(SimulationRect, susceptible.Position, susceptible.MovementVector, susceptible.ImmunityRate, 35);
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
                this._people[i] = new Recovered(SimulationRect, infecious.Position, infecious.MovementVector, infecious.ImmunityRate*100, 35);
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
                this._people[i] = new Dead(SimulationRect, infecious.Position, new Vector2(0,0), 0, 0);
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

        private void logInfection(Person person, Person secondPerson) 
        {
            System.Console.WriteLine($"\n\nInfected!\n\nPerson1 : {person.Type()}\tHashCode : {person.GetHashCode()}\n\nPerson2 : {secondPerson.Type()}\t{secondPerson.GetHashCode()}");
        }
        public void Pause() { this._Pause = !this._Pause; }
    }
}
