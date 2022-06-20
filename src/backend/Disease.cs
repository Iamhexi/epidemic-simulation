namespace EpidemicSimulation
{
    public class Disease
    {
        /**
        Class representing an instance of disease, contains all parameters used to simulate behavior of such.
        */
        public static float Lethality = 0.1f; //  <0.0001 - 0.1>

        public static float Duration = 2000f;// <1500 - 2500>

        public static float Communicability = 0.03f; // <0.001 - 0.1>

        public static float RequiredFieldIntersetion = 0.3f; // <0.05 - 0.5>

        /**
        Method responsible for setting chosen params from UI
        @param lethality as factor of dying rate
        @param duration as number of simulation Update() calls required to change state
        @param communicability as factor of transmission probability
        @param requiredFieldIntersetion as minimal outhger field intersection value (0 - 1)
        */
        public static void s_SetUpParams(
            float? lethality = null,
            float? duration = null,
            float? communicability = null,
            float? requiredFieldIntersetion = null)
        {
            Disease.Lethality = lethality ?? Disease.Lethality;
            Disease.Duration = duration ?? Disease.Duration;
            Disease.Communicability = communicability ?? Disease.Communicability;
            Disease.RequiredFieldIntersetion = requiredFieldIntersetion ?? Disease.RequiredFieldIntersetion;
        }
    }
}
