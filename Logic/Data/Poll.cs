using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Data
{
    public class Polls : Configuration<Polls>
    {
        public Polls(ulong guildId) : base(guildId)
        {
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override void Update()
        {
            //throw new NotImplementedException();
        }
    }

    public class Poll
    {
        public ulong MessageId { get; set; }
        public string Message { get; private set; }

    }
}
