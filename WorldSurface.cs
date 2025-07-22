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

    public void Update()
    {
        _world.Update();
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
                MovementAction action = new MovementAction(targetPoint);
                _world.ActiveActor.Execute(action);
                _world.ActiveActor = _world.ActivatableAllies.Dequeue();
            }
            handled = true;
        }

        return handled;
    }
}