using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarungkuTMG.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public required string CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? DisabledDate { get; set; }
        public bool IsDisabled { get; set; }
        [StringLength(100)]
        public string? DisabledBy { get; set; }
    }
}
