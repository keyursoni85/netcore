using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Entities
{
    public class AthleteTestMapping
    {
        [Key]
        public int MapId { get; set; }

        public int AthleteTestId { get; set; }

        public int AthleteId { get; set; }

        public string AthleteName { get; set; }

        public decimal Distance { get; set; }

        public bool IsDeleted { get; set; }
    }
}
