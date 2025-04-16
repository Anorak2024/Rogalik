using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Image : Atom {
    /// <summary>
    /// The fraction on which the center of the image is located.
    /// </summary>
    public double center_x, center_y;
    public double h_part, w_part;
    public double w_dev_h;
    public override float depth => GLOB.DEPTH_IMAGES_L1;

    public Image() : base() {}

    public Image(double center_x, double center_y, double w_part, double h_part) : base() {
        this.center_x = center_x;
        this.center_y = center_y;
        this.h_part = h_part;
        this.w_part = w_part;
        Texture2D texture = getTexture();
        if (texture == null)
            return;

        w_dev_h = texture.Width / (double) texture.Height;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Client client, double x, double y, double mult)
    {
        var (x0, y0, w, h) = getPos(client.game.Window);
        base.Draw(gameTime, spriteBatch, client, x0, y0, getTexture() == null ? 1 : ((double) w / getTexture().Width));
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

    public override Atom getAtomOnPos(int x, int y, double mult, GameWindow window) {
        var (x0, y0, w, h) = getPos(window);
        if (x < x0 || y < y0 || x >= x0 + w || y >= y0 + h)
            return null;

        foreach (var A in content) {
            Atom found = A.getAtomOnPos(x, y, mult, window);
            if (found != null)
                return found;
        }

        Texture2D texture = getTexture();
        if (texture != null && GLOB.GetPixel(texture, 
                (int) (texture.Width / (double) w * ((x - x0) / mult)), 
                (int) (texture.Height / (double) h * ((y - y0) / mult))).A != 0)
            return this;
        else
            return null;
    }
}