public class Game
{
    private readonly Player _player = new(new Position(0, 0));
    private readonly List<Enemy> _enemies =
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
        while (!IsEnemiesCollide())
        {
            Display();

            LogEnemies();
            MoveEnemies();

            var direction = GetPlayerCommand();
            _player.Move(direction);
        }

        DisplayEndGame();
    }

    private void Display()
    {
        Console.WriteLine(_player.Position);
    }

    private void DisplayEndGame()
    {
        Console.WriteLine("You've been caught by enemy");
    }

    private Direction GetPlayerCommand()
    {
        var input = Console.ReadLine();
        var command = int.Parse(input ?? "");
        return CommandMapping[command];
    }

    private void LogEnemies()
    {
        foreach (var enemy in _enemies)
        {
            Console.WriteLine(enemy.Position);
        }
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.MoveTowards(_player.Position);
        }
    }

    private bool IsEnemiesCollide()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.Position.Equals(_player.Position))
                return true;
        }

        return false;
    }
}
