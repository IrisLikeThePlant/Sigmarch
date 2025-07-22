namespace Sigmarch;

public class MovementAction : Action
{
    public Point _targetPoint { get; private set; }

    public MovementAction(Point targetPoint)
    {
        _targetPoint = targetPoint;
    }

    public override bool Execute(Actor actor)
    {
        actor.Position = _targetPoint;
        return true;
    }
}