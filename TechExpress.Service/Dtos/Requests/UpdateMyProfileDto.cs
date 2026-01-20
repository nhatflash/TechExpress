using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TechExpress.Repository.Enums;

namespace TechExpress.Service.Dtos.Requests
{
    public class UpdateMyProfileDto
    {
        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }

        public Gender? Gender { get; set; }

        // public DateTime? Birthdate { get; set; }

        [MaxLength(100)]
        public string? Province { get; set; }

        // [MaxLength(100)]
        // public string? City { get; set; }

        // [MaxLength(100)]
        // public string? District { get; set; }

        [MaxLength(100)]
        public string? Ward { get; set; }

        [MaxLength(255)]
        public string? StreetAddress { get; set; }
    }
}
