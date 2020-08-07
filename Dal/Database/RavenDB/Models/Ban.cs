﻿using System;

namespace Dal.Database.RavenDB.Models
{
    public class Ban
    {
        public string Id { get; set; }
        public string ServerId { get; set; }
        public string UserId { get; set; }
        public DateTime? EndDate { get; set; }
        public string Reason { get; set; }
    }
}