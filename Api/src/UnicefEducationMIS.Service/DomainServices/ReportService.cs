using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.QueryModel.Reporting;
using UnicefEducationMIS.Core.Reporting.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;
using UnicefEducationMIS.Service.Report;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class ReportService : IReportService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFiveWReportBuilder _fiveWReportBuilder;
        private readonly ICampWiseReportBuilder _campWiseReportBuilder;
        private readonly IScheduleInstanceRepository _instanceRepository;
        private readonly IIndicatorRepository _indicatorRepository;
        private readonly ICampRepository _campRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IDuplicationReportExporter _duplicationReportExporter;
        private readonly ITargetFrameworkRepository _targetFrameworkRepository;
        private readonly IFacilityDynamicCellRepository _facilityDynamicCellRepository;
        private readonly IDynamicColumnRepositories _dynamicColumnRepositories;
        private readonly IGapAnalysisReportExporter _gapAnalysisReportExporter;
        private readonly IDamageReportExporter _damageReportExporter;
        private readonly ISummaryReportExporter _summaryReportExporter;
        private readonly IAgeGroupRepository _ageGroupRepository;
        private readonly IModelToIndicatorConverter _modelToIndicatorConverter;
        private readonly IEducationSectorPartnerRepository _educationSectorPartnerRepository;
        private readonly IFacilityDataCollectionStatusRepository _facilityDataCollectionStatusRepository;
        private readonly IBeneficiaryDataCollectionStatusRepository _beneficiaryDataCollectionStatusRepository;

        private readonly IInstanceMappingRepository _instanceMappingRepository;

        public ReportService(
            IFacilityRepository facilityRepository,
            IFiveWReportBuilder fiveWReportBuilder,
            IScheduleInstanceRepository instanceRepository,
            IIndicatorRepository indicatorRepository,
            ICampRepository campRepository,
            IBlockRepository blockRepository,
            IBeneficiaryRepository beneficiaryRepository,
            IDuplicationReportExporter duplicationReportExporter
            , ITargetFrameworkRepository targetFrameworkRepository
            , IFacilityDynamicCellRepository facilityDynamicCellRepository
            , IDynamicColumnRepositories dynamicColumnRepositories
            , IGapAnalysisReportExporter gapAnalysisReportExporter
            , ICampWiseReportBuilder campWiseReportBuilder, IDamageReportExporter damageReportExporter, ISummaryReportExporter summaryReportExporter
            , IAgeGroupRepository ageGroupRepository, IModelToIndicatorConverter modelToIndicatorConverter
            , IEducationSectorPartnerRepository educationSectorPartnerRepository
            , IFacilityDataCollectionStatusRepository facilityDataCollectionStatusRepository
            , IBeneficiaryDataCollectionStatusRepository beneficiaryDataCollectionStatusRepository, IInstanceMappingRepository instanceMappingRepository)
        {
            _facilityRepository = facilityRepository;
            _fiveWReportBuilder = fiveWReportBuilder;
            _instanceRepository = instanceRepository;
            _indicatorRepository = indicatorRepository;
            _campRepository = campRepository;
            _blockRepository = blockRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _duplicationReportExporter = duplicationReportExporter;
            _targetFrameworkRepository = targetFrameworkRepository;
            _facilityDynamicCellRepository = facilityDynamicCellRepository;
            _dynamicColumnRepositories = dynamicColumnRepositories;
            _gapAnalysisReportExporter = gapAnalysisReportExporter;
            _campWiseReportBuilder = campWiseReportBuilder;
            _damageReportExporter = damageReportExporter;
            _summaryReportExporter = summaryReportExporter;
            _ageGroupRepository = ageGroupRepository;
            _modelToIndicatorConverter = modelToIndicatorConverter;
            _educationSectorPartnerRepository = educationSectorPartnerRepository;
            _facilityDataCollectionStatusRepository = facilityDataCollectionStatusRepository;
            _beneficiaryDataCollectionStatusRepository = beneficiaryDataCollectionStatusRepository;
            _instanceMappingRepository = instanceMappingRepository;
        }

        public async Task<byte[]> Generate5wReport(string fileFullPath, long facilityInstanceId)
        {
            //var data = await _instanceRepository.GetAll()
            //    .Where(x => x.Status == InstanceStatus.Completed && x.Schedule.ScheduleFor == EntityType.Facility)
            //    .Select(i => i.Id)
            //    .ToListAsync();
            List<IndicatorSelectViewModel> indicators = new List<IndicatorSelectViewModel>();
            List<FacilityViewModel> facilities = new List<FacilityViewModel>();


            indicators = await _dynamicColumnRepositories.GetAll()
                                        .Where(a => FiveWReportConstants.Indicators.Contains(a.Id))
                                        .Select(a => new IndicatorSelectViewModel
                                        {
                                            EntityDynamicColumnId = a.Id,
                                            IndicatorName = a.Name,
                                            IndicatorNameInBangla = a.NameInBangla,
                                            ColumnDataType = a.ColumnType,
                                            IsMultivalued = a.IsMultiValued,
                                            ListObject = a.ColumnList,
                                            ListItems = a.ColumnList.ListItems
                                        }).ToListAsync();

            var allWithValue = await _facilityRepository.GetFacilityByIndicator(new FacilityQueryModel { InstanceId = facilityInstanceId }, FiveWReportConstants.Indicators);
            facilities = allWithValue.Data.ToList();
            facilities.ForEach(_modelToIndicatorConverter.ReplaceFacilityFixedIndicatorIdsWithValues);
            return await _fiveWReportBuilder.BuildReport(fileFullPath, facilities, indicators);
        }

        public async Task<byte[]> GetDuplicationReport(DuplicationReportQueryModel model)
        {
            //TODO: beneficiary new schema
            var maxBeneficiaryInstanceId = await _instanceRepository.GetMaxInstanceId(EntityType.Beneficiary);
            var maxFacilityInstanceId = await _instanceRepository.GetMaxInstanceId(EntityType.Facility);

            var baseDataset = await
            (
                from facility in _facilityRepository.GetFacilityView().Where(model.ToExpression())
                join camp in _campRepository.GetAll() on facility.CampId equals camp.Id
                join block in _blockRepository.GetAll() on facility.BlockId equals block.Id
                join beneficiary in _beneficiaryRepository.GetBeneficiaryView() on facility.Id equals beneficiary.FacilityId
                join pp in _educationSectorPartnerRepository.GetAll() on facility.ProgramPartnerId equals pp.Id
                join ip in _educationSectorPartnerRepository.GetAll() on facility.ImplementationPartnerId equals ip.Id
                where facility.InstanceId== maxFacilityInstanceId && beneficiary.InstanceId== maxBeneficiaryInstanceId
                select new
                {
                    facility.FacilityCode,
                    FacilityName = facility.Name,
                    CampName = camp.Name,
                    facility.CampId,
                    BlockName = block.Name,
                    facility.BlockId,
                    beneficiary.UnhcrId,
                    beneficiary.LevelOfStudy,
                    BeneficiaryName = beneficiary.Name,
                    beneficiary.FatherName,
                    beneficiary.MotherName,
                    PpName = pp.PartnerName,
                    IpName = ip.PartnerName
                }
                ).ToListAsync();



            var facilityWiseDuplates = baseDataset.GroupBy(x => new { x.FacilityCode, x.BlockId, x.CampId })
                .Select(g => new FailityWiseDuplicate
                {
                    FacilityCode = g.Key.FacilityCode,
                    CampName = g.Select(x => x.CampName).FirstOrDefault(),
                    ProgrammingPartnerName = g.Select(x => x.PpName).FirstOrDefault(),
                    ImplementationPartnerName = g.Select(x => x.IpName).FirstOrDefault(),
                    FacilityName = g.Select(x => x.FacilityName).FirstOrDefault(),
                    BlockName = g.Select(x => x.BlockName).FirstOrDefault(),
                    DuplicateStudents = g.Select(x => x.UnhcrId)
                                            .GroupBy(x => x)
                                            .Where(x => x.Count() > 1)
                                            .Count()
                                            ,
                    TotalStudents = g.Count()
                }).ToList();

            facilityWiseDuplates.ForEach(x => x.UniqueStudents = x.TotalStudents - x.DuplicateStudents);


            var studentWiseDuplicates =
                baseDataset.GroupBy(x => x.UnhcrId)
                .SelectMany(g => g.Select(x => new StudentWiseDuplicate
                {
                    ProgressId = g.Key,
                    BeneficiaryName = x.BeneficiaryName,
                    FatherName = x.FatherName,
                    MotherName = x.MotherName,
                    EnrollmentCount = g.Count(),
                    BlockName = x.BlockName,
                    CampName = x.CampName,
                    ProgrammingPartnerName = x.PpName,
                    ImplementationPartnerName = x.IpName,
                    FacilityCode = x.FacilityCode,
                    FacilityName = x.FacilityName,
                    LevelOfStudy = x.LevelOfStudy.ToString()
                }).ToList()
                )
                .ToList();

            return await _duplicationReportExporter.ExportAll(facilityWiseDuplates, studentWiseDuplicates.Where(x => x.EnrollmentCount > 1).ToList());
        }

        public async Task<byte[]> GetGapAnalysisReport(GapAnalysisReportQueryModel model)
        {

            var facilitatorIndicatorIds = new List<long> { 95, 96, 97, 98 };

            var teachersQuery = (from x in _facilityDynamicCellRepository.GetAll()
                                 join f in _facilityRepository.GetFacilityView() on new { x.InstanceId, x.FacilityId } equals new { f.InstanceId, FacilityId = f.Id }
                                 join fdcs in _facilityDataCollectionStatusRepository.GetAll() on new { f.InstanceId, FacilityId = f.Id } equals new { fdcs.InstanceId, fdcs.FacilityId }
                                 where facilitatorIndicatorIds.Contains(x.EntityDynamicColumnId) &&
                                 f.InstanceId == model.InstanceId && fdcs.Status== CollectionStatus.Approved
                                 select new
                                 {
                                     f.CampId,
                                     x.Value
                                 }).ToList();

            var campWiseTeachers = teachersQuery.GroupBy(x => x.CampId)
                .Select(g => new
                {
                    CampId = g.Key,
                    Teachers = g.Sum(y => int.Parse(y.Value))
                }).ToList();

            var campWiseTargetQuery = await _targetFrameworkRepository.GetAll()
                .Select(x => new
                {
                    x.CampId,
                    x.Target
                }).ToListAsync();


            var campWiseTargets =
                campWiseTargetQuery.GroupBy(x => x.CampId)
                    .Select(g => new
                    {
                        CampId = g.Key,
                        Target = g.Sum(x => x.Target)
                    }).ToList();
            //TODO: beneficiary new schema

            var maxBeneficiaryInstanceId = await _instanceRepository.GetMaxInstanceId(EntityType.Beneficiary);
            //var mapBeneficiaryInstanceId= await _instanceMappingRepository.GetAll().Where(a => a.FacilityInstanceId == model.InstanceId)
            //    .Select(a => a.BeneficiaryInstanceId).FirstOrDefaultAsync();

            //var studentsQuery = await
            //    (from camp in _campRepository.GetAll()
            //     join beneficiary in _beneficiaryRepository.GetBeneficiaryView() on camp.Id equals beneficiary.BeneficiaryCampId
            //     join bdcs in _beneficiaryDataCollectionStatusRepository.GetAll() on new {beneficiary.InstanceId, BeneficiaryId=beneficiary.Id} equals new { bdcs.InstanceId,bdcs.BeneficiaryId}
            //     where beneficiary.InstanceId== maxBeneficiaryInstanceId && bdcs.Status == CollectionStatus.Approved
            //     select new
            //     {
            //         CampName = camp.Name,
            //         CampId = camp.Id,
            //         Student = beneficiary.Id
            //     }).ToListAsync();

            var studentsQuery = await
                (from camp in _campRepository.GetAll()
                 join beneficiary in _beneficiaryRepository.GetBeneficiaryView() on camp.Id equals beneficiary.BeneficiaryCampId
                 //join bdcs in _beneficiaryDataCollectionStatusRepository.GetAll() on new { beneficiary.InstanceId, BeneficiaryId = beneficiary.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
                 where beneficiary.InstanceId == maxBeneficiaryInstanceId //&& bdcs.Status == CollectionStatus.Approved
                 select new
                 {
                     CampName = camp.Name,
                     CampId = camp.Id,
                     Student = beneficiary.Id
                 }).ToListAsync();

            var campWiseStudents =
            studentsQuery.GroupBy(x => new { x.CampName, x.CampId })
                .Select(g => new
                {
                    g.Key.CampId,
                    g.Key.CampName,
                    Students = g.Count()
                })
                .ToList();

            var campData = await _campRepository.GetAll().ToListAsync();
            var reportData = (
                from camp in campData
                join cwstudent  in campWiseStudents on camp.Id equals cwstudent.CampId into def1
                from student in def1.DefaultIfEmpty()
                join cwtarget in campWiseTargets on camp.Id equals cwtarget.CampId into def2
                from target in def2.DefaultIfEmpty()
                join cwt in campWiseTeachers on camp.Id equals cwt.CampId into def3
                from teacher in def3.DefaultIfEmpty()
             
                select new GapAnalysisReport
                {
                    CampName = camp.Name,
                    Target = target!=null? target.Target:0,
                    Outreach = student!=null? student.Students:0,
                    Gap = (target != null ? target.Target : 0 )- (student != null ? student.Students : 0),
                    Ratio = $"1 teacher per {Math.Round( teacher !=null? ((double)(student != null ? student.Students : 0) / (double)(teacher.Teachers)) : 0,2)} student"
                }).ToList();

            //var reportData = (
            //    from target in campWiseTargets
            //    join cwt in campWiseTeachers on target.CampId equals cwt.CampId into def1
            //    from teacher in def1.DefaultIfEmpty()
            //    join cws in campWiseStudents on target.CampId equals cws.CampId into def2
            //    from student in def2.DefaultIfEmpty()
            //    select new GapAnalysisReport
            //    {
            //        CampName = student.CampName,
            //        Target = target.Target,
            //        Outreach = student.Students,
            //        Gap = target.Target - student.Students,
            //        Ratio = $"1 teacher per {Math.Round((double)(student!=null?student.Students:0 )/ (double)(target!=null? target.Target:1), 2)} students"
            //    }).ToList();

            return await _gapAnalysisReportExporter.ExportAll(reportData);
        }

        public async Task<byte[]> GenerateCampWiseReport(string fileFullPath, long facilityInstanceId)
        {

            List<IndicatorSelectViewModel> indicators = new List<IndicatorSelectViewModel>();
            List<FacilityViewModel> facilities = new List<FacilityViewModel>();


            var instancedData = await _instanceRepository.GetById(facilityInstanceId);
            var facilityDataCollectionDate = instancedData.DataCollectionDate;

            var targetData = await _targetFrameworkRepository.GetAll()
                        .Select(a => new TargetForCampWiseReport
                        {
                            CampId = a.CampId,
                            AgeGroup = a.AgeGroup,
                            Gender = a.Gender,
                            PeopleInNeed = a.PeopleInNeed,
                            Target = a.Target,
                            StartYear = a.StartYear
                        })
                        .Where(a => a.StartYear == facilityDataCollectionDate.Year).ToListAsync();

            var allWithValue = await _facilityRepository.GetFacilityByIndicator(new FacilityQueryModel { InstanceId = facilityInstanceId }, FiveWReportConstants.Indicators);
            facilities = allWithValue.Data.ToList();

            return await _campWiseReportBuilder.BuildReport(fileFullPath, facilities, targetData);
        }

        public async Task<byte[]> GetDamageReport(DamageReportQueryModel model)
        {

            var currentSchedule = await _instanceRepository.GetById(model.InstanceId);

            var month = currentSchedule.DataCollectionDate.Month;
            var year = currentSchedule.DataCollectionDate.Year;

            var damageTookPlaceId = 113L;
            var repairStartId = 117L;
            var repairEndId = 118L;

            var ids = new List<long>() { damageTookPlaceId, repairStartId, repairEndId };

            Expression<Func<FacilityDynamicCell, bool>> filter = x =>
            x.Instance.DataCollectionDate.Year == year &&
            x.Instance.DataCollectionDate.Month <= month &&
            ids.Contains(x.EntityDynamicColumnId);

            var baseQuery =
                (from facility in _facilityRepository.GetFacilityView()
                 join fdcs in _facilityDataCollectionStatusRepository.GetAll() on new { facility.InstanceId, FacilityId = facility.Id } equals new { fdcs.InstanceId, fdcs.FacilityId }
                 join pp in _educationSectorPartnerRepository.GetAll() on facility.ProgramPartnerId equals pp.Id
                 join ip in _educationSectorPartnerRepository.GetAll() on facility.ImplementationPartnerId equals ip.Id
                 join dc in _facilityDynamicCellRepository.GetAll().Where(filter)
                 on facility.Id equals dc.FacilityId
                 where facility.InstanceId == model.InstanceId && fdcs.Status == CollectionStatus.Approved 
                 select new
                 {
                     DamageTookPlace = dc.EntityDynamicColumnId == damageTookPlaceId ? DateTime.Parse(dc.Value) : (DateTime?)null,
                     RepairStartDate = dc.EntityDynamicColumnId == repairStartId ? DateTime.Parse(dc.Value) : (DateTime?)null,
                     RepairEndDate = dc.EntityDynamicColumnId == repairEndId ? DateTime.Parse(dc.Value) : (DateTime?)null,
                     dc.EntityDynamicColumnId,
                     dc.InstanceId,
                     facility.ProgramPartnerId,
                     facility.ImplementationPartnerId,
                     FacilityId = facility.Id,
                     FacilityName = facility.Name,
                     PpName = pp.PartnerName,
                     IpName = ip.PartnerName

                 }).ToList();

            var uniqueFacilities = baseQuery.Select(x => new
            {
                x.InstanceId,
                x.ProgramPartnerId,
                x.ImplementationPartnerId,
                x.FacilityId,
                x.FacilityName,
                x.PpName,
                x.IpName
            }).Distinct().ToList();


            var baseDataset = (
                               from f in uniqueFacilities
                               join damageTookPlace in baseQuery.Where(x => x.EntityDynamicColumnId == damageTookPlaceId)
                               on f.FacilityId equals damageTookPlace.FacilityId into dGroup
                               from d in dGroup.DefaultIfEmpty()
                               join repairStarted in baseQuery.Where(x => x.EntityDynamicColumnId == repairStartId)
                               on f.FacilityId equals repairStarted.FacilityId into sGroup
                               from s in sGroup.DefaultIfEmpty()
                               join repairEnded in baseQuery.Where(x => x.EntityDynamicColumnId == repairEndId)
                               on f.FacilityId equals repairEnded.FacilityId into eGroup
                               from e in eGroup.DefaultIfEmpty()
                               select new
                               {
                                   DamageTookPlace = d?.DamageTookPlace ?? null,
                                   RepairStartDate = s?.RepairStartDate ?? null,
                                   RepairEndDate = e?.RepairEndDate ?? null,
                                   f.InstanceId,
                                   f.ProgramPartnerId,
                                   f.ImplementationPartnerId,
                                   f.FacilityId,
                                   f.FacilityName,
                                   f.PpName,
                                   f.IpName,
                               }).Distinct().ToList();


            var cumulativeAffected = baseDataset.Where(x => x.DamageTookPlace.HasValue)
                .GroupBy(x => new
                {
                    x.FacilityId,
                    x.DamageTookPlace
                }).Count();

            var cumulativeRepaired = baseDataset.Where(x => x.RepairEndDate.HasValue)
                .GroupBy(x => new { x.FacilityId, x.RepairEndDate })
                .Count();

            var partnerWise = baseDataset.Where(x => x.InstanceId == model.InstanceId)
                .GroupBy(x => new { x.ProgramPartnerId, x.ImplementationPartnerId })
                .Select(g => new PartnerWiseDamage
                {
                    FacilityName = g.Select(x => x.FacilityName).FirstOrDefault(),
                    IpId = g.Key.ImplementationPartnerId,
                    PpName = g.Select(x => x.PpName).FirstOrDefault(),
                    IpName = g.Select(x => x.IpName).FirstOrDefault(),
                    PpId = g.Key.ProgramPartnerId,
                    Affected = g.Where(x => x.DamageTookPlace.HasValue && x.DamageTookPlace.Value.Year == year && x.DamageTookPlace.Value.Month == month).Count(),
                    RepairStarted = g.Where(x => x.RepairStartDate.HasValue && x.RepairStartDate.Value.Year == year && x.RepairStartDate.Value.Month == month).Count(),
                    RepairFinished = g.Where(x => x.RepairEndDate.HasValue).Count(),
                    RepairOngoing = g.Where(x => x.RepairStartDate.HasValue && !x.RepairEndDate.HasValue).Count()
                })
                .ToList();

            var damageSummary = new DamageSummary()
            {
                Affected = partnerWise.Sum(x => x.Affected),
                InstanceName = currentSchedule.DataCollectionDate.ToString("MMM/yyyy"),
                RepairFinished = partnerWise.Sum(x => x.RepairFinished),
                RepairOngoing = partnerWise.Sum(x => x.RepairOngoing),
                RepairStarted = partnerWise.Sum(x => x.RepairStarted),
                TotalAffected = cumulativeAffected,
                TotalRepaired = cumulativeRepaired
            };



            var yearly =
            baseDataset
                .Where(x => x.DamageTookPlace.HasValue && x.DamageTookPlace.Value.Year == year)
                .Select(x => new
                {
                    x.FacilityId,
                    x.DamageTookPlace
                }).Distinct()
                .GroupBy(x => x.FacilityId)
                .Select(g => new
                {
                    FacilityId = g.Key,
                    DamageOccured = g.Count()
                })
                .GroupBy(x => x.DamageOccured)
                .Select(g => new YearlyDamageSummary
                {
                    NumOfFacility = g.Count(),
                    TimesDamageOccured = g.Key
                }).ToList();

            partnerWise.Add(new PartnerWiseDamage
            {
                PpName = "Total",
                Affected = partnerWise.Sum(x => x.Affected),
                RepairStarted = partnerWise.Sum(x => x.RepairStarted),
                RepairFinished = partnerWise.Sum(x => x.RepairFinished),
                RepairOngoing = partnerWise.Sum(x => x.RepairOngoing),
            });

            return await _damageReportExporter.ExportAll(damageSummary, partnerWise, yearly);
        }

        private List<StudentEnrollmentSummary> StudentEnrollmentSummaries(List<FacilityIndicatorWithValue> data, List<Core.Models.Framework.AgeGroup> ageGroupList)
        {
            List<StudentEnrollmentSummary> studentEnrollments = new List<StudentEnrollmentSummary>();
            foreach (var ageGroup in SummeryReportConstants.IndicatorAgeGroupWise)
            {
                var totalRefugeeFemale = data.Where(a => a.EntityDynamicColumnId == ageGroup.Value[Gender.Female]["refugee"]
                                                 && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                var totalHostFemale = data.Where(a => a.EntityDynamicColumnId == ageGroup.Value[Gender.Female]["host"]
                                                 && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                var totalRefugeeMale = data.Where(a => a.EntityDynamicColumnId == ageGroup.Value[Gender.Male]["refugee"]
                                                 && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                var totalHostMale = data.Where(a => a.EntityDynamicColumnId == ageGroup.Value[Gender.Male]["host"]
                                                 && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));

                studentEnrollments.Add(new StudentEnrollmentSummary()
                {
                    AgeGroupId = (int)ageGroup.Key,
                    AgeGroup = ageGroupList.Where(a => a.Id == (int)ageGroup.Key).FirstOrDefault().Name,
                    HFemale = totalHostFemale,
                    HMale = totalHostMale,
                    RFemale = totalRefugeeFemale,
                    RMale = totalRefugeeMale
                });
            }
            return studentEnrollments;
        }

        private List<DisabilitySummary> StudentDisabilityEnrollSummaries(List<FacilityIndicatorWithValue> data, List<Core.Models.Framework.AgeGroup> ageGroupList)
        {

            List<DisabilitySummary> studentDisabilityEnrollments = new List<DisabilitySummary>();

            foreach (var ageGroup in SummeryReportConstants.DisabilityIndicatorAgeGroupWise)
            {
                var camdWiseData = data.Where(a => SummeryReportConstants.DisabilityIndicatorList.Contains(a.EntityDynamicColumnId)
                                                && decimal.TryParse(a.Value, out var val) && val >= 0)
                    .GroupBy(a => new { a.CampId, a.CampName, a.EntityDynamicColumnId, a.EntityDynamicColumnName })
                    .Select(a => new FacilityIndicatorWithValue
                    {
                        CampId = a.Key.CampId,
                        CampName = a.Key.CampName,
                        EntityDynamicColumnId = a.Key.EntityDynamicColumnId,
                        EntityDynamicColumnName = a.Key.EntityDynamicColumnName,
                        Value = a.Sum(b => Convert.ToInt32(b.Value)).ToString()
                    }).ToList();

                var distinctCamp = camdWiseData.Select(a => new { a.CampId, a.CampName }).Distinct().ToList();
                foreach (var camp in distinctCamp)
                {
                    var totalRefugeeFemale = camdWiseData.Where(a => a.CampId == camp.CampId && a.EntityDynamicColumnId == ageGroup.Value[Gender.Female]["refugee"]
                                                 && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                    var totalHostFemale = camdWiseData.Where(a => a.CampId == camp.CampId && a.EntityDynamicColumnId == ageGroup.Value[Gender.Female]["host"]
                                                     && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                    var totalRefugeeMale = camdWiseData.Where(a => a.CampId == camp.CampId && a.EntityDynamicColumnId == ageGroup.Value[Gender.Male]["refugee"]
                                                     && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));
                    var totalHostMale = camdWiseData.Where(a => a.CampId == camp.CampId && a.EntityDynamicColumnId == ageGroup.Value[Gender.Male]["host"]
                                                     && decimal.TryParse(a.Value, out var val) && val >= 0).Sum(a => Convert.ToInt32(a.Value));

                    studentDisabilityEnrollments.Add(new DisabilitySummary()
                    {
                        CampName = camp.CampName,
                        AgeGroupId = (int)ageGroup.Key,
                        AgeGroup = ageGroupList.Where(a => a.Id == (int)ageGroup.Key).FirstOrDefault().Name,
                        HFemale = totalHostFemale,
                        HMale = totalHostMale,
                        RFemale = totalRefugeeFemale,
                        RMale = totalRefugeeMale
                    });
                }

            }

            return studentDisabilityEnrollments;
        }

        private List<FacilitatorSummary> FacilitatorSummaries(List<FacilityIndicatorWithValue> data)
        {
            var facilitators = data.Where(a => SummeryReportConstants.FacilitatorIndicatorList.Contains(a.EntityDynamicColumnId)
                                                     && decimal.TryParse(a.Value, out var val) && val >= 0)
                        .GroupBy(a => 1)
                        .Select(a => new FacilitatorSummary
                        {
                            HFemale = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColHostFemaleFacilitator)
                                      .Sum(b => Convert.ToInt32(b.Value)),
                            HMale = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColHostMaleFacilitator)
                                      .Sum(b => Convert.ToInt32(b.Value)),
                            RFemale = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRefugeeFemaleFacilitator)
                                      .Sum(b => Convert.ToInt32(b.Value)),
                            RMale = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRefugeeMaleFacilitator)
                                      .Sum(b => Convert.ToInt32(b.Value))
                        }).ToList();
            return facilitators;
        }
        private async Task<List<FacilityTypewiseStatus>> FacilityTypeWiseSummery()
        {

            var data = await _facilityRepository.GetFacilityView().ToListAsync();

            var returnData = data.Where(a => a.FacilityType != null)
                            .GroupBy(a => a.FacilityType)
                            .Select(a => new FacilityTypewiseStatus
                            {
                                Type = a.Key != null ? a.Key.ToString() : "",
                                Status = a.Count(facility => facility.FacilityStatus == FacilityStatus.Completed)
                            }).ToList();
            return returnData;
        }


        private async Task<List<JRP>> HostJrpSummary(List<TargetFramework> jrpRaw, List<StudentEnrollmentSummary> studentEnrollmentSummery)
        {
            var jrp = jrpRaw
                .Where(a => a.TargetedPopulation != TargetedPopulation.Refugee_Communities)
                .GroupBy(a => a.AgeGroupId)
                .Select(a => new JRP
                {
                    AgeGroupId = a.Select(a => a.AgeGroupId).FirstOrDefault(),
                    AgeGroup = a.Select(b => b.AgeGroup.Name).FirstOrDefault(),
                    Target = a.Sum(b => b.Target),
                });

            var jrpWithReachData = jrp.Join(studentEnrollmentSummery, a => a.AgeGroupId, b => b.AgeGroupId, (a, b) => new JRP
            {
                AgeGroupId = a.AgeGroupId,
                AgeGroup = a.AgeGroup,
                Target = a.Target,
                Reached = (b.HMale + b.HFemale)
            }).ToList();

            return jrpWithReachData;
        }


        private async Task<List<TargetFramework>> GetJrpSummary(long instanceId)
        {
            var instanceData = await _instanceRepository.GetById(instanceId);
            var jrpRaw = await _targetFrameworkRepository.GetAll()
                .Include(a => a.AgeGroup)
                .Where(a => a.StartYear == instanceData.DataCollectionDate.Year)
                .ToListAsync();
            return jrpRaw;
        }
        private async Task<List<JRP>> RefugeeJrpSummary(List<TargetFramework> jrpRaw, List<StudentEnrollmentSummary> studentEnrollmentSummery)
        {
            var jrp = jrpRaw
                .Where(a => a.TargetedPopulation != TargetedPopulation.Host_Communities)
                .GroupBy(a => a.AgeGroupId)
                 .Select(a => new JRP
                 {
                     AgeGroupId = a.Select(a => a.AgeGroupId).FirstOrDefault(),
                     AgeGroup = a.Select(b => b.AgeGroup.Name).FirstOrDefault(),
                     Target = a.Sum(b => b.Target),
                 });

            var jrpWithReachData = jrp.Join(studentEnrollmentSummery, a => a.AgeGroupId, b => b.AgeGroupId, (a, b) => new JRP
            {
                AgeGroupId = a.AgeGroupId,
                AgeGroup = a.AgeGroup,
                Target = a.Target,
                Reached = (b.RMale + b.RFemale)
            }).ToList();

            return jrpWithReachData;
        }

        private List<WashSummary> WashSummaries(List<FacilityIndicatorWithValue> data)
        {
            var washSummary = data.Where(a => SummeryReportConstants.WashIndicator.Contains(a.EntityDynamicColumnId)
                                                  && decimal.TryParse(a.Value, out var val) && val >= 0)
                    .GroupBy(a => new { a.CampId, a.CampName, a.ProgramPartnerId, a.ProgramPartenrName, a.ImplementationPartnerId, a.ImplementationPartnerName })
                    .Select(a => new WashSummary
                    {
                        CampId = a.Key.CampId,
                        CampName = a.Key.CampName,
                        PP = a.Key.ProgramPartenrName,
                        IP = a.Key.ImplementationPartnerName,
                        CommunityLatrines = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColCommunityLatrines)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        BoysLatrines = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColBoysLatrines)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        GirlsLatrines = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColGirlsLatrines)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        HandWashingStations = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColHandWashingStations)
                                  .Sum(b => Convert.ToInt32(b.Value))
                    }).ToList();

            return washSummary;
        }

        private List<StudyLevelWiseSummary> StudyLevelWiseSummaries(List<FacilityIndicatorWithValue> data)
        {
            var studyLevelWiseSummary = data.Where(a => SummeryReportConstants.StudyLevelIndicators.Contains(a.EntityDynamicColumnId)
                                                    && decimal.TryParse(a.Value, out var val) && val >= 0)
                     .GroupBy(a => new { a.ProgramPartenrName, a.ImplementationPartnerName })
                     .Select(a => new StudyLevelWiseSummary
                     {
                         PP = a.Key.ProgramPartenrName,
                         IP = a.Key.ImplementationPartnerName,
                         RGLevel1 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRGirlsLevel1)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RBLevel1 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRBoysLevel1)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RGLevel2 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRGirlsLevel2)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RBLevel2 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRBoysLevel2)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RGLevel3 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRGirlsLevel3)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RBLevel3 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRBoysLevel3)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RGLevel4 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRGirlsLevel4)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         RBLevel4 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRBoysLevel4)
                                   .Sum(b => Convert.ToInt32(b.Value)),
                         TotalFacility = a.Select(b => b.FacilityId).Distinct().Count()
                     }).ToList();
            return studyLevelWiseSummary;
        }
        private List<DrrCareGiverSummary> DRRcareGiverSummaries(List<FacilityIndicatorWithValue> data)
        {
            var drrCareGiverSummary = data.Where(a => SummeryReportConstants.DrrCareGiverIndicators.Contains(a.EntityDynamicColumnId)
                                                   && decimal.TryParse(a.Value, out var val) && val >= 0)
                    .GroupBy(a => new { a.CampId, a.CampName, a.ProgramPartnerId, a.ProgramPartenrName, a.ImplementationPartnerId, a.ImplementationPartnerName })
                    .Select(a => new DrrCareGiverSummary
                    {
                        CampName = a.Key.CampName,
                        PP = a.Key.ProgramPartenrName,
                        IP = a.Key.ImplementationPartnerName,
                        NumOfDrr = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColNumOfDrr)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        NumOfEduCommitee = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColNumOfEduCommitee)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        FemaleRCareGiver = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColFemaleRCareGiver)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        MaleRCareGiver = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColMaleRCareGiver)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        FemaleHCareGiver = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColFemaleHCareGiver)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        MaleHCareGiver = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColMaleHCareGiver)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        RGirls3_24 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRGirls3_24)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        RBoys3_24 = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColRBoys3_24)
                                  .Sum(b => Convert.ToInt32(b.Value)),

                        BenefittingFromFood = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColBenefittingFromFood)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        BenefittingFromClassRoom = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColBenefittingFromClassRoom)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        BenefittingFood = a.Where(b => b.EntityDynamicColumnId == SummeryReportConstants.ColBenefittingFood)
                                  .Sum(b => Convert.ToInt32(b.Value)),
                        TotalFacility = a.Select(b => b.FacilityId).Distinct().Count()
                    }).ToList();

            return drrCareGiverSummary;
        }
        public async Task<byte[]> GetSummaryReport(SummaryReportQueryModel model)
        {
            var ageGroupList = await _ageGroupRepository.GetAll().ToListAsync();
            var indicatorsWithValue = await _facilityRepository.GetFacilityIndicatorWithValue(new FacilityQueryModel(model.InstanceId), SummeryReportConstants.Indicators);
            var data = indicatorsWithValue.Data.ToList();


            var studentEnrollmentSummery = StudentEnrollmentSummaries(data, ageGroupList);

            var studentDisabilityEnrollSummery = StudentDisabilityEnrollSummaries(data, ageGroupList);

            var facilitatorSummery = FacilitatorSummaries(data);

            var facilityTypeWiseStatus = await FacilityTypeWiseSummery();


            var jrpSummary = await GetJrpSummary(model.InstanceId);

            var refugeeJrpSummary = await RefugeeJrpSummary(jrpSummary, studentEnrollmentSummery);
            var hostJrpSummary = await HostJrpSummary(jrpSummary, studentEnrollmentSummery);

            var washSummary = WashSummaries(data);

            var studyLevelWiseSummary = StudyLevelWiseSummaries(data);

            var drrCareGiverSummery = DRRcareGiverSummaries(data);
            var instance = await _instanceRepository.GetById(model.InstanceId);
            var summaryReport = new SummaryReport()
            {
                Instance = instance,
                EnrollmentSummary = studentEnrollmentSummery,
                DisabilitySummary = studentDisabilityEnrollSummery,
                FacilitatorSummary = facilitatorSummery,
                FacilityTypewiseStatus = facilityTypeWiseStatus,
                RefugeeJrps = refugeeJrpSummary,
                WashSummary = washSummary,
                StudyLevelWiseSummary = studyLevelWiseSummary,
                DrrCareGiverSummary = drrCareGiverSummery,
                HostJrps = hostJrpSummary,

            };


            return await _summaryReportExporter.ExportAll(summaryReport);
        }
    }
}
