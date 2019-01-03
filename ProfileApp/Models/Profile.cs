﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileApp.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Desc { get; set; }
        public string Phone { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeEdited { get; set; }
        public bool IsActive { get; set; }
        public GenderType Gender { get; set; }
    }
    public enum GenderType 
    {
        [Display(Name = "-")]
        None =0,
        [Display(Name="مرد")]
        Male=1,
        [Display(Name = "زن")]
        Female =2
    }
}
