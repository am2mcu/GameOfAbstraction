public class Game
{
    private readonly Player _player = new(new Position(0, 0));
    private readonly List<Chaser> _chasers =
    [
        new Chaser(new Position(0, 6)),
        new Chaser(new Position(8, 6)),
    ];
    private readonly Dictionary<int, Direction> CommandMapping = new()
    {
        [8] = Direction.Up,
        [2] = Direction.Down,
        [4] = Direction.Left,
        [6] = Direction.Right,
    };

    public void Start()
    {
        while (true)
        {
            Display();

            LogEnemies();
            MoveEnemies();

            var direction = GetPlayerCommand();
            _player.Move(direction);
        }
    }

    private void Display()
    {
        Console.WriteLine(_player.Position);
    }

    private Direction GetPlayerCommand()
    {
        var input = Console.ReadLine();
        var command = int.Parse(input ?? "");
        return CommandMapping[command];
    }

    private void LogEnemies()
    {
        foreach (var chaser in _chasers)
        {
            Console.WriteLine(chaser.Position);
        }
    }

    private void MoveEnemies()
    {
        foreach (var chaser in _chasers)
        {
            chaser.MoveTowards(_player.Position);
        }
    }
}
