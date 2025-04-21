using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Input;
using MyGame2;

public class Subsystem_Input : Subsystem {
    protected int ScrollWheel;
    protected bool clicking = false;
    public override double max_time_part => 0.1;
    protected override List<Task> defaultProcesses => [new(IDGiver.NoID, CheckInput, [], 10)];
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

    protected void MouseWheel() {
        int newScrollWheel = Mouse.GetState().ScrollWheelValue;
        int dlt = newScrollWheel - ScrollWheel;
        if (dlt == 0)
            return;

        ScrollWheel = newScrollWheel;
        double old_val = (double) game.client.preferences.getPref(GLOB.PREF_MAP_SIZE_MULT);
        game.client.preferences.setPref(GLOB.PREF_MAP_SIZE_MULT, Math.Clamp(old_val - dlt / 2000.0, 0.1, 3));
    }

    protected void Throw(MouseState state) {
        double mult = GLOB.getPixelMult(game.Window, ((Screen_Map)game.client.cur_screen).getXrange(), ((Screen_Map)game.client.cur_screen).getYrange());
        Mob thrower = game.client.getViewer().eye;
        Item I = thrower?.GetSelectedSlot()?.TakeOne();
        if (I == null)
            return;

        if (thrower?.GetSelectedSlot().count == 0)
            game.client.viewer.SwitchThrow();

        thrower.Throw(I, 
            (state.X - game.Window.ClientBounds.Width / 2) / mult / Turf.side_len, 
            (state.Y - game.Window.ClientBounds.Height / 2) / mult / Turf.side_len
        );
    }

    protected void CheckClick()
    {
        var state = Mouse.GetState();
        bool clicking_now = state.LeftButton == ButtonState.Pressed;
        if (clicking == clicking_now)
            return;

        Viewer viewer = game.client.getViewer();
        if (viewer != null && viewer.GetThrowMod() != false && !clicking && clicking_now) {
            Throw(state);
            clicking = clicking_now;
            return;
        }

        Atom A = game.client.cur_screen.GetAtomOnPos(game.Window, state.X, state.Y);
        if (A == null) {
            clicking = clicking_now;
            return;    
        }
        
        if (!clicking && clicking_now)
            A.OnClick(game.client);
        else
            A.OnUnclick(game.client);

        clicking = clicking_now;
    }


    protected void CheckKeys() {
        Atom controlled = game.client.getControlled();
        if (controlled == null)
            return;

        int moveX = 0, moveY = 0;
        var pressed = Keyboard.GetState().GetPressedKeys();
        Viewer viewer = game.client.getViewer();
        if (pressed.Contains(Keys.LeftControl) || pressed.Contains(Keys.RightControl)) {
            CheckKeysCtrl();
            return;
        }

        foreach (var key in pressed) {
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_RIGHT)) 
                moveX++;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_LEFT)) 
                moveX--;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_UP)) 
                moveY++;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_KEY_MOVE_DOWN)) 
                moveY--;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_INVENTORY_OPEN) && !keyboardState.IsKeyDown(key))
                viewer.inv_open = !viewer.inv_open;
            if(key == (Keys)game.client.preferences.getPref(GLOB.PREF_THROW) && !keyboardState.IsKeyDown(key))
                viewer.SwitchThrow();
        }
        
        keyboardState = Keyboard.GetState();
        if (moveX == 0 && moveY == 0)
            return;

        controlled.Move(controlled, GLOB.get_dir(moveX, moveY), controlled.GetSpeed() * game.lastGameTime.ElapsedGameTime.TotalSeconds);
    }

    protected void CheckKeysCtrl() {
        Atom controlled = game.client.getControlled();
        var pressed = Keyboard.GetState().GetPressedKeys();
        Viewer viewer = game.client.getViewer();
        foreach (var key in pressed) {
            if (key == Keys.S && !keyboardState.IsKeyDown(key) && game.client.cur_screen is Screen_Map)
                GLOB.GameWorld.Save();
        }
    }
}