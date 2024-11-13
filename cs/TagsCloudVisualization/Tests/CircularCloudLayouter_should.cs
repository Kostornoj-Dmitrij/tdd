using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private CircularCloudLayouter layouter = null!;
        private Size[] rectangleSizes = Array.Empty<Size>();
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
            rectangleSizes = new[]
            {
                new Size(10, 10),
                new Size(20, 20),
                new Size(30, 10),
                new Size(15, 25),
                new Size(10, 10)
            };
        }

        [Test]
        public void CircularCloud_ShouldBeEmpty_WhenCreated()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 10)]
        [TestCase(10, -1)]
        public void CircularCloud_ShouldThrowArgumentException_WhenInvalidCenterCoordinates(int width, int height)
        {
            var func = () => new CircularCloudLayouter(new Point(width, height));
            func.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        [TestCase(-1, 10)]
        [TestCase(10, -1)]
        [TestCase(0, 0)]
        [TestCase(-10, -10)]
        public void CircularCloud_ShouldThrowArgumentException_WhenInvalidRectangleSize(int width, int height)
        {
            var func = () => layouter.PutNextRectangle(new Size(width, height));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithSameSize()
        {
            var rectangleSize = new Size(20, 30);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            
            rectangle.Size.Should().BeEquivalentTo(rectangleSize);
        }

        [Test]
        public void PutNextRectangle_ShouldPlaceRectangleInCenter_WhenFirstRectangle()
        {
            var rectangleSize = new Size(10, 10);
            var expectedLocation = new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            
            rectangle.Location.Should().BeEquivalentTo(expectedLocation);
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersectWithPreviousRectangles()
        {
            foreach (var size in rectangleSizes)
            {
                layouter.PutNextRectangle(size);
            }
            VerifyRectanglesDontIntersect(layouter.Rectangles.ToList());
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersect_WhenPlacingIdenticalRectangles()
        {
            var rectangleSize = new Size(10, 10);
            var numberOfRectangles = 10;

            for (int i = 0; i < numberOfRectangles; i++)
            {
                layouter.PutNextRectangle(rectangleSize);
            }
            VerifyRectanglesDontIntersect(layouter.Rectangles.ToList());
        }

        [Test]
        public void CircularCloud_ShouldHaveAllPlacedRectangles()
        {
            foreach (var size in rectangleSizes)
            {
                layouter.PutNextRectangle(size);
            }

            layouter.Rectangles.Should().HaveCount(rectangleSizes.Length);
        }

        [Test]
        public void CircularCloudRectangles_ShouldBeCloseToCenter()
        {
            foreach (var size in rectangleSizes)
            {
                layouter.PutNextRectangle(size);
            }

            var rectangles = layouter.Rectangles.ToList();

            foreach (var rectangle in rectangles)
            {
                var distanceToCenter = Math.Sqrt(Math.Pow(rectangle.X + rectangle.Width / 2 - center.X, 2) +
                                                  Math.Pow(rectangle.Y + rectangle.Height / 2 - center.Y, 2));
                distanceToCenter.Should().BeLessThan(20);
            }
        }

        private void VerifyRectanglesDontIntersect(List<Rectangle> rectangles)
        {
            for (int i = 0; i < rectangles.Count; i++)
            {
                for (int j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }
    }
}