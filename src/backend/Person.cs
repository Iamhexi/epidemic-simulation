using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    class Person
    {
        // variables to set
        private Point _position;
        public Point Position { get { return _position; } set { this.Position = this._position; } }
        private static int _size = 10;
        public Vector2 moveVector;
        public static float moveSpeed { get; set; }
        private int _borderMargin = 100;
    
        // additional variables
        private const float PI = MathHelper.Pi;
        private static Random s_randomizer = new Random();
        private int _choice = 0;
        private float _directionChange = 0f;
        public Rectangle Rect { get; set; }
        private Point _middlePoint;
        public bool IsColliding = false;
        public Rectangle AnticipadedPositon;

        public Person()
        {
            this._position = new Point(s_randomizer.Next(_borderMargin, Simulation.simulation_width-_borderMargin), 
                                        s_randomizer.Next(_borderMargin, Simulation.simulation_width-_borderMargin));
            this.moveVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                        (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
            this.moveVector.Normalize();
        }
        
        public Person(Point startPosition, Vector2 moveVector)
        {
            this._position = startPosition;
            this.moveVector = moveVector;
        }

        public void Update_Self()
        {
            Move(); MoveRadiusField();
            this.Rect = new Rectangle((int)this._position.X, (int)this._position.Y, Person._size, Person._size);
            this.AnticipadedPositon = new Rectangle((int)this.Rect.X + 3*(int)System.Math.Round(this.moveVector.X * moveSpeed, MidpointRounding.AwayFromZero),
                                                    (int)this.Rect.Y + 3*(int)System.Math.Round(this.moveVector.Y * moveSpeed, MidpointRounding.AwayFromZero),
                                                    Person._size, Person._size);
            this._middlePoint = this.Rect.Center;
        }

        private void Move()
        {
            if (this._position.X < this._borderMargin || Simulation.simulation_height - this._position.Y < this._borderMargin  || this._position.Y < this._borderMargin || Simulation.simulation_width - this._position.X < this._borderMargin)
            {
                if (this._choice == 0 ) this._choice = Draw_Direction();
                if (this._directionChange < 3.6f) Chage_moveVector(this._choice);
            }
            else if(this.IsColliding)
            {
                Console.WriteLine("collision");
                if (this._choice == 0 ) this._choice = Draw_Direction();
                if (this._directionChange < 2f) Chage_moveVector(this._choice, 0.15f);
            }
            else { this._choice = 0; this._directionChange = 0; }

            this._position.X += (int)System.Math.Round(this.moveVector.X * moveSpeed, MidpointRounding.AwayFromZero);
            this._position.Y += (int)System.Math.Round(this.moveVector.Y * moveSpeed, MidpointRounding.AwayFromZero);
            this.IsColliding = false;
        }
        private void MoveRadiusField() {}
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
            this._directionChange += amount;
            this.moveVector.Normalize();
        }
        public static bool s_CheckCollision(Rectangle obj1, Rectangle obj2) { if (!Rectangle.Intersect(obj1, obj2).IsEmpty && !Rectangle.Equals(obj1, obj2)) return true; return false; }
        private int Draw_Direction() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; } 
     }
}
