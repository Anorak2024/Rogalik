using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public static partial class GLOB
{
    public static Random rand = new();
    public static World GameWorld;

    public static double getPixelMult(GameWindow window, double xrange, double yrange) {
        return Math.Max(window.ClientBounds.Width / xrange / Turf.side_len, window.ClientBounds.Height / yrange / Turf.side_len);
    }

    public static long getMilliseconds() {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    public static Color GetPixel(Texture2D texture, int x, int y) {
        Color[] data = new Color[texture.Width * texture.Height];
        texture.GetData(data);
        return data[y * texture.Width + x];
    }
}