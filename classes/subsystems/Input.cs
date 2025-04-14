using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MyGame2;

public class Subsystem_Input : Subsystem {
    private int ScrollWheel;
    private bool clicking = false;
    public override double max_time_part => 0.1;
    protected override List<Task> defaultProcesses => new(){new(null, CheckInput, [], 10)};

    public Subsystem_Input(Game1 game1) : base(game1) {
        ScrollWheel = Mouse.GetState().ScrollWheelValue;
    }

    public object CheckInput(Dictionary<string, object> args) {
        CheckMovement();
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


    public void CheckMovement() {
        Atom controlled = game.client.getControlled();
        if (controlled == null)
            return;

        int moveX = 0, moveY = 0;
        var pressed = Keyboard.GetState().GetPressedKeys();
        foreach (var key in pressed) {
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_RIGHT)) moveX++;
            else if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_LEFT)) moveX--;
            else if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_UP)) moveY++;
            else if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_DOWN)) moveY--;
        }
        
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