using System;
using System.Collections.Generic;
using MyGame2;

public class Subsystem_Throws : Subsystem
{
    public override double max_time_part => 0.05;
    public Subsystem_Throws(Game1 game1) : base(game1) {}
}


public class Throwing {
    public Atom thrown;
    /// <summary>
    /// The speed is given in tiles passed per second.
    /// </summary>
    public double xspeed, yspeed, just_speed;
    public double dist;
    public ID id;

    public Throwing(Atom thrown, double dx, double dy, double max_dist, double strength) {
        this.thrown = thrown;
        dist = Math.Min(Math.Sqrt(dx * dx + dy * dy), max_dist);
        double speed = dist / max_dist * Math.Sqrt(strength);
        xspeed = Math.Sqrt(speed * speed / (dy*dy / (dx * dx) + 1)) * (dx / Math.Abs(dx));
        yspeed = -xspeed * (dy / dx); // I dont know why "-", but it works (i hope), so i won't touch.
        just_speed = Math.Sqrt(xspeed * xspeed + yspeed * yspeed);
        id = IDGiver.get();
    }
}

/*
Хз где записать чтоб не потерять.
Вот наработки системы силы:
Есть кароче сила. От нее хз что будет зависить, но как минимум, броски.
Расстояние броска - сила/вес.
Макс скорость броска - Корень из силы.

*/