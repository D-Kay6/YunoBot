namespace Entity.RavenDB
{
    public class LanguageSetting
    {
        public Language Language { get; set; }

        public LanguageSetting()
        {
            this.Language = Language.English;
        }
    }
}