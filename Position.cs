public class Position(int x, int y)
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;

    public void Change(Offset offset)
    {
        X += offset.Dx;
        Y += offset.Dy;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
