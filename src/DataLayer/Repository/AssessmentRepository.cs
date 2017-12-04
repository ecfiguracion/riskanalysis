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
    public class AssessmentRepository : IAssessmentRepository
    {
        private IDbConnection db;

        public AssessmentRepository(IConfiguration appconfig)
        {            
            db = new SqlConnection(appconfig.GetConnectionString(AppConfigConstants.DefaultConnection));
        }

        public PagedResult<AssessmentListModel> GetAll(PagedParams pagedParams)
        {
            var sql = @"SELECT a.id, t.name as typhoonName, a.remarks
                        FROM Assessment a
                        INNER JOIN Typhoon t ON a.typhoonId = t.Id";
            var sqlWhereClause = string.Empty;
            // add filtering
            if (!string.IsNullOrEmpty(pagedParams.searchString))
            {
                sqlWhereClause += " AND t.name LIKE @searchString ";
            }
            sql += sqlWhereClause;


            // implement paging
            sql += @" ORDER BY t.name
                      OFFSET @pageNo ROWS
                      FETCH NEXT @pageSize ROWS ONLY";

            pagedParams.SetPageNo();
            var pageNo = (pagedParams.pageNo - 1) * pagedParams.pageSize;
            var searchString = "%" + pagedParams.searchString + "%";
            var assessments = db.Query<AssessmentListModel>(sql, new {  searchString, pageNo, pagedParams.pageSize });

            return new PagedResult<AssessmentListModel>(assessments, pagedParams, this.GetTotalCount(sqlWhereClause,searchString));
        }

        public int GetTotalCount(string whereClause, string searchString)
        {
            var sql = @"SELECT COUNT(a.id)
                        FROM Assessment a
                        INNER JOIN Typhoon t ON a.typhoonId = t.Id " + 
                        whereClause;
            return db.Query<int>(sql,new { searchString } ).Single();
        }

        public AssessmentModel GetById(int id)
        {
            var sql = @"SELECT a.Id, a.Remarks, t.Id, t.Name
                        FROM Assessment a
                        INNER JOIN Typhoon t ON a.TyphoonId = t.id
                        WHERE a.id = @id";
            var assessment = db.Query<AssessmentModel, DataLookUpModel, AssessmentModel>(
                sql, (data, typhoon) =>
                 {
                     data.Typhoon = typhoon.Id;
                     return data;
                 },
                new { id }).Single();

            #region Population

            sql = @"SELECT ap.Id, ap.Total, l.Id, l.Name, b.Id, b.Name
                    FROM AssessmentPopulation ap
                    INNER JOIN LookUp l ON ap.EntityId = l.Id
                    INNER JOIN Barangay b ON ap.BarangayId = b.Id
                    WHERE ap.AssessmentId = @id";

            var population = db.Query<AssessmentPopulationModel, DataLookUpModel, DataLookUpModel, AssessmentPopulationModel>(
                sql, (data, entity, barangay) =>
                 {
                     data.entity = entity;
                     data.barangay = barangay;
                     return data;
                 },
                new { id }).ToList();

            assessment.Population = population;

            #endregion

            #region Properties

            sql = @"SELECT ap.Id, ap.TotallyDamaged,ap.TotallyDamagedUnit, ap.CriticallyDamaged, ap.CriticallyDamagedUnit,
                        ap.EstimatedCost, l.id, l.Name, b.id, b.Name
                    FROM AssessmentProperties ap
                    INNER JOIN LookUp l ON ap.StructureId = l.Id
                    INNER JOIN Barangay b ON ap.BarangayId = b.Id
                    WHERE ap.AssessmentId  = @id";

            var properties = db.Query<AssessmentPropertiesModel, DataLookUpModel, DataLookUpModel, AssessmentPropertiesModel>(
                sql, (data, structure, barangay) =>
                {
                    data.structure = structure;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Properties = properties;

            #endregion

            #region Transportation

            sql = @"SELECT at.Id, at.Description, at.IsPassable, at.LengthKM, at.EstimatedCost, 
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentTransportation at
                    INNER JOIN LookUp l ON at.FacilityId = l.Id
                    INNER JOIN Barangay b ON at.BarangayId = b.Id
                    WHERE at.AssessmentId  = @id";

            var transportation = db.Query<AssessmentTransportationModel, DataLookUpModel, DataLookUpModel, AssessmentTransportationModel>(
                sql, (data, facility, barangay) =>
                {
                    data.facility = facility;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Transportation = transportation;

            #endregion

            #region Communication

            sql = @"SELECT ac.Id, ac.IsOperational, ac.Total, ac.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentCommunication ac
                    INNER JOIN LookUp l ON ac.FacilityId = l.Id
                    INNER JOIN Barangay b ON ac.BarangayId = b.Id
                    WHERE ac.AssessmentId  = @id";

            var communication = db.Query<AssessmentCommunicationModel, DataLookUpModel, DataLookUpModel, AssessmentCommunicationModel>(
                sql, (data, facility, barangay) =>
                {
                    data.facility = facility;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Communication = communication;

            #endregion

            #region Electrical

            sql = @"SELECT ae.Id, ae.IsOperational, ae.Total, ae.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentElectricalPower ae
                    INNER JOIN LookUp l ON ae.FacilityId = l.Id
                    INNER JOIN Barangay b ON ae.BarangayId = b.Id
                    WHERE ae.AssessmentId  = @id";

            var electrical = db.Query<AssessmentElectricalModel, DataLookUpModel, DataLookUpModel, AssessmentElectricalModel>(
                sql, (data, facility, barangay) =>
                {
                    data.facility = facility;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.ElectricalPower = electrical;

            #endregion

            #region Water Facilities

            sql = @"SELECT aw.Id, aw.IsOperational, aw.Total, aw.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentWaterFacilities aw
                    INNER JOIN LookUp l ON aw.FacilityId = l.Id
                    INNER JOIN Barangay b ON aw.BarangayId = b.Id
                    WHERE aw.AssessmentId  = @id";

            var waterfacilities = db.Query<AssessmentWaterFacilitiesModel, DataLookUpModel, DataLookUpModel, AssessmentWaterFacilitiesModel>(
                sql, (data, facility, barangay) =>
                {
                    data.facility = facility;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.WaterFacilities = waterfacilities;

            #endregion

            #region Crops

            sql = @"SELECT ac.Id, ac.AreaDamaged, ac.MetricTons, ac.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentCrops ac
                    INNER JOIN LookUp l ON ac.CropsId = l.Id
                    INNER JOIN Barangay b ON ac.BarangayId = b.Id
                    WHERE ac.AssessmentId  = @id";

            var crops = db.Query<AssessmentCropsModel, DataLookUpModel, DataLookUpModel, AssessmentCropsModel>(
                sql, (data, crop, barangay) =>
                {
                    data.crops = crop;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Crops = crops;

            #endregion

            #region Fisheries

            sql = @"SELECT af.Id, af.AreaDamaged, af.MetricTons, af.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentFisheries af
                    INNER JOIN LookUp l ON af.FisheryId = l.Id
                    INNER JOIN Barangay b ON af.BarangayId = b.Id
                    WHERE af.AssessmentId  = @id";

            var fisheries = db.Query<AssessmentFisheriesModel, DataLookUpModel, DataLookUpModel, AssessmentFisheriesModel>(
                sql, (data, fishery, barangay) =>
                {
                    data.fishery = fishery;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Fisheries = fisheries;

            #endregion

            #region Livestock

            sql = @"SELECT al.Id, al.Total, al.EstimatedCost,
	                    l.id, l.Name, b.id, b.Name
                    FROM AssessmentLivestock al
                    INNER JOIN LookUp l ON al.LivestockId = l.Id
                    INNER JOIN Barangay b ON al.BarangayId = b.Id
                    WHERE al.AssessmentId  = @id";

            var livestocks = db.Query<AssessmentLivestockModel, DataLookUpModel, DataLookUpModel, AssessmentLivestockModel>(
                sql, (data, livestock, barangay) =>
                {
                    data.livestock = livestock;
                    data.barangay = barangay;
                    return data;
                },
                new { id }).ToList();

            assessment.Livestocks = livestocks;

            #endregion

            return assessment;
        }

        public AssessmentModel Add(AssessmentModel assessment)
        {
            var sql = @"INSERT INTO assessment(typhoonid,remarks)
                        VALUES (@Typhoon,@Remarks)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                assessment.Typhoon,
                assessment.Remarks
            }).Single();
            assessment.Id = id;

            this.SavePopulationAssessment(assessment);
            this.SavePropertiesAssessment(assessment);
            this.SaveTransportationAssessment(assessment);
            this.SaveCommunicationAssessment(assessment);
            this.SaveElectricalAssessment(assessment);
            this.SaveWaterAssessment(assessment);
            this.SaveCropsAssessment(assessment);
            this.SaveFisheriesAssessment(assessment);
            this.SaveLivestockAssessment(assessment);

            return assessment;
        }

        public AssessmentModel Update(AssessmentModel assessment)
        {            
            var sql = @"UPDATE assessment SET 
                            typhoonid = @Typhoon,
                            remarks = @Remarks
                        WHERE id = @id";
            db.Execute(sql, new { assessment.Typhoon, assessment.Remarks, assessment.Id } );

            this.SavePopulationAssessment(assessment);
            this.SavePropertiesAssessment(assessment);
            this.SaveTransportationAssessment(assessment);
            this.SaveCommunicationAssessment(assessment);
            this.SaveElectricalAssessment(assessment);
            this.SaveWaterAssessment(assessment);
            this.SaveCropsAssessment(assessment);
            this.SaveFisheriesAssessment(assessment);
            this.SaveLivestockAssessment(assessment);

            return assessment;
        }

        private void SavePopulationAssessment(AssessmentModel assessment)
        {
            if (assessment.Population == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Population)
            {
                var entityId = item.entity.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentPopulation(AssessmentId, EntityId, BarangayId, Total) 
                            VALUES (@Id, @EntityId, @BarangayId, @Total)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new { assessment.Id, entityId, barangayId, item.total }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentPopulation SET
                                    EntityId = @EntityId, 
                                    BarangayId = @BarangayId,
                                    Total = @Total
                            WHERE Id = @Id";
                        db.Execute(sql, new { entityId, barangayId, item.total, item.id });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentPopulation WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SavePropertiesAssessment(AssessmentModel assessment)
        {
            if (assessment.Properties == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Properties)
            {
                var structureId = item.structure.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentProperties(AssessmentId, StructureId, BarangayId, TotallyDamaged,TotallyDamagedUnit, CriticallyDamaged,CriticallyDamagedUnit,EstimatedCost) 
                            VALUES (@Id, @StructureId, @BarangayId, @TotallyDamaged,@TotallyDamagedUnit, @CriticallyDamaged,@CriticallyDamagedUnit, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new {
                                assessment.Id, structureId, barangayId, item.totallyDamaged,item.totallyDamagedUnit,
                                item.criticallyDamaged,item.criticallyDamagedUnit, item.estimatedCost
                            }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentProperties SET
                                    StructureId = @StructureId, 
                                    BarangayId = @BarangayId,
                                    TotallyDamaged = @TotallyDamaged,
                                    TotallyDamagedUnit = @TotallyDamagedUnit,    
                                    CriticallyDamaged = @CriticallyDamaged,                                    
                                    CriticallyDamagedUnit = @CriticallyDamagedUnit,                                    
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new {
                            structureId,barangayId,item.totallyDamaged,item.totallyDamagedUnit,item.criticallyDamaged,item.criticallyDamagedUnit,item.estimatedCost,item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentProperties WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveTransportationAssessment(AssessmentModel assessment)
        {
            if (assessment.Transportation == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Transportation)
            {
                var facilityId = item.facility.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentTransportation(AssessmentId, Description, FacilityId, 
                                        BarangayId, IsPassable, LengthKM, EstimatedCost) 
                            VALUES (@Id, @Description, @FacilityId, @BarangayId, @IsPassable, @LengthKM, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        item.description,
                        facilityId,
                        barangayId,
                        item.isPassable,
                        item.lengthKM,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentTransportation SET
                                    Description = @Description,
                                    FacilityId = @FacilityId, 
                                    BarangayId = @BarangayId,
                                    IsPassable = @IsPassable,
                                    LengthKM = @LengthKM,                                    
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            item.description,
                            facilityId,
                            barangayId,
                            item.isPassable,
                            item.lengthKM,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentTransportation WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveCommunicationAssessment(AssessmentModel assessment)
        {
            if (assessment.Communication == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Communication)
            {
                var facilityId = item.facility.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentCommunication(AssessmentId, FacilityId, 
                                        BarangayId, IsOperational, Total, EstimatedCost) 
                            VALUES (@Id, @FacilityId, @BarangayId, @IsOperational, @Total, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        facilityId,
                        barangayId,
                        item.isOperational,
                        item.total,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentCommunication SET
                                    FacilityId = @FacilityId, 
                                    BarangayId = @BarangayId,
                                    IsOperational = @IsOperational,
                                    Total = @Total,                                    
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            facilityId,
                            barangayId,
                            item.isOperational,
                            item.total,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentCommunication WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveElectricalAssessment(AssessmentModel assessment)
        {
            if (assessment.ElectricalPower == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.ElectricalPower)
            {
                var facilityId = item.facility.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentElectricalPower(AssessmentId, FacilityId, 
                                        BarangayId, IsOperational, Total, EstimatedCost) 
                            VALUES (@Id, @FacilityId, @BarangayId, @IsOperational, @Total, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        facilityId,
                        barangayId,
                        item.isOperational,
                        item.total,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentElectricalPower SET
                                    FacilityId = @FacilityId, 
                                    BarangayId = @BarangayId,
                                    IsOperational = @IsOperational,
                                    Total = @Total,                                    
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            facilityId,
                            barangayId,
                            item.isOperational,
                            item.total,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentElectricalPower WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveWaterAssessment(AssessmentModel assessment)
        {
            if (assessment.WaterFacilities == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.WaterFacilities)
            {
                var facilityId = item.facility.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentWaterFacilities(AssessmentId, FacilityId, 
                                        BarangayId, IsOperational, Total, EstimatedCost) 
                            VALUES (@Id, @FacilityId, @BarangayId, @IsOperational, @Total, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        facilityId,
                        barangayId,
                        item.isOperational,
                        item.total,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentWaterFacilities SET
                                    FacilityId = @FacilityId, 
                                    BarangayId = @BarangayId,
                                    IsOperational = @IsOperational,
                                    Total = @Total,                                    
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            facilityId,
                            barangayId,
                            item.isOperational,
                            item.total,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentWaterFacilities WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveCropsAssessment(AssessmentModel assessment)
        {
            if (assessment.Crops == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Crops)
            {
                var cropsId = item.crops.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentCrops(AssessmentId, CropsId, 
                                        BarangayId, AreaDamaged, MetricTons, EstimatedCost) 
                            VALUES (@Id, @CropsId, @BarangayId, @AreaDamaged, @MetricTons, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        cropsId,
                        barangayId,
                        item.areaDamaged,
                        item.metricTons,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentCrops SET
                                    CropsId = @CropsId, 
                                    BarangayId = @BarangayId,
                                    AreaDamaged = @AreaDamaged,
                                    MetricTons = @MetricTons,                        
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            cropsId,
                            barangayId,
                            item.areaDamaged,
                            item.metricTons,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentCrops WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveFisheriesAssessment(AssessmentModel assessment)
        {
            if (assessment.Fisheries == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Fisheries)
            {
                var fisheryId = item.fishery.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentFisheries(AssessmentId, FisheryId, 
                                        BarangayId, AreaDamaged, MetricTons, EstimatedCost) 
                            VALUES (@Id, @FisheryId, @BarangayId, @AreaDamaged, @MetricTons, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        fisheryId,
                        barangayId,
                        item.areaDamaged,
                        item.metricTons,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentFisheries SET
                                    FisheryId = @FisheryId, 
                                    BarangayId = @BarangayId,
                                    AreaDamaged = @AreaDamaged,
                                    MetricTons = @MetricTons,                        
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            fisheryId,
                            barangayId,
                            item.areaDamaged,
                            item.metricTons,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentFisheries WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }

        private void SaveLivestockAssessment(AssessmentModel assessment)
        {
            if (assessment.Livestocks == null) return;

            var sql = string.Empty;
            foreach (var item in assessment.Livestocks)
            {
                var livestockId = item.livestock.Id;
                var barangayId = item.barangay.Id;
                if (item.id == 0)
                {
                    sql = @"INSERT INTO AssessmentLivestock(AssessmentId, LivestockId, BarangayId, Total, EstimatedCost) 
                            VALUES (@Id, @LivestockId, @BarangayId, @Total, @EstimatedCost)
                            SELECT CAST(SCOPE_IDENTITY() AS int)";
                    var id = db.Query<int>(sql, new
                    {
                        assessment.Id,
                        livestockId,
                        barangayId,
                        item.total,
                        item.estimatedCost
                    }).Single();
                    item.id = id;
                }
                else
                {
                    if (!item.isdeleted)
                    {
                        sql = @"UPDATE AssessmentLivestock SET
                                    LivestockId = @LivestockId, 
                                    BarangayId = @BarangayId,
                                    Total = @Total,                  
                                    EstimatedCost = @EstimatedCost
                            WHERE Id = @Id";
                        db.Execute(sql, new
                        {
                            livestockId,
                            barangayId,
                            item.total,
                            item.estimatedCost,
                            item.id
                        });
                    }
                    else
                    {
                        sql = "DELETE FROM AssessmentLivestock WHERE Id = @Id";
                        db.Execute(sql, new { item.id });
                    }
                }
            }
        }


        public void Remove(int id)
        {
            var sql = @"DELETE FROM assessment
                        WHERE id = @id";
            db.Query<AssessmentModel>(sql, new { id });
        }
    }
}
