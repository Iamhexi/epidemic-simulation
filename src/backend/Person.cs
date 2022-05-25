using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation.src.backend
{
    abstract class Person
    {
        // variables to set
        public Point Position { get; private set; }
        public static int _size { get; private set; }
        public Vector2 MovementVector;
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
        public Rectangle RadiusRect { get; private set; }
        private float precentegeOfRepulsion = 0;

        public abstract bool IsInfected();
        public abstract string Type();

        public Person(float? immunity = null, int? repulsionRate = null)
        {
            Person._size = 10;
            Position = new Point(s_randomizer.Next(_borderMargin, Simulation.s_SimulationWidth-_borderMargin),
                                        s_randomizer.Next(_borderMargin, Simulation.s_SimulationWidth-_borderMargin));
            MovementVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                        (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
            MovementVector.Normalize();
            Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            ImmunityRate = immunity ?? (float) s_randomizer.NextDouble();
            RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 2*Person._size);
        }
        public Person(Point startPosition, Vector2 MovementVector)
        {
            this.Position = startPosition;
            this.MovementVector = MovementVector;
        }
        public void UpdateSelf()
        {
            Move(); MoveRadiusField();
            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            this.AnticipadedPositon = new Rectangle((int)this.Rect.X + 3*(int)System.Math.Round(this.MovementVector.X * moveSpeed, MidpointRounding.AwayFromZero),
                                                    (int)this.Rect.Y + 3*(int)System.Math.Round(this.MovementVector.Y * moveSpeed, MidpointRounding.AwayFromZero),
                                                    Person._size, Person._size);
        }

        private void Move()
        {
            if (this.Position.X < 0 || this.Position.X > Simulation.s_SimulationWidth || this.Position.Y < 0 || this.Position.X > Simulation.s_SimulationHeight) { this.Position = new Point(400,400); return; }
            if (this.Position.X < this._borderMargin || Simulation.s_SimulationHeight - this.Position.Y < this._borderMargin  || this.Position.Y < this._borderMargin || Simulation.s_SimulationWidth - this.Position.X < this._borderMargin)
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

            Point temporaryPosition = new Point(
                Position.X + (int) System.Math.Round(MovementVector.X * moveSpeed, MidpointRounding.AwayFromZero),
                Position.Y + (int) System.Math.Round(MovementVector.Y * moveSpeed, MidpointRounding.AwayFromZero)
            );
            Position = temporaryPosition;
            this.IsColliding = false;
            //Console.WriteLine($"obj poz {this.Position}");
        }
        protected void MoveRadiusField()
      {
            this.RadiusRect = new Rectangle(Position.X, Position.Y, Person._size, Person._size);
            if (this.precentegeOfRepulsion >= 0 && this.precentegeOfRepulsion <= 1)
            {
                this.RadiusRect = new Rectangle(
                    this.Position.X-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    this.Position.Y-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion)
                );
                this.precentegeOfRepulsion += 0.01f;
            }
            else precentegeOfRepulsion = 0;

        }
        private void ChangeVector(int direction, float amount = 0.05f) {
            if (direction == 1)
            { // to right
                if (this.MovementVector.Y < 0 && this.MovementVector.X >= 0) { this.MovementVector.Y += amount; this.MovementVector.X += amount; }// top right
                if (this.MovementVector.Y < 0 && this.MovementVector.X < 0) { this.MovementVector.Y -= amount; this.MovementVector.X += amount; } // top left
                if (this.MovementVector.Y >= 0 && this.MovementVector.X >= 0) { this.MovementVector.Y += amount; this.MovementVector.X -= amount; } // bottom right
                if (this.MovementVector.Y >= 0 && this.MovementVector.X < 0) { this.MovementVector.Y -= amount; this.MovementVector.X -= amount; } // bottom left
            } // to left
            else
            {
                if (this.MovementVector.Y < 0 && this.MovementVector.X >= 0) { this.MovementVector.Y -= amount; this.MovementVector.X -= amount; }// top right
                if (this.MovementVector.Y < 0 && this.MovementVector.X < 0) { this.MovementVector.Y += amount; this.MovementVector.X -= amount; } // top left
                if (this.MovementVector.Y >= 0 && this.MovementVector.X >= 0) { this.MovementVector.Y -= amount; this.MovementVector.X += amount; } // bottom right
                if (this.MovementVector.Y >= 0 && this.MovementVector.X < 0) { this.MovementVector.Y += amount; this.MovementVector.X += amount; } // bottom left
            }
            this._directionChange += amount;
            this.MovementVector.Normalize();
        }
        public static bool s_CheckCollision(Rectangle obj1, Rectangle obj2) { if (!Rectangle.Intersect(obj1, obj2).IsEmpty && !Rectangle.Equals(obj1, obj2)) return true; return false; }
        private int DrawDirection() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; }
     }
}
