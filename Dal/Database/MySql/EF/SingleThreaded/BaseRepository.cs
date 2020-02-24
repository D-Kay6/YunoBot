﻿namespace Dal.Database.MySql.EF.SingleThreaded
{
    public abstract class BaseRepository
    {
        protected readonly DataContext Context;

        protected BaseRepository()
        {
            Context = new DataContext();
        }
    }
}