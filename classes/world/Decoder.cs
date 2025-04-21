using System.Linq;

public static class Decoder {
    public static long DecodeLong(string s, Counter C) {
        long ret = 0;
        while (C.v < s.Length && s[C.v] >= '0' && s[C.v] <= '9') {
            ret = ret * 10 + s[C.v] - '0';
            C.v++;
        }

        C.v++;
        return ret;
    }
}

public class Counter {
    public int v = 0;
}