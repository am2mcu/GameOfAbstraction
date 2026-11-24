public class Chaser(Position position)
{
    public Position Position { get; } = position;

    public void MoveTowards(Position position)
    {
        int dx = Math.Abs(Position.X - position.X);
        int dy = Math.Abs(Position.Y - position.Y);

        if (dx >= dy)
        {
            if (Position.X >= position.X)
                Position.Change(new Offset(-1, 0));
            else
                Position.Change(new Offset(1, 0));
        }
        else
        {
            if (Position.Y >= position.Y)
                Position.Change(new Offset(0, -1));
            else
                Position.Change(new Offset(0, 1));
        }
    }
}
