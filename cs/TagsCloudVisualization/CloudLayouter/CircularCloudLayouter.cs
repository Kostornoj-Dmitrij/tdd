using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Point Center;
        private List<Rectangle> rectangles;
        private double angle;
        private const double SpiralStep = 0.1;
        public List<Rectangle> Rectangles => rectangles;
        public double Angle => angle;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Center point coordinates cannot be negative.", nameof(center));
            
            Center = center;
            rectangles = new List<Rectangle>();
            angle = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size should be positive value", nameof(rectangleSize));

            Rectangle newRectangle;
            do
            {
                var location = GetNextPointOnSpiral();
                var rectangleLocation = new Point(location.X - rectangleSize.Width / 2, 
                                                location.Y - rectangleSize.Height / 2);
                newRectangle = new Rectangle(rectangleLocation, rectangleSize);
            } while (IsIntersects(newRectangle));

            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Point GetNextPointOnSpiral()
        {
            var x = (int)(Center.X + Angle * Math.Cos(Angle));
            var y = (int)(Center.Y + Angle * Math.Sin(Angle));
            angle += SpiralStep;
            return new Point(x, y);
        }

        private bool IsIntersects(Rectangle rectangle)
        {
            foreach (var existingrectangle in Rectangles)
            {
                if (existingrectangle.IntersectsWith(rectangle))
                {
                    return true;
                }
            }
            return false;
        }
    }
}