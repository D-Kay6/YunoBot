using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public abstract class MultiThreadedRepository
    {
        protected static readonly SemaphoreSlim Semaphore;

        static MultiThreadedRepository()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        protected async Task Execute(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                return;
            }
            finally
            {
                Semaphore.Release();
            }
        }

        protected async Task<T> Execute<T>(Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (Exception e)
            {
                return default;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
