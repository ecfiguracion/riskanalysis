﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.Models.Entities;

namespace TYRISKANALYSIS.Models.LookUp
{
    public class DataLookUpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
