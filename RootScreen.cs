
namespace Sigmarch;

class RootScreen : ScreenObject
{
    private World _world;
    private WorldSurface _worldSurface;

    public RootScreen()
    {
        _world = new World();
        _worldSurface = new WorldSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT, _world);
        Children.Add(_worldSurface);
    }

    public override void Render(TimeSpan delta)
    {
        _worldSurface.Render();
        base.Render(delta);
    }

    public override void Update(TimeSpan delta)
    {
        _worldSurface.Update(delta);
        base.Update(delta);
    }
}
