using System;
using System.Diagnostics;

namespace TestSuite
{
    public class TestRunner
    {
        public static int failedTests = 0;
        public static int numberOfTests = 0;

        public static void Main(string[] args)
        {
            SimulationTest.TestGenerateOutputLists();
            PersonTest.TestCheckCollisionWhileTheyAreIntersectingWithOnePoint();
            PersonTest.TestCheckCollisionWhileIntersectingWithSomeArea();
            PersonTest.TestCheckCollisionWhileOneContainsTheOther();
            PersonTest.TestCheckCollisionWhileTheyNotShareAnyArea();
            PersonTest.TestFieldIntersectionPrecentegeWith0Percent();
            PersonTest.TestFieldIntersectionPrecentegeWith1Percent();
            PersonTest.TestFieldIntersectionPrecentegeWith100Percent();
            

            PrintSummary();
        }

        public static void AssertTrue(bool value)
        {
            StackTrace stackTrace = new StackTrace();
            string nameOfTest = stackTrace.GetFrame(1).GetMethod().Name;

            numberOfTests++;
            if (!value)
            {
                failedTests++;
                Console.WriteLine(nameOfTest + " has failed!");
                Console.WriteLine("expected = 'true' and provided = 'false'");
            }
        }

        public static void AssertFalse(bool value)
        {
            AssertTrue(!value);
        }

        public static void AssertEquals(dynamic expected, dynamic actual)
        {
            numberOfTests++;
            StackTrace stackTrace = new StackTrace();
            string nameOfTest = stackTrace.GetFrame(1).GetMethod().Name;

            if (expected != actual)
            {
                failedTests++;
                Console.WriteLine(nameOfTest + " has failed!");
                Console.WriteLine("expected = '" + expected + "' but received = '" + actual + "'");
            }
        }

        public static void PrintSummary()
        {
            Console.WriteLine("Failed tests: " + failedTests);
            Console.WriteLine("All tests: " + numberOfTests);
        }

    }

}
