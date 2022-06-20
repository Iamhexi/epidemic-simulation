using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    /** 
    The base class of person instance in simulation. As abstract unites all types of forseen states. Implements all moving and updating logic, 
    generates and handles parametes used for every individual.
    */
    abstract class Person
    {
        // variables to set
        public Point Position { get; private set; }
        public static int _size { get; private set; }
        public Vector2 MovementVector;
        public static float s_MovementSpeed { get; set; }
        private int _borderMargin = 20;
        private Rectangle SimulationRect;

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
        public static List<Rectangle> Obsticles = new List<Rectangle>();
        public abstract string Type();

        /** 
        Elementary constructor generates all features of an individual wrapped in certain borders. Sets up initial vector of direction, speed and position.
        @param SimulationRect as adjustment of relative position, restricts the accesible field .
        @param immunity as resistivity of getting infected and as counter factor to Diseases lethality.
        @param repulsionRate as the furthest reach of outhger field.
        */

        public Person(Rectangle SimulationRect, float? immunity = null, int? repulsionRate = null)
        {
            this.SimulationRect = SimulationRect;
            Person._size = 10;
            Position = new Point(s_randomizer.Next(SimulationRect.Location.X+_borderMargin, SimulationRect.Location.X+SimulationRect.Width-_borderMargin),
                                        s_randomizer.Next(SimulationRect.Location.Y+_borderMargin, SimulationRect.Location.Y+SimulationRect.Height-_borderMargin));
            MovementVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                        (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
            MovementVector.Normalize();
            Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            while(true) {
                float temp_immunity = immunity ?? (float) s_randomizer.NextDouble();
                if (temp_immunity < Disease.Lethality) { ImmunityRate = temp_immunity; break; }
            }
            RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 3*Person._size);
            RepulsionExpand = true;
        }

        /** 
        Secondary constructor constructing an instace of Person with predefinied position and direction vector.
        @param SimulationRect as adjustment of relative position, restricts the accesible field .
        @param startPosition as initial point of spawn.
        @ MovementVector as initial set direction.
        @param immunity as resistivity of getting infected and as counter factor to Diseases lethality.
        @param repulsionRate as the furthest reach of outhger field.
        */

        public Person(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
        {
            this.SimulationRect = SimulationRect;
            this.Position = startPosition;
            this.MovementVector = MovementVector;
            MovementVector.Normalize();
            Person._size = 10;
            Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            ImmunityRate = immunity ?? (float) s_randomizer.NextDouble();
            RepulsionRate = repulsionRate ?? s_randomizer.Next(Person._size, 2*Person._size);
            RepulsionExpand = true;
        }

        /** 
        Main method of every change made to Person instance, contains partial logic of changing position, 
        animating the outhger field and controlling the forced move to certain point action.
        */
        public virtual void UpdateSelf()
        {
            if (this._goingToPoint) { GoToPoint(); MoveRadiusField(); }
            else { Move(); MoveRadiusField(); }

            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Person._size, Person._size);
            this.AnticipadedPositon = new Rectangle((int)this.Rect.X + 3*(int)System.Math.Round(this.MovementVector.X * s_MovementSpeed, MidpointRounding.AwayFromZero),
                                                    (int)this.Rect.Y + 3*(int)System.Math.Round(this.MovementVector.Y * s_MovementSpeed, MidpointRounding.AwayFromZero),
                                                    Person._size, Person._size);
        }
        /** 
        Method that handles boundries detection, off desired field position, detection of walls, colliding with others and setting up a direction of turn.
        */
        protected virtual void Move()
        {
            if (this.Position.X < SimulationRect.Location.X+this._borderMargin-30 || this.Position.X > SimulationRect.Location.X+SimulationRect.Width-this._borderMargin+30 || this.Position.Y < SimulationRect.Location.Y+this._borderMargin-30 || this.Position.Y > SimulationRect.Location.Y+SimulationRect.Height-this._borderMargin+30)
            {
                this.Position = new Point(s_randomizer.Next(SimulationRect.Location.X+this._borderMargin+1, SimulationRect.Location.X+SimulationRect.Width-this._borderMargin-1), s_randomizer.Next(SimulationRect.Location.Y+this._borderMargin+1, SimulationRect.Location.Y+SimulationRect.Height-this._borderMargin-1));
            }
            foreach(Rectangle obst in Obsticles)
            {
                if (obst.Contains(Rect.Location.X+_size/2, Rect.Location.Y) || obst.Contains(Rect.Location.X+_size/2, Rect.Location.Y+_size)) { MovementVector.Y = -1* MovementVector.Y; }
                else if (obst.Contains(Rect.Location.X, Rect.Location.Y+_size/2) || obst.Contains(Rect.Location.X+_size, Rect.Location.Y+_size/2)) { MovementVector.X = -1* MovementVector.X; }
            }
            if (this.Position.X < SimulationRect.Location.X+this._borderMargin || SimulationRect.Location.Y+SimulationRect.Height - this.Position.Y < this._borderMargin  || this.Position.Y < SimulationRect.Location.Y+this._borderMargin || SimulationRect.Location.X+SimulationRect.Width - this.Position.X < this._borderMargin)
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

        /** 
        Method containg logic of outhger field grown and shrinkage.
        */
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
        /** 
        Method controls every change action of moving direction when faced any bounder.

        @param direction as decision of turning left or right.
        @param amount as predefined quantity of change to the direction vector, used in range ( 0.05 for wide turn,  0.2 for rapid bounce ).
        */
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
        /** 
        Method forsing Peron instance to immidiately follow to the set point.

        @param centerPoint as the pointed point to arrive at.
        @param probability as rate of executing
        */
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
                    this.MovementVector = new Vector2((float)System.Math.Sin(s_randomizer.NextDouble())*2*PI,
                                                    (float)System.Math.Sin(s_randomizer.NextDouble())*2*PI );
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
        /** 
        Static method checks if collision is true by calculating overlapping area of two Person instances rectangles.
        */
        public static bool s_CheckCollision(Rectangle obj1, Rectangle obj2) { if (!Rectangle.Intersect(obj1, obj2).IsEmpty && !Rectangle.Equals(obj1, obj2)) return true; return false; }
        /** 
        Static method calculating value of overlapping area of two Person instances rectangles.
        */
        public static float s_FieldIntersectionPrecentege(Rectangle obj1, Rectangle obj2) { return Person.RectSurface(Rectangle.Intersect(obj1, obj2))/Person.RectSurface(obj1); }
        /** 
        Static method returning area of the Rectangle.
        */
        protected static float RectSurface(Rectangle obj) { return obj.Height * obj.Width; }
        /** 
        Method that returns the chosen direction of turn.
        */
        protected int DrawDirection() { if (s_randomizer.Next(0, 100) >= 50) return 1; return -1; }
     }
}
