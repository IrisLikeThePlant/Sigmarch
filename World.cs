using Microsoft.Xna.Framework.Input;
using SadConsole.Ansi;

namespace Sigmarch;

public class World
{
    public readonly int Width;
    public readonly int Height;

    private readonly Tile[,] _tiles;
    public Actor ActiveActor;
    public readonly List<Actor> Actors = new();
    public Queue<Actor> ActivatableAllies = new();
    public Queue<Actor> ActivatableEnemies;
    public Point MousePos = new Point(0, 0);

    public World()
    {
        Width = 80;
        Height = 50;
        _tiles = new Tile[Width, Height];

        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '1')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '2')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '3')));
        Actors.Add(new Actor(100, Faction.Ally, new ColoredGlyph(Color.White, Color.Black, '4')));

        PopulateActivatableAllies();

        ActiveActor = ActivatableAllies.Dequeue();

        TestRender();
    }

    private void PopulateActivatableAllies()
    {
        foreach (var actor in Actors.Where(actor => actor.Faction == Faction.Ally))
        {
            ActivatableAllies.Enqueue(actor);
        }
    }

    public void Update()
    {
        if (ActivatableAllies.Count == 0)
        {
            PopulateActivatableAllies();
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

        surface.DrawLine(ActiveActor.Position, GetMouseCellPosition(surface), null, null, Color.Red, null);
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
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _tiles[x, y] = new Tile(new ColoredGlyph(Color.White, Color.Black, ' '), true);
            }
        }

        // spawn actor
        for (var idx = 0; idx < Actors.Count; ++idx)
        {
            Actors[idx].Position = new Point(10 + idx, 15);
        }
    }

    private Point GetMouseCellPosition(ScreenSurface surface)
    {
        return new Point(MousePos.X / surface.FontSize.X, MousePos.Y / surface.FontSize.Y);
    }
}