namespace Entity.RavenDB
{
    using Core.Enum;

    public class LanguageSetting
    {
        public LanguageSetting()
        {
            Language = Language.English;
        }

        public Language Language { get; set; }
    }
}