using System;
using Microsoft.Xna.Framework;


namespace EpidemicSimulation.src.backend
{
    abstract class Person
    {
        // variables to set
        private Point _position;
        public Point Position { get { return _position; } private set { this.Position = this._position; } }
        public static int _size { get; private set; }
        public Vector2 moveVector;
        public static float moveSpeed { get; set; }
        private int _borderMargin = 100;
    
        // additional variables
        private const float PI = MathHelper.Pi;
        private static Random s_randomizer = new Random();
        private int _choice = 0;
        private float _directionChange = 0f;
        public Rectangle Rect { get; set; }
        public bool IsColliding = false;
        public Rectangle AnticipadedPositon;
        public float ImmunityRate { get; private set; }
        public int RepulsionRate { get; private set; }
        public Rectangle RadiusRect {get; private set;}
        private float precentegeOfRepulsion = 0;
        public Person(float? immunity = null, int? repulsionRate = null)
        {
            Person._size = 10;
            this._position = new Point(s_randomizer.Next(_borderMargin, Simulation.simulation_width-_borderMargin), 
                                        s_randomizer.Next(_borderMargin, Simulation.simulation_width-_borderMargin));
            this.moveVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                        (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
            this.moveVector.Normalize();
            this.Rect = new Rectangle((int)this._position.X, (int)this._position.Y, Person._size, Person._size);
            this.ImmunityRate = immunity ?? (float) s_randomizer.NextDouble();
            this.RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 2*Person._size);
        }
        public Person(Point startPosition, Vector2 moveVector)
        {
            this._position = startPosition;
            this.moveVector = moveVector;
        }
        public void UpdateSelf()
        {
            Move(); MoveRadiusField();
            this.Rect = new Rectangle((int)this._position.X, (int)this._position.Y, Person._size, Person._size);
            this.AnticipadedPositon = new Rectangle((int)this.Rect.X + 3*(int)System.Math.Round(this.moveVector.X * moveSpeed, MidpointRounding.AwayFromZero),
                                                    (int)this.Rect.Y + 3*(int)System.Math.Round(this.moveVector.Y * moveSpeed, MidpointRounding.AwayFromZero),
                                                    Person._size, Person._size);
        }

        private void Move()
        {
            if (this.Position.X < 0 || this.Position.X > Simulation.simulation_width || this.Position.Y < 0 || this.Position.X > Simulation.simulation_height) { this.Position = new Point(400,400); return; }
            if (this._position.X < this._borderMargin || Simulation.simulation_height - this._position.Y < this._borderMargin  || this._position.Y < this._borderMargin || Simulation.simulation_width - this._position.X < this._borderMargin)
            {
                if (this._choice == 0 ) this._choice = DrawDirection();
                if (this._directionChange < 3.6f) ChangeVector(this._choice);
            }
            else if(this.IsColliding)
            {
                if (this._choice == 0 ) this._choice = DrawDirection();
                if (this._directionChange < 2f) ChangeVector(this._choice, 0.15f);
            }
            else { this._choice = 0; this._directionChange = 0; }

            this._position.X += (int)System.Math.Round(this.moveVector.X * moveSpeed, MidpointRounding.AwayFromZero);
            this._position.Y += (int)System.Math.Round(this.moveVector.Y * moveSpeed, MidpointRounding.AwayFromZero);
            this.IsColliding = false;
            //Console.WriteLine($"obj poz {this.Position}");
        }
        protected void MoveRadiusField() 
      { 
            this.RadiusRect = new Rectangle(this.Position.X, this.Position.Y, Person._size, Person._size);
            if (this.precentegeOfRepulsion >= 0 && this.precentegeOfRepulsion <= 1) 
            {
                this.RadiusRect = new Rectangle(this.Position.X-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2), 
                                                this.Position.Y-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2), 
                                                Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion), 
                                                Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion));
                this.precentegeOfRepulsion += 0.01f;
            }
            else precentegeOfRepulsion = 0;
            
        }
        private void ChangeVector(int direction, float amount = 0.05f) {
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
        private int DrawDirection() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; } 
        public abstract bool IsInfected();
        public abstract string Type();
     }
}