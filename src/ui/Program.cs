using System;
using System.Drawing;
using System.Windows.Forms;
using EpidemicSimulation;

// FIXME: Application cannot be closed easily.

public class Program : Form
{
    private ISimulation _simulation;
    private TrackBar _slider;

    static public void Main()
    {
        Application.Run( new Program() );
    }

    public Program()
    {
        Size = new Size(500, 500);

        _slider = new TrackBar();
        Button b = new Button();

        _slider.Height = 50;
        _slider.Width = 400;
        _slider.Location = new Point(0, 50);

        b.Text = "Run simulation";

        b.Click += new EventHandler (Button_Click);
        _slider.Scroll += new System.EventHandler(Slider_Scroll);

        Controls.Add(b);
        Controls.Add(_slider);
    }

    private void Button_Click(object sender, EventArgs e)
    {
        _simulation = new SingleCommunitySimulation( (uint) _slider.Value);
        _simulation.Start();
    }

    private void Slider_Scroll(object sender, EventArgs e)
    {
        //MessageBox.Show(_slider.Value.ToString());
    }
}
