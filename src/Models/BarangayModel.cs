using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.Models.Entities;

namespace TYRISKANALYSIS.Models
{
    public class BarangayModel
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int? Latitude { get; set; }

        public int? Longitude { get; set; }
    }

    public class BarangaysModel
    {
        public IEnumerable<Barangay> Barangay { get; set; }
        public PagedParams ParameterQuery { get; set; }
    }
}
