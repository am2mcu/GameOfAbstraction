public class Chaser(Position position) : Enemy(position)
{
    public override void MoveTowards(Position position)
    {
        int dx = Math.Abs(Position.X - position.X);
        int dy = Math.Abs(Position.Y - position.Y);

        if (dx >= dy)
        {
            if (Position.X >= position.X)
                Position.ChangeByOffset(new Offset(-1, 0));
            else
                Position.ChangeByOffset(new Offset(1, 0));
        }
        else
        {
            if (Position.Y >= position.Y)
                Position.ChangeByOffset(new Offset(0, -1));
            else
                Position.ChangeByOffset(new Offset(0, 1));
        }
    }
}
