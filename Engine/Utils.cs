namespace Sigmarch;

public static class Utils
{
    public static double PointDistance(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
    }

    public static double DegToRad(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    public static double RadToDeg(double angle)
    {
        return angle * (180 / Math.PI);
    }
}