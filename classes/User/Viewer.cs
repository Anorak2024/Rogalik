using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Controller between Client and mob controlled by client.
/// </summary>
public class Viewer {
    public Client client;
    public Atom eye;
    public LazyEye lazyEye;
    /// <summary>
    /// Is atom can be controlled by viewer?
    /// </summary>
    public bool control = true;

    public Viewer(Atom eye, Client client) {
        this.eye = eye;
        this.client = client;
        lazyEye = new LazyEye(client.Window);
    }

    private void setEye(Atom newEye) {
        if (eye != null)
            eye.UnregisterSignal(Signal.MOVE, lazyEye.applyLazyEye);
        
        eye = newEye;
        eye.RegisterSignal(Signal.MOVE, lazyEye.applyLazyEye);
    }
    
    public Map getMap() {
        return eye?.getTurf().owner;
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
