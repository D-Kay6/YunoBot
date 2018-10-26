using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yuno.Logic.Core;

namespace Yuno.Logic
{
    public static class DomainTranslator
    {
        public static PersistenceLogic Translate(Persistence persistence)
        {
            var newPersistence = new PersistenceLogic();
            newPersistence.SetAutoChannel(Translate(persistence.AutoChannel));
            return newPersistence;
        }

        public static AutoChannelLogic Translate(AutoChannel autoChannel)
        {
            var newAutoChannel = new AutoChannelLogic();
            newAutoChannel.LoadChannels(autoChannel.Channels);
            newAutoChannel.SetAutoChannelIcon(autoChannel.AutoChannelIcon);
            return newAutoChannel;
        }
    }
}
