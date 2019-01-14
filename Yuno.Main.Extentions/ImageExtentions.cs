using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
