using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using MyGame2;

public class Subsystem {
    protected Game1 game;
    /// <summary>
    /// Max part of time used for Update, that can be spend on this subsystem.
    /// </summary>
    public virtual double max_time_part => 0.1;
    protected virtual List<Task> defaultProcesses => [];

    /// <summary>
    /// Tasks for this subsystem.
    /// </summary>
    PriorityQueue<Task, long> tasks = new PriorityQueue<Task, long>();
    HashSet<ID> should_del = [];

    public Subsystem(Game1 game1) {
        game = game1;
        foreach (Task t in defaultProcesses)
            AddProcess(t);
    }
    
    public ID AddTask(Func<Dictionary<string, object>, object> func, Dictionary<string, object> args, long after, ID id = null) {
        if (id == null)
            id = IDGiver.get();
        
        tasks.Enqueue(new Task(id, func, args, Task.no_recall), GLOB.getMilliseconds() + after);
        return id;
    }

    public ID AddProcess(Task task) {
        return AddProcess(task.func, task.args, task.recall, task.id);
    }

    public ID AddProcess(Func<Dictionary<string, object>, object> func, Dictionary<string, object> args, long period, ID id = null) {
        if (id == null)
            id = IDGiver.get();

        tasks.Enqueue(new Task(id, func, args, period), GLOB.getMilliseconds() + period);
        return id;
    }

    public void RemoveTask(ID id) {
        should_del.Add(id);
    }

    public void RemoveProcess(ID id) {
        RemoveTask(id);
    }

    public void doTasks(long end_time) {
        while (tasks.TryPeek(out Task task, out long priority))
        {
            if (priority > end_time || GLOB.getMilliseconds() < priority)
                break;

            tasks.Dequeue();
            if (should_del.Contains(task.id)) {
                should_del.Remove(task.id);
                continue;
            }
            
            task.func(task.args);
            if (task.recall != Task.no_recall)
                AddProcess(task.func, task.args, task.recall, task.id); // Same ID
        }
    }

    public static void InitSubsystems(Game1 game) {
        input_reader =  new Subsystem_Input(game);
        visual =        new Subsystem_Visual(game);
        throws =        new Subsystem_Throws(game);

        game.AddSubsystem(input_reader);
        game.AddSubsystem(visual);
        game.AddSubsystem(throws);
    }

    public static Subsystem_Input input_reader;
    public static Subsystem_Visual visual;
    public static Subsystem_Throws throws;
}

public class Task {
    public ID id;
    public Func<Dictionary<string, object>, object> func;
    public Dictionary<string, object> args;
    // Time befor recall. If no recall, -1.
    public long recall = no_recall;

    public const long no_recall = -1;

    public Task(ID id, Func<Dictionary<string, object>, object> func, Dictionary<string, object> args, long recall) {
        if (id == null)
            id = IDGiver.get();

        this.id = id;
        this.func = func;
        this.args = args;
        this.recall = recall;
    }
}