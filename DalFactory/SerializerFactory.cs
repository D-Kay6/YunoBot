using Dal.Serializer;
using IDal.Interfaces;

namespace DalFactory
{
    public class SerializerFactory
    {
        public static ISerializer GenerateSerializer()
        {
            return new Serializer();
        }
    }
}
