using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Sigmarch;

public enum MapGenerationType
{
    Arena, // big circle with columns
    Forest, // rectangular with lots of trees
    River, // few trees, small bridges to go through
}

public class MapGenerator
{
    // public Tile[,] Tiles { get; private set; }
    private int _width;
    private int _height;
    private Tile[,] _tiles;

    public MapGenerator(int width, int height)
    {
        _width = width;
        _height = height;
        _tiles = new Tile[width, height];
    }

    public Tile[,] BuildMap(MapGenerationType type)
    {
        switch (type)
        {
            case MapGenerationType.Arena:
                return BuildArena();
            case MapGenerationType.Forest:
                return BuildForest();
            case MapGenerationType.River:
                return BuildRiver();
            default:
                throw new SwitchExpressionException(type);
        }
    }

    private Tile[,] BuildArena()
    {
        // terrain
        Point center = new Point(_width / 2, _height / 2);
        int radius = _width / 2;
        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                if (Utils.PointDistance(center, new Point(x, y)) < radius)
                    _tiles[x, y] = new Tile(new ColoredGlyph(Color.SandyBrown, Color.Black, 219), true);
                else
                    _tiles[x, y] = new Tile(new ColoredGlyph(Color.Black, Color.Black, 219), false);
            }
        }
        
        for (int n = 0; n < 200; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(Color.Black, Color.SandyBrown, '.'), true);
        }
        for (int n = 0; n < 200; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(Color.Black, Color.SandyBrown, ','), true);
        }
        for (int n = 0; n < 200; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(Color.Black, Color.SandyBrown, '`'), true);
        }
        for (int n = 0; n < 200; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(Color.Black, Color.SandyBrown, '~'), true);
        }

        // 5 column
        var numColumns = 7;
        var colPosRadius = _width / 3.5f;
        var colRadius = 6;
        var angle = 360.0f / numColumns;
        var colCenters = new List<Point>();
        for (int i = 0; i < numColumns; ++i)
        {
            // ew ugly
            colCenters.Add(new Point((int) (center.X + colPosRadius * Math.Sin(Utils.DegToRad(angle * i))), (int) (center.Y + colPosRadius * Math.Cos(Utils.DegToRad(angle * i)))));
        }
        foreach (var column in colCenters)
        {
            for (int x = 0; x < _width; ++x)
            {
                for (int y = 0; y < _height; ++y)
                {
                    if (Utils.PointDistance(column, new Point(x, y)) < colRadius)
                        _tiles[x, y] = new Tile(new ColoredGlyph(Color.Black, Color.Black, 219), false);
                }
            }
        }

        return _tiles;
    }

    private Tile[,] BuildForest()
    {
        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                _tiles[x, y] = new Tile(new ColoredGlyph(new Color(88, 129, 87), Color.Black, 219), true);
            }
        }

        // trees
        for (int n = 0; n < 1000; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(new Color(58, 90, 64), _tiles[randomPos.X, randomPos.Y].Glyph.Foreground, 244), false);
        }

        return _tiles;
    }

    private Tile[,] BuildRiver()
    {
        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                _tiles[x, y] = new Tile(new ColoredGlyph(new Color(88, 129, 87), Color.Black, 219), true);
            }
        }

        // a few trees
        for (int n = 0; n < 300; ++n)
        {
            Point randomPos = new Point(Game.Instance.Random.Next(0, _width), Game.Instance.Random.Next(0, _height));
            if (_tiles[randomPos.X, randomPos.Y].Walkable)
                _tiles[randomPos.X, randomPos.Y] = new Tile(new ColoredGlyph(new Color(58, 90, 64), _tiles[randomPos.X, randomPos.Y].Glyph.Foreground, 244), false);
        }

        // river
        int cx = 0;
        int cy = _height / 2;
        int radius = 5;
        for (int i = 0; i < _width; ++i)
        {
            // paint a circle of tiles
            for (int x = -radius; x <= radius; ++x)
            {
                for (int y = -radius; y <= radius; ++y)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        int pointX = cx + x;
                        int pointY = cy + y;

                        if (pointX >= 0 && pointY >= 0 && pointX < _width && pointY < _height)
                            _tiles[pointX, pointY] = new Tile(new ColoredGlyph(Color.LightBlue, new Color(9, 77, 146), '~'), false);
                    }
                }
            }

            cx += 1;
            cy += Game.Instance.Random.Next(-1, 2);
        }

        // bridges
        int numBridges = 4;
        int halfBridgeSizeX = 4;
        int halfBridgeSizeY = 12;
        for (int i = 1; i < numBridges; ++i)
        {
            var bridgeCenter = new Point((_width / numBridges) * i, _height / 2);
            for (int x = bridgeCenter.X - halfBridgeSizeX; x <= bridgeCenter.X + halfBridgeSizeX; ++x)
            {
                for (int y = bridgeCenter.Y - halfBridgeSizeY; y <= bridgeCenter.Y + halfBridgeSizeY; ++y)
                {
                    if (x >= 0 && y >= 0 && x < _width && y < _height)
                        _tiles[x, y] = new Tile(new ColoredGlyph(new Color(143, 92, 56), Color.Black, 219), true);
                }
            }
        }

        return _tiles;
    }

}