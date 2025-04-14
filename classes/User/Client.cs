using Microsoft.Xna.Framework;
using MyGame2;

/// <summary>
/// Info about client.
/// </summary>
public class Client {
    public Preferences preferences = new Preferences();
    public Screen cur_screen;
    public GameWindow Window;
    public Game1 game;

    public Client(Game1 game) {
        this.game = game;
        Window = game.Window;
    }

    public Atom getControlled() {
        if (!(cur_screen is Screen_Map))
            return null;
        
        Screen_Map sv = (Screen_Map) cur_screen;
        Viewer viewer = sv.viewer;
        if (!viewer.control)
            return null;
        
        return viewer.eye;
    }
}