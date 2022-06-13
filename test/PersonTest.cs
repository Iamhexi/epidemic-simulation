using EpidemicSimulation;
using Microsoft.Xna.Framework;

namespace TestSuite
{
    class PersonTest
    {
        public static void TestCheckCollisionWhileTheyAreIntersectingWithOnePoint()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 100, 100);
            Rectangle rectangle2 = new Rectangle(100, 100, 50, 50);

            bool collides = Person.s_CheckCollision(rectangle1, rectangle2);

            TestRunner.AssertTrue(collides);
        }

        public static void TestCheckCollisionWhileIntersectingWithSomeArea()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 100, 100);
            Rectangle rectangle2 = new Rectangle(0, 25, 100, 100);

            bool collides = Person.s_CheckCollision(rectangle1, rectangle2);

            TestRunner.AssertTrue(collides);
        }

        public static void TestCheckCollisionWhileOneContainsTheOther()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 100, 100);
            Rectangle rectangle2 = new Rectangle(25, 25, 50, 50);

            bool collides = Person.s_CheckCollision(rectangle1, rectangle2);

            TestRunner.AssertTrue(collides);
        }

        public static void TestCheckCollisionWhileTheyNotShareAnyArea()
        {
            Rectangle rectangle1 = new Rectangle(500, 500, 100, 100);
            Rectangle rectangle2 = new Rectangle(0, 0, 50, 50);

            bool collides = Person.s_CheckCollision(rectangle1, rectangle2);

            TestRunner.AssertFalse(collides);
        }

        public static void TestFieldIntersectionPrecentegeWith1Percent()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 10, 10);
            Rectangle rectangle2 = new Rectangle(0, 0, 1, 1);

            float collidingArea = Person.s_FieldIntersectionPrecentege(rectangle1, rectangle2);

            TestRunner.AssertEquals(0.01f, collidingArea);
        }

        public static void TestFieldIntersectionPrecentegeWith0Percent()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 10, 10);
            Rectangle rectangle2 = new Rectangle(11, 11, 1, 1);

            float collidingArea = Person.s_FieldIntersectionPrecentege(rectangle1, rectangle2);

            TestRunner.AssertEquals(0f, collidingArea);
        }

        public static void TestFieldIntersectionPrecentegeWith100Percent()
        {
            Rectangle rectangle1 = new Rectangle(0, 0, 10, 10);
            Rectangle rectangle2 = new Rectangle(0, 0, 10, 10);

            float collidingArea = Person.s_FieldIntersectionPrecentege(rectangle1, rectangle2);

            TestRunner.AssertEquals(1f, collidingArea);
        }

    }
}
