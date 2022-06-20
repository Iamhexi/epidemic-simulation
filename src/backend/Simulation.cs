using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Random;

namespace EpidemicSimulation
{

    /**
    Simulation class is an overlaying extendion of base class game provided by Xna framework. It implements funcionality of graphic reprezentation,
    delegates to window instace via GraphicsDeviceManager, loades textures and draws all of the content on windows screen. Contains behavior logic of 
    People class instances and child classes, time management and input detection. Keeps track of all the statics during simulation process.
    */
    internal class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infectious;
        public Texture2D InfectiousRadius;
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
        private ChartManager _chartManager;

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
        private List<int> _speedValues = new List<int>() {32,16,8,4,2,1};
        private int _currentSpeedIndex = 2;
        protected SimulationSpeedValues SimulationSpeed;
        protected uint _susceptibleAmount;
        protected uint _infeciousAmount;
        protected List<Person> _people = new List<Person>();
        /**
        Elementary constructor creates default parameters used to further initialization. Assigns frame to simulation if provided and defines fields rectagle
        as area of simulating contents.
        @param susceptible as qyantity of susceptible people to create.
        @param infecious as quantity of infected people to create.
        @param frame is the rectagle object reprezenting assumed similation area.
        */
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

            _susceptibleAmount = susceptible;
            _infeciousAmount = infecious;
            Person.s_MovementSpeed = 2;
            SimulationSpeed = SimulationSpeedValues.x2;

            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible(SimulationRect));}
            for (int i = 0; i<_infeciousAmount; i++) { this._people.Add(new Infectious(SimulationRect)); }
        }

        /**
        Overridden Game class method initializing all underlying non graphic Game mechanics such as time interval between updating surface.
        */

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds((double)this.SimulationSpeed);
            base.Initialize();
        }
        /**
        Overridden Game class method responsible for loading all textures and assigning them to specified class fields. Is called once during initialization.
        */
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Susceptible = Content.Load<Texture2D>("suscetible");
            SusceptibleRadius = Content.Load<Texture2D>("suscetible-radius");
            Infectious = Content.Load<Texture2D>("infected");
            InfectiousRadius = Content.Load<Texture2D>("infected-radius");
            Recovered = Content.Load<Texture2D>("recovered");
            Dead = Content.Load<Texture2D>("dead");
            Wall = new Texture2D(GraphicsDevice, 1, 1); Wall.SetData(new Color[] {Color.Cyan});
            _chartManager = new ChartManager(
                new Vector2(10f, 250f),
                new Point(250, 250),
                this,
                GraphicsDevice
                );
            _chartManager.LoadContent();
        }
        /**
        Overridden Game class method contains all logic of input, contagion process logic, controls all Person UpdateSelf() methods accorigly to 
        ongoing situation. 
        */
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) 
                || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) ) Exit();

            if (Keyboard.GetState().GetPressedKeyCount() == 0) _ignoreInput = false;

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && !_ignoreInput) { 
                _ignoreInput = true;
                this._currentSpeedIndex += 1; 
                this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(this._speedValues[System.Math.Abs(this._currentSpeedIndex)%6]);
                System.Console.WriteLine($"current speed: {(int)(1.0f/(((float)this._speedValues[System.Math.Abs(this._currentSpeedIndex)%6])/1000f))} FPS "); }
            else if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && !_ignoreInput) { 
                _ignoreInput = true;
                this._currentSpeedIndex -= 1; 
                this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(this._speedValues[System.Math.Abs(this._currentSpeedIndex)%6]); 
                System.Console.WriteLine($"current speed: {(int)(1.0f/(((float)this._speedValues[System.Math.Abs(this._currentSpeedIndex)%6])/1000f))} FPS "); }
            else if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && !_ignoreInput) {Pause(); _ignoreInput = true; };

            if (!this._Pause)
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
                                        if ((person.Type() == "Susceptible" || person.Type() == "Recovered") && overlappingArea * Disease.Communicability - person.ImmunityRate > temp_random) 
                                        { this.SusceptibleToInfectious(person); return; }
                                        else if ((secondPerson.Type() == "Susceptible" || person.Type() == "Recovered") && overlappingArea * Disease.Communicability - secondPerson.ImmunityRate > temp_random) 
                                        { this.SusceptibleToInfectious(secondPerson); return; }
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
                if (GenerateOutputLists()["Infectious"] == 0)  {  System.Console.WriteLine("all done!"); Pause(); }
            }
            base.Update(gameTime);  
        }
        /**
        Overridden Game class method handles all drawing of game state. Using loaded textures and determined Rectangles of each in-game insntace draws 
        adequate texture in specified place.
        */
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
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X, SimulationRect.Location.Y, s_SimulationWidth, 2), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X, SimulationRect.Location.Y, 1, s_SimulationHeight), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X, SimulationRect.Location.Y+SimulationRect.Height-2, s_SimulationWidth, 2), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X+SimulationRect.Width-1, SimulationRect.Location.Y , 1, SimulationRect.Height), Color.White);
            _chartManager.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        /**
        Method forcing Person instance to move towards Point with specified value and probability.
        @param person is a pointer to Person object.
        */
        private void ActivateCenterPoint(Person person)
        {
            if (CenterPoint.HasValue) { person.GoToPoint(CenterPoint, VisitingProbability); }
        }
        /**
        Method that handles transmission from Susceptible class instance to Infectious class instance, transcripts essential parameters.
        @param person is a pointer to Person object.
        */
        private void SusceptibleToInfectious(Person susceptible)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == susceptible.GetHashCode())
                {
                this._people[i] = new Infectious(SimulationRect, susceptible.Position, susceptible.MovementVector, susceptible.ImmunityRate, 35);
                return;
                }
            }
        }
        /**
        Method that handles transmission from Infectious class instance to Recovered class instance, transcripts essential parameters.
        @param person is a pointer to Person object.
        */
        private void InfectiousToRecovered(Person infecious)
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
        /**
        Method that handles transmission from Infectious class instance to Dead class instance, transcripts essential parameters.
        @param person is a pointer to Person object.
        */
        private void InfectiousToDead(Person infecious)
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
        /**
        Method that returns Dictionary Generic Collection of current in-game statictics.
        */
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
        /**
        Visual output method logging infection of object A on object B.
        */
        private void logInfection(Person person, Person secondPerson)
        {
            System.Console.WriteLine($"\n\nInfected!\n\nPerson1 : {person.Type()}\tHashCode : {person.GetHashCode()}\n\nPerson2 : {secondPerson.Type()}\t{secondPerson.GetHashCode()}");
        }
        /**
        Method controlling Pause functionality of simulation.
        */
        public void Pause() { this._Pause = !this._Pause; }
    }
}
