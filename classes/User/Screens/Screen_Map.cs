using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A type of screen with a bound Viewer. 
/// Displays a map around the Atom that owns the Viewer.
/// </summary>
public class Screen_Map : Screen {
    public Viewer viewer;
    /// <summary>
    /// Number of full plates on x and y axis.
    /// </summary>
    const int xrange0 = 16, yrange0 = 9;
    List<InventorySlot> SeeableInventory = [];

    public Screen_Map(Client client) : base(client) {
        Mob eye = new Human();
        eye.transfer(GLOB.spawn_map.getSpawn());
        viewer = new Viewer(eye, client);
        
        viewer.eye.RegisterSignal(Signal.MOVE, viewer.lazyEye.applyLazyEye);
    }

    private double getXrange() {
        return xrange0 * (double) viewer.client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
    }

    private double getYrange() {
        return yrange0 * (double) viewer.client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
    }

    public override void Draw(GameTime gameTime, GameWindow window, SpriteBatch spriteBatch) {
        base.Draw(gameTime, window, spriteBatch);
        var seeable = getTurfsOnScreen(window);
        Map map = viewer.getMap();
        double mult = GLOB.getPixelMult(window, getXrange(), getYrange());
        foreach (var (tx, ty, x0, y0) in seeable)
            map.getTurf(tx, ty)?.Draw(  gameTime, 
                                        spriteBatch, 
                                        client, 
                                        x0 + window.ClientBounds.Width * viewer.lazyEye.lazy_eye_x, 
                                        y0 + window.ClientBounds.Height * viewer.lazyEye.lazy_eye_y,
                                        mult);
                
        if (!(viewer.eye is Mob))
            return;

        foreach (var image in viewer.eye.seeable)
            image.Draw(gameTime, spriteBatch, client, 0, 0, 0);
    }

    public override void Update(GameTime gameTime) {
        
    }

    public override Atom getAtomOnPos(GameWindow window, int x, int y) {
        Atom image = base.getAtomOnPos(window, x, y);
        if (image != null)
            return image;
        
        var seeable = getTurfsOnScreen(window);
        Map map = viewer.getMap();
        double mult = GLOB.getPixelMult(window, getXrange(), getYrange());
        foreach (var (tx, ty, x0, y0) in seeable) {
            Atom found = map.getTurf(tx, ty)?.getAtomOnPos(x - x0, y - y0, mult, window);
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

        Map map = viewer.getMap();
        double dltx = viewer.eye.x * mult;
        double dlty = viewer.eye.y * mult;
        Turf loc = viewer.eye.getTurf();
        int pos_x = (int) loc.cords[0];
        int pos_y = (int) loc.cords[1];
        int w_radius = (int) Math.Round(xrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;
        int h_radius = (int) Math.Round(yrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;

        List<Tuple<int, int, int, int>> ret = [];
        for (double x = -w_radius; x <= w_radius; ++x)
            for (double y = -h_radius; y <= h_radius; ++y) {
                int x0 = (int) (W / 2 + (x - 0.5) * w + dltx);
                int y0 = (int) (H / 2 + (y - 0.5) * h + dlty);
                int tx = (int) (pos_x + x);
                int ty = (int) (pos_y + y);
                ret.Add(new Tuple<int, int, int, int>(tx, ty, x0, y0));
            }

        return ret;
    }

    public override void onScreenSet()
    {
        base.onScreenSet();
        images.Add(viewer.eye.inventory);
    }

    public override void onScreenExit()
    {
        base.onScreenExit();
        images.Remove(viewer.eye.inventory);
    }
}
