﻿using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Dal.EF
{
    public class ServerRepository : IDbServer
    {
        public async Task AddServer(ulong id, string name)
        {
            var context = new DataContext();
            context.Servers.Add(new Server
            {
                Id = id,
                Name = name
            });
            await context.SaveChangesAsync();
        }

        public async Task AddServer(Server server)
        {
            var context = new DataContext();
            try
            {
                context.Servers.Add(server);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateServer(ulong id, string name)
        {
            var context = new DataContext();
            try
            {
                var server = await context.Servers.FindAsync(id);
                if (server == null)
                {
                    await AddServer(id, name);
                    return;
                }

                server.Name = name;
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
        }

        public async Task UpdateServer(Server server)
        {
            var context = new DataContext();
            context.Servers.Update(server);
            await context.SaveChangesAsync();
        }

        public async Task DeleteServer(ulong id)
        {
            var context = new DataContext();
            var server = await context.Servers.FindAsync(id);
            if (server == null) return;
            context.Servers.Remove(server);
            await context.SaveChangesAsync();
        }

        public async Task<Server> GetServer(ulong id)
        {
            var context = new DataContext();
            return await context.Servers.FindAsync(id);
        }

        public async Task Save()
        {
            var context = new DataContext();
            await context.SaveChangesAsync();
        }
    }
}
