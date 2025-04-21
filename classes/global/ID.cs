using System.Text.Json.Serialization;
using System.Text.Json;


public static class IDGiver {
    static long lastID = 0;
    public const int NoID = -1;

    public static long get() {
        return lastID++;
    }
}
