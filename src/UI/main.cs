//    roundedrects.cs
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cairo;
using Gtk;

public class GtkCairo
{
    static void Main ()
    {
    Application.Init ();
    Gtk.Window w = new Gtk.Window ("Mono-Cairo Rounded Rectangles");

    DrawingArea a = new CairoGraphic ();

    Box box = new HBox (true, 0);
    box.Add (a);
    w.Add (box);
    w.Resize (500, 500);
    w.DeleteEvent += close_window;
    w.ShowAll ();

    Application.Run ();
    }

    static void close_window (object obj, DeleteEventArgs args)
    {
    Application.Quit ();
    }
}

public class CairoGraphic : DrawingArea
{
    static double min (params double[] arr)
    {
    int minp = 0;
    for (int i = 1; i < arr.Length; i++)
        if (arr[i] < arr[minp])
        minp = i;

    return arr[minp];
    }

    static void DrawRoundedRectangle (Cairo.Context gr, double x, double y, double width, double height, double radius)
    {
    gr.Save ();

    if ((radius > height / 2) || (radius > width / 2))
        radius = min (height / 2, width / 2);

    gr.MoveTo (x, y + radius);
    gr.Arc (x + radius, y + radius, radius, Math.PI, -Math.PI / 2);
    gr.LineTo (x + width - radius, y);
    gr.Arc (x + width - radius, y + radius, radius, -Math.PI / 2, 0);
    gr.LineTo (x + width, y + height - radius);
    gr.Arc (x + width - radius, y + height - radius, radius, 0, Math.PI / 2);
    gr.LineTo (x + radius, y + height);
    gr.Arc (x + radius, y + height - radius, radius, Math.PI / 2, Math.PI);
    gr.ClosePath ();
    gr.Restore ();
    }

    static void DrawCurvedRectangle (Cairo.Context gr, double x, double y, double width, double height)
    {
    gr.Save ();
    gr.MoveTo (x, y + height / 2);
    gr.CurveTo (x, y, x, y, x + width / 2, y);
    gr.CurveTo (x + width, y, x + width, y, x + width, y + height / 2);
    gr.CurveTo (x + width, y + height, x + width, y + height, x + width / 2, y + height);
    gr.CurveTo (x, y + height, x, y + height, x, y + height / 2);
    gr.Restore ();
    }

    protected override bool OnExposeEvent (Gdk.EventExpose args)
    {
    using (Context g = Gdk.CairoHelper.Create (args.Window)){
        DrawCurvedRectangle (g, 30, 30, 300, 200);
        DrawRoundedRectangle (g, 70, 250, 300, 200, 40);
        g.Color = new Color (0.1, 0.6, 1, 1);
        g.FillPreserve ();
        g.Color = new Color (0.2, 0.8, 1, 1);
        g.LineWidth = 5;
        g.Stroke ();
    }
    return true;
    }
}
