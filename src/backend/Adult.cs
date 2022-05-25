using EpidemicSimulation;
using Microsoft.Xna.Framework.Graphics;

namespace EpidemicSimulation
{
    class Adult: Susceptible
    {
        public Adult(float? immunity = null, float? repulsionRate = null)
            : base(immunity, repulsionRate)
        {

        }
        
    }
}
