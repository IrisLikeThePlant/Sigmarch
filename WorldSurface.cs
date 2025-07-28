using SadConsole.Input;

namespace Sigmarch;

public class WorldSurface : ScreenSurface
{
    private World _world;

    public WorldSurface(int width, int height, World world) : base(width, height)
    {
        _world = world;
    }

    public void Render()
    {
        Surface.Clear();
        _world.Render(this);
    }

    public new void Update(TimeSpan delta)
    {
        _world.MousePos = GetMouseCellPosition();
        _world.Update(delta);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        bool handled = false;
        _world.MousePos = state.Mouse.ScreenPosition;
        // return base.ProcessMouse(state);
        if (state.Mouse.LeftClicked)
        {
            Point targetPoint = new Point(state.Mouse.ScreenPosition.X / FontSize.X, state.Mouse.ScreenPosition.Y / FontSize.Y);
            if (Surface.IsValidCell(targetPoint.X, targetPoint.Y))
            {
                _world.MovementPath = _world.ActivePath.GetRange(0, Math.Min(10, _world.ActivePath.Count)); // this should be in World!
                _world.isMoving = true;
            }
            handled = true;
        }

        return handled;
    }

    private Point GetMouseCellPosition()
    {
        return new Point(_world.MousePos.X / FontSize.X, _world.MousePos.Y / FontSize.Y);
    }
}