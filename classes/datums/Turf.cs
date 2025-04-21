public class Turf : Atom {
    public const int side_len = 64;
    public override float depth => GLOB.DEPTH_TURFS;
    public override int default_W => 64;
    public override int default_H => 64;
    public Map map {get; set;} = null;
    public object[] cords {get; set;} = null;

    public Turf() : base() {}

    public Turf(Map map, object[] cords) : base() {
        this.map = map;
        this.cords = cords;
    }

    public Turf Up() {
        return map.Up(this);
    }

    public Turf Down() {
        return map.Down(this);
    }

    public Turf Left() {
        return map.Left(this);
    }

    public Turf Right() {
        return map.Right(this);
    }
}

public class Turf_Grass : Turf {
    public override string Sprite_path => "sprites/turfs/Grass 1";

    public Turf_Grass() : base() {}

    public Turf_Grass(Map map, object[] cords) : base(map, cords) {}
}

public class Turf_Error : Turf {
    public override string Sprite_path => "sprites/other/Error";

    public Turf_Error() : base() {}

    public Turf_Error(Map map, object[] cords) : base(map, cords) {}
}