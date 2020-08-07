using System;

namespace Entity.RavenDB
{
    public class Ban
    {
        public string Id { get; set; }
        public ulong ServerId { get; set; }
        public ulong UserId { get; set; }
        public DateTime? EndDate { get; set; }
        public string Reason { get; set; }
    }
}