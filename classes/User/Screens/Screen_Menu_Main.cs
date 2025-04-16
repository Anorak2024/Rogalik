using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame2;

public class Screen_Menu_Main : Screen
{
    public Screen_Menu_Main(Client client) : base(client) {
        Image b1 = new ButtonContinue(0.2, 0.2, 0.3, 0.1);
        Image b2 = new ButtonNewGame(0.2, 0.3, 0.3, 0.1);
        Image b3 = new ButtonLoad(0.2, 0.4, 0.3, 0.1);
        Image b4 = new ButtonOnline(0.2, 0.5, 0.3, 0.1);
        Image b5 = new ButtonSettings(0.2, 0.6, 0.3, 0.1);
        Image b6 = new ButtonAbout(0.2, 0.7, 0.3, 0.1);
        Image b7 = new ButtonExit(0.2, 0.8, 0.3, 0.1);
        List<Image> imgs = [b1, b2, b3, b4, b5, b6, b7];
        foreach (var img in imgs)
            images.Add(img);
    }
}

public class ButtonTexted : Image {
    public ButtonTexted(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonTexted() : base() {}
    protected virtual string imageText => "";
    public override string sprite_path => "sprites/buttons/Button 1";

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Client client, double x, double y, double mult)
    {
        base.Draw(gameTime, spriteBatch, client, x, y, mult);
        spriteBatch.DrawString(Game1.arial, imageText, new Vector2(
            (int) (x + (W * mult - Game1.arial.MeasureString(imageText).X * mult) / 2), 
            (int) (y + (H * mult - Game1.arial.MeasureString(imageText).Y * mult) / 2)), 
            Color.Black, 0, new Vector2(0, 0), (float) mult, SpriteEffects.None, GLOB.DEPTH_TOP);
    }
}

public class ButtonContinue : ButtonTexted {
    public ButtonContinue(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonContinue() : base() {}
    protected override string imageText => "Continue";
    
    public override void onClick(Client clicker) {}
}

public class ButtonNewGame : ButtonTexted {
    public ButtonNewGame(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonNewGame() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "New Game";
    
    public override void onClick(Client clicker) {
        clicker.setScreen(new Screen_Map(clicker));
    }
}

public class ButtonLoad : ButtonTexted {
    public ButtonLoad(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonLoad() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "Load Game";
    
    public override void onClick(Client clicker) {}
}

public class ButtonOnline : ButtonTexted {
    public ButtonOnline(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonOnline() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "Play Online";
    
    public override void onClick(Client clicker) {}
}

public class ButtonSettings : ButtonTexted {
    public ButtonSettings(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonSettings() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "Settings";
    
    public override void onClick(Client clicker) {}
}

public class ButtonAbout : ButtonTexted {
    public ButtonAbout(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonAbout() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "About";
    
    public override void onClick(Client clicker) {}
}

public class ButtonExit : ButtonTexted {
    public ButtonExit(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public ButtonExit() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    protected override string imageText => "Exit";
    
    public override void onClick(Client clicker) {
        clicker.game.Exit();
    }
}
/*
public class MainMenuWall : Image {
    public MainMenuWall(double center_x, double center_y, double w_part, double h_part) : base(center_x, center_y, w_part, h_part) {}
    public MainMenuWall() : base() {}
    
    
}*/