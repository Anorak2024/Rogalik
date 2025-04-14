using System;
using Microsoft.Xna.Framework;


public static partial class GLOB
{
    public static Random rand;
    public static Map spawn_map;

    public static double getPixelMult(GameWindow window, double xrange, double yrange) {
        return Math.Max(window.ClientBounds.Width / xrange / Turf.side_len, window.ClientBounds.Height / yrange / Turf.side_len);
    }

    public static long getMilliseconds() {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}