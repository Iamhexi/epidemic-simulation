using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Charting;
using EpidemicSimulation;

namespace EpidemicSimulation
{
    class ChartManager
    {
        private Graph _graph;
        private Simulation _simulation;
        private GraphicsDevice _graphicsDevice;
        private Vector2 _position;
        private Point _size;

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
            UpdateInfectedPopulation();
            UpdateSusceptiblePopulation();
            UpdateRecoveredPopulation();
            UpdateDeadPopulation();
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
            _graph.Type = Charting.Graph.GraphType.Line;
        }

        private void UpdateInfectedPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Infectious"];
            _infectedTimeSeries.Add( (float) currentNum );
        }

        private void UpdateSusceptiblePopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Susceptible"];
            _susceptibleTimeSeries.Add( (float) currentNum );
        }

        private void UpdateRecoveredPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Recovered"];
            _recoveredTimeSeries.Add( (float) currentNum );
        }

        private void UpdateDeadPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Dead"];
            _deadTimeSeries.Add( (float) currentNum );
        }
    }

}
