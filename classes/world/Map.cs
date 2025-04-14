using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Map { 
    virtual public Turf getTurf(int x, int y) {
        return null;
    }

    virtual public Atom getSpawn() {
        return null;
    }

    public virtual Turf Up(Turf T) {
        return null;
    }

    public virtual Turf Down(Turf T) {
        return null;
    }

    public virtual Turf Left(Turf T) {
        return null;
    }

    public virtual Turf Right(Turf T) {
        return null;
    }

    public virtual List<Turf> onRange(Turf T, int range) {
        return [];
    }

    public virtual List<Turf> onScreen(Turf T, double xrange, double yrange) {
        return [];
    }
}

public class Map_Normal<TurfType> : Map where TurfType : Turf {
    Turf[][] turfs;
    bool cycled;

    public Map_Normal(int h, int w, bool cycled) {        
        this.cycled = cycled;
        turfs = new Turf[h][];
        for (int i = 0; i < h; ++i) {
            turfs[i] = new Turf[w];
            for (int j = 0; j < w; ++j) {
                if (GLOB.rand.NextInt64() % 2 == 0)
                    turfs[i][j] = (Turf) Activator.CreateInstance(typeof(TurfType), this, new object[]{j, i});
                else
                    turfs[i][j] = (Turf) Activator.CreateInstance(typeof(Turf_Error), this, new object[]{j, i});
            }
        }
    }

    public override Atom getSpawn() {
        return turfs[getH() / 2][getW() / 2];
    }

    public int getH() {
        return turfs.Count();
    }

    public int getW() {
        return turfs[0].Count();
    }

    public override Turf Up(Turf T)
    {
        return getTurf((int) T.cords[0], (int) T.cords[1] - 1);
    }

    public override Turf Down(Turf T)
    {
        return getTurf((int) T.cords[0], (int) T.cords[1] + 1);
    }

    public override Turf Right(Turf T)
    {
        return getTurf((int) T.cords[0] - 1, (int) T.cords[1]);
    }

    public override Turf Left(Turf T)
    {
        return getTurf((int) T.cords[0] + 1, (int) T.cords[1]);
    }

    public override Turf getTurf(int x, int y) {
        if (!cycled && (x < 0 || y < 0 || y > getH() || y > getW()))
            return null;

        int y1 = (y % getH() + getH()) % getH();
        int x1 = (x % getW() + getW()) % getW();
        return turfs[y1][x1];
    }

    public override List<Turf> onRange(Turf T, int range) {
        List<Turf> ret = [];
        int pos_x = (int) T.cords[0];
        int pos_y = (int) T.cords[1];
        for (double x = -range; x <= range; ++x)
            for (double y = -range; y <= range; ++y) {
                int tx = (int) (pos_x + x);
                int ty = (int) (pos_y + y);
                ret.Add(getTurf(tx, ty));
            }
        
        return ret;
    }

    public override List<Turf> onScreen(Turf T, double xrange, double yrange) {
        int w_radius = (int) Math.Round(xrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;
        int h_radius = (int) Math.Round(yrange / 2, 0, MidpointRounding.ToPositiveInfinity) + 1;
        List<Turf> ret = [];
        int pos_x = (int) T.cords[0];
        int pos_y = (int) T.cords[1];
        for (double x = -w_radius; x <= w_radius; ++x)
            for (double y = -h_radius; y <= h_radius; ++y) {
                int tx = (int) (pos_x + x);
                int ty = (int) (pos_y + y);
                ret.Add(getTurf(tx, ty));
            }

        return ret;
    }
}