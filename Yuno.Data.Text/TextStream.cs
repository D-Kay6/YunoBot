using System;
using System.IO;

namespace Yuno.Data.Text
{
    class TextStream
    {
        public void Write<T>(string path, T data)
        {
            using (var stream = new StreamWriter(path))
            {
                stream.Write(data);
            }
        }

        public void WriteLine<T>(string path, T data)
        {
            Write(path, data, true);
        }

        public void Overwrite<T>(string path, T data)
        {
            Write(path, data, false);
        }

        private void Write<T>(string path, T data, bool append)
        {
            using (var stream = new StreamWriter(path, append))
            {
                stream.WriteLine(data);
            }
        }

        public T Read<T>(string path)
        {
            using (var stream = new StreamReader(path))
            {
                var text = stream.ReadToEnd();
                return (T)Convert.ChangeType(text, typeof(T));
            }
        }
    }
}
