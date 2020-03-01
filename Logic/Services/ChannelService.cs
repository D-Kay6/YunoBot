using IDal.Database;

namespace Logic.Services
{
    public class ChannelService
    {
        private readonly IDbChannel _dbChannel;

        public ChannelService(IDbChannel dbChannel)
        {
            _dbChannel = dbChannel;
        }
    }
}