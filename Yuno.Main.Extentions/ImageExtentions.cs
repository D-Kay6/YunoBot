using System.IO;

namespace Yuno.Main.Extentions
{
    public static class ImageExtentions
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
