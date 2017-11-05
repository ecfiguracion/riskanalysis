using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.Models.Entities
{
    public class Barangay
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int? Latitude { get; set; }

        public int? Longitude { get; set; }


    }
}
