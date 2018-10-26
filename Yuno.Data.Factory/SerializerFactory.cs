using Yuno.Data.Core.Interfaces;

namespace Yuno.Data.Factory
{
    public class SerializerFactory
    {
        public static ISerializer GenerateSerializer()
        {
            return new Serializer.Serializer();
        }
    }
}
