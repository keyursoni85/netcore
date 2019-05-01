using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Web.Models
{
    public class TestModel
    {
        [Required]
        public int TestId { get; set; }

        [Required]
        public int TestType { get; set; }

        [Required]
        public string TestName { get; set; }

        [Required]
        public string Date { get; set; }
    }
}
