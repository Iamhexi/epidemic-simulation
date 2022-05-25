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
        private double simulation_speed = 16;
        private int people_amount;
        private List<Person> people = new List<Person>();

        //private enum: int gameSimulationSpeed { 480FPS = 2, 240FPS = 4, 120FPS = 8, 60FPS = 16 };

        public Simulation()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // parametry nadawane z menu
            people_amount = 100;
            Person.move_speed = 2;
            this.simulation_speed = 4; // 16 miliseconds -> 60 FPS | 8 miliseconds -> 120 FPS


        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = simulation_width;
            _graphics.PreferredBackBufferHeight = simulation_height;
            _graphics.ApplyChanges();
            
            this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(this.simulation_speed); // time elapsed from last frame ( mili = 10^-3)

            //Person Testowa = new Person(400, 400);people.Add(Testowa);

            for (int i = 0; i<people_amount; i++) { people.Add(new Person()); }
            
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
            //System.Diagnostics.Debug.WriteLine($"time span {gameTime.ElapsedGameTime}");

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            int i = 0;
            foreach (Person person in people)
            {
               person.Update_Self(gameTime);
               //System.Diagnostics.Debug.WriteLine($"--> Person {i} pozycja: x: {person.Get_Poz().X} y: {person.Get_Poz().Y} vector : {person.moveVector.ToString()}");
               i += 1;
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
           foreach(Person person in people) { _spriteBatch.Draw(susceptible, person.Get_Rect(), Color.White); }

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