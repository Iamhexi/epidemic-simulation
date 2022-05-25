using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    class Simulation : Game
    {
        public static int s_simulationWidth = 800;
        public static int s_simulationHeight = 800;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<Person> people = new List<Person>();
        private int _numberOfPeople;

        public Simulation()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "content";
            IsMouseVisible = true;

            _numberOfPeople = 0;


        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_simulationWidth;
            _graphics.PreferredBackBufferHeight = s_simulationHeight;
            _graphics.ApplyChanges();


            for (int i = 0; i < _numberOfPeople; i++)
                people.Add(new Adult());


            base.Initialize();
        }

        protected override void LoadContent()
        {
            Infecious.s_Texture = Content.Load<Texture2D>("infecious");
            Removed.s_Texture = Content.Load<Texture2D>("removed");
            Susceptible.s_Texture = Content.Load<Texture2D>("susceptible");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: rewrite the method using the clean code
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Draw with simulated people with _spriteBatch.
            //_spriteBatch.Begin();
            //foreach(Person person in people)
                //_spriteBatch.Draw(texture2D, Rectangle, Color.White);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
