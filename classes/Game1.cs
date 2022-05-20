using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation.classes
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
        private int people_amount;
        private List<Person> people = new List<Person>();
        

        public Simulation()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // parametry nadawane z menu
            people_amount = 0;


        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = simulation_width;
            _graphics.PreferredBackBufferHeight = simulation_height;
            _graphics.ApplyChanges();

            Person Testpwa = new Person(400, 400);
            people.Add(Testpwa);
            for (int i = 0; i<people_amount; i++) {
                people.Add(new Person());
            }
            
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
            foreach (Person person in people)
            {
                person.Update_Self(gameTime);
               System.Diagnostics.Debug.WriteLine($"--> Person {i} pozycja: x: {person.Get_Poz().Item1} y: {person.Get_Poz().Item2}");
                i += 1;
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
           foreach(Person person in people)
            {
                _spriteBatch.Draw(susceptible, person.Get_Rect(), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
