using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MyGame2;

public class Subsystem_Input : Subsystem {
    private int ScrollWheel;
    private bool clicking = false;
    public override double max_time_part => 0.1;
    protected override List<Task> defaultProcesses => new(){new(null, CheckInput, [], 10)};
    protected KeyboardState keyboardState;

    public Subsystem_Input(Game1 game1) : base(game1) {
        ScrollWheel = Mouse.GetState().ScrollWheelValue;
        keyboardState = Keyboard.GetState();
    }

    public object CheckInput(Dictionary<string, object> args) {
        if (!game.IsActive) {
            ScrollWheel = Mouse.GetState().ScrollWheelValue;
            keyboardState = Keyboard.GetState();
            return null;
        }
        
        CheckKeys();
        MouseWheel();
        CheckClick();
        return null;
    }

    private void CheckClick()
    {
        var state = Mouse.GetState();
        bool clicking_now = state.LeftButton == ButtonState.Pressed;
        if (clicking == clicking_now)
            return;

        Atom A = game.client.cur_screen.getAtomOnPos(game.Window, state.X, state.Y);
        if (A == null)
            return;

        if (!clicking && clicking_now)
            A.onClick(game.client);
        else
            A.onUnclick(game.client);

        clicking = clicking_now;
    }


    public void CheckKeys() {
        Atom controlled = game.client.getControlled();
        if (controlled == null)
            return;

        int moveX = 0, moveY = 0;
        var pressed = Keyboard.GetState().GetPressedKeys();
        foreach (var key in pressed) {
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_RIGHT)) moveX++;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_LEFT)) moveX--;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_UP)) moveY++;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_DOWN)) moveY--;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_INVENTORY_OPEN) && !keyboardState.IsKeyDown(key)) {
                Viewer viewer = game.client.getViewer();
                viewer.inv_open = !viewer.inv_open;
            }
        }
        
        keyboardState = Keyboard.GetState();
        if (moveX == 0 && moveY == 0)
            return;

        controlled.move(controlled, GLOB.get_dir(moveX, moveY), controlled.getSpeed() * game.lastGameTime.ElapsedGameTime.TotalSeconds);
    }

    public void MouseWheel() {
        int newScrollWheel = Mouse.GetState().ScrollWheelValue;
        int dlt = newScrollWheel - ScrollWheel;
        if (dlt == 0)
            return;

        ScrollWheel = newScrollWheel;
        double old_val = (double) game.client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
        game.client.preferences.setPref(GLOB.PREF_MAP_SIZE_MULT, Math.Clamp(old_val - dlt / 2000.0, 0.1, 3));
    }
}