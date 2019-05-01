using Application.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Web.Models
{
    public class AthleteModel
    {
        public List<AthleteTestMapping> athleteList { get; set; }

        public List<User> userList { get; set; }

        public int TestId { get; set; }

        public string TestName { get; set; }

        [Required(ErrorMessage = "Please choose Athlete")]
        public int AthleteId { get; set; }

        [Required(ErrorMessage = "Please enter distance")]
        [Range(0.001, 9999999999999999.999, ErrorMessage = "Distance should be grater then 0")]
        public Decimal Distance { get; set; }

        public bool IsEditMode { get; set; }

        public int MapId { get; set; }

        public DateTime TestDate { get; set; }
    }
}
