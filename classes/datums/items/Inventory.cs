using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame2;

public class Inventory : Image {
    public virtual int size => 50;
    public InventorySlot[] slots;
    const int noSlots = -1;
    public InventorySlot selected = null;
    public override string sprite_path => Atom.noTexture;

    public Inventory() {}

    public Inventory(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {
        w_dev_h = 2;
        slots = new InventorySlot[size];
        double left_x = center_x - w_part / 2;
        double top_y = center_y - h_part / 2;
        double slot_wpart = w_part / 10;
        double slot_hpart = h_part / ((size + 9) / 10);
        for (int i = 0; i * 10 < size; ++i)
            for (int j = 0; j < Math.Min(size - i * 10, 10); ++j) {
                slots[i * 10 + j] = new InventorySlot(left_x + slot_wpart * (j + 0.5), top_y + slot_hpart * (i + 0.5), slot_wpart, slot_hpart, i * 10 + j);
                slots[i * 10 + j].transfer(this);
            }
    }

    private int getFirstGoodSlot(Item I) {
        for (int i = 0; i < size; ++i)
            if (slots[i].getItem()?.GetType() == I.GetType())
                return i;

        for (int i = 0; i < size; ++i)
            if (slots[i].getItem() == null)
                return i;

        return noSlots;
    }

    public bool tryPut(Item I) {
        int slot = getFirstGoodSlot(I);
        if (slot == noSlots)
            return false;

        slots[slot].content = [I];
        slots[slot].count++;
        return true;
    }
}

public class InventorySlot : Image {
    public override string sprite_path => "sprites/other/Inventory_slot";
    public int count = 0;
    public int ind;
    protected override bool ShowContent => false;

    public Item getItem() {
        return content.Count > 0 ? (Item) content[0] : null;
    }

    public override void onClick(Client clicker)
    {
        Inventory inventory = clicker.getViewer().eye.inventory;
        if (inventory.selected != null)
            inventory.selected.overrided_sprite_path = null;
            
        inventory.selected = this;
        inventory.selected.overrided_sprite_path = "sprites/other/Inventory_slot_selected";
    }
    
    public InventorySlot() {}

    public InventorySlot(double center_x, double center_y, double w_part, double h_part, int index) : base(center_x, center_y, w_part, h_part) {
        ind = index;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Client client, double x, double y, double mult)
    {
        Viewer viewer = client.getViewer();
        if (!(viewer.inv_open || ind < 10))
            return;
        
        base.Draw(gameTime, spriteBatch, client, x, y, mult);
        var (x0, y0, w, h) = getPos(client.game.Window);
        Texture2D texture = getItem()?.getTexture();
        if (texture == null)
            return;

        spriteBatch.Draw(getItem().getTexture(), new Rectangle(
                x0, y0, w, h),
                null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, GLOB.DEPTH_IMAGES_L2);

        if (count > 1) {
            string imageText = count.ToString();
            spriteBatch.DrawString(Game1.arial, imageText, new Vector2(
                (int) (x0 + w * 0.95 - Game1.arial.MeasureString(imageText).X), 
                (int) (y0 + h * 0.95 - Game1.arial.MeasureString(imageText).Y)), 
                Color.White, 0, new Vector2(0, 0), (float) mult, SpriteEffects.None, GLOB.DEPTH_IMAGES_L3);
        }
    }
}

