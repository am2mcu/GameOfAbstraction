public class Ghost(Position position) : Enemy("Ghost", position)
{
    public override void MoveTowards(Position position)
    {
        Position.ChangeByOffset(
            new Offset(Position.X >= position.X ? -1 : 1, Position.Y >= position.Y ? -1 : 1)
        );
    }
}
