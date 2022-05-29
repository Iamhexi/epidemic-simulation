namespace EpidemicSimulation
{
    public class Disease
    {
        public static float Lethality = 0.2f; //  Lethality - immunity < random.NextDouble()

        // Let's assume that 1 second of the simulation equals one day in the simulated enviroment.
        public static float Duration = 1000f;// time after person gets recovered 1000 = +- 6.25 seconds (simulation speed x1)

        public static float Communicability = 0.03f; // fieldIntersetion * Communicability < random.NextDouble()

        // FIXME: Is this really a feature of ilness? In fact, any contact between people make chance for infection.
        public static float RequiredFieldIntersetion = 0.2f; // threshold of getting infected

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
