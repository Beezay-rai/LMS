﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }


    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }


}
