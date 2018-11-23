using Yuno.Data.Core.Interfaces;

namespace Yuno.Logic
{
    public class DataProvider
    {
        private ISerializer _persistence;

        public DataProvider(ISerializer persistence)
        {
            this._persistence = persistence;
        }
    }
}
