using System.IO;

namespace Logic.Extensions
{
    public static class ImageExtensions
    {
        public static string GetImagePath(string name)
        {
            var directory = Directory.GetCurrentDirectory();
            directory = Path.Combine(directory, "Data");
            directory = Path.Combine(directory, "Images");
            return Path.Combine(directory, name);
        }
    }
}