using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    public class Simulation : Game
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

            // parametry nadawane z menu
            _numberOfPeople = 0;


        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = s_simulationWidth;
            _graphics.PreferredBackBufferHeight = s_simulationHeight;
            _graphics.ApplyChanges();

            Person testPerson = new Person(400, 400);
            people.Add(testPerson);

            for (int i = 0; i < _numberOfPeople; i++)
                people.Add(new Person());

            base.Initialize();
        }

        protected override void LoadContent()
        {

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

            //foreach(Person person in people)
                //_spriteBatch.Draw(texture2D, Rectangle, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
