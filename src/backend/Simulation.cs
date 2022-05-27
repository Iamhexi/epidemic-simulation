using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Random;

namespace EpidemicSimulation.src.backend
{
    public class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infecious;
        public Texture2D InfeciousRadius;
        public Texture2D Removed;

        //game class setup
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        private static System.Random s_randomizer = new System.Random();

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
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // the parameters set from menu
            _susceptibleAmount = susceptible;
            _infeciousAmount = infecious; 
            Person.s_MovementSpeed = 2;
            Disease.s_SetUpParams();
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
            Removed = Content.Load<Texture2D>("Removed");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            foreach(Person person in this._people)
            {
                foreach(Person secondPerson in this._people) 
                {

                    if (person.Type() == "Infecious" ^ secondPerson.Type() == "Infecious") // xor gate
                    { 
                        if (Person.s_CheckCollision(person.RadiusRect, secondPerson.RadiusRect)) 
                        {
                            float overlappingArea = Person.s_FieldIntersectionPrecentege(person.RadiusRect, secondPerson.RadiusRect);
                            double temp_random = s_randomizer.NextDouble();
                            //System.Console.WriteLine(" overlapping area : " + overlappingArea);
                            if (overlappingArea > Disease.RequiredFieldIntersetion) 
                            {
                                if (overlappingArea * Disease.InfectionProbability > temp_random)
                                {
                                    System.Console.WriteLine($" Infected! : {overlappingArea * Disease.InfectionProbability} < {temp_random}" );
                                    if (person.Type() == "Susceptible") { this.SusceptibleToInfecious(person); return; }
                                    else { this.SusceptibleToInfecious(secondPerson); return; }
                                }
                            }
                            
                        }
                    }
                    
                    if (Person.s_CheckCollision(person.AnticipadedPositon, secondPerson.AnticipadedPositon) || Person.s_CheckCollision(person.Rect, secondPerson.Rect)) 
                    { 
                        person.IsColliding = true; 
                    }
                }
                if (person.Type() == "Infecious") 
                { 
                    person.InfectionDuration += 1; 
                    //System.Console.WriteLine($"duration {person.GetHashCode()} -> { person.InfectionDuration }");
                    if (person.InfectionDuration > Disease.Duration) { this.InfeciousToRecovered(person); return; }
                }
                person.UpdateSelf();
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
           foreach(Person person in this._people)
            {
                switch (person.GetType().ToString().Split(".").GetValue(3).ToString())
                {
                    case "Susceptible":
                        _spriteBatch.Draw(SusceptibleRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Susceptible, person.Rect, Color.White);
                        break;

                    case "Infecious":
                        _spriteBatch.Draw(InfeciousRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Infecious, person.Rect, Color.White);
                        break;
                    case "Removed":
                        _spriteBatch.Draw(Removed, person.Rect, Color.White);
                        break;
                    case "Recovered":
                        _spriteBatch.Draw(Removed, person.Rect, Color.White);
                        break;
                    case "Dead":
                        _spriteBatch.Draw(Removed, person.Rect, Color.White);
                        break;
                    default: System.Console.WriteLine($" unknown type found, { person.GetType().ToString().Split(".").GetValue(3) }"); break;
                }

                //System.Console.WriteLine($" {person.GetHashCode()} rect : {person.RadiusRect}");
            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }

        private void SusceptibleToInfecious(Person susceptible) 
        {
            for (int i = 0; i < this._people.Count; i++) 
            {
                if (this._people[i].GetHashCode() == susceptible.GetHashCode()) 
                {
                this._people[i] = new Infecious(susceptible.Position, susceptible.MovementVector, 0, 30);
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
                this._people[i] = new Recovered(infecious.Position, infecious.MovementVector, 0, 30);
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
                this._people[i] = new Dead(infecious.Position, infecious.MovementVector, 0, 30);
                return;
                }
            }
        }

        public Dictionary<string, int> GenerateOutputLists() {
            Dictionary<string, int> result_dict = new Dictionary<string, int>();
            result_dict.Add("susceptible", 0);
            result_dict.Add("infecious", 0);
            result_dict.Add("Removed", 0);
            result_dict.Add("recovered", 0);
            result_dict.Add("dead", 0);
            foreach (object person in this._people) {
                if (person.GetType().ToString() == "Susceptible")
                switch (person.GetType().ToString().Split(".").GetValue(3).ToString())
                {
                    case "Susceptible": result_dict["susceptible"] += 1; break;
                    case "Infecious": result_dict["infecious"] += 1; break;
                    case "Removed": result_dict["Removed"] += 1; break;
                    case "Recovered": result_dict["recovered"] += 1; break;
                    case "Dead": result_dict["dead"] += 1; break;
                    default: System.Console.WriteLine(" unknown type found "); break;
                }
            }
            return result_dict;
        }
    }
}
