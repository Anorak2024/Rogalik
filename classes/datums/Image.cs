using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Image : Atom {
    /// <summary>
    /// The fraction on which the center of the image is located.
    /// </summary>
    public double center_x, center_y;
    public double h_part, w_part;
    public double w_dev_h;

    public Image() : base() {}

    public Image(double center_x, double center_y, double h_part, double w_part) : base() {
        this.center_x = center_x;
        this.center_y = center_y;
        this.h_part = h_part;
        this.w_part = w_part;
        w_dev_h = getTexture().Width / (double) getTexture().Height;
    }

    public Tuple<int, int, int, int> getPos(GameWindow window) {
        int w = (int) (w_part * window.ClientBounds.Width);
        int h = (int) (h_part * window.ClientBounds.Height);
        w = Math.Min(w, (int) (h * w_dev_h));
        h = Math.Min(h, (int) (w / w_dev_h));
        int x0 = (int) (center_x * window.ClientBounds.Width - w / 2);
        int y0 = (int) (center_y * window.ClientBounds.Height - h / 2);
        return new Tuple<int, int, int, int>(x0, y0, w, h);
    }
}