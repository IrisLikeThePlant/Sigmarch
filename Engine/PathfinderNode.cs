namespace Sigmarch;

public class PathfinderNode
{
    public Point Position { get; }
    public PathfinderNode? Parent { get; }
    public int Depth { get; }

    public PathfinderNode(Point position, PathfinderNode? parent, int depth)
    {
        Position = position;
        Parent = parent;
        Depth = depth;
    }
}