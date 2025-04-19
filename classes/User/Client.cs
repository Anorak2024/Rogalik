using Microsoft.Xna.Framework;
using MyGame2;

/// <summary>
/// Info about client.
/// </summary>
public class Client {
    public Preferences preferences = new Preferences();
    public Screen cur_screen;
    public Viewer viewer;
    public GameWindow Window;
    public Game1 game;

    public Client(Game1 game) {
        this.game = game;
        Window = game.Window;
    }

    public Viewer getViewer() {
        return viewer;
    }

    public Atom getControlled() {
        if (viewer == null || !viewer.control)
            return null;
        
        return viewer.eye;
    }

    public void SetScreen(Screen screen) {
        cur_screen?.onScreenExit();
        cur_screen = screen;
        cur_screen.onScreenSet();
    }

    public void SetUpViewer(Mob eye) {
        viewer = new Viewer(this);
        viewer.SetEye(eye);
    }
}