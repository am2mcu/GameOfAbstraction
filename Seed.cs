public class Seed(Position position)
{
    public Position Position { get; } = position;

    public bool CanBeEaten(Player player)
    {
        return Position.Equals(player.Position);
    }

    public void Move(Offset offset)
    {
        Position.Change(offset);
    }
}
