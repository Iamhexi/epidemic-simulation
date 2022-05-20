using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.classes
{
    class Person 
    {
        private const float PI = MathHelper.Pi;
        private int position_x;
        private int position_y;
        private static int size = 10;
        public Vector2 moveVector;
        int move_speed;
        float repultion_rate;
        private static Random randomizer = new Random();

        public Person()
        {
            this.position_x = randomizer.Next(100, 700);
            this.position_y = randomizer.Next(100, 700);
            this.move_speed = 2;
            this.moveVector = new Vector2((float)System.Math.Sin(randomizer.NextDouble())*2*PI, (float)System.Math.Sin(randomizer.NextDouble())*2*PI );
            this.moveVector.Normalize();
            this.repultion_rate = 0.44f ;
        }

        public Person(int starting_pos_x, int starting_pos_y)
        {
            this.position_x = starting_pos_x;
            this.position_y = starting_pos_y;
            this.moveVector = new Vector2(0,1f);
            this.move_speed = 2;
        }

        public void Update_Self(GameTime gameTime)
        {
            Move(gameTime);
            //System.Diagnostics.Debug.WriteLine($"pos: {this.position_x} {this.position_y} | wektor ruchu: {this.moveVector.ToString()}");
        }

        private void Move(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //System.Diagnostics.Debug.WriteLine($"time since last frame : {elapsed}");
            // check borders
            if (this.position_y < 50) Chage_moveVector(false);
            else if(Simulation.simulation_height-this.position_y < 100) Chage_moveVector(false);
            else if (this.position_x < 100) Chage_moveVector(false);
            else if (Simulation.simulation_width-this.position_x < 100) Chage_moveVector(false);

            //check others - collisions

            // make a step
            this.position_x += (int)System.Math.Round(this.moveVector.X * this.move_speed, MidpointRounding.AwayFromZero);
            this.position_y += (int)System.Math.Round(this.moveVector.Y * this.move_speed, MidpointRounding.AwayFromZero);
            //System.Diagnostics.Debug.WriteLine($"adding y:{System.Math.Round(this.moveVector.Y * this.move_speed, MidpointRounding.AwayFromZero)}");
            //System.Diagnostics.Debug.WriteLine($"adding x:{System.Math.Round(this.moveVector.X * this.move_speed, MidpointRounding.AwayFromZero)}");
        }
        public Rectangle Get_Rect() { return new Rectangle(this.position_x, this.position_y, Person.size, Person.size); }
        public (int, int) Get_Poz() { return (this.position_x, this.position_y);  }

        private void Chage_moveVector(bool direction, float amount = 0.05f) {
            if (direction)
            { // to right
                if (this.moveVector.Y < 0 && this.moveVector.X >= 0) { this.moveVector.Y += amount; this.moveVector.X += amount; }// top right
                if (this.moveVector.Y < 0 && this.moveVector.X < 0) { this.moveVector.Y -= amount; this.moveVector.X += amount; } // top left
                if (this.moveVector.Y >= 0 && this.moveVector.X >= 0) { this.moveVector.Y += amount; this.moveVector.X -= amount; } // bottom right 
                if (this.moveVector.Y >= 0 && this.moveVector.X < 0) { this.moveVector.Y -= amount; this.moveVector.X -= amount; } // bottom left
            } // to left
            else
            {
                if (this.moveVector.Y < 0 && this.moveVector.X >= 0) { this.moveVector.Y -= amount; this.moveVector.X -= amount; }// top right
                if (this.moveVector.Y < 0 && this.moveVector.X < 0) { this.moveVector.Y += amount; this.moveVector.X -= amount; } // top left
                if (this.moveVector.Y >= 0 && this.moveVector.X >= 0) { this.moveVector.Y -= amount; this.moveVector.X += amount; } // bottom right 
                if (this.moveVector.Y >= 0 && this.moveVector.X < 0) { this.moveVector.Y += amount; this.moveVector.X += amount; } // bottom left
            }
            this.moveVector.Normalize();
        }
       
        private bool Draw_Direction() { if (randomizer.Next(0, 100) >= 50) return true; return false; }
     }
}
