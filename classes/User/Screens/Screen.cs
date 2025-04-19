using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Screen {
    protected Client client;
    /// <summary>
    /// List of atoms that we can see on screen. (For example buttons.)
    /// (Exept of ingame elements like turfs.)
    /// </summary>
    public List<Image> images = [];

    public Screen(Client client) {
        this.client = client;
    }

    public virtual void onScreenSet() {

    }

    public virtual void onScreenExit() {
        
    }

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(GameTime gameTime, GameWindow window, SpriteBatch spriteBatch) {
        foreach (var image in client.cur_screen.images) {
            var (x0, y0, w, h) = image.getPos(window);
            image.Draw(gameTime, spriteBatch, client, x0, y0, image.getTexture() == null ? 1 : ((double) w / image.getTexture().Width));
        }
    }

    public virtual Atom GetAtomOnPos(GameWindow window, int x, int y) {
        foreach (var image in images) {
            var (x0, y0, w, h) = image.getPos(window);
            double mult = image.getTexture() == null ? 1 : ((double) w / image.getTexture().Width);
            int x1 = x0 + w;
            int y1 = y0 + h;
            if (x0 <= x && y0 <= y && x1 > x && y1 > y)
                return image.GetAtomOnPos(x, y, mult, window);
        }

        return null;
    }
}