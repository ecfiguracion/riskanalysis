using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.Controllers.API.Utilities
{
    public class PagedParams
    {
        // Filter 
        public string searchString { get; set; } = string.Empty;
        public string prevSearchString { get; set; } = string.Empty;

        // Paging
        public int pageNo { get; set; } 
        public int pageSize { get; set; } 
        public int pageCount { get; set; }

        public void SetPageNo()
        {
            if (this.searchString != this.prevSearchString)
            {
                this.pageNo = 1;
            }
        }
    }
}
