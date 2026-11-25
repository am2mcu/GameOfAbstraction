var IsDebug = true;

var game = new Game(new Manhattan());

if (IsDebug)
    game.EnemiesMoved += (enemies) =>
    {
        foreach (var enemy in enemies)
            Console.WriteLine($"* {enemy.Name} {enemy.Position}");
    };

game.Start();
