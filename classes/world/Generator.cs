using System;
using System.Collections.Generic;

public static class Generator {
    public static void generate(Map map, Dictionary<string, object> args) {
        if ((string) args["type"] == GLOB.GEN_TYPE_DEBUG) {
            genCursedDebug((Map_Normal) map, args);
            return;
        }
    }

    public static void genCursedDebug(Map_Normal map, Dictionary<string, object> args) {
        map.cycled = (bool) args["cycled"];
        Type TurfType = (Type) args["TurfType"];
        var h = (int) args["h"];
        var w = (int) args["w"];
        map.turfs = new Turf[h][];
        for (int i = 0; i < h; ++i) {
            map.turfs[i] = new Turf[w];
            for (int j = 0; j < w; ++j) {
                if (GLOB.rand.NextInt64() % 2 == 0)
                    map.turfs[i][j] = (Turf) Activator.CreateInstance(TurfType, map, new object[]{j, i});
                else
                    map.turfs[i][j] = (Turf) Activator.CreateInstance(typeof(Turf_Error), map, new object[]{j, i});

                if (GLOB.rand.NextInt64() % 5 == 0) {
                    Item stone = new Item_Stone();
                    stone.Transfer(map.turfs[i][j], true);
                }
            }
        }
    }
}