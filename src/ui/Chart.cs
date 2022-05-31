using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Charting;
using EpidemicSimulation;


class ChartManager : Game
{
    public static int s_GameWidth { get; private set; }
    public static int s_GameHeight { get; private set; }

    private GraphicsDeviceManager _graphics;
    private Graph _graph;
    private ISimulation _simulation;

    private List<float> _susceptibleTimeSeries = new List<float>();
    private List<float> _infectedTimeSeries = new List<float>();
    private List<float> _recoveredTimeSeries = new List<float>();
    private List<float> _deadTimeSeries = new List<float>();

    public ChartManager(ISimulation simulation)
    {
        s_GameWidth = 500;
        s_GameHeight = 500;

        _graphics = new GraphicsDeviceManager(this);
        _simulation = simulation;
        IsMouseVisible = true;
    }

    private void updateInfectedPopulation()
    {
        int currentNum = _simulation.GetSimulationData()["infecious"];
        _infectedTimeSeries.Add( (float) currentNum );
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _graph.Draw(_infectedTimeSeries, Color.Red);

        base.Draw(gameTime);
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = s_GameWidth;
        _graphics.PreferredBackBufferHeight = s_GameHeight;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _graph = new Graph(GraphicsDevice, new Point( s_GameWidth, s_GameHeight ));
        _graph.Position = new Vector2( 0f, s_GameHeight );
        _graph.Size = new Point( s_GameWidth, s_GameHeight );
        _graph.MaxValue = 50;
        _graph.Type = Charting.Graph.GraphType.Fill;
    }

    protected override void Update(GameTime gameTime)
    {

    }
}
