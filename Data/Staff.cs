﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [StringLength(10), MinLength(10)]
        public string Contact { get; set; }

        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

        public string Name { get; set; }
        public string Position { get; set; }
    }
}
