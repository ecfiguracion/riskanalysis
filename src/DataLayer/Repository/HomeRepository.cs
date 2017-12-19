using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TYRISKANALYSIS.Constants;
using TYRISKANALYSIS.Controllers.API.Utilities;
using TYRISKANALYSIS.DataLayer.Interfaces;
using TYRISKANALYSIS.DataLayer.Model;
using TYRISKANALYSIS.Models.LookUp;

namespace TYRISKANALYSIS.DataLayer.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private IDbConnection db;

        public HomeRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public HomeLookupModel GetLookUps()
        {
            var lookup = new HomeLookupModel();

            var sql = @"select DISTINCT t.Id, t.Name
                        from Assessment a
                        inner join Typhoon t on a.TyphoonId = t.Id
                        order by t.name;

                        select code as id, name from section;

                        select category.code id,category.name,section.code as linkid
                        from Category
                        inner join Section ON Category.SectionId = Section.Id";
            using (var multi = this.db.QueryMultiple(sql)) {
                lookup.Typhoons = multi.Read<DataLookUpModel>().ToList();
                lookup.Sections = multi.Read<DataLookUpModel>().ToList();
                lookup.Categories = multi.Read<DataLookUpLinkModel>().ToList();
            };

            return lookup;
        }

        public RiskMapsModel GetRiskMaps(int id)    
        {
            var dataModel = new RiskMapsModel();

            #region Summary

            var sectionsSummary = new SectionSummaryModel();
            var total = 0;
            var sql = @"select coalesce(sum(total),0) as populationTotal
                    from Assessment a
                    inner join AssessmentPopulation ap ON a.Id = ap.AssessmentId
                    where a.TyphoonId = @id;
                    select coalesce(sum(ap.EstimatedCost),0) as propertiesTotal
                    from Assessment a
                    inner join AssessmentProperties ap ON a.Id = ap.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(at.EstimatedCost),0) transportationTotal
                    from Assessment a
                    inner join AssessmentTransportation at ON a.Id = at.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(ac.EstimatedCost),0) communicationTotal
                    from Assessment a
                    inner join AssessmentCommunication ac ON a.Id = ac.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(ae.EstimatedCost),0) electricalTotal
                    from Assessment a
                    inner join AssessmentElectricalPower ae ON a.Id = ae.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(aw.EstimatedCost),0) waterTotal
                    from Assessment a
                    inner join AssessmentWaterFacilities aw ON a.Id = aw.AssessmentId
                    where a.TyphoonId = @Id;                 
                    select coalesce(sum(ac.EstimatedCost),0) cropsTotal
                    from Assessment a
                    inner join AssessmentCrops ac ON a.Id = ac.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(af.EstimatedCost),0) fisheriesTotal
                    from Assessment a
                    inner join AssessmentFisheries af ON a.Id = af.AssessmentId
                    where a.TyphoonId = @Id;
                    select coalesce(sum(al.EstimatedCost),0) livestockTotal
                    from Assessment a
                    inner join AssessmentLivestock al ON a.Id = al.AssessmentId
                    where a.TyphoonId = @Id";
            using (var multi = this.db.QueryMultiple(sql, new { id }))
            {
                var totalPopulation = multi.Read<int>().SingleOrDefault();
                total += totalPopulation;
                sectionsSummary.Population = formatNumber(totalPopulation);

                var totalProperties = multi.Read<int>().SingleOrDefault();
                total += totalProperties;
                sectionsSummary.Properties = formatNumber(totalProperties);

                var totallifelines = 0;
                totallifelines += multi.Read<int>().SingleOrDefault();
                totallifelines += multi.Read<int>().SingleOrDefault();
                totallifelines += multi.Read<int>().SingleOrDefault();
                totallifelines += multi.Read<int>().SingleOrDefault();
                total += totallifelines;
                sectionsSummary.Lifelines = formatNumber(totallifelines);

                var totalagriculture = 0;
                totalagriculture += multi.Read<int>().SingleOrDefault();
                totalagriculture += multi.Read<int>().SingleOrDefault();
                totalagriculture += multi.Read<int>().SingleOrDefault();
                total += totalagriculture;
                sectionsSummary.Agriculture = formatNumber(totalagriculture);

                sectionsSummary.Total = formatNumber(total);
            };

            dataModel.Summary = sectionsSummary;

            #endregion

            #region Maps

            #region Population

            List<RiskMapDataModel> mapData = new List<RiskMapDataModel>();

            sql = @"select c.Name as category,c.sectionid, b.id as barangayid, b.Name as barangay,b.Latitude,b.Longitude, sum(ap.total) total
                        from Assessment a
                        inner join AssessmentPopulation ap on a.Id = ap.AssessmentId
                        inner join Barangay b on ap.BarangayId = b.Id
                        inner join LookUp l ON ap.EntityId = l.id
                        inner join category c on l.CategoryId = c.Id
                        where a.TyphoonId = @Id
                        GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude";
            var populations = this.db.Query<RiskMapDBModel>(sql, new { id });
            PopulateRiskMapData(populations,mapData);

            #endregion

            #region Properties
            sql = @"select l.Name as category,c.sectionid, b.id as barangayid, b.Name as barangay,b.Latitude,b.Longitude, sum(ap.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentProperties ap on a.Id = ap.AssessmentId
                    inner join Barangay b on ap.BarangayId = b.Id
                    inner join LookUp l ON ap.StructureId = l.id
                    inner join category c on l.CategoryId = c.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude";
            var properties = this.db.Query<RiskMapDBModel>(sql, new { id });
            PopulateRiskMapData(properties, mapData);
            #endregion

            #region Transportation

            sql = @"select c.Name as category, c.sectionid, b.id as barangayid, b.Name as barangay,b.Latitude,b.Longitude, sum(at.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentTransportation at on a.Id = at.AssessmentId
                    inner join Barangay b on at.BarangayId = b.Id
                    inner join LookUp l ON at.FacilityId = l.id
                    inner join category c on l.CategoryId = c.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    UNION ALL
                    select c.Name as category, c.sectionid, b.id as barangayid,b.Name as barangay,b.Latitude,b.Longitude, sum(ac.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentCommunication ac on a.Id = ac.AssessmentId
                    inner join Barangay b on ac.BarangayId = b.Id
                    inner join LookUp l ON ac.FacilityId = l.id
                    inner join category c on l.CategoryId = c.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    UNION ALL
                    select c.Name as category,c.sectionid, b.id as barangayid, b.Name as barangay,b.Latitude,b.Longitude, sum(ae.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentElectricalPower ae on a.Id = ae.AssessmentId
                    inner join Barangay b on ae.BarangayId = b.Id
                    inner join LookUp l ON ae.FacilityId = l.id
                    inner join category c on l.CategoryId = c.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    UNION ALL
                    select c.Name as category,c.sectionid, b.id as barangayid, b.Name as barangay,b.Latitude,b.Longitude, sum(aw.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentWaterFacilities aw on a.Id = aw.AssessmentId
                    inner join Barangay b on aw.BarangayId = b.Id
                    inner join LookUp l ON aw.FacilityId = l.id
                    inner join category c on l.CategoryId = c.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    ORDER BY barangay,category";
            var lifelines = this.db.Query<RiskMapDBModel>(sql, new { id });
            PopulateRiskMapData(lifelines, mapData);

            #endregion

            #region Agriculture

            sql = @"select c.Name as category,c.sectionid, b.id as barangayid,b.Name as barangay,b.Latitude,b.Longitude, sum(ac.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentCrops ac on a.Id = ac.AssessmentId
                    inner join Barangay b on ac.BarangayId = b.Id
                    inner join LookUp l ON ac.CropsId = l.id
                    inner join category c on l.CategoryId = c.Id
                    inner join Section s on c.SectionId = s.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name,c.SectionId, b.Latitude,b.Longitude
                    UNION ALL
                    select c.Name as category,c.sectionid, b.id as barangayid,b.Name as barangay,b.Latitude,b.Longitude, sum(aF.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentFisheries af on a.Id = af.AssessmentId
                    inner join Barangay b on aF.BarangayId = b.Id
                    inner join LookUp l ON af.FisheryId = l.id
                    inner join category c on l.CategoryId = c.Id
                    inner join Section s on c.SectionId = s.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    UNION ALL
                    select c.Name as category,c.sectionid, b.id, b.Name as barangay,b.Latitude,b.Longitude,sum(al.EstimatedCost) total
                    from Assessment a
                    inner join AssessmentLivestock al on a.Id = al.AssessmentId
                    inner join Barangay b on al.BarangayId = b.Id
                    inner join LookUp l ON al.LivestockId = l.id
                    inner join category c on l.CategoryId = c.Id
                    inner join Section s on c.SectionId = s.Id
                    where a.TyphoonId = @Id
                    GROUP BY b.Id, c.sectionid,c.name, l.name, b.Name, b.Latitude,b.Longitude
                    ORDER BY barangay,category";
            var agriculture = this.db.Query<RiskMapDBModel>(sql, new { id });
            PopulateRiskMapData(agriculture, mapData);



            #endregion

            dataModel.Maps = mapData.AsEnumerable<RiskMapDataModel>();

            #endregion

            return dataModel;
        }

        public RiskTrendsModel GetTrends(int section, int category, int support)
        {
            var parameters = new DynamicParameters();
            parameters.Add("sectionCode", section);
            parameters.Add("categoryCode", category);
            parameters.Add("supportPercentage", support);
            parameters.Add("resetData", true);
            parameters.Add("id", DbType.Int32, direction: ParameterDirection.Output);
            db.Query<int>("GenerateTrends", parameters, commandType: CommandType.StoredProcedure);

            int id = parameters.Get<int>("id");

            var risktrends = new RiskTrendsModel();

            // Get the supports
            var sql = @"SELECT levelid,barangay,supportpercentage 
                        FROM RiskTrendsItems
                        WHERE RiskTrendsId = @id
                        ORDER BY LevelId";
            risktrends.RiskTrendsSupport = db.Query<RiskTrendsSupportModel>(sql, new { id });

            // Get the rules
            sql = @"SELECT id, RuleXBarangay, RuleYBarangay, SupportX, SupportY, Support, Confidence, Lift
                        FROM RiskTrendsRules
                        WHERE RiskTrendsId = @id";
            risktrends.RiskTrendsRules = db.Query<RiskTrendsRulesModel>(sql, new { id });

            if (risktrends.RiskTrendsRules.Any())
            {
                risktrends.supportStart = risktrends.RiskTrendsSupport.Min(x => x.LevelId);
                risktrends.supportEnd = risktrends.RiskTrendsSupport.Max(x => x.LevelId);
            }

            return risktrends;
        }

        public IEnumerable<ChartDataModel> GetCharts(int id)
        {
            List<ChartDataModel> charts = new List<ChartDataModel>();

            #region Maps

            #region Population

            var sql = @"select b.Name, c.SectionId,sum(total) as total
                        from Assessment a
                        inner join AssessmentPopulation ap ON a.Id = ap.AssessmentId
                        inner join Category c on ap.EntityId = c.Id
                        inner join Barangay b on ap.BarangayId = b.Id
                        where a.TyphoonId = @id
                        group by b.Id,b.Name,c.SectionId";
            var populations = this.db.Query<ChartDataModel>(sql, new { id });
            PopulateChartsData(populations, charts);

            #endregion

            #region Properties

            sql = @"select l.Name, c.SectionId, sum(ap.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentProperties ap ON a.Id = ap.AssessmentId
                    inner join LookUp l on ap.StructureId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by l.Id,l.Name,c.SectionId";
            var properties = this.db.Query<ChartDataModel>(sql, new { id });
            PopulateChartsData(properties, charts);
            #endregion

            #region Lifelines

            sql = @"select c.Name, c.SectionId, sum(at.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentTransportation at ON a.Id = at.AssessmentId
                    inner join LookUp l on at.FacilityId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId
                    UNION ALL
                    select c.Name, c.SectionId, sum(ac.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentCommunication ac ON a.Id = ac.AssessmentId
                    inner join LookUp l on ac.FacilityId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId
                    UNION ALL
                    select c.Name, c.SectionId, sum(ae.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentElectricalPower ae ON a.Id = ae.AssessmentId
                    inner join LookUp l on ae.FacilityId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId
                    UNION ALL
                    select c.Name, c.SectionId, sum(aw.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentWaterFacilities aw ON a.Id = aw.AssessmentId
                    inner join LookUp l on aw.FacilityId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId";
            var lifelines = this.db.Query<ChartDataModel>(sql, new { id });
            PopulateChartsData(lifelines, charts);

            #endregion

            #region Agriculture

            sql = @"select c.Name, c.SectionId, sum(ac.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentCrops ac ON a.Id = ac.AssessmentId
                    inner join LookUp l on ac.CropsId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId
                    UNION ALL
                    select c.Name, c.SectionId, sum(af.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentFisheries af ON a.Id = af.AssessmentId
                    inner join LookUp l on af.FisheryId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId
                    UNION ALL
                    select c.Name, c.SectionId, sum(al.EstimatedCost) as total
                    from Assessment a
                    inner join AssessmentLivestock al ON a.Id = al.AssessmentId
                    inner join LookUp l on al.LivestockId = l.Id
                    inner join Category c on l.CategoryId = c.Id
                    where a.TyphoonId = @id
                    group by c.Id,c.Name,c.SectionId";
            var agriculture = this.db.Query<ChartDataModel>(sql, new { id });
            PopulateChartsData(agriculture, charts);

            #endregion

            #endregion

            return charts.AsEnumerable<ChartDataModel>();
        }

        public IEnumerable<PopulationCommonModel> GetDisplacedEvacuated(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay,ap.Total
                        from Assessment a
                        inner join AssessmentPopulation ap ON a.Id = ap.AssessmentId
                        inner join Barangay b on ap.BarangayId = b.Id
                        inner join LookUp l on ap.EntityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id AND c.code = @DisplacedEvacuatedId";
            var items = this.db.Query<PopulationCommonModel>(sql, new { id, DisplacedEvacuatedId = CategoryConstants.DisplacedEntities });

            return items;            
        }

        public IEnumerable<PopulationCommonModel> GetCasualties(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay,ap.Total
                        from Assessment a
                        inner join AssessmentPopulation ap ON a.Id = ap.AssessmentId
                        inner join Barangay b on ap.BarangayId = b.Id
                        inner join LookUp l on ap.EntityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id AND c.code = @CasualtiesId";
            var items = this.db.Query<PopulationCommonModel>(sql, new { id, CasualtiesId = CategoryConstants.CasualtiesEntities });

            return items;
        }

        public IEnumerable<DamagedPropertiesModel> GetDamagedProperties(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        ap.totallydamaged,ap.totallydamagedunit,ap.criticallydamaged,
	                        ap.criticallydamagedunit, ap.EstimatedCost
                        from Assessment a
                        inner join AssessmentProperties ap ON a.Id = ap.AssessmentId
                        inner join Barangay b on ap.BarangayId = b.Id
                        inner join LookUp l on ap.StructureId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<DamagedPropertiesModel>(sql, new { id });

            return items;
        }

        public IEnumerable<TransportationModel> GetTransportation(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, at.description, b.Name as barangay, 
	                        at.IsPassable, at.LengthKM, at.EstimatedCost
                        from Assessment a
                        inner join AssessmentTransportation at ON a.Id = at.AssessmentId
                        inner join Barangay b on at.BarangayId = b.Id
                        inner join LookUp l on at.FacilityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<TransportationModel>(sql, new { id });

            return items;
        }

        public IEnumerable<LifelinesCommonModel> GetCommunication(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        ac.IsOperational, ac.Total, ac.EstimatedCost
                        from Assessment a
                        inner join AssessmentCommunication ac ON a.Id = ac.AssessmentId
                        inner join Barangay b on ac.BarangayId = b.Id
                        inner join LookUp l on ac.FacilityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id ";
            var items = this.db.Query<LifelinesCommonModel>(sql, new { id });

            return items;
        }

        public IEnumerable<LifelinesCommonModel> GetElectrical(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        ae.IsOperational,ae.total, ae.EstimatedCost
                        from Assessment a
                        inner join AssessmentElectricalPower ae ON a.Id = ae.AssessmentId
                        inner join Barangay b on ae.BarangayId = b.Id
                        inner join LookUp l on ae.FacilityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<LifelinesCommonModel>(sql, new { id });

            return items;
        }

        public IEnumerable<LifelinesCommonModel> GetWaterFacilities(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        aw.IsOperational,aw.total, aw.EstimatedCost
                        from Assessment a
                        inner join AssessmentWaterFacilities aw ON a.Id = aw.AssessmentId
                        inner join Barangay b on aw.BarangayId = b.Id
                        inner join LookUp l on aw.FacilityId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<LifelinesCommonModel>(sql, new { id });

            return items;
        }

        public IEnumerable<AgricultureCommonModel> GetCrops(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        ac.AreaDamaged,ac.MetricTons,ac.EstimatedCost
                        from Assessment a
                        inner join AssessmentCrops ac ON a.Id = ac.AssessmentId
                        inner join Barangay b on ac.BarangayId = b.Id
                        inner join LookUp l on ac.CropsId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<AgricultureCommonModel>(sql, new { id });
            return items;
        }

        public IEnumerable<AgricultureCommonModel> GetFisheries(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, 
	                        af.AreaDamaged,af.MetricTons,af.EstimatedCost
                        from Assessment a
                        inner join AssessmentFisheries af ON a.Id = af.AssessmentId
                        inner join Barangay b on af.BarangayId = b.Id
                        inner join LookUp l on af.FisheryId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<AgricultureCommonModel>(sql, new { id });
            return items;
        }

        public IEnumerable<LivestockModel> GetLivestocks(int id)
        {
            var sql = @"select l.Id as entityid, l.Name as entityname, b.Name as barangay, al.Total,al.EstimatedCost
                        from Assessment a
                        inner join AssessmentLivestock al ON a.Id = al.AssessmentId
                        inner join Barangay b on al.BarangayId = b.Id
                        inner join LookUp l on al.LivestockId = l.Id
                        inner join Category c on l.CategoryId = c.Id
                        where a.TyphoonId = @id";
            var items = this.db.Query<LivestockModel>(sql, new { id });
            return items;
        }

        #region Utilities

        private IEnumerable<RiskMapDataModel> PopulateRiskMapData(IEnumerable<RiskMapDBModel> source, ICollection<RiskMapDataModel> destination)
        {
            var summary = string.Empty;
            var barangayId = 0;
            RiskMapDBModel currentItem = null;
            foreach (var item in source)
            {
                currentItem = item;
                if (string.IsNullOrEmpty(summary) || item.BarangayId == barangayId)
                {
                    summary += item.Category + ": " + formatNumber(item.Total) + "<br>";
                }
                else
                {
                    destination.Add(SetRiskMapData(item,summary));

                    summary = item.Category + ": " + formatNumber(item.Total) + "<br>";
                }
                barangayId = item.BarangayId;
            }

            if (currentItem != null)
            {
                destination.Add(SetRiskMapData(currentItem, summary));
            }

            return destination.AsEnumerable<RiskMapDataModel>();
        }

        private RiskMapDataModel SetRiskMapData(RiskMapDBModel data,string summary)
        {
            var item = new RiskMapDataModel();
            item.SectionId = data.SectionId;
            item.Barangay = data.Barangay;
            item.Summary = summary;
            item.Latitude = data.Latitude;
            item.Longitude = data.Longitude;
            return item;
        }


        private void PopulateChartsData(IEnumerable<ChartDataModel> source, List<ChartDataModel> charts)
        {
            foreach (var item in source)
            {
                charts.Add(item);
            }
        }

        private string formatNumber(decimal number)
        {
            var result = string.Empty;
            if (number > 1000000)
                result = Math.Round(number / 1000000).ToString() + "M";
            else if (number > 1000)
                result = Math.Round(number / 1000).ToString() + "K";
            else
                result = number.ToString();
            return result;
        }

    #endregion
    }
}
