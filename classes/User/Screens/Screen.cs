using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Screen {
    protected Client owner;
    /// <summary>
    /// List of atoms that we can see on screen. (For example buttons.)
    /// (Exept of ingame elements like turfs.)
    /// </summary>
    protected virtual List<Image> images => [];

    public Screen(Client client) {
        owner = client;
    }

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(GameTime gameTime, GameWindow window, SpriteBatch spriteBatch) {
        foreach (var image in images) {
            var (x0, y0, w, h) = image.getPos(window);
            image.Draw(gameTime, spriteBatch, null, x0, y0, (double) w / image.getTexture().Width);
        }
    }

    public virtual Atom getAtomOnPos(GameWindow window, int x, int y) {
        foreach (var image in images) {
            var (x0, y0, w, h) = image.getPos(window);
            int x1 = x0 + w;
            int y1 = y0 + h;
            if (x0 <= x && y0 <= y && x1 > x && y1 > y)
                return image;
        }

        return null;
    }
}