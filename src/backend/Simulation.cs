using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation.src.backend
{
    public class Simulation : Game
    {
        //Textures
        public Texture2D susceptible;
        public Texture2D susceptible_radius;
        public Texture2D infectious;
        public Texture2D infectious_radius;
        public Texture2D removed;

        //game class setup
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // enviroment variables
        public static int s_SimulationWidth = 800;
        public static int s_SimulationHeight = 800;
        private double simulationSpeed;
        private Dictionary<string, int> simulationSpeedList = new Dictionary<string, int>(){ {"0.5x", 32}, {"1x", 16}, {"2x", 8} ,{"4x", 4}, {"8x", 2} };
        private int _susceptibleAmount;
        private int _infeciousAmount;
        private List<Person> _people = new List<Person>();
        public Simulation()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../content";
            IsMouseVisible = true;

            // the parameters set from menu
            _susceptibleAmount = 10;
            _infeciousAmount = 10;
            Person.moveSpeed = 1;
            this.simulationSpeed = this.simulationSpeedList["8x"];
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_SimulationWidth;
            _graphics.PreferredBackBufferHeight = s_SimulationHeight;
            _graphics.ApplyChanges();
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(this.simulationSpeed);

            for (int i = 0; i<_susceptibleAmount; i++) { this._people.Add(new Susceptible(0 , 30)); }
            for (int i = 0; i<_infeciousAmount; i++) { this._people.Add(new Infecious(0 , 30)); }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            susceptible = Content.Load<Texture2D>("suscetible");
            susceptible_radius = Content.Load<Texture2D>("suscetible-radius");
            infectious = Content.Load<Texture2D>("infected");
            infectious_radius = Content.Load<Texture2D>("infected-radius");
            removed = Content.Load<Texture2D>("removed");
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
                        _spriteBatch.Draw(susceptible_radius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(susceptible, person.Rect, Color.White);
                        break;

                    case "Infecious":
                        _spriteBatch.Draw(infectious_radius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(infectious, person.Rect, Color.White);
                        break;
                    case "Removed":
                        _spriteBatch.Draw(removed, person.RadiusRect, Color.White);
                        break;
                    case "Recovered":
                        _spriteBatch.Draw(removed, person.RadiusRect, Color.White);
                        break;
                    case "Dead":
                        _spriteBatch.Draw(removed, person.RadiusRect, Color.White);
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
                    case "Susceptible": result_dict["susceptible"] += 1; break;
                    case "Infecious": result_dict["infecious"] += 1; break;
                    case "Removed": result_dict["removed"] += 1; break;
                    case "Recovered": result_dict["recovered"] += 1; break;
                    case "Dead": result_dict["dead"] += 1; break;
                    default: System.Console.WriteLine(" unknown type found "); break;
                }
            }
            return result_dict;
        }
    }
}
