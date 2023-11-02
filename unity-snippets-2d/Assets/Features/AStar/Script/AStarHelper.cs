public static class AStarHelper
{
    public static readonly (int, int) Up = (0, -1);
    public static readonly (int, int) Down = (0, 1);
    public static readonly (int, int) Left = (-1, 0);
    public static readonly (int, int) Right = (1, 0);
    public static readonly (int, int)[] Directions = new (int, int)[] { Up, Down, Left, Right };

    public const int GridRows = 10;
    public const int GridCols = 10;
    public const float CellSize = 0.5f;
    public const int NodeCost = 10;
}