using System;

public static partial class GLOB
{
    public static double DIR_RIGHT = Math.PI *      0 / 4;
    public static double DIR_RIGHT_UP = Math.PI *   1 / 4;
    public static double DIR_UP = Math.PI *         2 / 4;
    public static double DIR_UP_LEFT = Math.PI *    3 / 4;
    public static double DIR_LEFT = Math.PI *       4 / 4;
    public static double DIR_LEFT_DOWN = Math.PI *  5 / 4;
    public static double DIR_DOWN = Math.PI *       6 / 4;
    public static double DIR_DOWN_RIGHT = Math.PI * 7 / 4;

    public static double get_dir(int x, int y) {
        return Math.Atan2(y, -x);
    }
}