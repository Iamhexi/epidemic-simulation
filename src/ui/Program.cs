using System;
using System.Drawing;
using System.Windows.Forms;
using EpidemicSimulation;
using System.Threading;

public class Program : Form
{
    private Thread _simulationThread;
    private ISimulation _simulation;
    private TrackBar _populationSlider;
    private TextBox _populationTextBox;
    private Button _simulationStartingButton;

    static public void Main()
    {
        Application.Run( new Program() );
    }

    public Program()
    {
        Size = new Size(500, 500);

        SetUpSimulationStartingButton();
        SetUpPopulationAdjustingComponents();
    }


    private void SetUpPopulationAdjustingComponents()
    {
        _populationSlider = new TrackBar();
        _populationTextBox = new TextBox();

        _populationTextBox.ReadOnly = true;
        _populationTextBox.Height = 50;
        _populationTextBox.Width = 100;
        _populationTextBox.Location = new Point(200, 150);

        _populationSlider.Minimum = 10;
        _populationSlider.Maximum = 50;
        _populationSlider.Height = 50;
        _populationSlider.Width = 400;
        _populationSlider.Location = new Point(50, 100);

        _populationSlider.Scroll += new System.EventHandler(Slider_Scroll);

        Controls.Add(_populationSlider);
        Controls.Add(_populationTextBox);
    }

    private void SetUpSimulationStartingButton()
    {
        _simulationStartingButton = new Button();
        _simulationStartingButton.Location = new Point(200, 25);
        _simulationStartingButton.Text = "Run simulation";

        _simulationStartingButton.Click += new EventHandler (Button_Click);

        Controls.Add(_simulationStartingButton);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        _simulation = new SingleCommunitySimulation( (uint) _populationSlider.Value);
        _simulationThread = new Thread(_simulation.Start);
        _simulationThread.Start();
    }

    private void Slider_Scroll(object sender, EventArgs e)
    {
        _populationTextBox.Text = _populationSlider.Value.ToString();
    }

    private void Program_FormClosing(Object sender, FormClosingEventArgs e)
    {
        //Close();
        _simulationThread.Abort();
    }
}
