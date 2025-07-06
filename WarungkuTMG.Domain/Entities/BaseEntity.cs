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
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? DisabledDate { get; set; }
        public bool IsDisabled { get; set; }
        [StringLength(100)]
        public string? DisabledBy { get; set; }
    }
}
