using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Role
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}