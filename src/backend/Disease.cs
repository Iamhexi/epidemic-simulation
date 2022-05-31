namespace EpidemicSimulation
{
    public class Disease
    {
        public static float Lethality = 0.0001f; // FIXME: stable values beetween: <0.00001f and 0.0008> (0-100 * 10^(-4) in ui)
        // Let's assume that 1 second of the simulation equals one day in the simulated enviroment.
        public static float Duration = 1500f; // 
        public static float Communicability = 0.05f;
        // FIXME: Is this really a feature of ilness? In fact, any contact between people make chance for infection.
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
