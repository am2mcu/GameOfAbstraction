public class Manhattan : IDistanceMethod
{
    public int Distance(Position position1, Position position2)
    {
        return Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
    }
}
