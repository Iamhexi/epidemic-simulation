namespace EpidemicSimulation
{
    public class Disease
    {
        public static float Lethality = 0.2f; //  <0.0001 - 0.1>

        public static float Duration = 1000f;// <1500 - 2500>

        public static float Communicability = 0.03f; // <0.001 - 0.1>

        public static float RequiredFieldIntersetion = 0.2f; // <0.05 - 0.5>

        public static void s_SetUpParams(
            float? lethality = null,
            float? duration = null,
            float? Communicability = null,
            float? requiredFieldIntersetion = null)
        {
            Disease.Lethality = lethality ?? Disease.Lethality;
            Disease.Duration = duration ?? Disease.Duration;
            Disease.Communicability = Communicability ?? Disease.Communicability;
            Disease.RequiredFieldIntersetion = requiredFieldIntersetion ?? Disease.RequiredFieldIntersetion;
        }
    }
}
