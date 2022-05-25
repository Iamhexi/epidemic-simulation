using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation.src.backend
{
    public class Simulation : Game
    {
        //Textures 
        Texture2D susceptible;
        Texture2D suspectible_radius;
        Texture2D infectious;
        Texture2D infectious_radius;
        Texture2D removed;

        //game class setup
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // enviroment variables
        public static int simulation_width = 800;
        public static int simulation_height = 800;
        private double simulationSpeed;
        private Dictionary<string, int> simulationSpeedList = new Dictionary<string, int>(){ {"0.5x", 32}, {"1x", 16}, {"2x", 8} ,{"4x", 4}, {"8x", 2} };
        private int _peopleAmount;
        private List<Person> _people = new List<Person>();
        private List<Susceptible> _susceptibles = new List<Susceptible>();
        private List<Infecious> _infecious = new List<Infecious>();
        private List<Removed> _removed = new List<Removed>();
        private List<Recovered> _recovered = new List<Recovered>();
        private List<Dead> _dead = new List<Dead>();
        

        //private enum: int gameSimulationSpeed { 480FPS = 2, 240FPS = 4, 120FPS = 8, 60FPS = 16 };

        public Simulation()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // parametry nadawane z menu
            _peopleAmount = 5;
            Person.moveSpeed = 3;
            this.simulationSpeed = this.simulationSpeedList["2x"];


        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = simulation_width;
            _graphics.PreferredBackBufferHeight = simulation_height;
            _graphics.ApplyChanges();
            
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(this.simulationSpeed); // time elapsed from last frame ( mili = 10^-3)

            // Person Testowa1 = new Person(new Point(200,400), new Vector2(1, 0));_people.Add(Testowa1);
            // Person Testowa2 = new Person(new Point(600,400), new Vector2(-1, 0));_people.Add(Testowa2);

            for (int i = 0; i<_peopleAmount; i++) { _people.Add(new Person()); }
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            susceptible = Content.Load<Texture2D>("suscetible");
            suspectible_radius = Content.Load<Texture2D>("suscetible-radius");
            infectious = Content.Load<Texture2D>("infected");
            infectious_radius = Content.Load<Texture2D>("infected-radius");
            removed = Content.Load<Texture2D>("removed");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            int i = 0;
            foreach (Person person in _people)
            {
                foreach(Person secondPerson in _people) if (Person.s_CheckCollision(person.AnticipadedPositon, secondPerson.AnticipadedPositon)) person.IsColliding = true;
                person.Update_Self();
               //System.Diagnostics.Debug.WriteLine($"--> Person {i} pozycja: x: {person.Get_Poz().X} y: {person.Get_Poz().Y} vector : {person.moveVector.ToString()}");
               i += 1;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
           foreach(Person person in _people) { _spriteBatch.Draw(susceptible, person.Rect, Color.White); }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


/* lista następnych: 
 * pole osoby -> radius, osobna klasa? 
 * kolizje z innymi ? 
 * klasy dziedziczne -> zdrowy, chory, usuniety
 * klasa choroby, interface ?
 * 
*/