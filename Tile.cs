namespace Sigmarch;

internal class Tile
{
    public ColoredGlyph Glyph { get; set; }
    public bool Walkable { get; private set; }

    public Tile(ColoredGlyph glyph, bool walkable)
    {
        Glyph = glyph;
        Walkable = walkable;
    }
}