using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Entities
{
    public class TestType
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
