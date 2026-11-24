public abstract class Enemy(Position position)
{
    public Position Position { get; } = position;

    public abstract void MoveTowards(Position position);
}
