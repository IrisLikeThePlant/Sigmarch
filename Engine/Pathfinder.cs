namespace Sigmarch;

public static class Pathfinder
{
    public static List<Point> GetPath(Point start, Point end, World world)
    {
        var openList = new PriorityQueue<PathfinderNode, int>();
        openList.Enqueue(new PathfinderNode(start, null, 0), 0);
        var closedList = new HashSet<Point> { start };

        while (openList.TryDequeue(out var node, out _))
        {
            foreach (var step in GetValidNeighbors(node.Position))
            {
                if (step == end)
                {
                    return BuildPath(new PathfinderNode(step, node, node.Depth));
                }

                if (closedList.Contains(step)) continue;
                if (!world.IsTileWalkable(step)) continue;
                if (!world.IsWithinMap(step)) continue;

                int newDepth = node.Depth + 1;
                openList.Enqueue(new PathfinderNode(step, node, newDepth), newDepth);
                closedList.Add(step);
            }
        }

        return new List<Point>();
    }

    private static List<Point> BuildPath(PathfinderNode node)
    {
        List<Point> path = new();
        PathfinderNode currentNode = node;
        while (currentNode != null)
        {
            path.Add(currentNode.Position);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            currentNode = currentNode.Parent;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        path.Reverse();
        path.RemoveAt(0);
        return path;
    }

    private static IEnumerable<Point> GetValidNeighbors(Point pos)
    {
        yield return new Point(pos.X - 1, pos.Y);
        yield return new Point(pos.X + 1, pos.Y);
        yield return new Point(pos.X, pos.Y - 1);
        yield return new Point(pos.X, pos.Y + 1);
    }
}