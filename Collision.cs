public class Collision(bool isCollided, Enemy? enemy)
{
    public bool IsCollided { get; } = isCollided;
    public Enemy? Enemy { get; } = enemy;
}
