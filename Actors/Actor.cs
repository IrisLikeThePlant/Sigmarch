namespace Sigmarch;

public class Actor : GameObject
{
    public int Health { get; private set; }
    public Faction Faction;

    public Actor(int health, Faction faction, ColoredGlyph glyph)
    {
        Health = health;
        Faction = faction;
        SetColoredGlyph(glyph);
    }

    public bool Execute(Action action)
    {
        return action.Execute(this);
    }
}