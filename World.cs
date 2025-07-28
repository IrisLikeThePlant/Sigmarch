using SadRogue.Primitives.GridViews;

namespace Sigmarch;

public class World
{
    public readonly int Width;
    public readonly int Height;

    private readonly Tile[,] _tiles;
    public Actor ActiveActor;
    public readonly List<Actor> Actors = new();
    public Queue<Actor> ActivatableAllies = new();
    public Queue<Actor> ActivatableEnemies = new();
    public List<Point> ActivePath = new();
    public List<Point> MovementPath = new();
    public Point MousePos = new Point(0, 0);
    private float _animationStep = 0.0f;
    public bool isMoving = false;

    public World()
    {
        Width = GameSettings.GAME_WIDTH;
        Height = GameSettings.GAME_HEIGHT;

        MapGenerator generator = new(Width, Height);
        _tiles = generator.BuildMap(MapGenerationType.River);

        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '1')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '2')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '3')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '4')));

        PopulateActivatableAllies();

        ActiveActor = ActivatableAllies.Dequeue();
        ActivePath = FindPath(new Point(40, 15), ActiveActor);

        TestRender();
    }

    private void PopulateActivatableAllies()
    {
        foreach (var actor in Actors.Where(actor => actor.Faction == Faction.Ally))
        {
            ActivatableAllies.Enqueue(actor);
        }
    }

    public void Update(TimeSpan delta)
    {
        if (ActivatableAllies.Count == 0)
        {
            PopulateActivatableAllies();
        }

        ActivePath = FindPath(MousePos, ActiveActor);

        if (isMoving)
        {
            _animationStep += delta.Ticks;
            if (_animationStep > 0.5f)
            {
                _animationStep = 0.0f;
                MovementAction action = new MovementAction(MovementPath[0]);
                ActiveActor.Execute(action);
                MovementPath.RemoveAt(0);

                if (MovementPath.Count == 0)
                {
                    isMoving = false;
                    ActiveActor = ActivatableAllies.Dequeue();
                }
            }
        }
    }

    public void Render(ScreenSurface surface)
    {
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                _tiles[x, y].Glyph.CopyAppearanceTo(surface.Surface[x, y]);
            }
        }

        RenderActors(surface);

        for (int i = 0; i < Math.Min(10, ActivePath.Count()); ++i)
        {
            new ColoredGlyph(Color.Red, Color.Red, ' ').CopyAppearanceTo(surface.Surface[ActivePath[i].X, ActivePath[i].Y]);
        }
    }

    public void RenderActors(ScreenSurface surface)
    {
        foreach (var actor in Actors)
        {
            actor.Glyph?.CopyAppearanceTo(surface.Surface[actor.Position]);
        }
    }

    private void TestRender()
    {
        // spawn actor
        for (var idx = 0; idx < Actors.Count; ++idx)
        {
            Actors[idx].Position = new Point(GameSettings.GAME_WIDTH / 2 + idx, GameSettings.GAME_HEIGHT / 2);
        }
    }

    private List<Point> FindPath(Point target, Actor activeActor)
    {
        return Pathfinder.GetPath(activeActor.Position, target, this);
    }

    public bool IsTileWalkable(Point pos)
    {
        return true;
        // return _tiles[pos.X, pos.Y].Walkable; it's oob because of when it gets initialized, should be fixed once we stop drawing by default
    }

    public bool IsWithinMap(Point pos)
    {
        return pos.X >= 0 && pos.Y >= 0 && pos.X <= GameSettings.GAME_WIDTH && pos.Y <= GameSettings.GAME_HEIGHT;
    }
}