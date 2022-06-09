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

    private GroupBox _radioButtons;
    private RadioButton _singleCommunitySimulationButton = new RadioButton();
    private RadioButton _multiCommunitySimulationButton = new RadioButton();
    private RadioButton _shoppingCommunitySimulationButton = new RadioButton();
    // TODO: set up and make possible chosing a scenario

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
        this.FormClosing += new FormClosingEventHandler(Program_FormClosing);

        _radioButtons = new GroupBox();
        _radioButtons.Width = 500;
        setUpRadioBox(ref _singleCommunitySimulationButton, "Single Community", 0, _singleCommunitySimulationButton_Click);
        setUpRadioBox(ref _shoppingCommunitySimulationButton, "Shopping Community", 1, _shoppingCommunitySimulationButton_Click);
        setUpRadioBox(ref _multiCommunitySimulationButton, "Multi Community", 2, _multiCommunitySimulationButton_Click);
        Controls.Add(_radioButtons);

        SetUpSimulationStartingButton();
        SetUpAdjustmentComponents(ref _lethalitySlider, ref _lethalityLabel, 2, 1, 100, 100, _lethalitySlider_Scroll);
        SetUpAdjustmentComponents(ref _diseaseDurationSlider, ref _diseaseDurationLabel, 4, 3, 30, 100, _diseaseDurationSlider_Scroll);
        SetUpAdjustmentComponents(ref _communicabilitySlider, ref _communicabilityLabel, 6, 1, 100, 135, _communicabilitySlider_Scroll);
        SetUpAdjustmentComponents(ref _populationSlider, ref _populationLabel, 8, 1, 50, 150, _populationSlider_Scroll);
    }

    private delegate void ScrollMethod(object sender, EventArgs e);

    private delegate void RadioButtonClickMethod(object sender, EventArgs e);

    private void setUpRadioBox(
        ref RadioButton radioButton,
        string label,
        ushort order,
        RadioButtonClickMethod clickEvent
        )
    {
        radioButton = new RadioButton();
        radioButton.AutoCheck = true;
        radioButton.Text = label;
        radioButton.Height = 50;
        radioButton.Width = 100;
        radioButton.Location = new Point(order*radioButton.Width, 10);

        if (order == 0u)
            radioButton.Checked = true;

        radioButton.Click += new System.EventHandler(clickEvent);

        _radioButtons.Controls.Add(radioButton);
    }

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
        _simulationStartingButton.Location = new Point(200, 500);
        _simulationStartingButton.Text = "Run simulation";

        _simulationStartingButton.Click += new EventHandler(Button_Click);

        Controls.Add(_simulationStartingButton);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        Disease.s_SetUpParams(
            _lethalitySlider.Value / 1000f,
            10f * _diseaseDurationSlider.Value + 1500f,
            _communicabilitySlider.Value / (4f * 4f)
        );

        if (_simulation == null)
            this._singleCommunitySimulationButton_Click(null, null);

        Console.WriteLine(_simulation);

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

    private void _diseaseDurationSlider_Scroll(object sender, EventArgs e)
    {
        _diseaseDurationLabel.Text = "Duration: " + _diseaseDurationSlider.Value.ToString() + " days";
    }

    private void _communicabilitySlider_Scroll(object sender, EventArgs e)
    {
        _communicabilityLabel.Text = "Communicability: " + (float) _communicabilitySlider.Value / 4 + "%";
    }

    private void _singleCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        if (_simulationThread == null)
            _simulation = new SingleCommunitySimulation( (uint) _populationSlider.Value);
    }

    private void _multiCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        Console.WriteLine("MultigroupCommunitySimulation has been chosen.");
            _simulation = new MultigroupCommunitySimulation( 4, (uint) _populationSlider.Value);
        Console.WriteLine(_simulation);
    }

    private void _shoppingCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        Microsoft.Xna.Framework.Point centerPoint = new Microsoft.Xna.Framework.Point(Simulation.s_SimulationWidth/2, Simulation.s_SimulationWidth/2);
        _simulation = new ShoppingCommunitySimulation( (uint) _populationSlider.Value, centerPoint);
    }

    private void Program_FormClosing(Object sender, FormClosingEventArgs e)
    {
        if (_simulation != null)
        {
            StatisticsPrinter printer = new StatisticsPrinter(_simulation);
            printer.Print();
        }

        if (_simulationThread != null)
            _simulationThread.Abort();

        Close();
    }
}
