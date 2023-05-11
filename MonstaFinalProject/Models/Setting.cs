﻿using System.ComponentModel.DataAnnotations;

namespace MonstaFinalProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Key { get; set; }
        [StringLength(1000)]
        public string Value { get; set; }
        public string? Image { get; set; }
    }
}
