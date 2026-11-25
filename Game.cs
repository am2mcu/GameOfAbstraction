public class Game(IDistanceMethod distanceMethod, bool isDebug = false, int gridSize = 16)
{
    public int GridSize { get; } = gridSize;
    private readonly bool _isDebug = isDebug;
    private readonly int _dangerDistance = 10;
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

            var direction = GetPlayerCommand();
            _player.Move(direction);

            EatSeed();

            MoveEnemies();
            if (_isDebug)
                LogEnemies();

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
        Console.Write("arrows [ ");
        ConsoleWriteArrows();
        Console.WriteLine("]");
    }

    private Dictionary<Direction, int> MinDistances()
    {
        Dictionary<Direction, int> minDistances = new()
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

            // FIXME: move distance calculation in Game since it is not an enemy responsibility
            var distance = enemy.Distance(_distanceMethod, _player.Position);

            if (dy > 0)
                minDistances[Direction.Up] = Math.Min(minDistances[Direction.Up], distance);
            if (dy < 0)
                minDistances[Direction.Down] = Math.Min(minDistances[Direction.Down], distance);
            if (dx < 0)
                minDistances[Direction.Left] = Math.Min(minDistances[Direction.Left], distance);
            if (dx > 0)
                minDistances[Direction.Right] = Math.Min(minDistances[Direction.Right], distance);
        }

        return minDistances;
    }

    private void ConsoleWriteArrows()
    {
        var minDistances = MinDistances();
        int minSafeDistance = minDistances
            .Values.Where(distance => distance >= _dangerDistance)
            .DefaultIfEmpty(int.MaxValue)
            .Min();

        foreach (var distance in minDistances)
        {
            ConsoleWriteColored(
                $"{ArrowMapping[distance.Key]} ",
                GetArrowColor(distance.Value, minSafeDistance)
            );
        }
    }

    private ConsoleColor GetArrowColor(int distance, int minSafeDistance)
    {
        if (distance < _dangerDistance)
            return ConsoleColor.Red;

        if (distance == minSafeDistance)
            return ConsoleColor.Yellow;

        return ConsoleColor.Green;
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
        int command;
        do
        {
            Console.Write("Enter direction (8 2 4 6): ");
            var input = Console.ReadLine();
            command = int.TryParse(input, out command) ? command : -1;
        } while (command == -1 || !CommandMapping.ContainsKey(command)); // TODO: simplify input handling

        return CommandMapping[command];
    }

    private void LogEnemies()
    {
        foreach (var enemy in _enemies)
        {
            Console.WriteLine($"* {enemy.Name} {enemy.Position}");
        }
    }

    private void MoveEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.MoveTowards(new Position(_player.Position.X, _player.Position.Y));
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

    // FIXME: Make this an event
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
            int x = Random.Shared.Next(-GridSize + 1, GridSize);
            int y = Random.Shared.Next(-GridSize + 1, GridSize);
            _seed.Move(new Position(x, y));
        } while (_seed.CanBeEaten(_player));
    }
}
