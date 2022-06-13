using System;
using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    abstract class Person
    {
        // variables to set
        public Point Position { get; private set; }
        public static int _size { get; private set; }
        public Vector2 MovementVector;
        public static float s_MovementSpeed { get; set; }
        private int _borderMargin = 40;

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
        public bool RepulsionExpand { get; private set; }
        public Rectangle RadiusRect { get; private set; }
        private float precentegeOfRepulsion = 0;
        public float InfectionDuration;
        public bool IgnoreColision = false;
        private bool _goingToPoint = false;
        private int _timeinPoint;

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
            RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 3*Person._size);
            RepulsionExpand = true;
        }

        public Person(Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
        {
            this.Position = startPosition;
            this.MovementVector = MovementVector;
            MovementVector.Normalize();
            Person._size = 10;
            Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            ImmunityRate = immunity ?? (float) s_randomizer.NextDouble();
            RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 2*Person._size);
            RepulsionExpand = true;
        }

        public virtual void UpdateSelf()
        {
            if (this._goingToPoint) { GoToPoint(); MoveRadiusField(); }
            else { Move(); MoveRadiusField(); }

            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            this.AnticipadedPositon = new Rectangle(
                (int)this.Rect.X + 3*(int)System.Math.Round(this.MovementVector.X * s_MovementSpeed, MidpointRounding.AwayFromZero),
                (int)this.Rect.Y + 3*(int)System.Math.Round(this.MovementVector.Y * s_MovementSpeed, MidpointRounding.AwayFromZero),
                Person._size,
                Person._size);
        }

        protected virtual void Move()
        {
            if (this.Position.X < this._borderMargin-30 || this.Position.X > Simulation.s_SimulationWidth-this._borderMargin+30 || this.Position.Y < this._borderMargin-30 || this.Position.Y > Simulation.s_SimulationHeight-this._borderMargin+30)
            {
                this.Position = new Point(s_randomizer.Next(this._borderMargin+1, Simulation.s_SimulationWidth-this._borderMargin-1), s_randomizer.Next(this._borderMargin+1, Simulation.s_SimulationHeight-this._borderMargin-1));
            }
            if (this.Position.X < this._borderMargin || Simulation.s_SimulationHeight - this.Position.Y < this._borderMargin  || this.Position.Y < this._borderMargin || Simulation.s_SimulationWidth - this.Position.X < this._borderMargin)
            {
                if (this._choice == 0 ) this._choice = DrawDirection();
                if (this._directionChange < 3.6f) ChangeVector(this._choice);
            }
            else if(this.IsColliding)
            {
                if (this._choice == 0 ) this._choice = DrawDirection();
                if (this._directionChange < 3f) ChangeVector(this._choice, 0.15f);
            }
            else { this._choice = 0; this._directionChange = 0; }

            Point temporaryPosition = new Point(
                Position.X + (int) System.Math.Round(MovementVector.X * s_MovementSpeed, MidpointRounding.AwayFromZero),
                Position.Y + (int) System.Math.Round(MovementVector.Y * s_MovementSpeed, MidpointRounding.AwayFromZero)
            );
            Position = temporaryPosition;
            this.IsColliding = false;
        }

        protected void MoveRadiusField()
        {
            this.RadiusRect = new Rectangle(Position.X, Position.Y, Person._size, Person._size);
            if (this.precentegeOfRepulsion >= 1) this.RepulsionExpand = false;
            else if (this.precentegeOfRepulsion <= 0)this.RepulsionExpand = true;
            if (this.RepulsionExpand)
            {
                this.RadiusRect = new Rectangle(
                    this.Position.X-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    this.Position.Y-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion)
                );
                this.precentegeOfRepulsion += 0.01f;
            }
            else
            {
                this.RadiusRect = new Rectangle(
                    this.Position.X-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    this.Position.Y-(int)(this.RepulsionRate*this.precentegeOfRepulsion/2),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion),
                    Person._size+(int)(this.RepulsionRate*this.precentegeOfRepulsion)
                );
                this.precentegeOfRepulsion -= 0.01f;
            }

        }

        private void ChangeVector(int direction, float amount = 0.11f) {
            if (direction == 1)
            {
                if (this.MovementVector.Y < 0 && this.MovementVector.X >= 0) { this.MovementVector.Y += amount; this.MovementVector.X += amount; }// top right
                if (this.MovementVector.Y < 0 && this.MovementVector.X < 0) { this.MovementVector.Y -= amount; this.MovementVector.X += amount; } // top left
                if (this.MovementVector.Y >= 0 && this.MovementVector.X >= 0) { this.MovementVector.Y += amount; this.MovementVector.X -= amount; } // bottom right
                if (this.MovementVector.Y >= 0 && this.MovementVector.X < 0) { this.MovementVector.Y -= amount; this.MovementVector.X -= amount; } // bottom left
            }
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

        public void GoToPoint(Point? centerPoint = null, float probability = 0)
        {
            if (centerPoint.HasValue)
            {
                if (probability > s_randomizer.NextDouble() && !this._goingToPoint)
                {
                    this.IgnoreColision = true; this._goingToPoint = true; this._timeinPoint = 200;
                }
                else if (!Rectangle.Intersect(new Rectangle(centerPoint.Value.X-15, centerPoint.Value.Y-15, 30, 30), this.Rect).IsEmpty)
                {
                    if (this._timeinPoint < 0)
                    {
                        this.IgnoreColision = false;
                        this._goingToPoint = false;
                        this.MovementVector = new Vector2(
                            (float) System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                            (float) System.Math.Sin(s_randomizer.NextDouble())*2*PI );
                        this.MovementVector.Normalize();
                    }
                    else
                    {
                        this.MovementVector = new Vector2(0,0);
                        this._timeinPoint -= 1;
                    }
                }
                else if(this._goingToPoint)
                {
                    this.MovementVector = new Vector2(centerPoint.Value.X - this.Position.X, centerPoint.Value.Y - this.Position.Y);
                    this.MovementVector.Normalize();
                    Point temporaryPosition = new Point(
                    Position.X + (int) System.Math.Round(this.MovementVector.X * s_MovementSpeed, MidpointRounding.AwayFromZero),
                    Position.Y + (int) System.Math.Round(this.MovementVector.Y * s_MovementSpeed, MidpointRounding.AwayFromZero));
                    Position = temporaryPosition;
                }
            }
        }

        public static bool s_CheckCollision(Rectangle obj1, Rectangle obj2) { if (!Rectangle.Intersect(obj1, obj2).IsEmpty && !Rectangle.Equals(obj1, obj2)) return true; return false; }
        public static float s_FieldIntersectionPrecentege(Rectangle obj1, Rectangle obj2) { return Person.RectSurface(Rectangle.Intersect(obj1, obj2))/Person.RectSurface(obj1); }
        protected static float RectSurface(Rectangle obj) { return obj.Height * obj.Width; }
        protected int DrawDirection() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; }
     }
}
