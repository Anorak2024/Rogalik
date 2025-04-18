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
    public virtual string Sprite_path => "sprites/other/Error";
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

    public virtual Func<Atom, double, double, object> Move {get; set;} = (mover, dir, len) => {
        MakeStep(mover, dir, len);
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
        if (Sprite_path == Atom.noTexture)
            return;

        Texture2D texture = content.Load<Texture2D>(Sprite_path);
        all_textures[Sprite_path] = texture;
    }

    public Texture2D getTexture() {
        Texture2D texture;
        if (overrided_sprite_path != null) {
            all_textures.TryGetValue(overrided_sprite_path, out texture);
            return texture;
        }

        if (Sprite_path == noTexture)
            return null;
        
        all_textures.TryGetValue(Sprite_path, out texture);
        return texture;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Client client, double x, double y, double mult) {
        x += this.x * mult;
        y += this.y * mult;
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

    public Turf GetTurf() {
        return this is Turf turf ? turf : loc?.GetTurf();
    }

    public Map GetMap() {
        Turf T = GetTurf();
        return T?.map;
    }

    public Atom GetPreTurf() {
        if (this is Turf)
            return null;
        
        return loc is Turf ? this : loc?.GetPreTurf();
    }

    public void Transfer(Atom target, bool random_pos = false) {
        loc?.content.Remove(this);
        loc = target;
        loc.content.Add(this);
        if (loc != null && random_pos) {
            x = (int) (GLOB.rand.NextInt64() % loc.W);
            y = (int) (GLOB.rand.NextInt64() % loc.H);
        }
    }

    public static void MakeStep(Atom mover, double dir, double len) {
        if (mover.loc is not Turf)
            return;
        
        len *= Turf.side_len;
        double x = len * Math.Cos(dir);
        double y = len * Math.Sin(dir);
        mover.SendSignal(Signal.MOVE, mover, new Dictionary<string, object>{{"dx", x}, {"dy", y}});
        mover.x -= x;
        mover.y -= y;

        Turf loc = (Turf) mover.loc;
        while (mover.x >= loc.W) {
            Turf R = loc.Right();
            if (R != null) {
                mover.x -= loc.W;
                mover.Transfer(R);
                loc = (Turf) mover.loc;
                continue;
            } else mover.x = loc.W - 1;
        }

        while (mover.x < 0) {
            Turf L = loc.Left();
            if (L != null) {
                mover.Transfer(L);
                loc = (Turf) mover.loc;
                mover.x += loc.W;
                continue;
            } else mover.x = 0;
        }

        while (mover.y >= loc.H) {
            Turf U = loc.Up();
            if (U != null) {
                mover.y -= loc.H;
                mover.Transfer(U);
                loc = (Turf) mover.loc;
                continue;
            } else mover.y = loc.H - 1;
        }

        while (mover.y < 0) {
            Turf D = loc.Down();
            if (D != null) {
                mover.Transfer(D);
                loc = (Turf) mover.loc;
                mover.y += loc.H;
                continue;
            } else mover.y = 0;
        }
    }

    public virtual double GetSpeed() {
        return 0;
    }

    public virtual Atom GetAtomOnPos(int x, int y, double mult, GameWindow window) {
        // For a situation in which the atom is almost entirely in the wrong tile.
        if (x < - Turf.side_len * mult || y < - Turf.side_len * mult || x >= Turf.side_len * mult * 2 || y >= Turf.side_len * mult * 2)
            return null;

        foreach (var A in content) {
            Atom found = A.GetAtomOnPos((int) (x - A.x * mult), (int) (y - A.y * mult), mult, window);
            if (found != null)
                return found;
        }

        if (x < 0 || y < 0 || x >= W * mult || y >= H * mult)
            return null;

        if (GLOB.GetPixel(getTexture(), 
                (int) (getTexture().Width / (double) W * (x / mult)), 
                (int) (getTexture().Height / (double) H * (y / mult))).A != 0)
            return this;
        else
            return null;
    }

    public virtual void OnClick(Client clicker) {}

    public virtual void OnUnclick(Client clicker) {}
}
