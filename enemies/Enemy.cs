public abstract class Enemy(Position position)
{
    public Position Position { get; } = position;

    public abstract void MoveTowards(Position position);

    public bool IsCollided(Player player)
    {
        var offset = Position.Offset(player.Position);

        if (offset.Dx == 0 && offset.Dy == 0)
            return true;

        if ((offset.Dx == 0 && offset.Dy == 1) || (offset.Dx == 1 && offset.Dy == 0))
            return true;

        return false;
    }
}
