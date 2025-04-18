using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A type of screen with a bound Viewer. 
/// Displays a map around the Atom that owns the Viewer.
/// </summary>
public class Screen_Map : Screen {
    /// <summary>
    /// Number of full plates on x and y axis.
    /// </summary>
    const int xrange0 = 16, yrange0 = 9;

    public Screen_Map(Client client) : base(client) {
        Mob eye = new Human(); // Will be changed.
        eye.Transfer(GLOB.spawn_map.getSpawn(), true);
        client.SetUpViewer(eye);
    }

    public double getXrange() {
        return xrange0 * (double) client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
    }

    public double getYrange() {
        return yrange0 * (double) client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
    }

    public override void Draw(GameTime gameTime, GameWindow window, SpriteBatch spriteBatch) {
        base.Draw(gameTime, window, spriteBatch);
        var seeable = getTurfsOnScreen(window);
        Map map = client.viewer.getMap();
        double mult = GLOB.getPixelMult(window, getXrange(), getYrange());
        foreach (var (tx, ty, x0, y0) in seeable)
            map.getTurf(tx, ty)?.Draw(  gameTime, 
                                        spriteBatch, 
                                        client, 
                                        x0 + window.ClientBounds.Width * client.viewer.lazyEye.lazy_eye_x, 
                                        y0 + window.ClientBounds.Height * client.viewer.lazyEye.lazy_eye_y,
                                        mult);

        foreach (var image in client.viewer.eye.seeable)
            image.Draw(gameTime, spriteBatch, client, 0, 0, 0);
    }

    public override void Update(GameTime gameTime) {
        
    }

    public override Atom GetAtomOnPos(GameWindow window, int x, int y) {
        Atom image = base.GetAtomOnPos(window, x, y);
        if (image != null)
            return image;
        
        var seeable = getTurfsOnScreen(window);
        Map map = client.viewer.getMap();
        double mult = GLOB.getPixelMult(window, getXrange(), getYrange());
        foreach (var (tx, ty, x0, y0) in seeable) {
            Atom found = map.getTurf(tx, ty)?.GetAtomOnPos(x - x0, y - y0, mult, window);
            if (found != null)
                return found;
        }

        return null;
    }

    /// <summary>
    /// Finds all seable turfs on screen.
    /// </summary>
    /// <returns>x cord, y cord, x pixel, y pixel</returns>
    public List<Tuple<int, int, int, int>> getTurfsOnScreen(GameWindow window) {
        int W = window.ClientBounds.Width;
        int H = window.ClientBounds.Height;
        double xrange = getXrange();
        double yrange = getYrange();
        double mult = GLOB.getPixelMult(window, xrange, yrange);
        double h = Turf.side_len * mult;
        double w = Turf.side_len * mult;
        double dltx = client.viewer.eye.x * mult;
        double dlty = client.viewer.eye.y * mult;
        Turf loc = client.viewer.eye.GetTurf();
        int pos_x = (int) loc.cords[0];
        int pos_y = (int) loc.cords[1];
        int w_radius = (int) Math.Round(xrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;
        int h_radius = (int) Math.Round(yrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;

        List<Tuple<int, int, int, int>> ret = [];
        for (double x = -w_radius; x <= w_radius; ++x)
            for (double y = -h_radius; y <= h_radius; ++y) {
                int x0 = (int) (W / 2 + (x - 0.5) * w - dltx);
                int y0 = (int) (H / 2 + (y - 0.5) * h - dlty);
                int tx = (int) (pos_x + x);
                int ty = (int) (pos_y + y);
                ret.Add(new Tuple<int, int, int, int>(tx, ty, x0, y0));
            }

        return ret;
    }

    public override void onScreenSet()
    {
        base.onScreenSet();
        images.Add(client.viewer.eye.inventory);
    }

    public override void onScreenExit()
    {
        base.onScreenExit();
        images.Remove(client.viewer.eye.inventory);
    }
}
