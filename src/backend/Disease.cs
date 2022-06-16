namespace EpidemicSimulation
{
    public class Disease
    {
        public static float Lethality = 0.0001f; 
        public static float Duration = 2200f;
        public static float Communicability = 0.01f;
        public static float RequiredFieldIntersetion = 0.1f;
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
