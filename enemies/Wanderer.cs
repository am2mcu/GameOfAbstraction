public class Wanderer(Position position, List<Direction> directionsPattern)
    : Enemy("Wanderer", position)
{
    private readonly List<Direction> _directionsPattern = directionsPattern;

    private int _currentDirectionIndex = 0;
    private readonly Dictionary<Direction, Offset> DirectionMapping = new()
    {
        [Direction.Up] = new Offset(0, 1),
        [Direction.Down] = new Offset(0, -1),
        [Direction.Left] = new Offset(-1, 0),
        [Direction.Right] = new Offset(1, 0),
    };

    public override void MoveTowards(Position position)
    {
        var offset = DirectionMapping[CurrentDirection()];
        Position.ChangeByOffset(offset);
    }

    private Direction CurrentDirection()
    {
        if (_currentDirectionIndex >= _directionsPattern.Count)
            _currentDirectionIndex = 0;
        return _directionsPattern[_currentDirectionIndex++];
    }
}
