using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Item : Atom {
    public override string sprite_path => "sprites/items/Stone";
    public override float depth => GLOB.DEPTH_ITEMS;
    public virtual int max_stack_items => 999;

    public override void onClick(Client clicker)
    {
        base.onClick(clicker);
        if (loc is Turf)
            tryPickUp(clicker.getControlled());
    }

    public void tryPickUp(Atom taker) {
        if (!(taker is Mob))
            return;
        
        if (!near(taker))
            return;

        if (((Mob) taker).inventory.tryPut(this))
            transfer(taker);
    }
}

public class Item_Stone : Item {

}