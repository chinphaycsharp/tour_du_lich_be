using EPS.Utils.Repository.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPS.Data.Entities
{
    public partial class Unit : ICascadeDelete
    {
        public Unit()
        {
            Roles = new HashSet<Role>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Code { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Unit Parent { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public void OnDelete()
        {
            
        }
    }
}
