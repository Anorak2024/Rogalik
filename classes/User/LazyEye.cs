using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class LazyEye(GameWindow window)
{
    public double lazy_eye_x = 0, lazy_eye_y = 0;
    bool relaxing = false;
    readonly GameWindow window = window;
    long relaxing_id;
    long last_move;

    public object Relax(Dictionary<string, object> args) {
        if (GLOB.getMilliseconds() - last_move < 100)
            return null;
        
        double sign_x = Math.Abs(lazy_eye_x) < 0.001 ? 0 : lazy_eye_x / Math.Abs(lazy_eye_x);
        double sign_y = Math.Abs(lazy_eye_y) < 0.001 ? 0 : lazy_eye_y / Math.Abs(lazy_eye_y);
        lazy_eye_x -= sign_x * Math.Max(Math.Min(Math.Abs(lazy_eye_x), 0.0001), Math.Abs(lazy_eye_x) / 10);
        lazy_eye_y -= sign_y * Math.Max(Math.Min(Math.Abs(lazy_eye_y), 0.0001), Math.Abs(lazy_eye_y) / 10);
        if (lazy_eye_x == 0 && lazy_eye_y == 0) {
            relaxing = false;
            Subsystem.visual.RemoveProcess(relaxing_id);
        }
        
        return null;
    }

    public object applyLazyEye(Datum sender, Datum reciver, Dictionary<string, object> args) {
        last_move = GLOB.getMilliseconds();
        double idx = (double) args["dx"];
        double idy = (double) args["dy"];
        double ddx = Math.Abs(idx) < 0.001 ? 0 : window.ClientBounds.Width / idx;
        double ddy = Math.Abs(idy) < 0.001 ? 0 : window.ClientBounds.Height / idy;
        int h = 1000000;
        lazy_eye_x = Math.Clamp((lazy_eye_x * (h - 1) + ddx) / h, -0.01, 0.01);
        lazy_eye_y = Math.Clamp((lazy_eye_y * (h - 1) + ddy) / h, -0.01, 0.01);
        if (!relaxing) {
            relaxing = true;
            relaxing_id = Subsystem.visual.AddProcess(Relax, [], 30);
        }
        
        return null;
    }
}