using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using EpidemicSimulation;

public class Program : Form
{
    private Thread _simulationThread;
    private ISimulation _simulation;
    private Button _simulationStartingButton;

    private TrackBar _populationSlider;
    private TrackBar _lethalitySlider;
    private TrackBar _diseaseDurationSlider;
    private TrackBar _communicabilitySlider;

    private Label _populationLabel;
    private Label _lethalityLabel;
    private Label _diseaseDurationLabel;
    private Label _communicabilityLabel;

    static public void Main()
    {
        Application.Run( new Program() );
    }

    public Program()
    {
        Size = new Size(500, 1000);

        SetUpSimulationStartingButton();
        SetUpAdjustmentComponents(ref _lethalitySlider, ref _lethalityLabel, 2, 1, 100, 100, _lethalitySlider_Scroll);
        SetUpAdjustmentComponents(ref _diseaseDurationSlider, ref _diseaseDurationLabel, 4, 3, 30, 100, _dieseaseDurationSlider_Scroll);
        SetUpAdjustmentComponents(ref _communicabilitySlider, ref _communicabilityLabel, 6, 1, 100, 135, _communicabilitySlider_Scroll);
        SetUpAdjustmentComponents(ref _populationSlider, ref _populationLabel, 8, 1, 50, 150, _populationSlider_Scroll);
    }

    private delegate void ScrollMethod(object sender, EventArgs e);

    private void SetUpAdjustmentComponents(
        ref TrackBar slider,
        ref Label textLabel,
        ushort order,
        int sliderMinValue,
        int sliderMaxValue,
        int textLabelWidth,
        ScrollMethod sliderScrollMethod)
    {
        int elementHeight = 50;
        int posY = order * elementHeight;

        slider = new TrackBar();
        textLabel = new Label();

        textLabel.Height = elementHeight;
        textLabel.Width = textLabelWidth;
        textLabel.Location = new Point(200, posY + elementHeight);

        slider.Minimum = sliderMinValue;
        slider.Maximum = sliderMaxValue;
        slider.Value = (sliderMaxValue + sliderMinValue) / 2;
        slider.Height = elementHeight;
        slider.Width = 400;
        slider.Location = new Point(50, posY);

        slider.Scroll += new System.EventHandler(sliderScrollMethod);

        Controls.Add(slider);
        Controls.Add(textLabel);

        sliderScrollMethod(null, null);
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
        _simulation.Start();
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
        _communicabilityLabel.Text = "Communicability: " + _communicabilitySlider.Value.ToString() + "%";
    }

    private void Program_FormClosing(Object sender, FormClosingEventArgs e)
    {
        //Close();
        _simulationThread.Abort();
    }
}
