using System;

public class Mob : Atom {
    public override string sprite_path => "sprites/mobs/Human";
    public override float depth => GLOB.DEPTH_MOBS;

    public override int getSpeed() {
        return 320;
    }
}