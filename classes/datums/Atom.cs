using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Atom : Datum {
    static Dictionary<string, Texture2D> all_textures = new Dictionary<string, Texture2D>();
    public virtual string sprite_path => "sprites/other/Error";
    private List<Atom> content = []; // Atom, posx, posy
    private bool ShowContent = true;
    private Atom loc = null;
    public int H = 32;
    public int W = 32;
    public virtual int default_W => -1;
    public virtual int default_H => -1;
    public virtual float depth => 0;
    public double x = 0;
    public double y = 0;

    public virtual Func<Atom, double, double, object> move {get; set;} = (mover, dir, len) => {
        makeStep(mover, dir, len);
        return null;
    };

    public Atom() {
        Texture2D texture = getTexture();

        if (default_W != -1)
            W = default_W;
        else if (texture != null)
            W = texture.Width;

        if (default_H != -1)
            H = default_H;
        else if (texture != null)
            H = texture.Height;
    }

    public void LoadContent(ContentManager content) {
        Texture2D texture = content.Load<Texture2D>(sprite_path);
        all_textures[sprite_path] = texture;
    }

    public Texture2D getTexture() {
        all_textures.TryGetValue(sprite_path, out Texture2D texture);
        return texture;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Viewer viewer, double x, double y, double mult) {
        x -= this.x * mult;
        y -= this.y * mult;

        if (viewer == null || viewer.canSee(this))
            spriteBatch.Draw(getTexture(), new Rectangle((int) x, (int) y, (int) (W * mult + 1), (int) (H * mult + 1)), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, depth);

        if (!ShowContent)
            return;

        foreach (var A in content)
            A.Draw(gameTime, spriteBatch, viewer, x, y, mult);
    }

    public Turf getTurf() {
        return this is Turf ? (Turf)this : loc?.getTurf();
    }

    public void transfer(Atom target) {
        loc?.content.Remove(this);
        loc = target;
        loc?.content.Add(this);
    }

    public static void makeStep(Atom mover, double dir, double len) {
        if (!(mover.loc is Turf))
            return;
        
        double x = len * Math.Cos(dir);
        double y = len * Math.Sin(dir);
        mover.SendSignal(Signal.MOVE, mover, new Dictionary<string, object>{{"dx", x}, {"dy", y}});
        mover.x += x;
        mover.y += y;

        Turf loc = (Turf) mover.loc;
        while (mover.x >= loc.W) {
            Turf R = loc.Right();
            if (R != null) {
                mover.x -= loc.W;
                mover.transfer(R);
                loc = (Turf) mover.loc;
                continue;
            } else mover.x = loc.W - 1;
        }

        while (mover.x < 0) {
            Turf L = loc.Left();
            if (L != null) {
                mover.transfer(L);
                loc = (Turf) mover.loc;
                mover.x += loc.W;
                continue;
            } else mover.x = 0;
        }

        while (mover.y >= loc.H) {
            Turf U = loc.Up();
            if (U != null) {
                mover.y -= loc.H;
                mover.transfer(U);
                loc = (Turf) mover.loc;
                continue;
            } else mover.y = loc.H - 1;
        }

        while (mover.y < 0) {
            Turf D = loc.Down();
            if (D != null) {
                mover.transfer(D);
                loc = (Turf) mover.loc;
                mover.y += loc.H;
                continue;
            } else mover.y = 0;
        }
    }

    public virtual int getSpeed() {
        return 0;
    }

    public Atom getAtomOnPos(int x, int y, double mult) {
        if (x < 0 || y < 0 || x >= W || y >= H)
            return null;

        foreach (var A in content) {
            Atom found = A.getAtomOnPos((int) (x - A.x), (int) (y - A.y), mult);
            if (found != null)
                return found;
        }

        return this;
    }

    public virtual void onClick(Client clicker) {}

    public virtual void onUnclick(Client clicker) {}
}
