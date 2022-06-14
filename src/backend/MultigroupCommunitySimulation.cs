using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace EpidemicSimulation
{
    class MultigroupCommunitySimulation : Game, ISimulation
    {
        private List<Simulation> communities = new List<Simulation>();
        protected GraphicsDeviceManager _graphics;
        //protected SpriteBatch _spriteBatch;
        public enum Communities: uint
        {
            two = 2,
            four = 4,
            six = 6, 
            eight = 8
        };
        public MultigroupCommunitySimulation(Communities communities, uint defaultPopulation = 50, uint defaultInfected = 3) // params set for window of size 1000x1000
        {
            // _graphics =  new GraphicsDeviceManager(this);
            // base.Initialize();

            switch ((int)communities)
            {
                case 2: this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,0,490,1000))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(510,0,490,1000))); 
                        break;
                case 4: this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,0,490,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(510,0,490,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,510,490,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(510,510,490,490))); 
                        break;
                case 6: this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,0,300,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(300,0,300,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(600,0,300,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,510,300,490)));
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(300,510,300,490))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(600,510,300,490))); 
                        break;
                case 8: this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,0,250,500))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(250,0,250,500))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(500,0,250,500))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(750,0,250,500)));
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(0,500,250,250))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(250,500,250,500)));
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(500,500,250,500))); 
                        this.communities.Add(new Simulation(defaultPopulation, defaultInfected, new Rectangle(750,590,250,500))); 
                        break;
                default: this.communities.Add(new Simulation()); break;

            }
        }

        public Dictionary<string, int> GetSimulationData()
        {
            Dictionary<string, int> compoundedData = new Dictionary<string, int>();
            compoundedData.Add("Susceptible", 0);
            compoundedData.Add("Infectious", 0);
            compoundedData.Add("Recovered", 0);
            compoundedData.Add("Dead", 0);

            foreach (var c in communities)
            {
                var dictionary = c.GenerateOutputLists();
                compoundedData["Susceptible"] += dictionary["Susceptible"];
                compoundedData["Infectious"] += dictionary["Infectious"];
                compoundedData["Recovered"] += dictionary["Recovered"];
                compoundedData["Dead"] += dictionary["Dead"];
            }

            return compoundedData;
        }

        public void Start()
        {
            this.Run();
            // foreach (Simulation simulation in communities)
            //     simulation.Run();
        }

        public void Pause()
        {
            foreach (Simulation simulation in communities)
                simulation.Pause();
        }

        public void Close()
        {
            foreach (Simulation simulation in communities)
                simulation.Exit();
        }
     }
    }