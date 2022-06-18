using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Charting;
using EpidemicSimulation;

/**
    Class manages the Graph class providing basic setup and letting easily update
    displayed statistics.
*/

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

        /**
            Constructor sets position of the the graph inside a window, size of
            the graph, instance of Simulation providing data for plotting and a
            graphics card object.

            @param position Position of the graph inside window
            @param size Size of the graph in pixels
            @param simulation Instance of Simulation to dervive data from
            @param graphicsDevice A graphics card object
        */

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

        /**
            Updates the statistics culling them from provided instance of Simulation.
        */

        public void Update()
        {
            UpdateInfectedPopulation();
            UpdateSusceptiblePopulation();
            UpdateRecoveredPopulation();
            UpdateDeadPopulation();
        }

        /**
            Plots all data: infected in red, susceptible in blue, recovered in
            green and dead in gray.
        */

        public void Draw()
        {
            _graph.Draw(_infectedTimeSeries, Color.Red);
            _graph.Draw(_susceptibleTimeSeries, Color.Blue);
            _graph.Draw(_recoveredTimeSeries, Color.Green);
            _graph.Draw(_deadTimeSeries, Color.Gray);
        }

        /**
            Initializes instance of Graph and configures it.
        */

        public void LoadContent()
        {
            _graph = new Graph(_graphicsDevice, _size);
            _graph.Position = _position;
            _graph.Size = _size;
            _graph.MaxValue = 50;
            _graph.Type = Charting.Graph.GraphType.Line;
        }

        /**
            Updates the number of infected people.
        */

        private void UpdateInfectedPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Infectious"];
            _infectedTimeSeries.Add( (float) currentNum );
        }

        /**
            Updates the number of susceptible people.
        */

        private void UpdateSusceptiblePopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Susceptible"];
            _susceptibleTimeSeries.Add( (float) currentNum );
        }

        /**
            Updates the number of recovered people.
        */

        private void UpdateRecoveredPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Recovered"];
            _recoveredTimeSeries.Add( (float) currentNum );
        }

        /**
            Updates the number of dead people.
        */

        private void UpdateDeadPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Dead"];
            _deadTimeSeries.Add( (float) currentNum );
        }
    }

}
