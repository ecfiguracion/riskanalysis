using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYRISKANALYSIS.Controllers.API.Utilities
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; private set; }

        public PagedParams PagedInfo { get; private set; }

        public PagedResult(IEnumerable<T> items, PagedParams param, int totalRecordCount)
        {
            param.SetPageNo();

            var pageInfo = param;
            pageInfo.pageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)param.pageSize)
                    : 0;

            Items = items;
            PagedInfo = pageInfo;


        }

    }
}
