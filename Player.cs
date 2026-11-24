public class Player(Position position)
{
    public Position Position { get; } = position;
    private readonly Dictionary<Direction, Offset> DirectionMapping = new()
    {
        [Direction.Up] = new Offset(0, 1),
        [Direction.Down] = new Offset(0, -1),
        [Direction.Left] = new Offset(-1, 0),
        [Direction.Right] = new Offset(1, 0),
    };

    public void Move(Direction direction)
    {
        Position.Change(DirectionMapping[direction]);
    }
}
