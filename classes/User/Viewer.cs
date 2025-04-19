using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Controller between Client and mob controlled by client.
/// </summary>
public class Viewer {
    public Client client;
    public Mob eye;
    public LazyEye lazyEye;
    /// <summary>
    /// Is atom can be controlled by viewer?
    /// </summary>
    public bool control = true;
    public bool inv_open = false;
    public int inv_selected = 0;
    protected bool throw_mod = false;
    public List<Image> seeable = [];
    protected static ThrowImage throwImage = new(0.41, 0.02, 0.03, 0.03);

    public Viewer(Client client) {
        this.client = client;
        lazyEye = new LazyEye(client.Window);
    }

    public void SwitchThrow() {
        throw_mod = !throw_mod;
        if (throw_mod)
            seeable.Add(throwImage);
        else
            seeable.Remove(throwImage);
    }

    public bool GetThrowMod() {
        return throw_mod;
    }

    public void SetEye(Mob newEye) {
        if (eye != null) {
            eye.UnregisterSignal(Signal.MOVE, lazyEye.applyLazyEye);
            eye.viewer = null;
        }
        
        eye = newEye;
        eye.viewer = this;
        eye.RegisterSignal(Signal.MOVE, lazyEye.applyLazyEye);
    }
    
    public Map GetMap() {
        return eye?.GetTurf().map;
    }

    /// <summary>
    /// Can viewer see this atom?
    /// </summary>
    /// <param name="a">atom</param>
    /// <returns></returns>
    public bool canSee(Atom a) {
        return true;
    }
}
