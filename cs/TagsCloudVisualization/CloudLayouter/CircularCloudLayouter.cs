using SixLabors.ImageSharp;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly PointF center;
    private readonly SpiralPointGenerator spiral;
    public List<RectangleF> Rectangles { get; }

    public CircularCloudLayouter(PointF center)
    {
        this.center = center;

        Rectangles = new List<RectangleF>();
        spiral = new SpiralPointGenerator(center);
    }

    public RectangleF PutNextRectangle(SizeF rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size should be positive value", nameof(rectangleSize));

        RectangleF newRectangle;
        do
        {
            var location = spiral.GetNextPoint();
            var rectangleLocation = new PointF(location.X - rectangleSize.Width / 2,
                location.Y - rectangleSize.Height / 2);
            newRectangle = new RectangleF(rectangleLocation, rectangleSize);
            
            if (!newRectangle.IntersectsWithAny(Rectangles))
            {
                ShiftRectanglesTowardsCenter();
            }
        } while (newRectangle.IntersectsWithAny(Rectangles));

        Rectangles.Add(newRectangle);
        return newRectangle;
    }

    private void ShiftRectanglesTowardsCenter()
    {
        const float shiftFactor = 0.005f;
        var moved = true;
        while (moved)
        {
            moved = false;

            for (int i = 0; i < Rectangles.Count; i++)
            {
                var rectangle = Rectangles[i];
                var centerX = center.X;
                var centerY = center.Y;
                
                var shiftX = (centerX - rectangle.X) * shiftFactor;
                var newRectangleX = new RectangleF(rectangle.X + shiftX, rectangle.Y, rectangle.Width, rectangle.Height);
                if (!newRectangleX.IntersectsWithAny(Rectangles))
                {
                    rectangle.X += shiftX;
                    moved = true;
                }
                
                var shiftY = (centerY - rectangle.Y) * shiftFactor;
                var newRectangleY = new RectangleF(rectangle.X, rectangle.Y + shiftY, rectangle.Width, rectangle.Height);
                if (!newRectangleY.IntersectsWithAny(Rectangles))
                {
                    rectangle.Y += shiftY;
                    moved = true;
                }
            }
        }
    }
}