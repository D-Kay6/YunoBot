﻿using Core.Entity;
using Core.Enum;
using IDal;
using Logic.Extensions;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class LocalizationService
    {
        private readonly ILocalization _localization;

        private Localization _data;

        public LocalizationService(ILocalization localization)
        {
            _localization = localization;
        }

        public async Task Load(Language language)
        {
            _data = await _localization.Read(language.ToString());
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