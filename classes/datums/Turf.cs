public class Turf : Atom {
    public const int side_len = 64;
    public override float depth => GLOB.DEPTH_TURFS;
    public override int default_W => 64;
    public override int default_H => 64;
    public Map owner;
    public object[] cords;

    public Turf() : base() {}

    public Turf(Map owner, object[] cords) : base() {
        this.owner = owner;
        this.cords = cords;
    }

    public Turf Up() {
        return owner.Up(this);
    }

    public Turf Down() {
        return owner.Down(this);
    }

    public Turf Left() {
        return owner.Left(this);
    }

    public Turf Right() {
        return owner.Right(this);
    }
}

public class Turf_Grass : Turf {
    public override string sprite_path => "sprites/turfs/Grass 1";

    public Turf_Grass() : base() {}

    public Turf_Grass(Map owner, object[] cords) : base(owner, cords) {}
}

public class Turf_Error : Turf {
    public override string sprite_path => "sprites/other/Error";

    public Turf_Error() : base() {}

    public Turf_Error(Map owner, object[] cords) : base(owner, cords) {}
}