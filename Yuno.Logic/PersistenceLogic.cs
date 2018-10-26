using System;
using Yuno.Logic.Core;

namespace Yuno.Logic
{
    [Serializable]
    public class PersistenceLogic : Persistence
    {
        public PersistenceLogic()
        {
            this.AutoChannel = new AutoChannelLogic();
        }

        public void SetAutoChannel(AutoChannelLogic autoChannel)
        {
            this.AutoChannel = autoChannel;
        }
    }
}
