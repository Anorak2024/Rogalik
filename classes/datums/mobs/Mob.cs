using System;
using System.Collections.Generic;

public class Mob : Atom {
    public override string Sprite_path => "sprites/mobs/Human";
    public override float depth => GLOB.DEPTH_MOBS;
    protected override bool ShowContent => false;
    public Inventory inventory;
    public Viewer viewer;
    public List<Image> seeable = [];
    public virtual double DefaultStrength => 10;

    public Mob() {
        inventory = new(0.2, 0.2, 0.4, 0.4);
        seeable.Add(inventory);
    }

    public InventorySlot GetSelectedSlot() {
        return inventory.selected ?? null;
    }

    public override double GetSpeed() {
        return 4;
    }

    public virtual double GetStrength() {
        return DefaultStrength;
    }

    public void Throw(Item A, double dx, double dy) {
        A.Transfer(GetTurf());
        A.x = x;
        A.y = y;
        Throwing throwing = new(A, dx, dy, GetStrength() / A.GetWeight(), GetStrength());

        Func<Dictionary<string, object>, object> ProcessThrow = (Dictionary<string, object> args) => {
            Throwing throwing = (Throwing) args["throwing"];
            throwing.just_speed = Math.Min(throwing.just_speed, throwing.dist);
            throwing.thrown.Move(throwing.thrown, 
                GLOB.get_dir(throwing.xspeed, throwing.yspeed), throwing.just_speed);
            throwing.dist -= throwing.just_speed;
            if (throwing.dist <= 0.01)
                Subsystem.throws.RemoveProcess(throwing.id);

            return null;
        };

        Subsystem.throws.AddProcess(ProcessThrow, new Dictionary<string, object>(){{"throwing", throwing}}, 10, throwing.id);
    }
}