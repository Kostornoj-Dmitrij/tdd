using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization
{
    class RandomVisualisation
    {
        static void Main(string[] args)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName
                    ?? throw new InvalidOperationException("Не удалось определить директорию проекта.");
            var imagesDirectory = Path.Combine(projectDirectory, "Images");
            Console.WriteLine(imagesDirectory);
            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            for (int j = 1; j <= 3; j++)
            {
                var layouter = new CircularCloudLayouter(new Point(600, 450));
                var random = new Random();
                var numberOfRectangles = 150;

                for (int i = 0; i < numberOfRectangles; i++)
                {
                    var width = random.Next(5, 80);
                    var height = random.Next(5, 80);
                    layouter.PutNextRectangle(new Size(width, height));
                }

                layouter.SaveVisualization(Path.Combine(imagesDirectory, $"tag_cloud_random_{j}.png"));
            }
            Console.WriteLine("Визуализация была сохранена.");
        }
    }
}
