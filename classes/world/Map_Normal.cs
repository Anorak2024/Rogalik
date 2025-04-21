using System;
using System.Collections.Generic;
using System.Linq;

public class Map_Normal : Map {
    public Turf[][] turfs {get; set;} = null;
    public int H {get; set;} = 0;
    public int W {get; set;} = 0;
    public bool cycled {get; set;} = true;

    public Map_Normal(int h, int w, bool cycled, Type TurfType) { 
        H = h;
        W = w;
        Generator.generate(this, new Dictionary<string, object>{
            {"type", GLOB.GEN_TYPE_DEBUG},
            {"h", h},
            {"w", w},
            {"cycled", cycled},
            {"TurfType", TurfType}
        });
    }

    public override Atom GetSpawn() {
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
        return getTurf((int) T.cords[0], (int) T.cords[1] + 1);
    }

    public override Turf Down(Turf T)
    {
        return getTurf((int) T.cords[0], (int) T.cords[1] - 1);
    }

    public override Turf Right(Turf T)
    {
        return getTurf((int) T.cords[0] + 1, (int) T.cords[1]);
    }

    public override Turf Left(Turf T)
    {
        return getTurf((int) T.cords[0] - 1, (int) T.cords[1]);
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

    public override bool Near(Atom A1, Atom A2) {
        double D = Dist(A1, A2);
        return D != noWay && D < 2;
    }

    private double Dist(Tuple<double, double> point1, Tuple<double, double> point2) {
        double dx = Math.Abs(point1.Item1 - point2.Item1);
        double dy = Math.Abs(point1.Item2 - point2.Item2);
        dx = Math.Min(dx, W - dx);
        dy = Math.Min(dy, H - dy);
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override double Dist(Atom A1, Atom A2) {
        Turf T1 = A1.GetTurf();
        Turf T2 = A2.GetTurf();
        if (T1 == null || T2 == null || T1.map != T2.map)
            return noWay;

        A1 = A1.GetPreTurf();
        A2 = A2.GetPreTurf();
        
        List<Tuple<double, double>> angles1, angles2;
        if (A1 == null)
            angles1 = [ new((double) (int) T1.cords[0],                             (double) (int) T1.cords[1]),
                        new((double) (int) T1.cords[0],                             (double) (int) T1.cords[1] + 1),
                        new((double) (int) T1.cords[0] + 1,                         (double) (int) T1.cords[1]),
                        new((double) (int) T1.cords[0] + 1,                         (double) (int) T1.cords[1] + 1),];
        else
            angles1 = [ new((double) (int) T1.cords[0] + A1.x / T1.W,               (double) (int) T1.cords[1] + A1.y / T1.H),
                        new((double) (int) T1.cords[0] + A1.x / T1.W,               (double) (int) T1.cords[1] + A1.y / T1.H + A1.H / T1.H),
                        new((double) (int) T1.cords[0] + A1.x / T1.W + A1.W / T1.H, (double) (int) T1.cords[1] + A1.y / T1.H),
                        new((double) (int) T1.cords[0] + A1.x / T1.W + A1.W / T1.H, (double) (int) T1.cords[1] + A1.y / T1.H + A1.H / T1.H),];
            

        if (A2 == null)
            angles2 = [ new((double) (int) T2.cords[0],                             (double) (int) T2.cords[1]),
                        new((double) (int) T2.cords[0],                             (double) (int) T2.cords[1] + 1),
                        new((double) (int) T2.cords[0] + 1,                         (double) (int) T2.cords[1]),
                        new((double) (int) T2.cords[0] + 1,                         (double) (int) T2.cords[1] + 1),];
        else
            angles2 = [ new((double) (int) T2.cords[0] + A2.x / T2.W,               (double) (int) T2.cords[1] + A2.y / T2.H),
                        new((double) (int) T2.cords[0] + A2.x / T2.W,               (double) (int) T2.cords[1] + A2.y / T2.H + A2.H / T2.H),
                        new((double) (int) T2.cords[0] + A2.x / T2.W + A2.W / T2.H, (double) (int) T2.cords[1] + A2.y / T2.H),
                        new((double) (int) T2.cords[0] + A2.x / T2.W + A2.W / T2.H, (double) (int) T2.cords[1] + A2.y / T2.H + A2.H / T2.H),];

        double D = Dist(angles1[0], angles2[0]);
        for (int i = 0; i < 4; ++i)
            for (int j = 0; j < 4; ++j)
                D = Math.Min(D, Dist(angles1[i], angles2[j]));

        return D;
    }

    public override string Encode() {
        string ret = "";
        ret += W + ";" + H + ";" + cycled + ";";
        return ret;
    }
}