public class Wanderer(Position position) : Enemy("Wanderer", position)
{
    private readonly int _moveRange = 1;

    public override void MoveTowards(Position position)
    {
        int dx = Random.Shared.Next(-_moveRange, _moveRange + 1);
        int dy = Random.Shared.Next(-_moveRange, _moveRange + 1);

        Position.ChangeByOffset(new Offset(dx, dy));
    }
}
