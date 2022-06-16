using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation
{
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
        protected float VisitingProbability = 0.003f;
        private bool _Pause = false;
        private bool _ignoreInput = false;
        public Point? CenterPoint;
        private ChartManager _chartManager;

        // enviroment variables
        public Rectangle SimulationRect;
        public static int s_SimulationWidth = 1000;
        public static int s_SimulationHeight = 1000;
        public enum SimulationSpeedValues: uint {
            half = 32,
            x1 = 16,
            x2 = 8,
            x4 = 4,
            x8 = 2,
            x16 = 1,
        };
        private List<int> _speedValues = new List<int>() {32,16,8,4,2,1};
        private int _currentSpeedIndex = 2;
        public SimulationSpeedValues SimulationSpeed;
        protected uint _susceptibleAmount;
        protected uint _infectiousAmount;
        protected List<Person> _people = new List<Person>();

        public Simulation(uint susceptible = 20, uint infectious = 2, Rectangle? frame = null)
        {
            System.Console.Clear();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            if (frame.HasValue) { 
                s_SimulationWidth = frame.Value.Width;
                s_SimulationHeight = frame.Value.Height;
                SimulationRect = frame.Value;
                }
            else { SimulationRect = new Rectangle(0, 0, s_SimulationWidth, s_SimulationHeight); }
            
            // the parameters set from menu
            _susceptibleAmount = susceptible;
            _infectiousAmount = infectious;
            Person.s_MovementSpeed = 2;
            SimulationSpeed = SimulationSpeedValues.x2; // default speed
            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible(SimulationRect));}
            for (int i = 0; i<_infectiousAmount; i++) { this._people.Add(new Infectious(SimulationRect)); }
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
            Infectious = Content.Load<Texture2D>("infected");
            InfectiousRadius = Content.Load<Texture2D>("infected-radius");
            Recovered = Content.Load<Texture2D>("recovered");
            Dead = Content.Load<Texture2D>("dead");
            Wall = new Texture2D(GraphicsDevice, 1, 1); Wall.SetData(new Color[] {Color.Cyan});
            _chartManager = new ChartManager(
                new Vector2(10f, 200f),
                new Point(250, 200),
                this,
                GraphicsDevice
                );
            _chartManager.LoadContent();
        }

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
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location, new Point(s_SimulationWidth, 2)), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location, new Point(1, s_SimulationHeight)), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X, SimulationRect.Location.Y+SimulationRect.Height-2, s_SimulationWidth, 2), Color.White);
            _spriteBatch.Draw(Wall, new Rectangle(SimulationRect.Location.X+SimulationRect.Width-1, SimulationRect.Location.Y , 1, SimulationRect.Height), Color.White);
            _chartManager.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ActivateCenterPoint(Person person) 
        { 
            if (CenterPoint.HasValue && person.Type() != "Dead") { person.GoToPoint(CenterPoint, VisitingProbability); }
        }
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

        private void InfectiousToRecovered(Person infectious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == infectious.GetHashCode())
                {
                    this._people[i] = new Recovered(SimulationRect, infectious.Position, infectious.MovementVector, infectious.ImmunityRate*100, 35);
                    return;
                }
            }
        }

        private void InfectiousToDead(Person infectious)
        {
            for (int i = 0; i < this._people.Count; i++)
            {
                if (this._people[i].GetHashCode() == infectious.GetHashCode())
                {
                    this._people[i] = new Dead(SimulationRect, infectious.Position, new Vector2(0,0), 0, 0);
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

        private void logInfection(Person person, Person secondPerson) 
        {
            System.Console.WriteLine($"\n\nInfected!\n\nPerson1 : {person.Type()}\tHashCode : {person.GetHashCode()}\n\nPerson2 : {secondPerson.Type()}\t{secondPerson.GetHashCode()}");
        }
        public void Pause() { this._Pause = !this._Pause; }
    }
}
