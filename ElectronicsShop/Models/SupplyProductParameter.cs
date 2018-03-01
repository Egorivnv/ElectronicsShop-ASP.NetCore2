using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class SupplyProductParameter
    { 
        [Key]
        public int Id { get; set; }
        [Display(Name ="Supply frequency, days")]
        [Required(ErrorMessage = "Please enter supply frequency!")]
        [Range(1, int.MaxValue)]
        public int SupplyFrequency { get; set; }
        [Display(Name = "Time to form a supply, days")]
        [Required(ErrorMessage = "Please enter the required time to form a supplying!")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive quantity of days required time to form a supplying!")]
        public int TimeToFormSupply { get; set; }
        [Display(Name = "Safety ratio, %")]
        [Required(ErrorMessage = "Please enter a safety ratio!")]
        public int SafetyRatio { get; set; }
        public Product Product { get; set; }
    }
}
