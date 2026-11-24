public class Position(int x, int y)
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;

    public void ChangeByOffset(Offset offset)
    {
        X += offset.Dx;
        Y += offset.Dy;
    }

    public Offset Offset(Position position)
    {
        return new Offset(Math.Abs(X - position.X), Math.Abs(Y - position.Y));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Position position)
            return false;

        return X == position.X && Y == position.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
