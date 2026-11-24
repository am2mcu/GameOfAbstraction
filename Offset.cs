public class Offset(int dx, int dy)
{
    public int Dx { get; } = dx;
    public int Dy { get; } = dy;

    public override string ToString()
    {
        return $"dx {Dx} dy {Dy}";
    }
}
