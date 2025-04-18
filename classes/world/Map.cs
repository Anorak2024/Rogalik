using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Map {
    public const double noWay = -1;

    public void generate(Dictionary<string, object> args) {
        Generator.generate(this, args);
    }

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

    public virtual bool Near(Atom A1, Atom A2) {
        Turf T1 = A1.GetTurf();
        Turf T2 = A2.GetTurf();
        return T1 == T2;
    }

    public virtual double Dist(Atom A1, Atom A2) {
        return noWay;
    }
}