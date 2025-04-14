using System;
using System.Collections.Generic;
using System.Linq;

public class Datum {
    /// <summary>
    /// defined signal_name + func started after getting signal. <br />
    /// func(sender, reciver, args[]) <br />
    ///     return null;
    /// </summary>
    protected Dictionary<Signal, List<Func<Datum, Datum, Dictionary<string, object>, object>>> registred_signals = [];
    
    protected virtual void Initialize() {

    }

    protected virtual void Destroy() {

    }

    public void RegisterSignal(Signal signal, Func<Datum, Datum, Dictionary<string, object>, object> func) {
        if(!registred_signals.ContainsKey(signal))
            registred_signals[signal] = [];
        
        registred_signals[signal].Add(func);
    }

    public void UnregisterSignal(Signal signal, Func<Datum, Datum, Dictionary<string, object>, object> func) {
        registred_signals[signal].Remove(func);
        if (registred_signals[signal].Count == 0)
            registred_signals.Remove(signal);
    }

    public void SendSignal(Signal signal, Datum sender, Dictionary<string, object> args) {
        if(!registred_signals.ContainsKey(signal))
            return;
        
        foreach (var func in registred_signals[signal])
            func(sender, this, args);
    }
}