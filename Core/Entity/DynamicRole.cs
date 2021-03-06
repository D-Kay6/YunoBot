﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class DynamicRole : Automation
    {
        [Required]
        [MaxLength(100)]
        public string Status { get; set; }

        public virtual HashSet<DynamicRoleData> Roles { get; set; }

        public DynamicRole()
        {
            Roles = new HashSet<DynamicRoleData>();
        }
    }
}