using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Entities
{
    public class Test
    {
        [Key]
        public int TestId { get; set; }

        public string TestName { get; set; }
        
        [Required(ErrorMessage = "Please select Test")]
        public int TestTypeId { get; set; }

        [Required(ErrorMessage = "Please select Date.")]
        public DateTime TestDate { get; set; }

        public int? TotalParticipats { get; set; }

        public bool IsDeleted { get; set; }
    }
}
