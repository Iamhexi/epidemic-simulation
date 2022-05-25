using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    class Person
    {
        // variables to set
        private int position_x;
        private int position_y;
        private static int size = 10;
        public Vector2 moveVector;
        public static float BaseMoveSpeed = 4;
        private float actualMoveSpeed;
        private int border_margin = 100;
        //float repultion_rate;
    
 
        // additional variables
        private const float PI = MathHelper.Pi;
        private static Random s_randomizer = new Random();
        private int choice = 0;

        public Person()
        {
            this.position_x = s_randomizer.Next(100, 700);
            this.position_y = s_randomizer.Next(100, 700);
            this.moveVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                          (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
            this.moveVector.Normalize();
            //this.repultion_rate = 0.44f ;
        }
        
        public Person(int starting_pos_x, int starting_pos_y)
        {
            this.position_x = starting_pos_x;
            this.position_y = starting_pos_y;
            this.moveVector = new Vector2(0,1f);
        }

        public void Update_Self()
        {
            Move();
            //System.Diagnostics.Debug.WriteLine($"pos: {this.position_x} {this.position_y} | wektor ruchu: {this.moveVector.ToString()}");
        }

        private void Move()
        {
        
            //System.Diagnostics.Debug.WriteLine($"time since last frame : {elapsed}");

            // check borders
            if (this.position_y < this.border_margin || Simulation.simulation_height - this.position_y < this.border_margin  || this.position_x < this.border_margin || Simulation.simulation_width - this.position_x < this.border_margin)
            {
                if (this.choice == 0) this.choice = Draw_Direction();
                Chage_moveVector(this.choice);
            }
            else { this.choice = 0; }
            
            //check others - collisions TO DO
            //fix endless tourning

            // make a step
            this.position_x += (int)System.Math.Round(this.moveVector.X * this.actualMoveSpeed, MidpointRounding.AwayFromZero);
            this.position_y += (int)System.Math.Round(this.moveVector.Y * this.actualMoveSpeed, MidpointRounding.AwayFromZero);
        }
        public Rectangle Get_Rect() { return new Rectangle(this.position_x, this.position_y, Person.size, Person.size); }
        public Vector2 Get_Poz() { return new Vector2(this.position_x, this.position_y);  }
        private void Chage_moveVector(int direction, float amount = 0.05f) {
            if (direction == 1)
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
        private int Draw_Direction() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; } 
     }
}
