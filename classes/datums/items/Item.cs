using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Item : Atom {
    public override string Sprite_path => "sprites/items/Stone";
    public override float depth => GLOB.DEPTH_ITEMS;
    public virtual int max_stack_items => 999;
    public virtual double DefaultWeight => 1;

    public override void OnClick(Client clicker)
    {
        base.OnClick(clicker);
        if (loc is Turf)
            TryPickUp(clicker.getControlled());
    }

    public void TryPickUp(Atom taker) {
        if (taker is not Mob)
            return;
        
        if (!GetMap().Near(this, taker))
            return;

        if (((Mob) taker).inventory.TryPut(this))
            Transfer(taker);
    }

    public double GetWeight() {
        return DefaultWeight;
    }
}

public class Item_Stone : Item {

}