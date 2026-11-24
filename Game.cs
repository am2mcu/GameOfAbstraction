public class Game(IDistanceMethod distanceMethod, int gridSize = 16)
{
    public int GridSize { get; } = gridSize;
    private IDistanceMethod _distanceMethod = distanceMethod;
    private readonly Player _player = new(new Position(0, 0));
    private readonly Seed _seed = new(new Position(1, 1));
    private readonly List<Enemy> _enemies =
    [
        new Chaser(new Position(6, 0)),
        new Chaser(new Position(6, 8)),
        new Ghost(new Position(16, 16)),
        new Wanderer(new Position(14, 0)),
    ];
    private readonly Dictionary<int, Direction> CommandMapping = new()
    {
        [8] = Direction.Up,
        [2] = Direction.Down,
        [4] = Direction.Left,
        [6] = Direction.Right,
    };

    private readonly Dictionary<Direction, char> ArrowMapping = new()
    {
        [Direction.Up] = '^',
        [Direction.Down] = 'v',
        [Direction.Left] = '<',
        [Direction.Right] = '>',
    };

    public void Start()
    {
        Collision collision;
        do
        {
            DisplayGame();

            LogEnemies();
            MoveEnemies();

            var direction = GetPlayerCommand();
            _player.Move(direction);

            EatSeed();
            collision = IsEnemiesCollide();
        } while (!collision.IsCollided);

        DisplayEndGame(collision);
    }

    private void DisplayGame()
    {
        DisplayPlayerPosition();
        DisplayArrows();
        DisplaySeedOffset();
    }

    private void DisplayPlayerPosition()
    {
        Console.WriteLine($"position {_player.Position}");
    }

    private void DisplayArrows()
    {
        Dictionary<Direction, int> arrowValues = new()
        {
            [Direction.Up] = int.MaxValue,
            [Direction.Down] = int.MaxValue,
            [Direction.Left] = int.MaxValue,
            [Direction.Right] = int.MaxValue,
        };

        foreach (var enemy in _enemies)
        {
            int dx = enemy.Position.X - _player.Position.X;
            int dy = enemy.Position.Y - _player.Position.Y;

            var distance = enemy.Distance(_distanceMethod, _player.Position);

            if (dy > 0)
                arrowValues[Direction.Up] = Math.Min(arrowValues[Direction.Up], distance);
            if (dy < 0)
                arrowValues[Direction.Down] = Math.Min(arrowValues[Direction.Down], distance);
            if (dx < 0)
                arrowValues[Direction.Left] = Math.Min(arrowValues[Direction.Left], distance);
            if (dx > 0)
                arrowValues[Direction.Right] = Math.Min(arrowValues[Direction.Right], distance);
        }

        var farValues = arrowValues.Where(arrow => arrow.Value >= 10).ToList();
        int yellow = int.MaxValue;
        if (farValues.Count != 0)
            yellow = farValues.MinBy(arrow => arrow.Value).Value;
        foreach (var arrow in arrowValues)
        {
            var message = $"{ArrowMapping[arrow.Key]} ";
            if (arrow.Value < 10)
                ConsoleWriteColored(message, ConsoleColor.Red);
            else if (arrow.Value == yellow && arrow.Value != int.MaxValue)
                ConsoleWriteColored(message, ConsoleColor.Yellow);
            else
                ConsoleWriteColored(message, ConsoleColor.Green);
        }

        Console.WriteLine();
    }

    private void DisplaySeedOffset()
    {
        Console.WriteLine($"seed {_seed.Offset(_player)}");
    }

    private void ConsoleWriteColored(string message, ConsoleColor color)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = previous;
    }

    private void DisplayEndGame(Collision collision)
    {
        Console.WriteLine($"You've been caught by a {collision.Enemy?.Name}");
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

    private Collision IsEnemiesCollide()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsCollided(_distanceMethod, _player.Position))
                return new Collision(true, enemy);
        }

        return new Collision(false, null);
    }

    private void EatSeed()
    {
        if (_seed.CanBeEaten(_player))
        {
            Console.WriteLine("You ate a seed 🎉");
            MoveSeed();
        }
    }

    private void MoveSeed()
    {
        do
        {
            int x = Random.Shared.Next(-GridSize, GridSize);
            int y = Random.Shared.Next(-GridSize + 1, GridSize);
            _seed.Move(new Position(x, y));
        } while (_seed.CanBeEaten(_player));
    }
}
