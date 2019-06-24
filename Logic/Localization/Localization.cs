using DalFactory;
using IDal.Interfaces;
using IDal.Interfaces.Database;
using IDal.Structs.Localization;
using Logic.Extensions;

namespace Logic.Localization
{
    public class Localization
    {
        private IServerSettings _settings;
        private ILocalization _lang;
        private LanguageData _data;

        public Localization(ulong serverId)
        {
            _settings = DatabaseFactory.GenerateServerSettings();
            _lang = LocalizationFactory.GenerateLocalization();

            var language = _settings.GetLanguage(serverId);
            _data = _lang.Read(language.ToString());
        }

        public string GetMessage(string key)
        {
            return _data.Messages.ContainsKey(key) ? _data.Messages[key] : key;
        }

        public string GetMessage(string key, params object[] args)
        {
            return _data.Messages.ContainsKey(key) ? string.Format(_data.Messages[key], args) : key;
        }

        public string GetRandomUserPraise()
        {
            return _data.UserPraises.GetRandomItem();
        }

        public string GetRandomGroupPraise()
        {
            return _data.GroupPraises.GetRandomItem();
        }
    }
}
