using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Data.Factory;
using UnicefEducationMIS.Core.Specifications;
using System;
using System.Linq.Expressions;
using UnicefEducationMIS.Core.AppConstants;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.Extensions;
using System.Globalization;

namespace UnicefEducationMIS.Data.Repositories
{
    public class BeneficiaryRepository : BaseRepository<Beneficiary, long>, IBeneficiaryRepository
    {
        private readonly ICurrentLoginUserService _currentLoginUserService;
        private readonly BeneficiaryUserConditionFactory _beneficiaryUserConditionFactory;
        private readonly IBeneficiaryDynamicCellRepository _beneficiaryDynamicCellRepository;
        private readonly ILogger<BeneficiaryRepository> _logger;
        private readonly IServiceProvider _serviceProvider;
        public BeneficiaryRepository(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService
            , BeneficiaryUserConditionFactory beneficiaryUserConditionFactory, IBeneficiaryDynamicCellRepository beneficiaryDynamicCellRepository
            , ILogger<BeneficiaryRepository> logger,
            IServiceProvider serviceProvider) : base(context)
        {

            _currentLoginUserService = currentLoginUserService;
            _beneficiaryUserConditionFactory = beneficiaryUserConditionFactory;
            _beneficiaryDynamicCellRepository = beneficiaryDynamicCellRepository;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<Beneficiary> Add(Beneficiary beneficiary)
        {
            _context.Add(beneficiary);
            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<List<BeneficiaryRawViewModel>> GetAggregateBeneficiary(List<long> instanceIds)
        {
            var instanceMapping = await _context.InstanceMappings
                .Where(a => instanceIds.Contains(a.BeneficiaryInstanceId)).ToListAsync();
            var facilityInstanceIds = instanceMapping.Select(a => a.FacilityInstanceId).ToList();

            var campData = await _context.Camps.ToListAsync();
            var blockData = await _context.Blocks.ToListAsync();
            var subBlockData = await _context.SubBlocks.ToListAsync();
            var espData = await _context.EducationSectorPartners.ToListAsync();
            var instanceData = await _context.ScheduleInstances.Where(a => instanceIds.Contains(a.Id)).ToListAsync();

            var beneficiaryDynamicCell = await _context.BeneficiaryDynamicCells
                                .Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
            var beneficiaryRaw = beneficiaryDynamicCell.GroupBy(a => new { a.InstanceId, a.BeneficiaryId }).Select(f => new BeneficiaryRawViewModel
            {
                BeneficiaryName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Name).Select(h => h.Value).FirstOrDefault(),
                UnhcrId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.UnhcrId).Select(h => h.Value).FirstOrDefault(),
                Sex = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Sex).Select(h => (Gender)Enum.Parse(typeof(Gender), h.Value)).FirstOrDefault(),
                LevelOfStudy = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.LevelOfStudy).Select(h => (LevelOfStudy)Enum.Parse(typeof(LevelOfStudy), h.Value)).FirstOrDefault(),
                Disabled = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Disabled).Select(h => h.Value.ToLower().Trim() == "yes" ? true : false).FirstOrDefault(),

                DateOfBirth = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.DateOfBirth).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),
                EnrollmentDate = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.EnrollmentDate).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),

                InstanceId = f.Key.InstanceId,
                EntityId = f.Key.BeneficiaryId,

                FCNId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FCNId).Select(h => h.Value).FirstOrDefault(),
                FatherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FatherName).Select(h => h.Value).FirstOrDefault(),
                MotherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.MotherName).Select(h => h.Value).FirstOrDefault(),

                FacilityId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FacilityId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BeneficiaryCampId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.CampId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.BlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                SubBlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.SubBlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Remarks).Select(h => h.Value).FirstOrDefault(),

            }).ToList();

            var dataCollectionStatus = await _context.BeneciaryDataCollectionStatuses
               .Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();

            var benWithDataCollection = beneficiaryRaw.Join(dataCollectionStatus, b => new
            {
                b.InstanceId,
                b.EntityId
            }, bdcs => new
            {
                bdcs.InstanceId,
                EntityId = bdcs.BeneficiaryId
            }
            , (b, bdcs) => new BeneficiaryRawViewModel
            {
                BeneficiaryName = b.BeneficiaryName,
                UnhcrId = b.UnhcrId,
                Sex = b.Sex,
                LevelOfStudy = b.LevelOfStudy,
                Disabled = b.Disabled,

                DateOfBirth = b.DateOfBirth,
                EnrollmentDate = b.EnrollmentDate,

                InstanceId = b.InstanceId,
                EntityId = b.EntityId,

                FCNId = b.FCNId,
                FatherName = b.FatherName,
                MotherName = b.MotherName,

                FacilityId = b.FacilityId,
                BeneficiaryCampId = b.BeneficiaryCampId,
                BlockId = b.BlockId,
                SubBlockId = b.SubBlockId,
                Remarks = b.Remarks,

                CollectionStatus = bdcs.Status

            });

            var facilityDynamicCell = await _context.FacilityDynamicCells
                                .Where(a => facilityInstanceIds.Contains(a.InstanceId)).ToListAsync();
            var facilityRaw = facilityDynamicCell.GroupBy(a => new { a.InstanceId, a.FacilityId }).Select(f => new FacilityRawViewModel
            {
                Id = f.Key.FacilityId,
                FacilityName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Name).Select(h => h.Value).FirstOrDefault(),
                FacilityCode = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Code).Select(h => h.Value).FirstOrDefault(),
                UpazilaId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Upazila).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                UnionId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Union).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                TargetedPopulation = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.TargetedPopulation).Select(h => (TargetedPopulation)Enum.Parse(typeof(TargetedPopulation), h.Value)).FirstOrDefault(),
                FacilityType = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Type).Select(h => (FacilityType)Enum.Parse(typeof(FacilityType), h.Value)).FirstOrDefault(),
                FacilityStatus = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Status).Select(h => (FacilityStatus)Enum.Parse(typeof(FacilityStatus), h.Value)).FirstOrDefault(),
                ProgrammingPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ImplemantationPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                TeacherId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Teacher).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                InstanceId = f.Key.InstanceId,

                Latitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Latitude).Select(h => h.Value).FirstOrDefault(),
                longitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Longitude).Select(h => h.Value).FirstOrDefault(),
                Donors = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Donors).Select(h => h.Value).FirstOrDefault(),
                NonEducationPartner = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.NonEducationPartner).Select(h => h.Value).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Remarks).Select(h => h.Value).FirstOrDefault(),
                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ParaName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ParaName).Select(h => h.Value).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            var bneficairyDataCollectioStatus = await _context.BeneciaryDataCollectionStatuses
                .Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
            var facilityDataCollectioStatus = await _context.FacilityDataCollectionStatuses
                .Where(a => facilityInstanceIds.Contains(a.InstanceId)).ToListAsync();

            List<BeneficiaryRawViewModel> beneficiaryRaws = new List<BeneficiaryRawViewModel>();

            instanceIds.ForEach(async benInstanceId =>
            {
                var facilityInstanceId = instanceMapping.Where(a => a.BeneficiaryInstanceId == benInstanceId)
                .Select(a => a.FacilityInstanceId).FirstOrDefault();
                var benData = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                         .ApplyCondition(benWithDataCollection.Where(a => a.InstanceId == benInstanceId).ToList(), facilityInstanceId);
                beneficiaryRaws.AddRange(benData);
            });

            var indicatorData = await _context.InstanceIndicators
                .Include(a => a.EntityDynamicColumn)
                .ThenInclude(a => a.ColumnList)
                .ThenInclude(a => a.ListItems)
                .Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();

            var listItemData = new List<ListItem>();

            indicatorData.ForEach(a =>
            {
                if (a.EntityDynamicColumn.ColumnList != null)
                {
                    listItemData.AddRange(a.EntityDynamicColumn.ColumnList.ListItems);
                }
            });


            var beneficiaries = (from b in beneficiaryRaws
                                 join bcmp in campData on b.BeneficiaryCampId equals bcmp.Id
                                 join blk in blockData on b.BlockId equals blk.Id
                                 join sblk in subBlockData on b.SubBlockId equals sblk.Id
                                 join im in instanceMapping on b.InstanceId equals im.BeneficiaryInstanceId
                                 join f in facilityRaw on new { im.FacilityInstanceId, b.FacilityId }
                                      equals new { FacilityInstanceId = f.InstanceId, FacilityId = f.Id }
                                 join c in campData on f.CampId equals c.Id into ca
                                 from fcmp in ca.DefaultIfEmpty()
                                 join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                 join ip in espData on f.ImplemantationPartnerId equals ip.Id

                                 join ii in indicatorData on b.InstanceId equals ii.InstanceId
                                 join dc in beneficiaryDynamicCell on new {b.InstanceId, BeneficiaryId = b.EntityId, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new {dc.InstanceId, dc.BeneficiaryId, dc.EntityDynamicColumnId } into def2
                                 from dynCellData in def2.DefaultIfEmpty()
                                 join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                 from liData in def1.DefaultIfEmpty()
                                 join sci in instanceData on b.InstanceId equals sci.Id

                                 where b.CollectionStatus == CollectionStatus.Approved
                                 select new BeneficiaryRawViewModel
                                 {
                                     EntityId = b.EntityId,
                                     BeneficiaryName = b.BeneficiaryName,
                                     UnhcrId = b.UnhcrId,
                                     FCNId = b.FCNId,
                                     FatherName = b.FatherName,
                                     MotherName = b.MotherName,
                                     DateOfBirth = b.DateOfBirth,
                                     EnrollmentDate = b.EnrollmentDate,
                                     Disabled = b.Disabled,
                                     LevelOfStudy = b.LevelOfStudy,
                                     Sex = b.Sex,

                                     FacilityId = f.Id,
                                     FacilityName = f.FacilityName,

                                     FacilityCampId = fcmp?.Id ?? 0,
                                     FacilityCampName = fcmp?.Name,
                                     BeneficiaryCampId = bcmp?.Id ?? 0,
                                     BeneficiaryCampName = bcmp?.Name,
                                     BlockId = blk?.Id ?? 0,
                                     BlockName = blk?.Name,
                                     BlockCode = blk?.Code,
                                     FacilityCode = f.FacilityCode,
                                     SubBlockId = sblk?.Id ?? 0,

                                     CampSsId = bcmp?.SSID,
                                     SubBlockName = sblk?.Name,
                                     ProgrammingPartnerId = pp.Id,
                                     ProgrammingPartnerName = pp.PartnerName,
                                     ImplemantationPartnerId = ip.Id,
                                     ImplemantationPartnerName = ip.PartnerName,

                                     ColumnOrder = ii.ColumnOrder,
                                     EntityColumnId = ii.EntityDynamicColumnId,
                                     EntityColumnName = ii.EntityDynamicColumn.Name,
                                     ColumnListId = ii.EntityDynamicColumn.ColumnListId,
                                     ColumnListName = ii.EntityDynamicColumn.ColumnList?.Name,
                                     DataType = ii.EntityDynamicColumn.ColumnType,
                                     IsMultiValued = ii.EntityDynamicColumn.IsMultiValued,
                                     IsFixed = ii.EntityDynamicColumn.IsFixed,
                                     ListItemId = liData?.Id,
                                     ListItemTitle = liData?.Title,
                                     ListItemValue = liData?.Value,
                                     PropertiesId = dynCellData?.Id,
                                     PropertiesValue = dynCellData?.Value,
                                     CollectionStatus = b.CollectionStatus,

                                     InstanceId = sci.Id,
                                     InstanceName = sci.Title
                                 }).ToList();
            return beneficiaries;
        }

        public IQueryable<BeneficiaryRawViewModel> GetBeneficiaryByInstance(long instanceId, long maxFacilityInstanceId)
        {
            var beneficiary = (from b in _context.BeneficiaryView.Where(x => x.InstanceId == instanceId)
                               join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
                               join blk in _context.Blocks on b.BlockId equals blk.Id
                               join sblk in _context.SubBlocks on b.SubBlockId equals sblk.Id
                               join f in _context.FacilityView.Where(x => x.InstanceId == maxFacilityInstanceId)
                                   on b.FacilityId equals f.Id
                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from fcmp in ca.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id

                               join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Beneficiary } equals new { edc.EntityTypeId }
                               join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                               from columnListData in def.DefaultIfEmpty()
                               join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                               from listItemData in def1.DefaultIfEmpty()
                               join dc in _context.BeneficiaryDynamicCells on new { BeneficiaryId = b.Id, EntityDynamicColumnId = edc.Id } equals new { dc.BeneficiaryId, dc.EntityDynamicColumnId } into def2
                               from dynamicCellData in def2.DefaultIfEmpty()
                               join dcs in _context.BeneciaryDataCollectionStatuses
                                   on new { BeneficiaryId = b.Id, InstanceId = instanceId } equals
                                      new { dcs.BeneficiaryId, dcs.InstanceId }

                               join ii in _context.InstanceIndicators on new { dynamicCellData.EntityDynamicColumnId, dynamicCellData.InstanceId }
                                equals new { ii.EntityDynamicColumnId, ii.InstanceId }
                                into iiGroup
                               from indicator in iiGroup.DefaultIfEmpty()

                               where dynamicCellData.InstanceId == instanceId && dcs.Status == CollectionStatus.Approved
                               select new BeneficiaryRawViewModel
                               {
                                   EntityId = b.Id,
                                   BeneficiaryName = b.Name,
                                   UnhcrId = b.UnhcrId,
                                   FCNId = b.FCNId,
                                   FatherName = b.FatherName,
                                   MotherName = b.MotherName,
                                   DateOfBirth = b.DateOfBirth,
                                   EnrollmentDate = b.EnrollmentDate,
                                   Disabled = b.Disabled,
                                   LevelOfStudy = b.LevelOfStudy,
                                   Sex = b.Sex,

                                   FacilityId = f.Id,
                                   FacilityName = f.Name,


                                   FacilityCampId = fcmp.Id,
                                   FacilityCampName = fcmp.Name,
                                   BeneficiaryCampId = bcmp.Id,
                                   BeneficiaryCampName = bcmp.Name,
                                   BlockId = blk.Id,
                                   BlockName = blk.Name,
                                   BlockCode = blk.Code,
                                   FacilityCode = f.FacilityCode,
                                   SubBlockId = sblk.Id,

                                   CampSsId = bcmp.SSID,
                                   SubBlockName = sblk.Name,
                                   ProgrammingPartnerId = pp.Id,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = ip.Id,
                                   ImplemantationPartnerName = ip.PartnerName,

                                   ColumnOrder = indicator.ColumnOrder,
                                   EntityColumnId = edc.Id,
                                   EntityColumnName = edc.Name,
                                   ColumnListId = edc.ColumnListId,
                                   ColumnListName = columnListData.Name,
                                   DataType = edc.ColumnType,
                                   IsMultiValued = edc.IsMultiValued,
                                   IsFixed = edc.IsFixed,
                                   ListItemId = listItemData.Id,
                                   ListItemTitle = listItemData.Title,
                                   ListItemValue = listItemData.Value,
                                   PropertiesId = dynamicCellData.Id,
                                   PropertiesValue = dynamicCellData.Value

                               });

            return _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                .ApplyCondition(beneficiary, maxFacilityInstanceId);
        }

        public IQueryable<BeneficiaryRawViewModel> GetBeneficiaryByInstances(List<long> instanceIds, long maxFacilityInstanceId)
        {

            var beneficiary = (from b in _context.BeneficiaryView.Where(x => instanceIds.Contains(x.InstanceId))
                               join dcs in _context.BeneciaryDataCollectionStatuses
                               on new { BeneficiaryId = b.Id, b.InstanceId } equals
                               new { dcs.BeneficiaryId, dcs.InstanceId }


                               join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
                               join blk in _context.Blocks on b.BlockId equals blk.Id
                               join sblk in _context.SubBlocks on b.SubBlockId equals sblk.Id
                               join f in _context.FacilityView
                                       .Where(x => x.InstanceId == maxFacilityInstanceId)
                                   on b.FacilityId equals f.Id
                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from fcmp in ca.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id

                               join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Beneficiary } equals new { edc.EntityTypeId }
                               join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                               from columnListData in def.DefaultIfEmpty()
                               join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                               from listItemData in def1.DefaultIfEmpty()
                               join dc in _context.BeneficiaryDynamicCells
                                   on new { BeneficiaryId = b.Id, EntityDynamicColumnId = edc.Id, b.InstanceId } equals
                                   new { dc.BeneficiaryId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                               from dynamicCellData in def2.DefaultIfEmpty()



                               join ii in _context.InstanceIndicators on new { dynamicCellData.EntityDynamicColumnId, dynamicCellData.InstanceId }
                                equals new { ii.EntityDynamicColumnId, ii.InstanceId }
                                into iiGroup
                               from indicator in iiGroup.DefaultIfEmpty()

                               join sci in _context.ScheduleInstances on indicator.InstanceId equals sci.Id
                               where instanceIds.Contains(dynamicCellData.InstanceId) && dcs.Status == CollectionStatus.Approved
                               select new BeneficiaryRawViewModel
                               {
                                   EntityId = b.Id,
                                   BeneficiaryName = b.Name,
                                   UnhcrId = b.UnhcrId,
                                   FCNId = b.FCNId,
                                   FatherName = b.FatherName,
                                   MotherName = b.MotherName,
                                   DateOfBirth = b.DateOfBirth,
                                   EnrollmentDate = b.EnrollmentDate,
                                   Disabled = b.Disabled,
                                   LevelOfStudy = b.LevelOfStudy,
                                   Sex = b.Sex,

                                   FacilityId = f.Id,
                                   FacilityName = f.Name,

                                   FacilityCampId = fcmp.Id,
                                   FacilityCampName = fcmp.Name,
                                   BeneficiaryCampId = bcmp.Id,
                                   BeneficiaryCampName = bcmp.Name,
                                   BlockId = blk.Id,
                                   BlockName = blk.Name,
                                   BlockCode = blk.Code,
                                   FacilityCode = f.FacilityCode,
                                   SubBlockId = sblk.Id,

                                   CampSsId = bcmp.SSID,
                                   SubBlockName = sblk.Name,
                                   ProgrammingPartnerId = pp.Id,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = ip.Id,
                                   ImplemantationPartnerName = ip.PartnerName,

                                   ColumnOrder = indicator.ColumnOrder,
                                   EntityColumnId = edc.Id,
                                   EntityColumnName = edc.Name,
                                   ColumnListId = edc.ColumnListId,
                                   ColumnListName = columnListData.Name,
                                   DataType = edc.ColumnType,
                                   IsMultiValued = edc.IsMultiValued,
                                   IsFixed = edc.IsFixed,
                                   ListItemId = listItemData.Id,
                                   ListItemTitle = listItemData.Title,
                                   ListItemValue = listItemData.Value,
                                   PropertiesId = dynamicCellData.Id,
                                   PropertiesValue = dynamicCellData.Value,
                                   InstanceId = sci.Id,
                                   InstanceName = sci.Title
                               });

            instanceIds.ForEach(a =>
            {
                beneficiary = _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                .ApplyCondition(beneficiary, maxFacilityInstanceId);
            });
            return beneficiary;

        }

        private IQueryable<BeneficiaryRawViewModel> GetCollection(long instanceId, long maxFacilityInstanceId)
        {

            //TODO: beneficiary new schema
            var beneficiary = (from b in _context.BeneficiaryView
                               join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
                               join blk in _context.Blocks on b.BlockId equals blk.Id
                               join sblk in _context.SubBlocks on b.SubBlockId equals sblk.Id
                               join f in _context.FacilityView.Where(x => x.InstanceId == maxFacilityInstanceId)
                                   on b.FacilityId equals f.Id
                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from fcmp in ca.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id

                               join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Beneficiary } equals new { edc.EntityTypeId }
                               join ii in _context.InstanceIndicators on new { b.InstanceId, EntityDynamicColumnId = edc.Id } equals new { ii.InstanceId, ii.EntityDynamicColumnId }
                               join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                               from columnListData in def.DefaultIfEmpty()
                               join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                               from listItemData in def1.DefaultIfEmpty()
                               join dc in _context.BeneficiaryDynamicCells on new { BeneficiaryId = b.Id, EntityDynamicColumnId = edc.Id, InstanceId = b.InstanceId } equals new { dc.BeneficiaryId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                               from dynamicCellData in def2.DefaultIfEmpty()
                               join bdcs in _context.BeneciaryDataCollectionStatuses on new { BeneficiaryId = b.Id, InstanceId = b.InstanceId } equals new { bdcs.BeneficiaryId, bdcs.InstanceId }

                               where b.InstanceId == instanceId
                               select new BeneficiaryRawViewModel
                               {
                                   InstanceId = b.InstanceId,
                                   EntityId = b.Id,
                                   BeneficiaryName = b.Name,
                                   UnhcrId = b.UnhcrId,
                                   FCNId = b.FCNId,
                                   FatherName = b.FatherName,
                                   MotherName = b.MotherName,
                                   DateOfBirth = b.DateOfBirth,
                                   EnrollmentDate = b.EnrollmentDate,
                                   Disabled = b.Disabled,
                                   LevelOfStudy = b.LevelOfStudy,
                                   Sex = b.Sex,

                                   FacilityId = f.Id,
                                   FacilityName = f.Name,

                                   FacilityCampId = fcmp.Id,
                                   FacilityCampName = fcmp.Name,
                                   BeneficiaryCampId = bcmp.Id,
                                   BeneficiaryCampName = bcmp.Name,
                                   BlockId = blk.Id,
                                   BlockName = blk.Name,
                                   SubBlockId = sblk.Id,
                                   SubBlockName = sblk.Name,
                                   ProgrammingPartnerId = pp.Id,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = ip.Id,
                                   ImplemantationPartnerName = ip.PartnerName,

                                   EntityColumnId = edc.Id,
                                   EntityColumnName = edc.Name,
                                   ColumnListId = edc.ColumnListId,
                                   ColumnListName = columnListData.Name,
                                   ColumnOrder = ii.ColumnOrder,
                                   DataType = edc.ColumnType,
                                   IsMultiValued = edc.IsMultiValued,
                                   IsFixed = edc.IsFixed,
                                   ListItemId = listItemData.Id,
                                   ListItemTitle = listItemData.Title,
                                   ListItemValue = listItemData.Value,
                                   PropertiesId = dynamicCellData.Id,
                                   PropertiesValue = dynamicCellData.Value,
                                   CollectionStatus = bdcs.Status
                               });
            return beneficiary;
        }
        public List<BeneficiaryViewModel> SetBeneficiaryViewModel(List<BeneficiaryRawViewModel> beneficiaryRawData)
        {
            var beneficiary = beneficiaryRawData.GroupBy(
                        v => new
                        {
                            v.EntityId,
                            v.UnhcrId,
                            v.BeneficiaryName,
                            v.FCNId,
                            v.FacilityName,
                            v.FacilityId,
                            v.BeneficiaryCampId,
                            v.BeneficiaryCampName,
                            v.BlockId,
                            v.BlockName,
                            v.SubBlockId,
                            v.SubBlockName,
                            v.ProgrammingPartnerId,
                            v.ProgrammingPartnerName,
                            v.ImplemantationPartnerId,
                            v.ImplemantationPartnerName,
                            v.InstanceId,
                            v.InstanceName
                        })
                        .Select(g => new BeneficiaryViewModel
                        {
                            EntityId = g.Key.EntityId,
                            UnhcrId = g.Key.UnhcrId,
                            FCNId = g.Key.FCNId,
                            BeneficiaryName = g.Key.BeneficiaryName,
                            FacilityId = g.Key.FacilityId,
                            FacilityName = g.Key.FacilityName,
                            FacilityCode = g.Select(x => x.FacilityCode).FirstOrDefault(),
                            BeneficiaryCampId = g.Key.BeneficiaryCampId,
                            BeneficiaryCampSSID = g.Select(x => x.CampSsId).FirstOrDefault(),
                            BeneficiaryCampName = g.Key.BeneficiaryCampName,
                            FacilityCampId = g.Select(h => h.FacilityCampId).FirstOrDefault(),
                            FacilityCampName = g.Select(h => h.FacilityCampName).FirstOrDefault(),
                            BlockId = g.Key.BlockId,
                            BlockName = g.Key.BlockName,
                            BlockCode = g.Select(x => x.BlockCode).FirstOrDefault(),
                            SubBlockId = g.Key.SubBlockId,
                            SubBlockName = g.Key.SubBlockName,
                            ProgrammingPartnerId = g.Key.ProgrammingPartnerId,
                            ProgrammingPartnerName = g.Key.ProgrammingPartnerName,
                            ImplemantationPartnerId = g.Key.ImplemantationPartnerId,
                            ImplemantationPartnerName = g.Key.ImplemantationPartnerName,

                            FatherName = g.Select(h => h.FatherName).FirstOrDefault(),
                            MotherName = g.Select(h => h.MotherName).FirstOrDefault(),
                            DateOfBirth = g.Select(h => h.DateOfBirth).FirstOrDefault(),
                            EnrollmentDate = g.Select(h => h.EnrollmentDate).FirstOrDefault(),
                            Disabled = g.Select(h => h.Disabled).FirstOrDefault(),
                            Sex = g.Select(h => h.Sex).FirstOrDefault(),
                            LevelOfStudy = g.Select(h => h.LevelOfStudy).FirstOrDefault(),

                            CollectionStatus = g.Select(h => h.CollectionStatus).FirstOrDefault(),
                            InstanceId = g.Key.InstanceId,
                            InstanceName = g.Key.InstanceName,

                            Properties = g.GroupBy(f => new { f.EntityColumnId, f.EntityColumnName })
                                       .Select(m => new PropertiesInfo
                                       {
                                           ColumnOrder = m.Select(x => x.ColumnOrder).FirstOrDefault(),
                                           EntityColumnId = m.Key.EntityColumnId,
                                           Properties = m.Key.EntityColumnName,
                                           ColumnName = m.Key.EntityColumnName,
                                           ColumnNameInBangla = m.Select(n => n.EntityColumnNameInBangla).FirstOrDefault(),
                                           IsFixed = m.Select(n => n.IsFixed).FirstOrDefault(),
                                           IsMultiValued = m.Select(n => n.IsMultiValued).FirstOrDefault() ?? false,
                                           ColumnListId = m.Select(n => n.ColumnListId).FirstOrDefault(),
                                           ColumnListName = m.Select(n => n.ColumnListName).FirstOrDefault(),
                                           DataType = m.Select(n => n.DataType).FirstOrDefault(),
                                           Values = m.GroupBy(n => n.PropertiesId).Select(o => o.Select(p => p.PropertiesValue).FirstOrDefault()).ToList(),
                                           Status = m.Select(n => n.PropertiesDataCollectionStatus).FirstOrDefault(),
                                           ListItem = m.Where(n => n.ListItemId != null).GroupBy(n => n.ListItemId).Select(o => new ListItemViewModel()
                                           {
                                               Id = (long)o.Key,
                                               Title = o.Select(p => p.ListItemTitle).FirstOrDefault(),
                                               Value = (int)o.Select(p => p.ListItemValue).FirstOrDefault()
                                           }).ToList()
                                       }).OrderBy(m => m.ColumnOrder).ToList()
                        }).ToList();
            return beneficiary;
        }

        private async Task<List<BeneficiaryRawViewModel>> GetPaginatedBeneficiary(BeneficiaryQueryModel beneficiaryQuery)
        {
            return new List<BeneficiaryRawViewModel>();
            //TODO: beneficiary new schema
            //var beneficiary = (
            //    from b in _context.Beneficiary
            //    join f in _context.Facility on b.FacilityId equals f.Id
            //    join bdcs in _context.BeneciaryDataCollectionStatuses on new { beneficiaryQuery.InstanceId, BeneficiaryId = b.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
            //    where bdcs.Status == CollectionStatus.Approved
            //    && b.IsActive
            //    select new BeneficiaryRawViewModel
            //    {
            //        EntityId = b.Id,
            //        FacilityId = b.FacilityId,
            //        ProgrammingPartnerId = f.ProgramPartnerId,
            //        ImplemantationPartnerId = f.ImplementationPartnerId
            //    });

            //return await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //             .ApplyCondition(beneficiary).Skip(beneficiaryQuery.Skip())
            //             .Take(beneficiaryQuery.PageSize).ToListAsync();
        }
        private IQueryable<BeneficiaryRawViewModel> GetBeneficiaryRaw(BeneficiaryByViewIdQueryModel beneficiaryByViewId, long maxFacilityInstanceId)
        {
            var beneficiary = (
                from b in _context.BeneficiaryView
                join f in _context.FacilityView.Where(x => x.InstanceId == maxFacilityInstanceId)
                    on b.FacilityId equals f.Id
                join bdcs in _context.BeneciaryDataCollectionStatuses on
                    new { b.InstanceId, BeneficiaryId = b.Id } equals
                    new { bdcs.InstanceId, bdcs.BeneficiaryId }
                where b.InstanceId == beneficiaryByViewId.InstanceId
                select new BeneficiaryRawViewModel
                {
                    BeneficiaryName = b.Name,
                    UnhcrId = b.UnhcrId,
                    Sex = b.Sex,
                    LevelOfStudy = b.LevelOfStudy,
                    Disabled = b.Disabled,
                    BeneficiaryCampId = b.BeneficiaryCampId,
                    DateOfBirth = b.DateOfBirth,
                    EnrollmentDate = b.EnrollmentDate,
                    CollectionStatus = bdcs.Status,

                    EntityId = b.Id,
                    FacilityId = b.FacilityId,
                    ProgrammingPartnerId = f.ProgramPartnerId,
                    ImplemantationPartnerId = f.ImplementationPartnerId
                });

            beneficiary = ApplyFilter(beneficiary, beneficiaryByViewId);
            return _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                         .ApplyCondition(beneficiary, maxFacilityInstanceId);

        }

        private IQueryable<BeneficiaryRawViewModel> ApplyFilter(IQueryable<BeneficiaryRawViewModel> rawQuery, BeneficiaryByViewIdQueryModel beneficiaryByViewId)
        {
            if (!beneficiaryByViewId.CollectionStatus.NoneMatched())
            {
                rawQuery = rawQuery.Where(a => a.CollectionStatus == beneficiaryByViewId.CollectionStatus);
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.SearchText))
            {
                rawQuery = rawQuery.Where(a => a.BeneficiaryName.Contains(beneficiaryByViewId.Filter.SearchText)
                || a.UnhcrId.Contains(beneficiaryByViewId.Filter.SearchText));
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.DateOfBirth.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.DateOfBirth >= filterStartDate && a.DateOfBirth <= filterEndDate);
            }

            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.EnrolmentDate.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.EnrollmentDate >= filterStartDate && a.EnrollmentDate <= filterEndDate);
            }

            if (beneficiaryByViewId.Filter.Sex != null)
            {
                rawQuery = rawQuery.Where(a => a.Sex == beneficiaryByViewId.Filter.Sex);
            }

            if (beneficiaryByViewId.Filter.LevelOfStudy != null)
            {
                rawQuery = rawQuery.Where(a => a.LevelOfStudy == beneficiaryByViewId.Filter.LevelOfStudy);
            }

            if (beneficiaryByViewId.Filter.Disable != null)
            {
                rawQuery = rawQuery.Where(a => a.Disabled == beneficiaryByViewId.Filter.Disable);
            }

            if (beneficiaryByViewId.Filter.Camps.Count > 0)
            {
                var campIds = beneficiaryByViewId.Filter.Camps.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => campIds.Contains(a.BeneficiaryCampId));
            }
            if (beneficiaryByViewId.Filter.Facilities.Count > 0)
            {
                var facilityIds = beneficiaryByViewId.Filter.Facilities.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => facilityIds.Contains(a.FacilityId));
            }

            return rawQuery;
        }
        private IQueryable<BeneficiaryRawViewModel> ApplyFilter(IQueryable<BeneficiaryRawViewModel> rawQuery, BeneficiaryGetAllQueryModel beneficiaryByViewId)
        {
            if (beneficiaryByViewId.Filter == null)
            {
                return rawQuery;
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.SearchText))
            {
                rawQuery = rawQuery.Where(a => a.BeneficiaryName.Contains(beneficiaryByViewId.Filter.SearchText)
                || a.UnhcrId.Contains(beneficiaryByViewId.Filter.SearchText));
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.DateOfBirth.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.DateOfBirth >= filterStartDate && a.DateOfBirth <= filterEndDate);
            }

            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.EnrolmentDate.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.EnrollmentDate >= filterStartDate && a.EnrollmentDate <= filterEndDate);
            }

            if (beneficiaryByViewId.Filter.Sex != null)
            {
                rawQuery = rawQuery.Where(a => a.Sex == beneficiaryByViewId.Filter.Sex);
            }

            if (beneficiaryByViewId.Filter.LevelOfStudy != null)
            {
                rawQuery = rawQuery.Where(a => a.LevelOfStudy == beneficiaryByViewId.Filter.LevelOfStudy);
            }

            if (beneficiaryByViewId.Filter.Disable != null)
            {
                rawQuery = rawQuery.Where(a => a.Disabled == beneficiaryByViewId.Filter.Disable);
            }

            if (beneficiaryByViewId.Filter.Camps.Count > 0)
            {
                var campIds = beneficiaryByViewId.Filter.Camps.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => campIds.Contains(a.BeneficiaryCampId));
            }
            if (beneficiaryByViewId.Filter.Facilities.Count > 0)
            {
                var facilityIds = beneficiaryByViewId.Filter.Facilities.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => facilityIds.Contains(a.FacilityId));
            }

            return rawQuery;
        }
        private IQueryable<BeneficairyObjectViewModel> ApplyFilter(IQueryable<BeneficairyObjectViewModel> rawQuery, BeneficiaryGetAllQueryModel beneficiaryByViewId)
        {
            if (beneficiaryByViewId.Filter == null)
            {
                return rawQuery;
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.SearchText))
            {
                rawQuery = rawQuery
                    .Where(a => a.BeneficiaryName.Contains(beneficiaryByViewId.Filter.SearchText)
                    || a.UnhcrId.Contains(beneficiaryByViewId.Filter.SearchText));
            }
            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.DateOfBirth.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.DateOfBirth.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.DateOfBirth >= filterStartDate && a.DateOfBirth <= filterEndDate);
            }

            if (!string.IsNullOrEmpty(beneficiaryByViewId.Filter.EnrolmentDate.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(beneficiaryByViewId.Filter.EnrolmentDate.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.EnrollmentDate >= filterStartDate && a.EnrollmentDate <= filterEndDate);
            }

            if (beneficiaryByViewId.Filter.Sex != null)
            {
                rawQuery = rawQuery.Where(a => a.Sex == beneficiaryByViewId.Filter.Sex);
            }

            if (beneficiaryByViewId.Filter.LevelOfStudy != null)
            {
                rawQuery = rawQuery.Where(a => a.LevelOfStudy == beneficiaryByViewId.Filter.LevelOfStudy);
            }

            if (beneficiaryByViewId.Filter.Disable != null)
            {
                rawQuery = rawQuery.Where(a => a.Disabled == beneficiaryByViewId.Filter.Disable);
            }

            if (beneficiaryByViewId.Filter.Camps.Count > 0)
            {
                var campIds = beneficiaryByViewId.Filter.Camps.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => campIds.Contains(a.BeneficiaryCampId));
            }
            if (beneficiaryByViewId.Filter.Facilities.Count > 0)
            {
                var facilityIds = beneficiaryByViewId.Filter.Facilities.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => facilityIds.Contains(a.FacilityId));
            }

            return rawQuery;
        }



        public async Task<PagedResponse<BeneficiaryViewModel>> GetByFacilityId(BeneficiaryByFacilityIdQueryModel model, long maxFacilityInstanceId)
        {

            //var beneficairyIdRaw = (from b in _context.BeneficiaryView
            //                        join f in _context.FacilityView on b.FacilityId equals f.Id
            //                        join bdcs in _context.BeneciaryDataCollectionStatuses on
            //                        new { b.InstanceId, BeneficiaryId = b.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
            //                        where b.InstanceId == model.InstanceId
            //                        && f.InstanceId == maxFacilityInstanceId
            //                        && b.FacilityId == model.FacilityId
            //                        && (bdcs.Status != CollectionStatus.Inactivated &&
            //                        bdcs.Status != CollectionStatus.Deleted &&
            //                        bdcs.Status != CollectionStatus.Approved)
            //                        select new BeneficiaryRawViewModel
            //                        {
            //                            EntityId = b.Id,
            //                            ProgrammingPartnerId = f.ProgramPartnerId,
            //                            ImplemantationPartnerId = f.ImplementationPartnerId,
            //                            FacilityName = f.Name,
            //                            FacilityCode = f.FacilityCode,
            //                            FacilityId = f.Id
            //                        });


            //var beneficiaryListToGetIds = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //                     .ApplyCondition(beneficairyIdRaw, maxFacilityInstanceId).Skip(model.Skip()).Take(model.PageSize).ToListAsync();
            //var beneficiaryIds = beneficiaryListToGetIds.Select(a => a.EntityId).ToList();

            //var total = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //                     .ApplyCondition(beneficairyIdRaw, maxFacilityInstanceId).CountAsync();

            //var beneficiaryRaw = _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //                     .ApplyCondition(GetCollection(model.InstanceId, maxFacilityInstanceId), maxFacilityInstanceId);

            //var beneficiaryData = await beneficiaryRaw.Where(a => beneficiaryIds.Contains(a.EntityId)).ToListAsync();
            //var beneficiary = SetBeneficiaryViewModel(beneficiaryData);


            var baseDynamicCellData = await _context.BeneficiaryDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var beneficiaryRaw = baseDynamicCellData.GroupBy(a => a.BeneficiaryId).Select(f => new BeneficiaryRawViewModel
            {
                BeneficiaryName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Name).Select(h => h.Value).FirstOrDefault(),
                UnhcrId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.UnhcrId).Select(h => h.Value).FirstOrDefault(),
                Sex = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Sex).Select(h => (Gender)Enum.Parse(typeof(Gender), h.Value)).FirstOrDefault(),
                LevelOfStudy = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.LevelOfStudy).Select(h => (LevelOfStudy)Enum.Parse(typeof(LevelOfStudy), h.Value)).FirstOrDefault(),
                Disabled = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Disabled).Select(h => h.Value.ToLower().Trim() == "yes" ? true : false).FirstOrDefault(),

                DateOfBirth = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.DateOfBirth).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),
                EnrollmentDate = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.EnrollmentDate).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),

                InstanceId = model.InstanceId,
                EntityId = f.Key,

                FCNId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FCNId).Select(h => h.Value).FirstOrDefault(),
                FatherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FatherName).Select(h => h.Value).FirstOrDefault(),
                MotherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.MotherName).Select(h => h.Value).FirstOrDefault(),

                FacilityId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FacilityId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BeneficiaryCampId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.CampId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.BlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                SubBlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.SubBlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Remarks).Select(h => h.Value).FirstOrDefault(),

            }).ToList();

            var conditionalbeneficiaryRaw = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                         .ApplyCondition(beneficiaryRaw, maxFacilityInstanceId);
            var dataCollectionStatus = await _context.BeneciaryDataCollectionStatuses
               .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var benWithDataCollection = conditionalbeneficiaryRaw.Join(dataCollectionStatus, b => b.EntityId, bdcs => bdcs.BeneficiaryId
            , (b, bdcs) => new BeneficiaryRawViewModel
            {
                BeneficiaryName = b.BeneficiaryName,
                UnhcrId = b.UnhcrId,
                Sex = b.Sex,
                LevelOfStudy = b.LevelOfStudy,
                Disabled = b.Disabled,

                DateOfBirth = b.DateOfBirth,
                EnrollmentDate = b.EnrollmentDate,

                InstanceId = b.InstanceId,
                EntityId = b.EntityId,

                FCNId = b.FCNId,
                FatherName = b.FatherName,
                MotherName = b.MotherName,

                FacilityId = b.FacilityId,
                BeneficiaryCampId = b.BeneficiaryCampId,
                BlockId = b.BlockId,
                SubBlockId = b.SubBlockId,
                Remarks = b.Remarks,

                CollectionStatus = bdcs.Status

            });

            Func<BeneficiaryRawViewModel, bool> filter = bdcs => bdcs.FacilityId == model.FacilityId &&
                                (bdcs.CollectionStatus != CollectionStatus.Inactivated &&
                                  bdcs.CollectionStatus != CollectionStatus.Deleted &&
                                  bdcs.CollectionStatus != CollectionStatus.Approved);

            var total = benWithDataCollection.Where(filter).Count();
            var benData = benWithDataCollection.Where(filter).OrderBy(a => a.EntityId).Skip(model.Skip()).Take(model.PageSize).ToList();
            var beneficiaryIds = benData.Select(a => a.EntityId).ToList();
            var facilityIds = benData.Select(a => a.FacilityId).Distinct().ToList();

            var facilityDynamicCellData = await _context.FacilityDynamicCells
                .Where(a => a.InstanceId == maxFacilityInstanceId && facilityIds.Contains(a.FacilityId)
                && (a.EntityDynamicColumnId == FacilityFixedIndicators.Name
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Camp
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Block)
                ).ToListAsync();

            var facilityData = facilityDynamicCellData.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
                FacilityName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Name).Select(h => h.Value).FirstOrDefault(),

                ProgrammingPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ImplemantationPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),

                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();
            var campIds = benData.Select(a => a.BeneficiaryCampId).ToList().Union(facilityCampIds);
            var blockIds = benData.Select(a => a.BlockId).ToList().Union(facilityBlockIds);
            var subBlockIds = benData.Select(a => a.SubBlockId).ToList();
            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var campData = await _context.Camps.Where(a => campIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => blockIds.Contains(a.Id)).ToListAsync();
            var subBlockData = await _context.SubBlocks.Where(a => subBlockIds.Contains(a.Id)).ToListAsync();
            var espData = await _context.EducationSectorPartners.Where(a => espIds.Contains(a.Id)).ToListAsync();

            var indicatorData = await _context.InstanceIndicators
                .Include(a => a.EntityDynamicColumn)
                .ThenInclude(a => a.ColumnList)
                .ThenInclude(a => a.ListItems)
                .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var listItemData = new List<ListItem>();

            indicatorData.ForEach(a =>
            {
                if (a.EntityDynamicColumn.ColumnList != null)
                {
                    listItemData.AddRange(a.EntityDynamicColumn.ColumnList.ListItems);
                }
            });

            var entityColumnIds = indicatorData.Select(a => a.EntityDynamicColumnId).ToList();
            var dynamicCellDataForQuery = baseDynamicCellData.Where(a => beneficiaryIds.Contains(a.BeneficiaryId)
            && entityColumnIds.Contains(a.EntityDynamicColumnId)
            ).ToList();
            var combineAllData = (from b in benData
                                  join bcmp in campData on b.BeneficiaryCampId equals bcmp.Id
                                  join blk in blockData on b.BlockId equals blk.Id
                                  join sblk in subBlockData on b.SubBlockId equals sblk.Id
                                  join f in facilityData on b.FacilityId equals f.Id
                                  join c in campData on f.CampId equals c.Id into ca
                                  from fcmp in ca.DefaultIfEmpty()
                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id

                                  join ii in indicatorData on b.InstanceId equals ii.InstanceId
                                  join dc in dynamicCellDataForQuery on new { BeneficiaryId = b.EntityId, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.BeneficiaryId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()

                                  select new BeneficiaryRawViewModel
                                  {
                                      InstanceId = b.InstanceId,
                                      EntityId = b.EntityId,
                                      BeneficiaryName = b.BeneficiaryName,
                                      UnhcrId = b.UnhcrId,
                                      FCNId = b.FCNId,
                                      FatherName = b.FatherName,
                                      MotherName = b.MotherName,
                                      DateOfBirth = b.DateOfBirth,
                                      EnrollmentDate = b.EnrollmentDate,
                                      Disabled = b.Disabled,
                                      LevelOfStudy = b.LevelOfStudy,
                                      Sex = b.Sex,

                                      FacilityId = f.Id,
                                      FacilityName = f.FacilityName,

                                      FacilityCampId = fcmp?.Id,
                                      FacilityCampName = fcmp?.Name,
                                      BeneficiaryCampId = bcmp.Id,
                                      BeneficiaryCampName = bcmp.Name,
                                      BlockId = blk.Id,
                                      BlockName = blk.Name,
                                      SubBlockId = sblk.Id,
                                      SubBlockName = sblk.Name,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,

                                      EntityColumnId = ii.EntityDynamicColumnId,
                                      EntityColumnName = ii.EntityDynamicColumn.Name,
                                      EntityColumnNameInBangla = ii.EntityDynamicColumn.NameInBangla,
                                      ColumnListId = ii.EntityDynamicColumn.ColumnListId,
                                      ColumnListName = ii.EntityDynamicColumn.ColumnList?.Name,
                                      ColumnOrder = ii.ColumnOrder,
                                      DataType = ii.EntityDynamicColumn.ColumnType,
                                      IsMultiValued = ii.EntityDynamicColumn.IsMultiValued,
                                      IsFixed = ii.EntityDynamicColumn.IsFixed,
                                      ListItemId = liData?.Id,
                                      ListItemTitle = liData?.Title,
                                      ListItemValue = liData?.Value,
                                      PropertiesId = dynCellData?.Id,
                                      PropertiesValue = dynCellData?.Value ?? "",
                                      PropertiesDataCollectionStatus = dynCellData?.Status,
                                      CollectionStatus = b.CollectionStatus
                                  }).ToList();

            var beneficiary = SetBeneficiaryViewModel(combineAllData);


            return new PagedResponse<BeneficiaryViewModel>(beneficiary, total, model.PageNo, model.PageSize);

        }

        private async Task<List<BeneficiaryRawViewModel>> BeneficiaryRawAsync(List<BeneficiaryDynamicCell> baseDynamicCellData, BeneficiaryByViewIdQueryModel model, long maxFacilityInstanceId)
        {
            var beneficiaryRaw = baseDynamicCellData.GroupBy(a => a.BeneficiaryId).Select(f => new BeneficiaryRawViewModel
            {
                BeneficiaryName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Name).Select(h => h.Value).FirstOrDefault(),
                UnhcrId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.UnhcrId).Select(h => h.Value).FirstOrDefault(),
                Sex = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Sex).Select(h => (Gender)Enum.Parse(typeof(Gender), h.Value)).FirstOrDefault(),
                LevelOfStudy = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.LevelOfStudy).Select(h => (LevelOfStudy)Enum.Parse(typeof(LevelOfStudy), h.Value)).FirstOrDefault(),
                Disabled = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Disabled).Select(h => h.Value.ToLower().Trim() == "yes" ? true : false).FirstOrDefault(),

                DateOfBirth = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.DateOfBirth).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),
                EnrollmentDate = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.EnrollmentDate).Select(h => DateTime.ParseExact(h.Value, "dd-MMM-yyyy", CultureInfo.InvariantCulture)).FirstOrDefault(),

                InstanceId = model.InstanceId,
                EntityId = f.Key,

                FCNId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FCNId).Select(h => h.Value).FirstOrDefault(),
                FatherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FatherName).Select(h => h.Value).FirstOrDefault(),
                MotherName = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.MotherName).Select(h => h.Value).FirstOrDefault(),

                FacilityId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FacilityId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BeneficiaryCampId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.CampId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.BlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                SubBlockId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.SubBlockId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.Remarks).Select(h => h.Value).FirstOrDefault(),

            }).ToList();

            var conditionalbeneficiaryRaw = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                         .ApplyCondition(beneficiaryRaw, maxFacilityInstanceId);

            var dataCollectionStatus = await _context.BeneciaryDataCollectionStatuses
               .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var benWithDataCollection = conditionalbeneficiaryRaw.Join(dataCollectionStatus, b => b.EntityId, bdcs => bdcs.BeneficiaryId
            , (b, bdcs) => new BeneficiaryRawViewModel
            {
                BeneficiaryName = b.BeneficiaryName,
                UnhcrId = b.UnhcrId,
                Sex = b.Sex,
                LevelOfStudy = b.LevelOfStudy,
                Disabled = b.Disabled,

                DateOfBirth = b.DateOfBirth,
                EnrollmentDate = b.EnrollmentDate,

                InstanceId = b.InstanceId,
                EntityId = b.EntityId,

                FCNId = b.FCNId,
                FatherName = b.FatherName,
                MotherName = b.MotherName,

                FacilityId = b.FacilityId,
                BeneficiaryCampId = b.BeneficiaryCampId,
                BlockId = b.BlockId,
                SubBlockId = b.SubBlockId,
                Remarks = b.Remarks,

                CollectionStatus = bdcs.Status

            });

            var filterBeneficaryRaw = ApplyFilter(benWithDataCollection.AsQueryable(), model).ToList();

            return filterBeneficaryRaw;
        }
        public async Task<PagedResponse<BeneficiaryViewModel>> GetAllByViewId(BeneficiaryByViewIdQueryModel model, long maxFacilityInstanceId)
        {
            var baseDynamicCellData = await _context.BeneficiaryDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var filterBeneficaryRaw = await BeneficiaryRawAsync(baseDynamicCellData, model, maxFacilityInstanceId);

            var total = filterBeneficaryRaw.Count();
            var benData = filterBeneficaryRaw.OrderBy(a => a.EntityId).Skip(model.Skip()).Take(model.PageSize).ToList();
            var beneficiaryIds = benData.Select(a => a.EntityId).ToList();
            var facilityIds = benData.Select(a => a.FacilityId).Distinct().ToList();

            var facilityDynamicCellData = await _context.FacilityDynamicCells
                .Where(a => a.InstanceId == maxFacilityInstanceId && facilityIds.Contains(a.FacilityId)
                && (a.EntityDynamicColumnId == FacilityFixedIndicators.Name
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Camp
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Block)
                ).ToListAsync();

            var facilityData = facilityDynamicCellData.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
                FacilityName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Name).Select(h => h.Value).FirstOrDefault(),

                ProgrammingPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ImplemantationPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),

                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();


            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();
            var campIds = benData.Select(a => a.BeneficiaryCampId).ToList().Union(facilityCampIds);
            var blockIds = benData.Select(a => a.BlockId).ToList().Union(facilityBlockIds);
            var subBlockIds = benData.Select(a => a.SubBlockId).ToList();
            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var campData = await _context.Camps.Where(a => campIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => blockIds.Contains(a.Id)).ToListAsync();
            var subBlockData = await _context.SubBlocks.Where(a => subBlockIds.Contains(a.Id)).ToListAsync();
            var espData = await _context.EducationSectorPartners.Where(a => espIds.Contains(a.Id)).ToListAsync();

            var indicatorData = await _context.GridViewDetails
                .Include(a => a.EntityDynamicColumn)
                .ThenInclude(a => a.ColumnList)
                .ThenInclude(a => a.ListItems)
                .Where(a => a.GridViewId == model.ViewId).ToListAsync();

            var entityColumnIds = indicatorData.Select(a => a.EntityDynamicColumnId).ToList();
            var dynamicCellData = baseDynamicCellData.Where(a => a.InstanceId == model.InstanceId
                && beneficiaryIds.Contains(a.BeneficiaryId)
                && entityColumnIds.Contains(a.EntityDynamicColumnId)
                ).ToList();

            var listItemData = new List<ListItem>();

            indicatorData.ForEach(a =>
            {
                if (a.EntityDynamicColumn.ColumnList != null)
                {
                    listItemData.AddRange(a.EntityDynamicColumn.ColumnList.ListItems);
                }
            });

            var combineAllData = (from b in benData
                                  join bcmp in campData on b.BeneficiaryCampId equals bcmp.Id
                                  join blk in blockData on b.BlockId equals blk.Id
                                  join sblk in subBlockData on b.SubBlockId equals sblk.Id
                                  join f in facilityData on b.FacilityId equals f.Id
                                  join c in campData on f.CampId equals c.Id into ca
                                  from fcmp in ca.DefaultIfEmpty()
                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id

                                  join ii in indicatorData on model.ViewId equals ii.GridViewId
                                  join dc in dynamicCellData on new { BeneficiaryId = b.EntityId, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.BeneficiaryId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()

                                  select new BeneficiaryRawViewModel
                                  {
                                      InstanceId = b.InstanceId,
                                      EntityId = b.EntityId,
                                      BeneficiaryName = b.BeneficiaryName,
                                      UnhcrId = b.UnhcrId,
                                      FCNId = b.FCNId,
                                      FatherName = b.FatherName,
                                      MotherName = b.MotherName,
                                      DateOfBirth = b.DateOfBirth,
                                      EnrollmentDate = b.EnrollmentDate,
                                      Disabled = b.Disabled,
                                      LevelOfStudy = b.LevelOfStudy,
                                      Sex = b.Sex,

                                      FacilityId = f.Id,
                                      FacilityName = f.FacilityName,

                                      FacilityCampId = fcmp?.Id,
                                      FacilityCampName = fcmp?.Name,
                                      BeneficiaryCampId = bcmp.Id,
                                      BeneficiaryCampName = bcmp.Name,
                                      BlockId = blk.Id,
                                      BlockName = blk.Name,
                                      SubBlockId = sblk.Id,
                                      SubBlockName = sblk.Name,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,

                                      EntityColumnId = ii.EntityDynamicColumnId,
                                      EntityColumnName = ii.EntityDynamicColumn.Name,
                                      ColumnListId = ii.EntityDynamicColumn.ColumnListId,
                                      ColumnListName = ii.EntityDynamicColumn.ColumnList?.Name,
                                      DataType = ii.EntityDynamicColumn.ColumnType,
                                      IsMultiValued = ii.EntityDynamicColumn.IsMultiValued,
                                      IsFixed = ii.EntityDynamicColumn.IsFixed,
                                      ListItemId = liData?.Id,
                                      ListItemTitle = liData?.Title,
                                      ListItemValue = liData?.Value,
                                      PropertiesId = dynCellData?.Id,
                                      PropertiesValue = dynCellData?.Value,
                                      CollectionStatus = b.CollectionStatus
                                  }).ToList();

            var beneficiary = SetBeneficiaryViewModel(combineAllData);

            var finalBeneficiaries = SetFixedPropertiesName(beneficiary);

            return new PagedResponse<BeneficiaryViewModel>(beneficiary, total, model.PageNo, model.PageSize);
        }

        private List<BeneficiaryViewModel> SetFixedPropertiesName(List<BeneficiaryViewModel> beneficiaries)
        {
            beneficiaries.ForEach(ben =>
            {
                //var sexProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.Sex).FirstOrDefault();
                //if (sexProp != null)
                //    sexProp.Values = new List<string> { ((Gender)Convert.ToInt32(sexProp.Values[0])).ToString() };

                //var levelOfStudyProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.LevelOfStudy).FirstOrDefault();
                //if (levelOfStudyProp != null)
                //    levelOfStudyProp.Values = new List<string> { ((LevelOfStudy)Convert.ToInt32(levelOfStudyProp.Values[0])).ToString() };

                var facilityProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.FacilityId).FirstOrDefault();
                if (facilityProp != null)
                    facilityProp.Values = new List<string> { ben.FacilityName };

                var campProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.CampId).FirstOrDefault();
                if (campProp != null)
                    campProp.Values = new List<string> { ben.BeneficiaryCampName };

                var blockProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.BlockId).FirstOrDefault();
                if (blockProp != null)
                    blockProp.Values = new List<string> { ben.BlockName };

                var subblockProp = ben.Properties.Where(a => a.EntityColumnId == BeneficairyFixedColumns.SubBlockId).FirstOrDefault();
                if (subblockProp != null)
                    subblockProp.Values = new List<string> { ben.SubBlockName };
            });

            return beneficiaries;
        }
        public async Task<PagedResponse<BeneficiaryViewModel>> GetAllByInstanceId(BeneficiaryByViewIdQueryModel model, long maxFacilityInstanceId)
        {
            var columnIds = await _context.InstanceIndicators.Where(a => a.InstanceId == model.InstanceId)
                .Select(a => a.EntityDynamicColumnId).ToListAsync();
            if (columnIds.Count == 0)
            {
                throw new InstanceHasNoIndicatorException();
            }

            var dynamicCellData = await _context.BeneficiaryDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var filterBeneficaryRaw = await BeneficiaryRawAsync(dynamicCellData, model, maxFacilityInstanceId);

            var total = filterBeneficaryRaw.Count();
            var benData = filterBeneficaryRaw.OrderBy(a => a.EntityId).Skip(model.Skip()).Take(model.PageSize).ToList();
            var beneficiaryIds = benData.Select(a => a.EntityId).ToList();
            var facilityIds = benData.Select(a => a.FacilityId).Distinct().ToList();

            var facilityDynamicCellData = await _context.FacilityDynamicCells
                .Where(a => a.InstanceId == maxFacilityInstanceId && facilityIds.Contains(a.FacilityId)
                && (a.EntityDynamicColumnId == FacilityFixedIndicators.Name
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Camp
                || a.EntityDynamicColumnId == FacilityFixedIndicators.Block)
                ).ToListAsync();

            var facilityData = facilityDynamicCellData.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
                FacilityName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Name).Select(h => h.Value).FirstOrDefault(),

                ProgrammingPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ImplemantationPartnerId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),

                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();
            var campIds = benData.Select(a => a.BeneficiaryCampId).ToList().Union(facilityCampIds);
            var blockIds = benData.Select(a => a.BlockId).ToList().Union(facilityBlockIds);
            var subBlockIds = benData.Select(a => a.SubBlockId).ToList();
            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var campData = await _context.Camps.Where(a => campIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => blockIds.Contains(a.Id)).ToListAsync();
            var subBlockData = await _context.SubBlocks.Where(a => subBlockIds.Contains(a.Id)).ToListAsync();
            var espData = await _context.EducationSectorPartners.Where(a => espIds.Contains(a.Id)).ToListAsync();

            var indicatorData = await _context.InstanceIndicators
                .Include(a => a.EntityDynamicColumn)
                .ThenInclude(a => a.ColumnList)
                .ThenInclude(a => a.ListItems)
                .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var listItemData = new List<ListItem>();

            indicatorData.ForEach(a =>
            {
                if (a.EntityDynamicColumn.ColumnList != null)
                {
                    listItemData.AddRange(a.EntityDynamicColumn.ColumnList.ListItems);
                }
            });

            var entityColumnIds = indicatorData.Select(a => a.EntityDynamicColumnId).ToList();
            var dynamicCellDataForQuery = dynamicCellData.Where(a => beneficiaryIds.Contains(a.BeneficiaryId)
            && entityColumnIds.Contains(a.EntityDynamicColumnId)
            ).ToList();
            var combineAllData = (from b in benData
                                  join bcmp in campData on b.BeneficiaryCampId equals bcmp.Id
                                  join blk in blockData on b.BlockId equals blk.Id
                                  join sblk in subBlockData on b.SubBlockId equals sblk.Id
                                  join f in facilityData on b.FacilityId equals f.Id
                                  join c in campData on f.CampId equals c.Id into ca
                                  from fcmp in ca.DefaultIfEmpty()
                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id

                                  join ii in indicatorData on b.InstanceId equals ii.InstanceId
                                  join dc in dynamicCellDataForQuery on new { BeneficiaryId = b.EntityId, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.BeneficiaryId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()

                                  select new BeneficiaryRawViewModel
                                  {
                                      InstanceId = b.InstanceId,
                                      EntityId = b.EntityId,
                                      BeneficiaryName = b.BeneficiaryName,
                                      UnhcrId = b.UnhcrId,
                                      FCNId = b.FCNId,
                                      FatherName = b.FatherName,
                                      MotherName = b.MotherName,
                                      DateOfBirth = b.DateOfBirth,
                                      EnrollmentDate = b.EnrollmentDate,
                                      Disabled = b.Disabled,
                                      LevelOfStudy = b.LevelOfStudy,
                                      Sex = b.Sex,

                                      FacilityId = f.Id,
                                      FacilityName = f.FacilityName,

                                      FacilityCampId = fcmp?.Id,
                                      FacilityCampName = fcmp?.Name,
                                      BeneficiaryCampId = bcmp.Id,
                                      BeneficiaryCampName = bcmp.Name,
                                      BlockId = blk.Id,
                                      BlockName = blk.Name,
                                      SubBlockId = sblk.Id,
                                      SubBlockName = sblk.Name,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,

                                      EntityColumnId = ii.EntityDynamicColumnId,
                                      EntityColumnName = ii.EntityDynamicColumn.Name,
                                      ColumnListId = ii.EntityDynamicColumn.ColumnListId,
                                      ColumnListName = ii.EntityDynamicColumn.ColumnList?.Name,
                                      DataType = ii.EntityDynamicColumn.ColumnType,
                                      IsMultiValued = ii.EntityDynamicColumn.IsMultiValued,
                                      IsFixed = ii.EntityDynamicColumn.IsFixed,
                                      ListItemId = liData?.Id,
                                      ListItemTitle = liData?.Title,
                                      ListItemValue = liData?.Value,
                                      PropertiesId = dynCellData?.Id,
                                      PropertiesValue = dynCellData?.Value,
                                      CollectionStatus = b.CollectionStatus
                                  }).ToList();

            var beneficiary = SetBeneficiaryViewModel(combineAllData);

            var finalBeneficiaries = SetFixedPropertiesName(beneficiary);


            return new PagedResponse<BeneficiaryViewModel>(finalBeneficiaries, total, model.PageNo, model.PageSize);
        }


        public async Task<BeneficiaryEditViewModel> GetById(long id, long instanceId)
        {
            //TODO: beneficiary new schema

            var beneficiaryRaw = await (from b in _context.BeneficiaryView
                                        join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
                                        join blk in _context.Blocks on b.BlockId equals blk.Id
                                        join sblk in _context.SubBlocks on b.SubBlockId equals sblk.Id
                                        join f in _context.FacilityView on b.FacilityId equals f.Id
                                        join c in _context.Camps on f.CampId equals c.Id into ca
                                        from fcmp in ca.DefaultIfEmpty()
                                        join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                                        join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id

                                        where b.Id == id && b.InstanceId == instanceId
                                        select new BeneficiaryEditViewModel
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            FacilityId = f.Id,
                                            FacilityCode = f.FacilityCode,
                                            FacilityName = f.Name,
                                            FacilityCampId = f.CampId,
                                            FacilityCampName = fcmp.Name,
                                            BeneficiaryCampId = b.BeneficiaryCampId,
                                            BeneficiaryCampName = bcmp.Name,
                                            BlockId = b.BlockId,
                                            BlockName = blk.Name,
                                            SubBlockId = b.SubBlockId,
                                            SubBlockName = sblk.Name,
                                            ProgrammingPartnerId = f.ProgramPartnerId,
                                            ProgrammingPartnerName = pp.PartnerName,
                                            ImplemantationPartnerId = f.ImplementationPartnerId,
                                            ImplemantationPartnerName = ip.PartnerName,
                                            Disabled = b.Disabled,
                                            DateOfBirth = b.DateOfBirth.ToString("yyyy-MM-dd"),
                                            EnrollmentDate = b.EnrollmentDate.ToString("yyyy-MM-dd"),
                                            FatherName = b.FatherName,
                                            Sex = b.Sex,
                                            UnhcrId = b.UnhcrId,
                                            Remarks = b.Remarks,
                                            FCNId = b.FCNId,
                                            LevelOfStudy = b.LevelOfStudy,
                                            MotherName = b.MotherName
                                        }).FirstOrDefaultAsync();
            return beneficiaryRaw;
        }


        public async Task StartCollectionForSelectedBeneficiaries(long instanceId, List<long> beneficiaryIds)
        {

            var beneficiaryDataCollection = beneficiaryIds
                .Select(id => new BeneficiaryDataCollectionStatus
                {
                    BeneficiaryId = id,
                    InstanceId = instanceId,
                    Status = CollectionStatus.NotCollected
                }).ToList();
            _context.BeneciaryDataCollectionStatuses.AddRange(beneficiaryDataCollection);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Count(Specification<Beneficiary> specification)
        {
            return await _context.Beneficiary.Where(specification.ToExpression()).CountAsync();
        }

        public async Task<Beneficiary> GetBeneficiaryByProgressId(string unhcrId)
        {
            return new Beneficiary();
            //return await _context.Beneficiary.Where(x => x.UnhcrId == unhcrId).FirstOrDefaultAsync();
        }

        public async Task<List<BeneficiaryRawViewModel>> GetAllBeneficiariesForVersionDataTemplate(long instanceId, long maxFacilityInstanceId)
        {
            var query = from beneficiaryView in _context.BeneficiaryView
                            .Where(x => x.InstanceId == instanceId)
                        join facilityView in _context.FacilityView.Where(x => x.InstanceId == maxFacilityInstanceId)
                            on beneficiaryView.FacilityId equals facilityView.Id

                        join camps in _context.Camps on beneficiaryView.BeneficiaryCampId equals camps.Id
                            into campGroup
                        from camp in campGroup.DefaultIfEmpty()

                        join blocks in _context.Blocks on beneficiaryView.BlockId equals blocks.Id
                            into blockGroup
                        from block in blockGroup.DefaultIfEmpty()

                        join subBlocks in _context.SubBlocks on beneficiaryView.SubBlockId equals subBlocks.Id
                            into subBlocksGroup
                        from subBlock in subBlocksGroup.DefaultIfEmpty()
                        join collectionStatus in _context.BeneciaryDataCollectionStatuses on
                            new { BeneficiaryId = beneficiaryView.Id, InstanceId = instanceId } equals
                            new { collectionStatus.BeneficiaryId, collectionStatus.InstanceId }
                        where collectionStatus.Status != CollectionStatus.Deleted
                        select new BeneficiaryRawViewModel
                        {
                            BeneficiaryId = beneficiaryView.Id,
                            BeneficiaryName = beneficiaryView.Name,
                            UnhcrId = beneficiaryView.UnhcrId,
                            FatherName = beneficiaryView.FatherName,
                            MotherName = beneficiaryView.MotherName,
                            FCNId = beneficiaryView.FCNId,
                            DateOfBirth = beneficiaryView.DateOfBirth,
                            Sex = beneficiaryView.Sex,
                            Disabled = beneficiaryView.Disabled,
                            LevelOfStudy = beneficiaryView.LevelOfStudy,
                            EnrollmentDate = beneficiaryView.EnrollmentDate,
                            BeneficiaryCampId = beneficiaryView.BeneficiaryCampId,
                            CampSsId = camp.SSID,
                            BlockId = beneficiaryView.BlockId,
                            BlockCode = block.Code,
                            SubBlockId = beneficiaryView.SubBlockId,
                            SubBlockName = subBlock.Name,
                            Remarks = beneficiaryView.Remarks,
                            FacilityId = facilityView.Id,
                            FacilityName = facilityView.Name,
                            FacilityCode = facilityView.FacilityCode
                        };

            var allBeneficiaries = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
                .ApplyCondition(query, maxFacilityInstanceId).OrderBy(a => a.FacilityId).ToListAsync();

            return allBeneficiaries;
        }

        public long GetBeneficiaryCountByLevelOfStudyAndSex(List<BeneficiaryModelForAutoCalculation> beneficiaries, LevelOfStudy levelOfStudy, Gender gender)
        {
            int beneficiaryCount = beneficiaries
                .Where(b =>
                b.Sex.Equals(gender)
                && b.LevelOfStudy == levelOfStudy
                ).Count();
            return beneficiaryCount;
        }
        public long GetBeneficiaryCountByAgeAndSex(List<BeneficiaryModelForAutoCalculation> beneficiaries, AgeGroup ageGroup, Gender gender)
        {
            List<BeneficiaryModelForAutoCalculation> query = beneficiaries
                .Where(b => b.Sex.Equals(gender))
                .Select(a => new BeneficiaryModelForAutoCalculation
                {
                    DateOfBirth = a.DateOfBirth,
                }).ToList();
            query = AppendAgeQuery(ageGroup, query);
            return query.Count();
        }
        public long GetDisabledBeneficiaryCountByAgeAndSex(List<BeneficiaryModelForAutoCalculation> beneficiaries, AgeGroup ageGroup, Gender gender)
        {
            List<BeneficiaryModelForAutoCalculation> query = beneficiaries.Where(b =>
             b.Sex.Equals(gender) && b.Disabled
            ).Select(a => new BeneficiaryModelForAutoCalculation
            {
                DateOfBirth = a.DateOfBirth,
            }).ToList();
            query = AppendAgeQuery(ageGroup, query);
            return query.Count();
        }
        private List<BeneficiaryModelForAutoCalculation> AppendAgeQuery(AgeGroup ageGroup, List<BeneficiaryModelForAutoCalculation> query)
        {
            switch (ageGroup)
            {
                case AgeGroup.Three:
                    query = query.Where(b => b.Age <= 3).ToList();
                    break;
                case AgeGroup.Four_Five:
                    query = query.Where(b => b.Age >= 4 && b.Age <= 5).ToList();
                    break;
                case AgeGroup.Six_Fourteen:
                    query = query.Where(b => b.Age >= 6 && b.Age <= 14).ToList();
                    break;
                case AgeGroup.Fifteen_Eighteen:
                    query = query.Where(b => b.Age >= 15 && b.Age <= 18).ToList();
                    break;
                case AgeGroup.Nineteen_TwentyFour:
                    query = query.Where(b => b.Age >= 19 && b.Age <= 24).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ageGroup), ageGroup, null);
            }
            return query;
        }

        private void SaveAutoCalculatedField(UnicefEduDbContext unicefEduDbContext, long instanceId, long facilityId, int entityDynamicColumnId, string value)
        {
            var dynamicCell = unicefEduDbContext.FacilityDynamicCells
                .Where(a => a.InstanceId == instanceId && a.FacilityId == facilityId
                && a.EntityDynamicColumnId == entityDynamicColumnId).FirstOrDefault();
            if (dynamicCell != null)
            {
                dynamicCell.Value = value;
                unicefEduDbContext.FacilityDynamicCells.Update(dynamicCell);
            }
            else
            {
                var newDynamicCell = new FacilityDynamicCell()
                {
                    InstanceId = instanceId,
                    CreatedBy = _currentLoginUserService.UserId,
                    DateCreated = DateTime.Now,
                    FacilityId = facilityId,
                    EntityDynamicColumnId = entityDynamicColumnId,
                    Value = value
                };
                unicefEduDbContext.FacilityDynamicCells.Add(newDynamicCell);
            }
            unicefEduDbContext.SaveChanges();
        }
        public void AutoUpdateFacilityIndicators(FacilityApprovalViewModel model)
        {
            //var indicators = AutoCalculatedIndicatorConstants.AgeIndicators[AgeGroup.Three];
            //var facilities = new List<Facility>(); // from database 

            var taskList = new List<Task>();
            foreach (var facilityId in model.FacilityIds)
            {
                var task = new Task(() => UpdateFacilityIndicatorByFacilityId(facilityId, model.InstanceId));
                task.Start();
                taskList.Add(task);
            }

            if (taskList.Count > 0)
            {
                Task.WaitAll(taskList.ToArray());
            }
            //foreach (var indicatorId in indicators)
            //{
            //    // for 17: # of rohingya girls (3) enrolled in LF 
            //    foreach (var facility in facilities)
            //    {
            //        var count = GetBeneficiaryCountByAgeAndSex(facility.Id, ageGroup, Gender.Female);
            //        //TODO: insert / update at facility_dynamic_cell where InstanceId=, FacilityId=, EntityDynamicId=  
            //        // example of getting result from beneficiary indicator 
            //        //long count = GetBeneficiaryCountRecievedEduMaterials(ageGroup, facility.Id, instanceId);
            //    }
            //}
        }
        private void UpdateFacilityIndicatorByFacilityId(long facilityId, long instanceId)
        {

            //TODO: beneficiary new schema
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ucontext =
               scope.ServiceProvider
                   .GetRequiredService<UnicefEduDbContext>();

                var facilityInstanceDate = _ucontext.ScheduleInstances
                                            .Where(a => a.Id == instanceId).Select(a => a.DataCollectionDate).FirstOrDefault();

                var beneficiaryInstanceId = _ucontext.ScheduleInstances
                    .Where(a => a.Schedule.ScheduleFor == EntityType.Beneficiary
                    && a.Status == InstanceStatus.Completed)
                    .OrderByDescending(a => a.DataCollectionDate)
                    .Select(a => a.Id).FirstOrDefault();

                long benEduIndicatorId = BeneficairyFixedColumns.Have_received_educational_material;

                List<BeneficiaryModelForAutoCalculation> beneficiaryRaw =
                    (
                    from b in _ucontext.BeneficiaryView
                    join bdcs in _ucontext.BeneciaryDataCollectionStatuses on new { BeneficiaryId = b.Id, b.InstanceId } equals new { bdcs.BeneficiaryId, bdcs.InstanceId }
                    where b.InstanceId.Equals(beneficiaryInstanceId) &&
                    bdcs.Status == CollectionStatus.Approved && b.FacilityId.Equals(facilityId)

                    select
                    new BeneficiaryModelForAutoCalculation
                    {
                        DateOfBirth = b.DateOfBirth,
                        Disabled = b.Disabled,
                        LevelOfStudy = b.LevelOfStudy,
                        Sex = b.Sex
                    }
                    ).ToList();


                //_ucontext.BeneficiaryView
                //.Where(b => b.FacilityId.Equals(facilityId) //&& b.IsActive && b.IsApproved
                //            ).Select(a => new BeneficiaryModelForAutoCalculation
                //            {
                //                DateOfBirth = a.DateOfBirth,
                //                Disabled = a.Disabled,
                //                LevelOfStudy = a.LevelOfStudy,
                //                Sex = a.Sex
                //            }).ToList();
                List<BeneficiaryModelForAutoCalculation> beneficiaryEduMaterialRaw =
                    (from bdc in _ucontext.BeneficiaryDynamicCells
                     join b in _ucontext.BeneficiaryView on bdc.BeneficiaryId equals b.Id
                     join bdcs in _ucontext.BeneciaryDataCollectionStatuses on new { BeneficiaryId = b.Id, b.InstanceId } equals new { bdcs.BeneficiaryId, bdcs.InstanceId }
                     where bdc.InstanceId.Equals(beneficiaryInstanceId) &&
                     bdc.EntityDynamicColumnId.Equals(benEduIndicatorId) &&
                     bdc.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) &&
                     b.FacilityId.Equals(facilityId) && bdcs.Status == CollectionStatus.Approved
                     select new BeneficiaryModelForAutoCalculation
                     {
                         DateOfBirth = b.DateOfBirth,
                         Disabled = b.Disabled,
                         LevelOfStudy = b.LevelOfStudy,
                         Sex = b.Sex
                     }
                    ).ToList();

                foreach (var ageGroupCollection in AutoCalculatedIndicatorConstants.IndicatorAgeGroupWise)
                {
                    foreach (var genderCollection in ageGroupCollection.Value)
                    {
                        var enrollColumnId = genderCollection.Value["Enroll"];
                        var enrollValue = GetBeneficiaryCountByAgeAndSex(beneficiaryRaw, ageGroupCollection.Key, genderCollection.Key);
                        SaveAutoCalculatedField(_ucontext, instanceId, facilityId, enrollColumnId, enrollValue.ToString());

                        var disableEnrollColumnId = genderCollection.Value["DisableEnroll"];
                        var disableEnrollValue = GetDisabledBeneficiaryCountByAgeAndSex(beneficiaryRaw, ageGroupCollection.Key, genderCollection.Key);
                        SaveAutoCalculatedField(_ucontext, instanceId, facilityId, disableEnrollColumnId, disableEnrollValue.ToString());

                        var eduMaterialColumnId = genderCollection.Value["EduMaterialReceived"];
                        var eduMaterialValue = GetBeneficiaryCountByAgeAndSex(beneficiaryEduMaterialRaw, ageGroupCollection.Key, genderCollection.Key);
                        SaveAutoCalculatedField(_ucontext, instanceId, facilityId, eduMaterialColumnId, eduMaterialValue.ToString());
                    }
                }
                foreach (var levelOfStudyCollection in AutoCalculatedIndicatorConstants.IndicatorLevelOfStudyWise)
                {
                    foreach (var gender in levelOfStudyCollection.Value)
                    {
                        var columnId = gender.Value;
                        var value = GetBeneficiaryCountByLevelOfStudyAndSex(beneficiaryRaw, levelOfStudyCollection.Key, gender.Key);
                        SaveAutoCalculatedField(_ucontext, instanceId, facilityId, columnId, value.ToString());
                    }
                }
            }
        }

        public async Task Revert(long id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ucontext =
               scope.ServiceProvider
                   .GetRequiredService<UnicefEduDbContext>();

                var existingEntity = await _ucontext.Beneficiary.Where(a => a.Id == id).FirstOrDefaultAsync();

                if (existingEntity != null)
                {
                    _ucontext.Beneficiary.Remove(existingEntity);
                }
                await _ucontext.SaveChangesAsync();
            }
        }

        public async Task<List<BeneficiaryView>> GetBeneficiaryBasicData(List<long> beneficiaryIds, long instanceId)
        {
            return await _context.BeneficiaryView.Where(a => beneficiaryIds.Contains(a.Id) && a.InstanceId == instanceId)
                .ToListAsync();
        }

        public IQueryable<BeneficiaryView> GetBeneficiaryView()
        {
            return _context.BeneficiaryView.AsQueryable();
        }
    }
}

