namespace EpidemicSimulation
{
    public class Disease
    {
        public static float Lethality = 0.2f; //  Lethality - immunity < random.NextDouble()
        public static float Duration = 1000f;// time after person gets recovered 2000 = + - 25 s przy x2
        public static float InfectionProbability = 0.03f; // fieldIntersetion * InfectionProbability < random.NextDouble()
        public static float RequiredFieldIntersetion = 0.2f; // threshold of getting infected

        public static void s_SetUpParams(float? lethality = null , float? duration = null, float? infectionProbability = null, float? requiredFieldIntersetion = null) 
        {
            Disease.Lethality = lethality ?? Disease.Lethality;
            Disease.Duration = duration ?? Disease.Duration;
            Disease.InfectionProbability = infectionProbability ?? Disease.InfectionProbability;
            Disease.RequiredFieldIntersetion = requiredFieldIntersetion ?? Disease.RequiredFieldIntersetion;
        }
    }
}
