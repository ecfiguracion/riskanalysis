using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Model
{
    public class RiskTrendsSupportModel
    {
        public int LevelId { get; set; }
        public string Barangay { get; set; }
        public decimal SupportPercentage { get; set; }
    }

    public class RiskTrendsRulesModel
    {
        public int Id { get; set; } = 0;
        public string RuleXBarangay { get; set; }
        public string RuleYBarangay { get; set; }
        public decimal SupportX { get; set; }
        public decimal SupportY { get; set; }
        public decimal Support { get; set; }
        public decimal Confidence { get; set; }
        public decimal Lift { get; set; }
    }

    public class RiskTrendsModel
    {
        public IEnumerable<RiskTrendsRulesModel> RiskTrendsRules { get; set; }
        public IEnumerable<RiskTrendsSupportModel> RiskTrendsSupport { get; set; }
        public int supportStart { get; set; } = 0;
        public int supportEnd { get; set; } = 0;
    }
}
