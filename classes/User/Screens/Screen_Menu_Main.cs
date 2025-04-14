using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Screen_Menu_Main : Screen
{
    public Screen_Menu_Main(Client client) : base(client) {}

    protected override List<Image> images => [Continue, NewGame, Load, Online, Settings, About, Exit];
    public static Image Continue = new ButtonContinue(0.2, 0.2, 0.1, 0.3);
    public static Image NewGame = new ButtonNewGame(0.2, 0.3, 0.1, 0.3);
    public static Image Load = new ButtonLoad(0.2, 0.4, 0.1, 0.3);
    public static Image Online = new ButtonOnline(0.2, 0.5, 0.1, 0.3);
    public static Image Settings = new ButtonSettings(0.2, 0.6, 0.1, 0.3); 
    public static Image About = new ButtonAbout(0.2, 0.7, 0.1, 0.3);
    // public static Image GiveMeYourMoney; // :3
    public static Image Exit = new ButtonExit(0.2, 0.8, 0.1, 0.3);

    //public static Image MenuWall = new MainMenuWall();
}

public class ButtonContinue : Image {
    public ButtonContinue(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonContinue() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {}
}

public class ButtonNewGame : Image {
    public ButtonNewGame(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonNewGame() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {
        clicker.cur_screen = new Screen_Map(clicker);;
    }
}

public class ButtonLoad : Image {
    public ButtonLoad(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonLoad() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {}
}

public class ButtonOnline : Image {
    public ButtonOnline(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonOnline() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {}
}

public class ButtonSettings : Image {
    public ButtonSettings(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonSettings() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {}
}

public class ButtonAbout : Image {
    public ButtonAbout(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonAbout() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {}
}

public class ButtonExit : Image {
    public ButtonExit(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public ButtonExit() : base() {}
    public override string sprite_path => "sprites/buttons/Button 1";
    
    public override void onClick(Client clicker) {
        clicker.game.Exit();
    }
}
/*
public class MainMenuWall : Image {
    public MainMenuWall(double center_x, double center_y, double h_part, double w_part) : base(center_x, center_y, h_part, w_part) {}
    public MainMenuWall() : base() {}
    
    
}*/