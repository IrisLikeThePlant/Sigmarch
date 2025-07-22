namespace Sigmarch;

public abstract class GameObject
{
    public Point Position { get; set; }
    public ColoredGlyph? Glyph { get; private set; }

    public void SetColoredGlyph(ColoredGlyph glyph) => Glyph = glyph;
}