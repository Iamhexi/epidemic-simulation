using System;
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Random;

namespace EpidemicSimulation
{
    public class Simulation : Game
    {
        //Textures
        public Texture2D Susceptible;
        public Texture2D SusceptibleRadius;
        public Texture2D Infecious;
        public Texture2D InfeciousRadius;
        public Texture2D Recovered;
        public Texture2D Dead;

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
            Content.RootDirectory = "../content";
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
            Recovered = Content.Load<Texture2D>("recovered");
            Dead = Content.Load<Texture2D>("dead");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) Exit();

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
                                if (overlappingArea * Disease.Communicability > temp_random)
                                {
                                    System.Console.WriteLine($" Infected! : {overlappingArea * Disease.Communicability} < {temp_random}" );
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

                    case "Infecious":
                        _spriteBatch.Draw(InfeciousRadius, person.RadiusRect, Color.White);
                        _spriteBatch.Draw(Infecious, person.Rect, Color.White);
                        break;
                    case "Recovered":
                        _spriteBatch.Draw(Recovered, person.Rect, Color.White);
                        break;
                    case "Dead":
                        _spriteBatch.Draw(Dead, person.Rect, Color.White);
                        break;
                    default: System.Console.WriteLine($" unknown type found, { person.Type() }"); break;
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

        public Dictionary<string, int> GenerateOutputLists()
        {
            Dictionary<string, int> result_dict = new Dictionary<string, int>();
            result_dict.Add("susceptible", 0);
            result_dict.Add("infecious", 0);
            result_dict.Add("removed", 0);
            result_dict.Add("recovered", 0);
            result_dict.Add("dead", 0);
            foreach (Person person in _people) {
                if (person.Type() == "Susceptible")
                switch (person.Type())
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

        public void Pause()
        {
            // TODO: Implement (un)pausing a simulation.
        }

    }
}
