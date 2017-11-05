using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class LookUpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
