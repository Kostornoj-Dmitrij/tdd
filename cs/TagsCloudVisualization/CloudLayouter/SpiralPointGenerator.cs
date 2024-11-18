using SixLabors.ImageSharp;

namespace TagsCloudVisualization.CloudLayouter;

public class SpiralPointGenerator
{
    private readonly PointF center;
    private const double SpiralStep = 0.1;
    private double angle;

    public SpiralPointGenerator(PointF center)
    {
        this.center = center;
        angle = 0;
    }

    public PointF GetNextPoint()
    {
        var x = (float)(center.X + angle * Math.Cos(angle));
        var y = (float)(center.Y + angle * Math.Sin(angle));
        angle += SpiralStep;
        return new PointF(x, y);
    }
}