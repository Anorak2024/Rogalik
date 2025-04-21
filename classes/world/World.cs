using System.Collections.Generic;
using System.IO; 
using Microsoft.VisualBasic;

public class World {
    public Dictionary<long, Map> maps { get; } = [];
    public long SpawnWorldID = IDGiver.NoID;

    public World() {}

    public World(string data, Counter C) {
        SpawnWorldID = Decoder.DecodeLong(data, C);
        long len = Decoder.DecodeLong(data, C);
        for (int i = 0; i < len; ++i) {
            long key = Decoder.DecodeLong(data, C);
            //maps[key] = new Map(string data, Counter C);
        }
    }

    public string Encode() {
        string ret = "";
        ret += SpawnWorldID.ToString() + ";";
        ret += maps.Count + ";";
        foreach (var (key, value) in maps)
            ret += key + ":" + value.Encode();

        ret += ';';
        return ret;
    }

    public static string Encode(World world) {
        return world.Encode();
    }

    public static World Decode(string data) {
        return new World(data, new Counter());
    }

    public Atom GetSpawn() {
        return maps[SpawnWorldID].GetSpawn();
    }

    public void AddMap(Map map, long id) {
        maps[id] = map;
        if (SpawnWorldID == IDGiver.NoID)
            SpawnWorldID = id;
    }

    public void Save() {
        string timeString = "";
        foreach (char c in DateAndTime.TimeString)
            timeString += c == ':' ? '-' : c;

        string name = "autosave_" + DateAndTime.DateString + "_" + timeString;
        string path = "..\\..\\..\\Meta\\Saves\\" + name;
        File.WriteAllText(path, Encode());
    }
}