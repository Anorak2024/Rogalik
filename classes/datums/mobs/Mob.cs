using System;
using System.Collections.Generic;

public class Mob : Atom {
    public override string sprite_path => "sprites/mobs/Human";
    public override float depth => GLOB.DEPTH_MOBS;
    protected override bool ShowContent => false;
    public Inventory inventory;
    public List<Image> seeable = [];

    public Mob() {
        inventory = new(0.2, 0.2, 0.4, 0.4);
        seeable.Add(inventory);
    }

    public override int getSpeed() {
        return 320;
    }
}