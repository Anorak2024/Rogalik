using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public class Preferences {
    Dictionary<string, object> prefs = new Dictionary<string, object>() {
        {GLOB.PREF_KEY_MOVE_RIGHT, Keys.D},
        {GLOB.PREF_KEY_MOVE_LEFT, Keys.A},
        {GLOB.PREF_KEY_MOVE_UP, Keys.W},
        {GLOB.PREF_KEY_MOVE_DOWN, Keys.S},
        {GLOB.PREF_MAP_SIZE_MULT, 1.0},
        {GLOB.PREF_INVENTORY_OPEN, Keys.E},
        {GLOB.PREF_THROW, Keys.R},
    };

    public void setPref(string pref, object new_val) {
        prefs[pref] = new_val;
    }

    public object getPref(string pref) {
        return prefs[pref];
    }
}