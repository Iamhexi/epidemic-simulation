using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation.src.backend
{
    public class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infectious;
        public Texture2D InfectiousRadius;
        public Texture2D Removed;

        //game class setup
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

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
        protected List<Person> _people = new List<Person>();

        public Simulation(uint population = (uint) 20)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../content";
            IsMouseVisible = true;

            // the parameters set from menu
            _susceptibleAmount = population - 1;
            _infeciousAmount = 1; // always start from 1 ill person
            Person.s_MovementSpeed = 2;


            SimulationSpeed = SimulationSpeedValues.x2;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_SimulationWidth;
            _graphics.PreferredBackBufferHeight = s_SimulationHeight;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds( (double) SimulationSpeed );

            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible(0 , 30)); }
            for (int i = 0; i<_infeciousAmount; i++) { this._people.Add(new Infecious(0 , 30)); }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Susceptible= Content.Load<Texture2D>("suscetible");
            SusceptibleRadius = Content.Load<Texture2D>("suscetible-radius");
            Infectious = Content.Load<Texture2D>("infected");
            InfectiousRadius = Content.Load<Texture2D>("infected-radius");
            Removed = Content.Load<Texture2D>("removed");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            foreach(Person person in this._people)
            {
                foreach(Person secondPerson in this._people)
                if (Person.s_CheckCollision(person.AnticipadedPositon, secondPerson.AnticipadedPositon) ||
                Person.s_CheckCollision(person.Rect, secondPerson.Rect)) person.IsColliding = true;
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
                        _spriteBatch.Draw(InfectiousRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Infectious, person.Rect, Color.White);
                        break;
                    case "Removed":
                        _spriteBatch.Draw(Removed, person.RadiusRect, Color.White);
                        break;
                    case "Recovered":
                        _spriteBatch.Draw(Removed, person.RadiusRect, Color.White);
                        break;
                    case "Dead":
                        _spriteBatch.Draw(Removed, person.RadiusRect, Color.White);
                        break;
                    default: System.Console.WriteLine($" unknown type found, { person.GetType().ToString().Split(".").GetValue(3) }"); break;
                }

                //System.Console.WriteLine($" {person.GetHashCode()} rect : {person.RadiusRect}");
            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }
        public Dictionary<string, int> GenerateOutputLists() {
            Dictionary<string, int> result_dict = new Dictionary<string, int>();
            result_dict.Add("susceptible", 0);
            result_dict.Add("infecious", 0);
            result_dict.Add("removed", 0);
            result_dict.Add("recovered", 0);
            result_dict.Add("dead", 0);
            foreach (object person in this._people) {
                if (person.GetType().ToString() == "Susceptible")
                switch (person.GetType().ToString())
                {
                    case "Susceptible": result_dict["susceptible"]++; break;
                    case "Infecious": result_dict["infecious"]++; break;
                    case "Removed": result_dict["removed"]++; break;
                    case "Recovered": result_dict["recovered"]++; break;
                    case "Dead": result_dict["dead"]++; break;
                    default: System.Console.WriteLine(" unknown type found "); break;
                }
            }
            return result_dict;
        }
    }
}
