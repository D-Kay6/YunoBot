using Core.Enum;

namespace Entity.RavenDB
{
    public class LanguageSetting
    {
        public LanguageSetting()
        {
            Language = Language.English;
        }

        public Language Language { get; set; }
    }
}