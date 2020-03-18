﻿namespace IDal.Database.Raven
{
    using System.Threading.Tasks;
    using Entity.RavenDB;

    public interface IDbChannel
    {
        Task Add(ChannelAutomation value);
        Task Update(ChannelAutomation value);
        Task Remove(string id);
        Task<ChannelAutomation> Get(ulong serverId);
    }
}