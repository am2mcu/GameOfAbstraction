public abstract class Enemy(string name, Position position)
{
    public string Name { get; } = name;
    public Position Position { get; } = position;

    // TODO: either receive a immutable Position e.g. PositionSnapshot or receive int value types e.g. int x, int y
    public abstract void MoveTowards(Position position);

    public bool IsCollided(IDistanceMethod distanceMethod, Position position)
    {
        var distance = Distance(distanceMethod, position);
        return distance <= 1;
    }

    public int Distance(IDistanceMethod distanceMethod, Position position)
    {
        return distanceMethod.Distance(Position, position);
    }
}
