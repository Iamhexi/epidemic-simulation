using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EpidemicSimulation
{
    class ChartManager
    {
        private Graph _graph;
        private Simulation _simulation;
        private GraphicsDevice _graphicsDevice;
        private Vector2 _position;
        private Point _size;
        private Dictionary<string, int> _simulationStats;
        private List<float> _susceptibleTimeSeries = new List<float>();
        private List<float> _infectedTimeSeries = new List<float>();
        private List<float> _recoveredTimeSeries = new List<float>();
        private List<float> _deadTimeSeries = new List<float>();

        public ChartManager(
            Vector2 position,
            Point size,
            Simulation simulation,
            GraphicsDevice graphicsDevice)
        {
            _position = position;
            _size = size;
            _simulation = simulation;
            _graphicsDevice = graphicsDevice;
        }

        public void Update()
        {
            this._simulationStats = _simulation.GenerateOutputLists();
            UpdateInfectedPopulation(_simulationStats["Infectious"]);
            UpdateSusceptiblePopulation(_simulationStats["Susceptible"]);
            UpdateRecoveredPopulation(_simulationStats["Recovered"]);
            UpdateDeadPopulation(_simulationStats["Dead"]);
        }

        public void Draw()
        {
            _graph.Draw(_infectedTimeSeries, Color.Red);
            _graph.Draw(_susceptibleTimeSeries, Color.Blue);
            _graph.Draw(_recoveredTimeSeries, Color.Green);
            _graph.Draw(_deadTimeSeries, Color.Gray);
        }

        public void LoadContent()
        {
            _graph = new Graph(_graphicsDevice, _size);
            _graph.Position = _position;
            _graph.Size = _size;
            _graph.MaxValue = 50;
            _graph.Type = Graph.GraphType.Line;
        }

        private void UpdateInfectedPopulation(float infected)
        {
            _infectedTimeSeries.Add( infected );
        }

        private void UpdateSusceptiblePopulation(float susceptible)
        {
            _susceptibleTimeSeries.Add( susceptible );
        }

        private void UpdateRecoveredPopulation(float recovered)
        {
            _recoveredTimeSeries.Add( recovered );
        }

        private void UpdateDeadPopulation(float dead)
        {
            _deadTimeSeries.Add( dead );
        }
    }

}
