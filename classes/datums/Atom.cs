using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Atom : Datum {
    public const string noTexture = "notexture";
    public static string[] unused_load = {
        "sprites/other/Inventory_slot_selected",
    };
    public static Dictionary<string, Texture2D> all_textures = [];
    public virtual string sprite_path => "sprites/other/Error";
    public string overrided_sprite_path = null;
    public List<Atom> content = []; // Atom, posx, posy
    protected virtual bool ShowContent => true;
    protected Atom loc = null;
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
        if (sprite_path == Atom.noTexture)
            return;

        Texture2D texture = content.Load<Texture2D>(sprite_path);
        all_textures[sprite_path] = texture;
    }

    public Texture2D getTexture() {
        Texture2D texture;
        if (overrided_sprite_path != null) {
            all_textures.TryGetValue(overrided_sprite_path, out texture);
            return texture;
        }

        if (sprite_path == noTexture)
            return null;
        
        all_textures.TryGetValue(sprite_path, out texture);
        return texture;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Client client, double x, double y, double mult) {
        x -= this.x * mult;
        y -= this.y * mult;
        Viewer viewer = client.getViewer();

        if (getTexture() != null && (viewer == null || viewer.canSee(this)))
            spriteBatch.Draw(getTexture(), new Rectangle(
                (int) Math.Round(x, 0, MidpointRounding.ToNegativeInfinity), 
                (int) Math.Round(y, 0, MidpointRounding.ToNegativeInfinity), 
                (int) Math.Round(W * mult, 0, MidpointRounding.ToNegativeInfinity) + 1, 
                (int) Math.Round(H * mult, 0, MidpointRounding.ToNegativeInfinity) + 1),
                null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, depth);

        if (!ShowContent)
            return;

        foreach (var A in content)
            A.Draw(gameTime, spriteBatch, client, x, y, mult);
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

    public virtual Atom getAtomOnPos(int x, int y, double mult, GameWindow window) {
        if (x < 0 || y < 0 || x >= W * mult || y >= H * mult)
            return null;

        foreach (var A in content) {
            Atom found = A.getAtomOnPos((int) (x - A.x * mult), (int) (y - A.y * mult), mult, window);
            if (found != null)
                return found;
        }

        if (GLOB.GetPixel(getTexture(), 
                (int) (getTexture().Width / (double) W * (x / mult)), 
                (int) (getTexture().Height / (double) H * (y / mult))).A != 0)
            return this;
        else
            return null;
    }

    public virtual void onClick(Client clicker) {}

    public virtual void onUnclick(Client clicker) {}

    public bool near(Atom a) {
        Turf t1 = getTurf();
        Turf t2 = a.getTurf();
        Map_Normal map = (Map_Normal)t1.owner;
        if (t1.owner != t2.owner)
            return false;
        
        int x1 = (int) t1.cords[0];
        int x2 = (int) t2.cords[0];
        int y1 = (int) t1.cords[1];
        int y2 = (int) t2.cords[1];
        int dx = Math.Abs(x1 - x2);
        int dy = Math.Abs(y1 - y2);
        return  Math.Min(dx, map.W - dx) + 
                Math.Min(dy, map.H - dy) < 3;
    }
}
