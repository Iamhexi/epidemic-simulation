using System;
using System.Drawing;
using System.Windows.Forms;
using EpidemicSimulation;
using System.Threading;

/*

Population
Lethality
Duration
Communicability

*/

public class Program : Form
{
    private Thread _simulationThread;
    private ISimulation _simulation;

    private TrackBar _populationSlider;
    private TrackBar _lethalitySlider;
    private TrackBar _diseaseDurationSlider;
    private TrackBar _communicabilitySlider;

    private Label _populationLabel;
    private Label _lethalityLabel;
    private Label _diseaseDurationLabel;
    private Label _communicabilityLabel;

    private Button _simulationStartingButton;

    static public void Main()
    {
        Application.Run( new Program() );
    }

    public Program()
    {
        Size = new Size(500, 800);

        SetUpSimulationStartingButton();
        SetUpPopulationAdjustingComponents();
        SetUpLethalityAdjustingComponents();
        SetUpDiseaseDurationAdjustingComponents();
        SetUpCommunicabilityAdjustingComponents();
    }


    private void SetUpLethalityAdjustingComponents()
    {
        _lethalitySlider = new TrackBar();
        _lethalityLabel = new Label();

        _lethalityLabel.Height = 50;
        _lethalityLabel.Width = 80;
        _lethalityLabel.Location = new Point(200, 250);

        _lethalitySlider.Value = 3;
        _lethalitySlider.Minimum = 1;
        _lethalitySlider.Maximum = 100;
        _lethalitySlider.Height = 50;
        _lethalitySlider.Width = 400;
        _lethalitySlider.Location = new Point(50, 200);

        _lethalitySlider.Scroll += new System.EventHandler(_lethalitySlider_Scroll);

        Controls.Add(_lethalitySlider);
        Controls.Add(_lethalityLabel);

        _lethalitySlider_Scroll(null, null);
    }

    private void SetUpCommunicabilityAdjustingComponents()
    {
        _communicabilitySlider = new TrackBar();
        _communicabilityLabel = new Label();

        _communicabilityLabel.Height = 50;
        _communicabilityLabel.Width = 120;
        _communicabilityLabel.Location = new Point(200, 350);

        _communicabilitySlider.Value = 5;
        _communicabilitySlider.Minimum = 1;
        _communicabilitySlider.Maximum = 100;
        _communicabilitySlider.Height = 50;
        _communicabilitySlider.Width = 400;
        _communicabilitySlider.Location = new Point(50, 300);

        _communicabilitySlider.Scroll += new System.EventHandler(_communicabilitySlider_Scroll);

        Controls.Add(_communicabilitySlider);
        Controls.Add(_communicabilityLabel);

        _communicabilitySlider_Scroll(null, null);
    }

    private void SetUpPopulationAdjustingComponents()
    {
        _populationSlider = new TrackBar();
        _populationLabel = new Label();

        _populationLabel.Height = 50;
        _populationLabel.Width = 150;
        _populationLabel.Location = new Point(200, 150);

        _populationSlider.Minimum = 10;
        _populationSlider.Maximum = 50;
        _populationSlider.Value = 35;
        _populationSlider.Height = 50;
        _populationSlider.Width = 400;
        _populationSlider.Location = new Point(50, 100);

        _populationSlider.Scroll += new System.EventHandler(_populationSlider_Scroll);

        Controls.Add(_populationSlider);
        Controls.Add(_populationLabel);

        _populationSlider_Scroll(null, null);
    }

    private void SetUpDiseaseDurationAdjustingComponents()
    {
        _diseaseDurationSlider = new TrackBar();
        _diseaseDurationLabel = new Label();

        _diseaseDurationLabel.Height = 50;
        _diseaseDurationLabel.Width = 100;
        _diseaseDurationLabel.Location = new Point(200, 450);

        _diseaseDurationSlider.Minimum = 3;
        _diseaseDurationSlider.Maximum = 30;
        _diseaseDurationSlider.Value = 17;
        _diseaseDurationSlider.Height = 50;
        _diseaseDurationSlider.Width = 400;
        _diseaseDurationSlider.Location = new Point(50, 400);

        _diseaseDurationSlider.Scroll += new System.EventHandler(_dieseaseDurationSlider_Scroll);

        Controls.Add(_diseaseDurationSlider);
        Controls.Add(_diseaseDurationLabel);

        _dieseaseDurationSlider_Scroll(null, null);
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
        int dieaseDurationMultipier = 160;

        Disease.s_SetUpParams(
            _lethalitySlider.Value / 100f,
            _diseaseDurationSlider.Value * dieaseDurationMultipier,
            _communicabilitySlider.Value / 100f
        );

        _simulation = new SingleCommunitySimulation( (uint) _populationSlider.Value);
        _simulationThread = new Thread(_simulation.Start);
        _simulationThread.Start();
    }

    private void _populationSlider_Scroll(object sender, EventArgs e)
    {
        _populationLabel.Text = "Population: " + _populationSlider.Value.ToString() + " people";
    }

    private void _lethalitySlider_Scroll(object sender, EventArgs e)
    {
        _lethalityLabel.Text = "Lethality: " + _lethalitySlider.Value.ToString() + "%";
    }

    private void _dieseaseDurationSlider_Scroll(object sender, EventArgs e)
    {
        _diseaseDurationLabel.Text = "Duration: " + _diseaseDurationSlider.Value.ToString() + " days";
    }

    private void _communicabilitySlider_Scroll(object sender, EventArgs e)
    {
        _communicabilityLabel.Text = "Communicability: " + _communicabilitySlider.Value.ToString() + " %";
    }

    private void Program_FormClosing(Object sender, FormClosingEventArgs e)
    {
        //Close();
        _simulationThread.Abort();
    }
}
