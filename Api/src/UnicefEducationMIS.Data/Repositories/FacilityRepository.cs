using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Reporting.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.Specifications;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Data.Factory;

namespace UnicefEducationMIS.Data.Repositories
{


    public class FacilityRepository : BaseRepository<Facility, long>, IFacilityRepository
    {

        private readonly FacilityUserConditionFactory _facilityUserConditionFactory;
        private readonly IModelToIndicatorConverter _modelToIndicatorConverter;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;

        public FacilityRepository(UnicefEduDbContext context, FacilityUserConditionFactory facilityUserConditionFactory,
            ICurrentLoginUserService currentLoginUserService, IModelToIndicatorConverter modelToIndicatorConverter
            , IScheduleInstanceRepository scheduleInstanceRepository) : base(context)
        {
            _facilityUserConditionFactory = facilityUserConditionFactory;
            _modelToIndicatorConverter = modelToIndicatorConverter;
            _scheduleInstanceRepository = scheduleInstanceRepository;
        }

        public new async Task Add(Facility facility)
        {
            await _context.AddAsync(facility);
        }

        public async Task Import(Facility facility)
        {
            await _context.AddAsync(facility);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetMaxFacilityCode()
        {
            var codeList = new List<string>() { SystemDefaults.FacilityCodeStart.ToString() };

            codeList.AddRange(
                await _context.FacilityDynamicCells
                .Where(x => x.EntityDynamicColumnId == FacilityFixedIndicators.Code)
                .Select(x => x.Value)
                .ToListAsync()
                );

            var maxCode =
                codeList
                    .Select(x =>
                    {
                        var isSuccess = long.TryParse(x, out var value);
                        return (value, isSuccess);
                    })
                    .Where(x => x.isSuccess)
                    .Select(x => x.value)
                    .Max();
            return (maxCode).ToString();
        }

        public new async Task Update(Facility facility)
        {
            var existing = _context.Facility.FirstOrDefault(x => x.Id == facility.Id);
            if (existing == null)
                throw new RecordNotFound();
            _context.Update(existing);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(List<long> entityIds)
        {
            var facilities = await _context.FacilityView.Where(x => entityIds.Contains(x.Id)).ToListAsync();
            if (facilities.Count > 0)
            {
                _context.FacilityView.RemoveRange(facilities);

                var dynamicCell = await _context.FacilityDynamicCells.Where(x => entityIds.Contains(x.FacilityId)).ToListAsync();
                if (dynamicCell.Count > 0)
                    _context.RemoveRange(dynamicCell);

                var facilityDataCollectionStatus = await _context.FacilityDataCollectionStatuses.Where(x => entityIds.Contains(x.FacilityId)).ToListAsync();
                if (facilityDataCollectionStatus.Count > 0)
                    _context.RemoveRange(facilityDataCollectionStatus);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new RecordNotFound("Facility Not Found");
            }
        }
        public async Task<bool> IsFacilityCodeExist(string facilityCode)
        {
            return await _context
                .FacilityDynamicCells
                .AnyAsync(a => a.EntityDynamicColumnId == FacilityFixedIndicators.Code && a.Value == facilityCode);
        }


        public IQueryable<FacilityRawViewModel> GetFacilityByInstance(long instanceId)
        {


            var query = (from f in _context.FacilityView.Where(x => x.InstanceId == instanceId)
                         join uno in _context.Unions on f.UnionId equals uno.Id
                         join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                         join c in _context.Camps on f.CampId equals c.Id into ca
                         from bcmp in ca.DefaultIfEmpty()
                         join b in _context.Blocks on f.BlockId equals b.Id into ba
                         from blk in ba.DefaultIfEmpty()

                         join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                         join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                         join tu in _context.Users.Where(x => !x.IsDeleted) on f.TeacherId equals tu.Id into t
                         from teacherData in t.DefaultIfEmpty()
                         join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Facility } equals new
                         { edc.EntityTypeId }

                         join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                         from columnListData in def.DefaultIfEmpty()
                         join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                         from listItemData in def1.DefaultIfEmpty()
                         join dc in _context.FacilityDynamicCells on new
                         { FacilityId = f.Id, EntityDynamicColumnId = edc.Id, InstanceId = instanceId } equals new
                         { dc.FacilityId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                         from dynamicCellData in def2.DefaultIfEmpty()

                         join ii in _context.InstanceIndicators on new { dynamicCellData.EntityDynamicColumnId, dynamicCellData.InstanceId }
                             equals new { ii.EntityDynamicColumnId, ii.InstanceId }
                         into iiGroup
                         from indicator in iiGroup.DefaultIfEmpty()
                         join dcs in _context.FacilityDataCollectionStatuses
                             on new { FacilityId = f.Id, InstanceId = instanceId } equals new { dcs.FacilityId, dcs.InstanceId }

                         where dynamicCellData.InstanceId == instanceId && dcs.Status == CollectionStatus.Approved
                         select new FacilityRawViewModel
                         {
                             Id = f.Id,
                             FacilityName = f.Name,
                             FacilityCode = f.FacilityCode,
                             BlockId = blk.Id,
                             BlockName = blk.Name,
                             UpazilaId = upa.Id,
                             UpazilaName = upa.Name,
                             UnionId = uno.Id,
                             UnionName = uno.Name,
                             CampId = f.CampId,
                             CampName = bcmp.Name,
                             CampSSID = bcmp.SSID,
                             BlockCode = blk.Code,
                             Donors = f.Donors,
                             Latitude = f.Latitude,
                             longitude = f.Longitude,
                             NonEducationPartner = f.NonEducationPartner,
                             ParaName = f.ParaName,
                             ProgrammingPartnerId = pp.Id,
                             ProgrammingPartnerName = pp.PartnerName,
                             ImplemantationPartnerId = ip.Id,
                             ImplemantationPartnerName = ip.PartnerName,
                             TeacherId = f.TeacherId,
                             TeacherName = teacherData.FullName,
                             TeacherEmail = teacherData.Email,
                             FacilityType = f.FacilityType,
                             FacilityStatus = f.FacilityStatus,
                             TargetedPopulation = f.TargetedPopulation,

                             Remarks = f.Remarks,


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
                             ColumnOrder = indicator.ColumnOrder,

                         });

            return _facilityUserConditionFactory.GetFacilityUserCondition()
                .ApplyCondition(query, instanceId);
        }

        public IQueryable<FacilityRawViewModel> GetFacilityByInstanceForImportTemplate(long instanceId)
        {
            var query = (from f in _context.FacilityView.Where(x => x.InstanceId == instanceId)
                         join uno in _context.Unions on f.UnionId equals uno.Id
                         join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                         join c in _context.Camps on f.CampId equals c.Id into ca
                         from bcmp in ca.DefaultIfEmpty()
                         join b in _context.Blocks on f.BlockId equals b.Id into ba
                         from blk in ba.DefaultIfEmpty()

                         join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                         join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                         join tu in _context.Users.Where(x => !x.IsDeleted) on f.TeacherId equals tu.Id into t
                         from teacherData in t.DefaultIfEmpty()
                         join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Facility } equals new
                         { edc.EntityTypeId }

                         join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                         from columnListData in def.DefaultIfEmpty()
                         join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                         from listItemData in def1.DefaultIfEmpty()
                         join dc in _context.FacilityDynamicCells on new
                         { FacilityId = f.Id, EntityDynamicColumnId = edc.Id, InstanceId = instanceId } equals new
                         { dc.FacilityId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                         from dynamicCellData in def2.DefaultIfEmpty()

                         join ii in _context.InstanceIndicators on new { dynamicCellData.EntityDynamicColumnId, dynamicCellData.InstanceId }
                             equals new { ii.EntityDynamicColumnId, ii.InstanceId }
                         into iiGroup
                         from indicator in iiGroup.DefaultIfEmpty()
                         join dcs in _context.FacilityDataCollectionStatuses
                             on new { FacilityId = f.Id, InstanceId = instanceId } equals new { dcs.FacilityId, dcs.InstanceId }

                         where dynamicCellData.InstanceId == instanceId && dcs.Status != CollectionStatus.Deleted
                         select new FacilityRawViewModel
                         {
                             Id = f.Id,
                             FacilityName = f.Name,
                             FacilityCode = f.FacilityCode,
                             BlockId = blk.Id,
                             BlockName = blk.Name,
                             UpazilaId = upa.Id,
                             UpazilaName = upa.Name,
                             UnionId = uno.Id,
                             UnionName = uno.Name,
                             CampId = f.CampId,
                             CampName = bcmp.Name,
                             CampSSID = bcmp.SSID,
                             BlockCode = blk.Code,
                             Donors = f.Donors,
                             Latitude = f.Latitude,
                             longitude = f.Longitude,
                             NonEducationPartner = f.NonEducationPartner,
                             ParaName = f.ParaName,
                             ProgrammingPartnerId = pp.Id,
                             ProgrammingPartnerName = pp.PartnerName,
                             ImplemantationPartnerId = ip.Id,
                             ImplemantationPartnerName = ip.PartnerName,
                             TeacherId = f.TeacherId,
                             TeacherName = teacherData.FullName,
                             TeacherEmail = teacherData.Email,
                             FacilityType = f.FacilityType,
                             FacilityStatus = f.FacilityStatus,
                             TargetedPopulation = f.TargetedPopulation,

                             Remarks = f.Remarks,


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
                             ColumnOrder = indicator.ColumnOrder,

                         });

            return _facilityUserConditionFactory.GetFacilityUserCondition()
                .ApplyCondition(query, instanceId);
        }

        public List<FacilityRawViewModel> GetFacilityByInstances(List<long> instanceIds)
        {
            var query = (from f in _context.FacilityView.Where(x => instanceIds.Contains(x.InstanceId))
                         join uno in _context.Unions on f.UnionId equals uno.Id
                         join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                         join c in _context.Camps on f.CampId equals c.Id into ca
                         from bcmp in ca.DefaultIfEmpty()
                         join b in _context.Blocks on f.BlockId equals b.Id into ba
                         from blk in ba.DefaultIfEmpty()

                         join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                         join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                         join tu in _context.Users on f.TeacherId equals tu.Id into t
                         from teacherData in t.DefaultIfEmpty()
                         join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Facility } equals new
                         { edc.EntityTypeId }

                         join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                         from columnListData in def.DefaultIfEmpty()
                         join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                         from listItemData in def1.DefaultIfEmpty()

                         join dc in _context.FacilityDynamicCells on
                             new { FacilityId = f.Id, EntityDynamicColumnId = edc.Id, f.InstanceId } equals
                             new { dc.FacilityId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                         from dynamicCellData in def2.DefaultIfEmpty()

                         join ii in _context.InstanceIndicators on
                             new { dynamicCellData.EntityDynamicColumnId, dynamicCellData.InstanceId }
                             equals new { ii.EntityDynamicColumnId, ii.InstanceId }
                         into iiGroup
                         from indicator in iiGroup.DefaultIfEmpty()

                         join sci in _context.ScheduleInstances on indicator.InstanceId equals sci.Id

                         join dcs in _context.FacilityDataCollectionStatuses
                             on new { FacilityId = f.Id, f.InstanceId } equals new { dcs.FacilityId, dcs.InstanceId }

                         where instanceIds.Contains(dynamicCellData.InstanceId) && dcs.Status == CollectionStatus.Approved
                         select new FacilityRawViewModel
                         {
                             Id = f.Id,
                             FacilityName = f.Name,
                             FacilityCode = f.FacilityCode,
                             BlockId = blk.Id,
                             BlockName = blk.Name,
                             UpazilaId = upa.Id,
                             UpazilaName = upa.Name,
                             UnionId = uno.Id,
                             UnionName = uno.Name,
                             CampId = f.CampId,
                             CampName = bcmp.Name,
                             CampSSID = bcmp.SSID,
                             BlockCode = blk.Code,
                             Donors = f.Donors,
                             Latitude = f.Latitude,
                             longitude = f.Longitude,
                             NonEducationPartner = f.NonEducationPartner,
                             ParaName = f.ParaName,
                             ProgrammingPartnerId = pp.Id,
                             ProgrammingPartnerName = pp.PartnerName,
                             ImplemantationPartnerId = ip.Id,
                             ImplemantationPartnerName = ip.PartnerName,
                             TeacherId = f.TeacherId,
                             TeacherName = teacherData.FullName,
                             TeacherEmail = teacherData.Email,
                             FacilityType = f.FacilityType,
                             FacilityStatus = f.FacilityStatus,
                             TargetedPopulation = f.TargetedPopulation,
                             InstanceId = indicator.InstanceId,
                             InstanceName = sci.Title,

                             Remarks = f.Remarks,


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
                             ColumnOrder = indicator.ColumnOrder
                         });

            var facilityRaws = new List<FacilityRawViewModel>();
            instanceIds.ForEach(async instanceId =>
            {
                query = _facilityUserConditionFactory.GetFacilityUserCondition()
                    .ApplyCondition(query, instanceId);

                facilityRaws.AddRange(query);
            });

            return facilityRaws;
        }

        public IQueryable<FacilityRawViewModel> GetCollection(long instanceId)
        {
            
            var beneficiary = (from f in _context.FacilityView
                               join uno in _context.Unions on f.UnionId equals uno.Id
                               join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from bcmp in ca.DefaultIfEmpty()
                               join b in _context.Blocks on f.BlockId equals b.Id into ba
                               from blk in ba.DefaultIfEmpty()

                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                               join tu in _context.Users on f.TeacherId equals tu.Id into t
                               from teadchrData in t.DefaultIfEmpty()
                               join edc in _context.EntityDynamicColumn on new { EntityTypeId = EntityType.Facility } equals new { edc.EntityTypeId }
                               join ii in _context.InstanceIndicators on new {f.InstanceId, EntityDynamicColumnId =edc.Id} equals new {ii.InstanceId,ii.EntityDynamicColumnId}
                               join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
                               from columnListData in def.DefaultIfEmpty()
                               join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
                               from listItemData in def1.DefaultIfEmpty()
                               join dc in _context.FacilityDynamicCells on new { FacilityId = f.Id, EntityDynamicColumnId = edc.Id, InstanceId = f.InstanceId } equals new { dc.FacilityId, dc.EntityDynamicColumnId, dc.InstanceId } into def2
                               from dynamicCellData in def2.DefaultIfEmpty()
                               join fdcs in _context.FacilityDataCollectionStatuses on new { FacilityId = f.Id, InstanceId = f.InstanceId } equals new { fdcs.FacilityId, fdcs.InstanceId }
                               join u in _context.Users on fdcs.UpdatedBy equals u.Id into def3
                               from approvedByUser in def3.DefaultIfEmpty()
                               where f.InstanceId.Equals(instanceId)
                               select new FacilityRawViewModel
                               {
                                   Id = f.Id,
                                   FacilityName = f.Name,
                                   FacilityCode = f.FacilityCode,
                                   BlockId = blk.Id,
                                   BlockName = blk.Name,
                                   UpazilaId = upa.Id,
                                   UpazilaName = upa.Name,
                                   UnionId = uno.Id,
                                   UnionName = uno.Name,
                                   CampId = f.CampId,
                                   CampName = bcmp.Name,
                                   CampSSID = bcmp.SSID,
                                   Donors = f.Donors,
                                   Latitude = f.Latitude,
                                   longitude = f.Longitude,
                                   NonEducationPartner = f.NonEducationPartner,

                                   ParaName = f.ParaName,
                                   ProgrammingPartnerId = pp.Id,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = ip.Id,
                                   ImplemantationPartnerName = ip.PartnerName,
                                   TeacherId = f.TeacherId,
                                   TeacherName = teadchrData.FullName,
                                   TeacherEmail = teadchrData.Email,
                                   FacilityType = f.FacilityType,
                                   FacilityStatus = f.FacilityStatus,
                                   TargetedPopulation = f.TargetedPopulation,

                                   Remarks = f.Remarks,


                                   EntityColumnId = edc.Id,
                                   EntityColumnName = edc.Name,
                                   ColumnListId = edc.ColumnListId,
                                   ColumnListName = columnListData.Name,
                                   ColumnOrder=ii.ColumnOrder,
                                   DataType = edc.ColumnType,
                                   IsMultiValued = edc.IsMultiValued,
                                   IsFixed = edc.IsFixed,
                                   ListItemId = listItemData.Id,
                                   ListItemTitle = listItemData.Title,
                                   ListItemValue = listItemData.Value,
                                   PropertiesId = dynamicCellData.Id,
                                   PropertiesValue = dynamicCellData.Value,
                                   CollectionStatus = fdcs.Status,
                                   ApproveDate = fdcs.LastUpdated,
                                   ApproveByUserEmail = approvedByUser.Email,
                                   ApproveByUserName = approvedByUser.FullName,
                                   ApproveByUserPhone = approvedByUser.PhoneNumber
                               });

            return beneficiary;
        }
        private async Task<List<FacilityRawViewModel>> GetPaginatedFacility(FacilityQueryModel facilityQueryModel)
        {
            var facility = (
                from f in _context.FacilityView
                join bdcs in _context.FacilityDataCollectionStatuses on new { f.InstanceId, FacilityId = f.Id } equals new { bdcs.InstanceId, bdcs.FacilityId }
                where f.InstanceId == facilityQueryModel.InstanceId && bdcs.Status == CollectionStatus.Approved
                select new FacilityRawViewModel
                {
                    Id = f.Id,
                    ProgrammingPartnerId = f.ProgramPartnerId,
                    ImplemantationPartnerId = f.ImplementationPartnerId
                });

            return await _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(facility, facilityQueryModel.InstanceId)
                         .Skip(facilityQueryModel.Skip())
                         .Take(facilityQueryModel.PageSize).ToListAsync();
        }
        private IQueryable<FacilityRawViewModel> GetRawFacility(FacilityByViewIdQueryModel facilityQueryModel)
        {
            var facility = (
                from f in _context.FacilityView
                join bdcs in _context.FacilityDataCollectionStatuses
                    on new { facilityQueryModel.InstanceId, FacilityId = f.Id }
                    equals new { bdcs.InstanceId, bdcs.FacilityId }
                where f.InstanceId == facilityQueryModel.InstanceId
                select new FacilityRawViewModel
                {
                    Id = f.Id,
                    FacilityName = f.Name,
                    FacilityCode = f.FacilityCode,
                    UpazilaId = f.UpazilaId,
                    UnionId = f.UnionId,
                    TargetedPopulation = f.TargetedPopulation,
                    FacilityType = f.FacilityType,
                    FacilityStatus = f.FacilityStatus,
                    ProgrammingPartnerId = f.ProgramPartnerId,
                    ImplemantationPartnerId = f.ImplementationPartnerId,
                    TeacherId = f.TeacherId,
                    CollectionStatus = bdcs.Status,
                }).Distinct();
            facility = ApplyFilter(facility, facilityQueryModel);
            return _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(facility, facilityQueryModel.InstanceId);
        }
        private IQueryable<FacilityRawViewModel> ApplyFilter(IQueryable<FacilityRawViewModel> rawFacility,
            FacilityByViewIdQueryModel filter)
        {

            if (!filter.CollectionStatus.NoneMatched())
            {
                rawFacility =
                    rawFacility.Where(x => x.CollectionStatus == filter.CollectionStatus);
            }

            if (!string.IsNullOrEmpty(filter.Filter.SearchText))
            {
                rawFacility = rawFacility.Where(a => a.FacilityName.Contains(filter.Filter.SearchText)
                || a.FacilityCode.Contains(filter.Filter.SearchText));
            }
            if (filter.Filter.UpazilaId != null)
            {

                rawFacility = rawFacility.Where(a => a.UpazilaId == filter.Filter.UpazilaId);
            }

            if (filter.Filter.UnionId != null)
            {

                rawFacility = rawFacility.Where(a => a.UnionId == filter.Filter.UnionId);
            }

            if (filter.Filter.TargetedPopulation != null)
            {
                rawFacility = rawFacility.Where(a => a.TargetedPopulation == filter.Filter.TargetedPopulation);
            }

            if (filter.Filter.FacilityType != null)
            {
                rawFacility = rawFacility.Where(a => a.FacilityType == filter.Filter.FacilityType);
            }

            if (filter.Filter.FacilityStatus != null)
            {
                rawFacility = rawFacility.Where(a => a.FacilityStatus == filter.Filter.FacilityStatus);
            }

            if (filter.Filter.ProgramPartner.Count > 0)
            {
                var ppIds = filter.Filter.ProgramPartner.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => ppIds.Contains(a.ProgrammingPartnerId));
            }
            if (filter.Filter.ImplementationPartner.Count > 0)
            {
                var ipIds = filter.Filter.ImplementationPartner.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => ipIds.Contains(a.ImplemantationPartnerId));
            }
            if (filter.Filter.Teachers.Count > 0)
            {
                var tIds = filter.Filter.Teachers.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => tIds.Contains((int)a.TeacherId));
            }
            return rawFacility;
        }
        private IQueryable<FacilityRawViewModel> ApplyFilter(IQueryable<FacilityRawViewModel> rawFacility, FacilityGetAllQueryModel facilityByViewId)
        {
            //var rawQuery = beneficiaryRaws;
            if (!string.IsNullOrEmpty(facilityByViewId.Filter.SearchText))
            {
                rawFacility = rawFacility.Where(a => a.FacilityName.Contains(facilityByViewId.Filter.SearchText)
                || a.FacilityCode.Contains(facilityByViewId.Filter.SearchText));
            }
            if (facilityByViewId.Filter.UpazilaId != null)
            {

                rawFacility = rawFacility.Where(a => a.UpazilaId == facilityByViewId.Filter.UpazilaId);
            }

            if (facilityByViewId.Filter.UnionId != null)
            {

                rawFacility = rawFacility.Where(a => a.UnionId == facilityByViewId.Filter.UnionId);
            }

            if (facilityByViewId.Filter.TargetedPopulation != null)
            {
                rawFacility = rawFacility.Where(a => a.TargetedPopulation == facilityByViewId.Filter.TargetedPopulation);
            }

            if (facilityByViewId.Filter.FacilityType != null)
            {
                rawFacility = rawFacility.Where(a => a.FacilityType == facilityByViewId.Filter.FacilityType);
            }

            if (facilityByViewId.Filter.FacilityStatus != null)
            {
                rawFacility = rawFacility.Where(a => a.FacilityStatus == facilityByViewId.Filter.FacilityStatus);
            }

            if (facilityByViewId.Filter.ProgramPartner.Count > 0)
            {
                var ppIds = facilityByViewId.Filter.ProgramPartner.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => ppIds.Contains(a.ProgrammingPartnerId));
            }
            if (facilityByViewId.Filter.ImplementationPartner.Count > 0)
            {
                var ipIds = facilityByViewId.Filter.ImplementationPartner.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => ipIds.Contains(a.ImplemantationPartnerId));
            }
            if (facilityByViewId.Filter.Teachers.Count > 0)
            {
                var tIds = facilityByViewId.Filter.Teachers.Select(a => a.Id).ToList();
                rawFacility = rawFacility.Where(a => tIds.Contains((int)a.TeacherId));
            }
            return rawFacility;
        }
        private IQueryable<FacilityObjectViewModel> ApplyFilter(IQueryable<FacilityObjectViewModel> rawQuery, FacilityGetAllQueryModel facilityByViewId)
        {
            //var rawQuery = beneficiaryRaws;
            if (!string.IsNullOrEmpty(facilityByViewId.Filter.SearchText))
            {
                rawQuery = rawQuery.Where(a => a.FacilityName.Contains(facilityByViewId.Filter.SearchText)
                || a.FacilityCode.Contains(facilityByViewId.Filter.SearchText));
            }
            if (facilityByViewId.Filter.UpazilaId != null)
            {

                rawQuery = rawQuery.Where(a => a.UpazilaId == facilityByViewId.Filter.UpazilaId);
            }

            if (facilityByViewId.Filter.UnionId != null)
            {

                rawQuery = rawQuery.Where(a => a.UnionId == facilityByViewId.Filter.UnionId);
            }

            if (facilityByViewId.Filter.TargetedPopulation != null)
            {
                rawQuery = rawQuery.Where(a => a.TargetedPopulation == facilityByViewId.Filter.TargetedPopulation);
            }

            if (facilityByViewId.Filter.FacilityType != null)
            {
                rawQuery = rawQuery.Where(a => a.FacilityType == facilityByViewId.Filter.FacilityType);
            }

            if (facilityByViewId.Filter.FacilityStatus != null)
            {
                rawQuery = rawQuery.Where(a => a.FacilityStatus == facilityByViewId.Filter.FacilityStatus);
            }

            if (facilityByViewId.Filter.ProgramPartner.Count > 0)
            {
                var ppIds = facilityByViewId.Filter.ProgramPartner.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => ppIds.Contains(a.ProgrammingPartnerId));
            }
            if (facilityByViewId.Filter.ImplementationPartner.Count > 0)
            {
                var ipIds = facilityByViewId.Filter.ImplementationPartner.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => ipIds.Contains(a.ImplemantationPartnerId));
            }

            return rawQuery;
        }
        public List<FacilityViewModel> SetFacilityViewModel(List<FacilityRawViewModel> facilityRawData)
        {
            var facility = facilityRawData.GroupBy(v =>
                        new
                        {
                            v.Id,
                            v.FacilityName,
                            v.ProgrammingPartnerId,
                            v.ProgrammingPartnerName,
                            v.ImplemantationPartnerId,
                            v.ImplemantationPartnerName,
                            v.InstanceId,
                            v.InstanceName
                        })
                        .Select(g => new FacilityViewModel
                        {
                            Id = g.Key.Id,
                            UnionId = g.Select(h => h.UnionId).FirstOrDefault(),
                            UnionName = g.Select(h => h.UnionName).FirstOrDefault(),
                            UpazilaId = g.Select(h => h.UpazilaId).FirstOrDefault(),
                            UpazilaName = g.Select(h => h.UpazilaName).FirstOrDefault(),
                            CampId = g.Select(h => h.CampId).FirstOrDefault(),
                            CampName = g.Select(h => h.CampName).FirstOrDefault(),

                            CampSSID = g.Select(h => h.CampSSID).FirstOrDefault(),
                            Donors = g.Select(h => h.Donors).FirstOrDefault(),
                            Latitude = g.Select(h => h.Latitude).FirstOrDefault(),
                            longitude = g.Select(h => h.longitude).FirstOrDefault(),
                            NonEducationPartner = g.Select(h => h.NonEducationPartner).FirstOrDefault(),

                            BlockId = g.Select(h => h.BlockId).FirstOrDefault(),
                            BlockName = g.Select(h => h.BlockName).FirstOrDefault(),
                            BlockCode = g.Select(x => x.BlockCode).FirstOrDefault(),
                            ParaId = g.Select(h => h.ParaId).FirstOrDefault(),
                            ParaName = g.Select(h => h.ParaName).FirstOrDefault(),
                            ProgrammingPartnerId = g.Key.ProgrammingPartnerId,
                            ProgrammingPartnerName = g.Key.ProgrammingPartnerName,
                            ImplemantationPartnerId = g.Key.ImplemantationPartnerId,
                            ImplemantationPartnerName = g.Key.ImplemantationPartnerName,
                            TeacherId = g.Select(h => h.TeacherId).FirstOrDefault(),
                            TeacherName = g.Select(h => h.TeacherName).FirstOrDefault(),
                            TeacherEmail = g.Select(x => x.TeacherEmail).FirstOrDefault(),
                            FacilityName = g.Key.FacilityName,
                            FacilityCode = g.Select(h => h.FacilityCode).FirstOrDefault(),
                            CollectionStatus = g.Select(h => h.CollectionStatus).FirstOrDefault(),
                            FacilityType = g.Select(h => h.FacilityType).FirstOrDefault(),
                            FacilityStatus = g.Select(h => h.FacilityStatus).FirstOrDefault(),
                            TargetedPopulation = g.Select(h => h.TargetedPopulation).FirstOrDefault(),
                            InstanceId = g.Key.InstanceId,
                            InstanceName = g.Key.InstanceName,
                            Remarks = g.Select(h => h.Remarks).FirstOrDefault(),
                            ApproveDate = g.Select(h => h.ApproveDate).FirstOrDefault(),
                            ApproveByUserName = g.Select(h => h.ApproveByUserName).FirstOrDefault(),
                            ApproveByUserEmail = g.Select(h => h.ApproveByUserEmail).FirstOrDefault(),
                            ApproveByUserPhone = g.Select(h => h.ApproveByUserPhone).FirstOrDefault(),
                            Properties = g.GroupBy(f => new { f.EntityColumnId, f.EntityColumnName })
                                       .Select(m => new PropertiesInfo
                                       {
                                           ColumnOrder = m.Select(x => x.ColumnOrder).FirstOrDefault(),
                                           EntityColumnId = m.Key.EntityColumnId,
                                           ColumnName = m.Select(x => x.EntityColumnName).FirstOrDefault(),
                                           Properties = m.Key.EntityColumnName,
                                           ColumnNameInBangla= m.Select(x => x.EntityColumnNameInBangla).FirstOrDefault(),
                                           IsFixed = m.Select(n => n.IsFixed).FirstOrDefault(),
                                           IsMultiValued = m.Select(n => n.IsMultiValued).FirstOrDefault() ?? false,
                                           ColumnListId = m.Select(n => n.ColumnListId).FirstOrDefault(),
                                           ColumnListName = m.Select(n => n.ColumnListName).FirstOrDefault(),
                                           DataType = m.Select(n => n.DataType).FirstOrDefault(),
                                           Values = m.GroupBy(n => n.PropertiesId)
                                               .Select(o => o.Select(o => o.PropertiesValue).FirstOrDefault()
                                               ).ToList(),
                                           Status= m.Select(n => n.PropertiesDataCollectionStatus).FirstOrDefault(),
                                           ListItem = m.Where(n => n.ListItemId != null).GroupBy(n => n.ListItemId).Select(o => new ListItemViewModel()
                                           {
                                               Id = (long)o.Key,
                                               Title = o.Select(o => o.ListItemTitle).FirstOrDefault(),
                                               Value = (int)o.Select(o => o.ListItemValue).FirstOrDefault()
                                           }).ToList()
                                       }).OrderBy(a=>a.ColumnOrder).ToList()
                        }).ToList();
            return facility;
        }

        private async Task<List<FacilityRawViewModel>> GetRawFacility(long indicatorInstanceId, long dynamicCellInstanceId, List<long> ids)
        {

            var facilityRawData = await (from fdcs in _context.FacilityDataCollectionStatuses
                                         join f in _context.FacilityView on fdcs.FacilityId equals f.Id
                                         join uno in _context.Unions on f.UnionId equals uno.Id
                                         join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                                         join c in _context.Camps on f.CampId equals c.Id into ca
                                         from bcmp in ca.DefaultIfEmpty()
                                         join b in _context.Blocks on f.BlockId equals b.Id into ba
                                         from blk in ba.DefaultIfEmpty()
                                             //join pa in _context.Paras on f.ParaId equals pa.Id
                                             //into p from para in p.DefaultIfEmpty()
                                         join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                                         join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                                         join tu in _context.Users on f.TeacherId equals tu.Id into t
                                         from teadchrData in t.DefaultIfEmpty()
                                         join ii in _context.InstanceIndicators on fdcs.InstanceId equals ii.InstanceId
                                         join si in _context.ScheduleInstances on ii.InstanceId equals si.Id
                                         join edc in _context.EntityDynamicColumn on ii.EntityDynamicColumnId equals edc.Id
                                         join lo in _context.ListDataType on edc.ColumnListId equals lo.Id into def
                                         from listObject in def.DefaultIfEmpty()
                                         join li in _context.ListItems on listObject.Id equals li.ColumnListId into def1
                                         from listItem in def1.DefaultIfEmpty()
                                         join bdc in _context.FacilityDynamicCells on new { FacilityId = f.Id, ii.EntityDynamicColumnId, InstanceId = dynamicCellInstanceId } equals new { bdc.FacilityId, bdc.EntityDynamicColumnId, bdc.InstanceId } into def2
                                         from dynamicCell in def2.DefaultIfEmpty()
                                         where ii.InstanceId == indicatorInstanceId && ids.Contains(f.Id)
                                         select new FacilityRawViewModel
                                         {
                                             Id = f.Id,
                                             FacilityName = f.Name,
                                             FacilityCode = f.FacilityCode,
                                             UpazilaId = upa.Id,
                                             UpazilaName = upa.Name,
                                             UnionId = uno.Id,
                                             UnionName = uno.Name,
                                             BlockId = blk.Id,
                                             BlockName = blk.Name,
                                             CampId = f.CampId,
                                             CampName = bcmp.Name,

                                             ParaName = f.ParaName,
                                             ProgrammingPartnerId = pp.Id,
                                             ProgrammingPartnerName = pp.PartnerName,
                                             ImplemantationPartnerId = ip.Id,
                                             ImplemantationPartnerName = ip.PartnerName,
                                             TeacherId = f.TeacherId,
                                             TeacherName = teadchrData.FullName,
                                             InstanceId = ii.InstanceId,
                                             CollectionStatus = fdcs.Status,
                                             FacilityType = f.FacilityType,
                                             FacilityStatus = f.FacilityStatus,
                                             TargetedPopulation = f.TargetedPopulation,
                                             Remarks = f.Remarks,


                                             PropertiesId = dynamicCell.Id,
                                             PropertiesValue = dynamicCell.Value,
                                             EntityColumnId = edc.Id,
                                             EntityColumnName = edc.Name,
                                             ColumnOrder = ii.ColumnOrder,
                                             PropertiesDataCollectionStatus = indicatorInstanceId == dynamicCellInstanceId ? dynamicCell.Status : CollectionStatus.NotCollected,

                                             IsMultiValued = edc.IsMultiValued,
                                             DataType = edc.ColumnType,
                                             ColumnListId = edc.ColumnListId,
                                             ColumnListName = listObject.Name,
                                             ListItemId = listItem.Id,
                                             ListItemTitle = listItem.Title,
                                             ListItemValue = listItem.Value

                                         }).ToListAsync();
            return facilityRawData;
        }

        private List<FacilityViewModel> GetFacilityViews(List<FacilityRawViewModel> rawViewModels)
        {
            var facility = rawViewModels.GroupBy(v => new { v.Id, v.FacilityName, v.ProgrammingPartnerId, v.ProgrammingPartnerName, v.ImplemantationPartnerId, v.ImplemantationPartnerName })
                        .Select(g => new FacilityViewModel
                        {
                            Id = g.Key.Id,
                            UnionId = g.Select(h => h.UnionId).FirstOrDefault(),
                            UnionName = g.Select(h => h.UnionName).FirstOrDefault(),
                            UpazilaId = g.Select(h => h.UpazilaId).FirstOrDefault(),
                            UpazilaName = g.Select(h => h.UpazilaName).FirstOrDefault(),
                            CampId = g.Select(h => h.CampId).FirstOrDefault(),
                            CampName = g.Select(h => h.CampName).FirstOrDefault(),
                            BlockId = g.Select(h => h.BlockId).FirstOrDefault(),
                            BlockName = g.Select(h => h.BlockName).FirstOrDefault(),
                            ParaId = g.Select(h => h.ParaId).FirstOrDefault(),
                            ParaName = g.Select(h => h.ParaName).FirstOrDefault(),
                            ProgrammingPartnerId = g.Key.ProgrammingPartnerId,
                            ProgrammingPartnerName = g.Key.ProgrammingPartnerName,
                            ImplemantationPartnerId = g.Key.ImplemantationPartnerId,
                            ImplemantationPartnerName = g.Key.ImplemantationPartnerName,
                            TeacherId = g.Select(h => h.TeacherId).FirstOrDefault(),
                            TeacherName = g.Select(h => h.TeacherName).FirstOrDefault(),
                            FacilityName = g.Key.FacilityName,
                            FacilityCode = g.Select(h => h.FacilityCode).FirstOrDefault(),
                            CollectionStatus = g.Select(h => h.CollectionStatus).FirstOrDefault(),
                            FacilityType = g.Select(h => h.FacilityType).FirstOrDefault(),
                            FacilityStatus = g.Select(h => h.FacilityStatus).FirstOrDefault(),
                            TargetedPopulation = g.Select(h => h.TargetedPopulation).FirstOrDefault(),

                            Remarks = g.Select(h => h.Remarks).FirstOrDefault(),

                            Properties = g.GroupBy(f => new { f.EntityColumnId, f.EntityColumnName })
                                       .Select(m => new PropertiesInfo
                                       {
                                           EntityColumnId = m.Key.EntityColumnId,
                                           Properties = m.Key.EntityColumnName,
                                           IsFixed = m.Select(n => n.IsFixed).FirstOrDefault(),
                                           IsMultiValued = m.Select(n => n.IsMultiValued).FirstOrDefault() ?? false,
                                           ColumnListId = m.Select(n => n.ColumnListId).FirstOrDefault(),
                                           ColumnListName = m.Select(n => n.ColumnListName).FirstOrDefault(),
                                           DataType = m.Select(n => n.DataType).FirstOrDefault(),
                                           Status = m.Select(n => n.PropertiesDataCollectionStatus).FirstOrDefault() ?? CollectionStatus.NotCollected,
                                           ListItem = m.Where(n => n.ListItemId != null).GroupBy(n => n.ListItemId).Select(o => new ListItemViewModel()
                                           {
                                               Id = (long)o.Key,
                                               Title = o.Select(o => o.ListItemTitle).FirstOrDefault(),
                                               Value = (int)o.Select(o => o.ListItemValue).FirstOrDefault()
                                           }).ToList(),
                                           Values = m.Select(o => o.PropertiesValue).ToList().Where(o => !string.IsNullOrEmpty(o)).ToList()

                                       }).ToList()
                        }).ToList();

            return facility;
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> GetPaginatedFacilityObject(FacilityQueryModel queryModel)
        {
            //var response = new PagedResponse<FacilityObjectViewModel>();

            //var facilityQuery = GetAll();

            //if (!string.IsNullOrEmpty(queryModel.SearchText))
            //{
            //    facilityQuery = facilityQuery.Where(a => a.Name.Contains(queryModel.SearchText) ||
            //                                                   a.FacilityCode.Contains(queryModel.SearchText));
            //}
            //facilityQuery = _facilityUserConditionFactory.GetFacilityUserCondition()
            //            .ApplyCondition(facilityQuery);

            //response.Total = await (from f in facilityQuery
            //                        join bdcs in _context.FacilityDataCollectionStatuses on new
            //                        { f.Id, InstanceId = queryModel.InstanceId } equals new { Id = bdcs.FacilityId, bdcs.InstanceId }
            //                            into
            //                            def1
            //                        from colStatus in def1.DefaultIfEmpty()
            //                        where colStatus.Status != CollectionStatus.Approved
            //                        select f.Id).CountAsync();

            //response.Data = await (from f in facilityQuery
            //                       join bdcs in _context.FacilityDataCollectionStatuses on new
            //                       { f.Id, InstanceId = queryModel.InstanceId } equals new { Id = bdcs.FacilityId, bdcs.InstanceId }
            //                           into
            //                           def1
            //                       from colStatus in def1.DefaultIfEmpty()
            //                       join camp in _context.Camps on f.CampId equals camp.Id into def2
            //                       from campInfo in def2.DefaultIfEmpty()
            //                       join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
            //                       join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
            //                       where colStatus.Status != CollectionStatus.Approved
            //                       select new FacilityObjectViewModel
            //                       {
            //                           Id = f.Id,
            //                           FacilityName = f.Name,
            //                           FacilityCode = f.FacilityCode,
            //                           UpazilaId = f.UpazilaId,
            //                           UnionId = f.UnionId,
            //                           BlockId = f.BlockId,
            //                           CampId = f.CampId,
            //                           CampName = campInfo.Name,
            //                           Donors = f.Donors,
            //                           Latitude = f.Latitude,
            //                           longitude = f.Longitude,
            //                           NonEducationPartner = f.NonEducationPartner,

            //                           ParaName = f.ParaName,
            //                           FacilityStatus = f.FacilityStatus,
            //                           FacilityType = f.FacilityType,
            //                           TargetedPopulation = f.TargetedPopulation,
            //                           ProgrammingPartnerId = f.ProgramPartnerId,
            //                           ProgrammingPartnerName = pp.PartnerName,
            //                           ImplemantationPartnerId = f.ImplementationPartnerId,
            //                           ImplemantationPartnerName = ip.PartnerName,
            //                           TeacherId = f.TeacherId,
            //                           CollectionStatus = colStatus.Status
            //                       })
            //                       .OrderBy(a => a.FacilityCode)
            //    .Skip(queryModel.Skip())
            //    .Take(queryModel.PageSize)
            //    .ToListAsync();
            //return response;
            throw new NotImplementedException();
        }


        public async Task<PagedResponse<FacilityViewModel>> GetAllForDevice(FacilityQueryModel model)
        {

            //var facilityIdRaw = (from f in _context.FacilityView
            //                     join fdcs in _context.FacilityDataCollectionStatuses on
            //                     new { f.InstanceId, FacilityId = f.Id } equals new { fdcs.InstanceId, fdcs.FacilityId }
            //                     where f.InstanceId == model.InstanceId
            //                     && (fdcs.Status != CollectionStatus.Inactivated &&
            //                     fdcs.Status != CollectionStatus.Deleted &&
            //                     fdcs.Status != CollectionStatus.Approved)
            //                     select new FacilityRawViewModel
            //                     {
            //                         Id = f.Id,
            //                         ProgrammingPartnerId = f.ProgramPartnerId,
            //                         ImplemantationPartnerId = f.ImplementationPartnerId,
            //                         FacilityName = f.Name,
            //                         FacilityCode = f.FacilityCode
            //                     });


            //var facilityListToGetIds = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //             .ApplyCondition(facilityIdRaw, model.InstanceId).Skip(model.Skip()).Take(model.PageSize).ToListAsync();
            //var facilityIds = facilityListToGetIds.Select(a => a.Id).ToList();

            //var total = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //             .ApplyCondition(facilityIdRaw, model.InstanceId).CountAsync();

            //var facilityRaw = _facilityUserConditionFactory.GetFacilityUserCondition()
            //                     .ApplyCondition(GetCollection(model.InstanceId), model.InstanceId);

            //var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();
            //var facility = SetFacilityViewModel(facilityData);




            var baseDynamicCellData = await _context.FacilityDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var facilityRaw = baseDynamicCellData.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
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
                InstanceId = model.InstanceId,

                Latitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Latitude).Select(h => h.Value).FirstOrDefault(),
                longitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Longitude).Select(h => h.Value).FirstOrDefault(),
                Donors = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Donors).Select(h => h.Value).FirstOrDefault(),
                NonEducationPartner = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.NonEducationPartner).Select(h => h.Value).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Remarks).Select(h => h.Value).FirstOrDefault(),
                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ParaName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ParaName).Select(h => h.Value).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            var conditionalfacilityRaw = _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(facilityRaw);
            var dataCollectionStatus = await _context.FacilityDataCollectionStatuses
               .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var facilityWithCollectionStatusData = conditionalfacilityRaw.Join(dataCollectionStatus, dynCell => dynCell.Id, dCollection => dCollection.FacilityId
            , (dynCell, dCollection) => new FacilityRawViewModel
            {
                Id = dynCell.Id,
                FacilityName = dynCell.FacilityName,
                FacilityCode = dynCell.FacilityCode,
                UpazilaId = dynCell.UpazilaId,
                UnionId = dynCell.UnionId,
                TargetedPopulation = dynCell.TargetedPopulation,
                FacilityType = dynCell.FacilityType,
                FacilityStatus = dynCell.FacilityStatus,
                ProgrammingPartnerId = dynCell.ProgrammingPartnerId,
                ImplemantationPartnerId = dynCell.ImplemantationPartnerId,
                TeacherId = dynCell.TeacherId,
                CollectionStatus = dCollection.Status,
                InstanceId = dynCell.InstanceId,
                Latitude = dynCell.Latitude,
                longitude = dynCell.longitude,
                Donors = dynCell.Donors,
                NonEducationPartner = dynCell.NonEducationPartner,
                Remarks = dynCell.Remarks,
                CampId = dynCell.CampId,
                ParaName = dynCell.ParaName,
                BlockId = dynCell.BlockId,
                UpdatedBy = dCollection.UpdatedBy,
                ApproveDate = dCollection.LastUpdated

            });

            Func<FacilityRawViewModel, bool> filter = fdcs => fdcs.CollectionStatus != CollectionStatus.Inactivated &&
                                  fdcs.CollectionStatus != CollectionStatus.Deleted &&
                                  fdcs.CollectionStatus != CollectionStatus.Approved;

            var total = facilityWithCollectionStatusData.Where(filter).Count();
            var facilityData = facilityWithCollectionStatusData.Where(filter).OrderBy(a => a.Id).Skip(model.Skip()).Take(model.PageSize).ToList();
            var facilityIds = facilityData.Select(a => a.Id).ToList();


            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();

            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var userData = await _context.Users.ToListAsync();
            var upazilaData = await _context.Upazilas.ToListAsync();
            var unionData = await _context.Unions.ToListAsync();
            var campData = await _context.Camps.Where(a => facilityCampIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => facilityBlockIds.Contains(a.Id)).ToListAsync();

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
            var dynamicCellData = baseDynamicCellData.Where(a => a.InstanceId == model.InstanceId
                && facilityIds.Contains(a.FacilityId)
                && entityColumnIds.Contains(a.EntityDynamicColumnId)
                ).ToList();

            var combineAllData = (from f in facilityData
                                  join uno in unionData on f.UnionId equals uno.Id
                                  join upa in upazilaData on f.UpazilaId equals upa.Id

                                  join c in campData on f.CampId equals c.Id into ca
                                  from bcmp in ca.DefaultIfEmpty()
                                  join b in blockData on f.BlockId equals b.Id into ba
                                  from blk in ba.DefaultIfEmpty()

                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id
                                  join tu in userData on f.TeacherId equals tu.Id into t
                                  from teadchrData in t.DefaultIfEmpty()

                                  join ii in indicatorData on f.InstanceId equals ii.InstanceId
                                  join dc in dynamicCellData on new { FacilityId = f.Id, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.FacilityId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()


                                  join u in userData on f.UpdatedBy equals u.Id into def3
                                  from approvedByUser in def3.DefaultIfEmpty()

                                  select new FacilityRawViewModel
                                  {
                                      Id = f.Id,
                                      FacilityName = f.FacilityName,
                                      FacilityCode = f.FacilityCode,
                                      BlockId = blk?.Id,
                                      BlockName = blk?.Name,
                                      UpazilaId = upa.Id,
                                      UpazilaName = upa.Name,
                                      UnionId = uno.Id,
                                      UnionName = uno.Name,
                                      CampId = f?.CampId,
                                      CampName = bcmp?.Name,
                                      CampSSID = bcmp?.SSID,
                                      Donors = f.Donors,
                                      Latitude = f.Latitude,
                                      longitude = f.longitude,
                                      NonEducationPartner = f.NonEducationPartner,

                                      ParaName = f.ParaName,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,
                                      TeacherId = f.TeacherId,
                                      TeacherName = teadchrData?.FullName,
                                      TeacherEmail = teadchrData?.Email,
                                      FacilityType = f.FacilityType,
                                      FacilityStatus = f.FacilityStatus,
                                      TargetedPopulation = f.TargetedPopulation,

                                      Remarks = f.Remarks,


                                      EntityColumnId = ii.EntityDynamicColumnId,
                                      EntityColumnName = ii.EntityDynamicColumn.Name,
                                      EntityColumnNameInBangla= ii.EntityDynamicColumn.NameInBangla,
                                      ColumnListId = ii.EntityDynamicColumn.ColumnListId,
                                      ColumnListName = ii.EntityDynamicColumn.ColumnList?.Name,
                                      ColumnOrder=ii.ColumnOrder,
                                      DataType = ii.EntityDynamicColumn.ColumnType,
                                      IsMultiValued = ii.EntityDynamicColumn.IsMultiValued,
                                      IsFixed = ii.EntityDynamicColumn.IsFixed,
                                      ListItemId = liData?.Id,
                                      ListItemTitle = liData?.Title,
                                      ListItemValue = liData?.Value,
                                      PropertiesId = dynCellData?.Id,
                                      PropertiesValue = dynCellData?.Value??"",
                                      PropertiesDataCollectionStatus= dynCellData?.Status,
                                      CollectionStatus = f.CollectionStatus,
                                      ApproveDate = f.ApproveDate,
                                      ApproveByUserEmail = approvedByUser?.Email,
                                      ApproveByUserName = approvedByUser?.FullName,
                                      ApproveByUserPhone = approvedByUser?.PhoneNumber
                                  }).ToList();

            var facility = SetFacilityViewModel(combineAllData);










            return new PagedResponse<FacilityViewModel>(facility, total, model.PageNo, model.PageSize);

        }
        public async Task<PagedResponse<FacilityObjectViewModel>> GetAll(BaseQueryModel facilityQuery,long instanceId)
        {
            //var maxInstanceId = await _scheduleInstanceRepository.GetMaxInstanceId(EntityType.Facility);

            var paginatedData = (
                 from f in _context.FacilityView
                 where f.InstanceId == instanceId
                 select new FacilityRawViewModel
                 {
                     Id = f.Id,
                     ProgrammingPartnerId = f.ProgramPartnerId,
                     ImplemantationPartnerId = f.ImplementationPartnerId,
                     FacilityName = f.Name,
                     FacilityCode = f.FacilityCode
                 });
            if (!string.IsNullOrEmpty(facilityQuery.SearchText))
            {
                paginatedData = paginatedData.Where(a => a.FacilityCode.Contains(facilityQuery.SearchText) || a.FacilityName.Contains(facilityQuery.SearchText));

            }

            var total = await _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(paginatedData, instanceId).CountAsync();
            var facilityIds = await _facilityUserConditionFactory.GetFacilityUserCondition()
                          .ApplyCondition(paginatedData, instanceId).OrderBy(a => a.FacilityCode).Select(a => a.Id).Skip(facilityQuery.Skip())
                          .Take(facilityQuery.PageSize).ToListAsync();

            var facilityRaw = (from f in _context.FacilityView
                               join uno in _context.Unions on f.UnionId equals uno.Id
                               join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from bcmp in ca.DefaultIfEmpty()
                               join b in _context.Blocks on f.BlockId equals b.Id into ba
                               from blk in ba.DefaultIfEmpty()
                                   //join pa in _context.Paras on f.ParaId equals pa.Id
                                   //into p from para in p.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                               join tu in _context.Users on f.TeacherId equals tu.Id into t
                               from teacherData in t.DefaultIfEmpty()
                               where f.InstanceId == instanceId
                               select new FacilityObjectViewModel
                               {
                                   Id = f.Id,
                                   FacilityName = f.Name,
                                   FacilityCode = f.FacilityCode,
                                   UpazilaId = upa.Id,
                                   UpazilaName = upa.Name,
                                   UnionId = uno.Id,
                                   UnionName = uno.Name,
                                   BlockId = f.BlockId,
                                   BlockName = blk.Name,
                                   BlockCode = blk.Code,
                                   CampId = f.CampId,
                                   CampName = bcmp.Name,
                                   CampSSID = bcmp.SSID,
                                   Donors = f.Donors,
                                   Latitude = f.Latitude,
                                   longitude = f.Longitude,
                                   NonEducationPartner = f.NonEducationPartner,

                                   ParaName = f.ParaName,
                                   FacilityStatus = f.FacilityStatus,
                                   FacilityType = f.FacilityType,
                                   TargetedPopulation = f.TargetedPopulation,
                                   ProgrammingPartnerId = f.ProgramPartnerId,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = f.ImplementationPartnerId,
                                   ImplemantationPartnerName = ip.PartnerName,
                                   TeacherId = f.TeacherId,
                                   TeacherName = teacherData.FullName,
                                   TeacherEmail = teacherData.Email,
                                   Remarks = f.Remarks
                               });
            if (!string.IsNullOrEmpty(facilityQuery.SearchText))
                facilityRaw = facilityRaw.Where(a => a.FacilityName.Contains(facilityQuery.SearchText) || a.FacilityCode.Contains(facilityQuery.SearchText));

            var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();

            return new PagedResponse<FacilityObjectViewModel>(facilityData, total, facilityQuery.PageNo, facilityQuery.PageSize);
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> GetAll(FacilityQueryModel facilityQuery)
        {
            
            var paginatedData = (from f in _context.FacilityView
                                 join fdcs in _context.FacilityDataCollectionStatuses on
                                 new { f.InstanceId, FacilityId = f.Id } equals new { fdcs.InstanceId, fdcs.FacilityId }
                                 where f.InstanceId == facilityQuery.InstanceId
                                 && (fdcs.Status != CollectionStatus.Inactivated &&
                                 fdcs.Status != CollectionStatus.Deleted)
                                 select new FacilityRawViewModel
                                 {
                                     Id = f.Id,
                                     ProgrammingPartnerId = f.ProgramPartnerId,
                                     ImplemantationPartnerId = f.ImplementationPartnerId,
                                     FacilityName = f.Name,
                                     FacilityCode = f.FacilityCode
                                 });
            if (!string.IsNullOrEmpty(facilityQuery.SearchText))
            {
                paginatedData = paginatedData.Where(a => a.FacilityCode.Contains(facilityQuery.SearchText) || a.FacilityName.Contains(facilityQuery.SearchText));

            }

            var total = await _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(paginatedData, facilityQuery.InstanceId).CountAsync();
            var facilityIds = await _facilityUserConditionFactory.GetFacilityUserCondition()
                          .ApplyCondition(paginatedData, facilityQuery.InstanceId).OrderBy(a => a.FacilityCode).Select(a => a.Id).Skip(facilityQuery.Skip())
                          .Take(facilityQuery.PageSize).ToListAsync();

            var facilityRaw = (from f in _context.FacilityView
                               join uno in _context.Unions on f.UnionId equals uno.Id
                               join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from bcmp in ca.DefaultIfEmpty()
                               join b in _context.Blocks on f.BlockId equals b.Id into ba
                               from blk in ba.DefaultIfEmpty()
                                   //join pa in _context.Paras on f.ParaId equals pa.Id
                                   //into p from para in p.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                               join tu in _context.Users on f.TeacherId equals tu.Id into t
                               from teacherData in t.DefaultIfEmpty()
                               where f.InstanceId == facilityQuery.InstanceId
                               select new FacilityObjectViewModel
                               {
                                   Id = f.Id,
                                   FacilityName = f.Name,
                                   FacilityCode = f.FacilityCode,
                                   UpazilaId = upa.Id,
                                   UpazilaName = upa.Name,
                                   UnionId = uno.Id,
                                   UnionName = uno.Name,
                                   BlockId = f.BlockId,
                                   BlockName = blk.Name,
                                   BlockCode = blk.Code,
                                   CampId = f.CampId,
                                   CampName = bcmp.Name,
                                   CampSSID = bcmp.SSID,
                                   Donors = f.Donors,
                                   Latitude = f.Latitude,
                                   longitude = f.Longitude,
                                   NonEducationPartner = f.NonEducationPartner,

                                   ParaName = f.ParaName,
                                   FacilityStatus = f.FacilityStatus,
                                   FacilityType = f.FacilityType,
                                   TargetedPopulation = f.TargetedPopulation,
                                   ProgrammingPartnerId = f.ProgramPartnerId,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = f.ImplementationPartnerId,
                                   ImplemantationPartnerName = ip.PartnerName,
                                   TeacherId = f.TeacherId,
                                   TeacherName = teacherData.FullName,
                                   TeacherEmail = teacherData.Email,
                                   Remarks = f.Remarks
                               });
            if (!string.IsNullOrEmpty(facilityQuery.SearchText))
                facilityRaw = facilityRaw.Where(a => a.FacilityName.Contains(facilityQuery.SearchText) || a.FacilityCode.Contains(facilityQuery.SearchText));

            var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();

            return new PagedResponse<FacilityObjectViewModel>(facilityData, total, facilityQuery.PageNo, facilityQuery.PageSize);
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> ExportAll(BaseQueryModel facilityQuery)
        {
            //var paginatedData = (
            //     from f in _context.FacilityView
            //     select new FacilityRawViewModel
            //     {
            //         Id = f.Id,
            //         ProgrammingPartnerId = f.ProgramPartnerId,
            //         ImplemantationPartnerId = f.ImplementationPartnerId
            //     });
            //var total = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //             .ApplyCondition(paginatedData).CountAsync();
            //var facilityIds = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //              .ApplyCondition(paginatedData).Select(a => a.Id).Skip(facilityQuery.Skip())
            //              .Take(facilityQuery.PageSize).ToListAsync();

            //var facilityRaw = (from f in _context.FacilityView
            //                   join uno in _context.Unions on f.UnionId equals uno.Id
            //                   join upa in _context.Upazilas on f.UpazilaId equals upa.Id

            //                   join c in _context.Camps on f.CampId equals c.Id into ca
            //                   from bcmp in ca.DefaultIfEmpty()
            //                   join b in _context.Blocks on f.BlockId equals b.Id into ba
            //                   from blk in ba.DefaultIfEmpty()
            //                   join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
            //                   join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
            //                   join tu in _context.Users on f.TeacherId equals tu.Id into t
            //                   from teacherData in t.DefaultIfEmpty()
            //                   select new FacilityObjectViewModel
            //                   {
            //                       Id = f.Id,
            //                       FacilityName = f.Name,
            //                       FacilityCode = f.FacilityCode,
            //                       UpazilaId = upa.Id,
            //                       UpazilaName = upa.Name,
            //                       UnionId = uno.Id,
            //                       UnionName = uno.Name,
            //                       BlockId = f.BlockId,
            //                       BlockName = blk.Name,
            //                       BlockCode = blk.Code,
            //                       CampId = f.CampId,
            //                       CampName = bcmp.Name,
            //                       CampSSID = bcmp.SSID,
            //                       Donors = f.Donors,
            //                       Latitude = f.Latitude,
            //                       longitude = f.Longitude,
            //                       NonEducationPartner = f.NonEducationPartner,

            //                       ParaName = f.ParaName,
            //                       FacilityStatus = f.FacilityStatus,
            //                       FacilityType = f.FacilityType,
            //                       TargetedPopulation = f.TargetedPopulation,
            //                       ProgrammingPartnerId = f.ProgramPartnerId,
            //                       ProgrammingPartnerName = pp.PartnerName,
            //                       ImplemantationPartnerId = f.ImplementationPartnerId,
            //                       ImplemantationPartnerName = ip.PartnerName,
            //                       TeacherId = f.TeacherId,
            //                       TeacherName = teacherData.FullName,
            //                       Remarks = f.Remarks
            //                   });

            //var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();
            //return new PagedResponse<FacilityObjectViewModel>(facilityData, total, facilityQuery.PageNo, facilityQuery.PageSize);

            throw new NotImplementedException();
        }
        public async Task<PagedResponse<FacilityObjectViewModel>> GetAllFilteredData(FacilityGetAllQueryModel facilityQuery)
        {
            throw new NotImplementedException();
            //var paginatedData = (
            //     from f in _context.FacilityView
            //     select new FacilityRawViewModel
            //     {
            //         Id = f.Id,
            //         FacilityName = f.Name,
            //         FacilityCode = f.FacilityCode,
            //         UpazilaId = f.UpazilaId,
            //         UnionId = f.UnionId,
            //         TargetedPopulation = f.TargetedPopulation,
            //         FacilityType = f.FacilityType,
            //         FacilityStatus = f.FacilityStatus,
            //         ProgrammingPartnerId = f.ProgramPartnerId,
            //         ImplemantationPartnerId = f.ImplementationPartnerId,
            //         TeacherId = f.TeacherId
            //     });
            //paginatedData = ApplyFilter(paginatedData, facilityQuery);

            //var total = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //             .ApplyCondition(paginatedData).CountAsync();
            //var facilityIds = await _facilityUserConditionFactory.GetFacilityUserCondition()
            //              .ApplyCondition(paginatedData).Select(a => a.Id).Skip(facilityQuery.Skip())
            //              .Take(facilityQuery.PageSize).ToListAsync();

            //var facilityRaw = (from f in _context.FacilityView
            //                   join uno in _context.Unions on f.UnionId equals uno.Id
            //                   join upa in _context.Upazilas on f.UpazilaId equals upa.Id

            //                   join c in _context.Camps on f.CampId equals c.Id into ca
            //                   from bcmp in ca.DefaultIfEmpty()
            //                   join b in _context.Blocks on f.BlockId equals b.Id into ba
            //                   from blk in ba.DefaultIfEmpty()
            //                       //join pa in _context.Paras on f.ParaId equals pa.Id
            //                       //into p from para in p.DefaultIfEmpty()
            //                   join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
            //                   join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
            //                   join tu in _context.Users on f.TeacherId equals tu.Id into t
            //                   from teacherData in t.DefaultIfEmpty()
            //                   select new FacilityObjectViewModel
            //                   {
            //                       Id = f.Id,
            //                       FacilityName = f.Name,
            //                       FacilityCode = f.FacilityCode,
            //                       UpazilaId = upa.Id,
            //                       UpazilaName = upa.Name,
            //                       UnionId = uno.Id,
            //                       UnionName = uno.Name,
            //                       BlockId = f.BlockId,
            //                       BlockName = blk.Name,
            //                       CampId = f.CampId,
            //                       CampName = bcmp.Name,
            //                       CampSSID = bcmp.SSID,
            //                       Donors = f.Donors,
            //                       Latitude = f.Latitude,
            //                       longitude = f.Longitude,
            //                       NonEducationPartner = f.NonEducationPartner,

            //                       ParaName = f.ParaName,
            //                       FacilityStatus = f.FacilityStatus,
            //                       FacilityType = f.FacilityType,
            //                       TargetedPopulation = f.TargetedPopulation,
            //                       ProgrammingPartnerId = f.ProgramPartnerId,
            //                       ProgrammingPartnerName = pp.PartnerName,
            //                       ImplemantationPartnerId = f.ImplementationPartnerId,
            //                       ImplemantationPartnerName = ip.PartnerName,
            //                       TeacherId = f.TeacherId,
            //                       TeacherName = teacherData.FullName,

            //                       Remarks = f.Remarks
            //                   });
            //facilityRaw = ApplyFilter(facilityRaw, facilityQuery);
            //var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();

            //return new PagedResponse<FacilityObjectViewModel>(facilityData, total, facilityQuery.PageNo, facilityQuery.PageSize);
        }

        private async Task<List<FacilityRawViewModel>> FacilityRawAsync(List<FacilityDynamicCell> dynamicCells, FacilityByViewIdQueryModel model)
        {
            var facilityRaw = dynamicCells.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
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
                InstanceId = model.InstanceId,

                Latitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Latitude).Select(h => h.Value).FirstOrDefault(),
                longitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Longitude).Select(h => h.Value).FirstOrDefault(),
                Donors = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Donors).Select(h => h.Value).FirstOrDefault(),
                NonEducationPartner = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.NonEducationPartner).Select(h => h.Value).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Remarks).Select(h => h.Value).FirstOrDefault(),
                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ParaName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ParaName).Select(h => h.Value).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            var conditionalfacilityRaw = _facilityUserConditionFactory.GetFacilityUserCondition()
                         .ApplyCondition(facilityRaw);

            var dataCollectionStatus = await _context.FacilityDataCollectionStatuses
                .Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var facilityWithCollectionStatusData = conditionalfacilityRaw.Join(dataCollectionStatus, dynCell => dynCell.Id, dCollection => dCollection.FacilityId
            , (dynCell, dCollection) => new FacilityRawViewModel
            {
                Id = dynCell.Id,
                FacilityName = dynCell.FacilityName,
                FacilityCode = dynCell.FacilityCode,
                UpazilaId = dynCell.UpazilaId,
                UnionId = dynCell.UnionId,
                TargetedPopulation = dynCell.TargetedPopulation,
                FacilityType = dynCell.FacilityType,
                FacilityStatus = dynCell.FacilityStatus,
                ProgrammingPartnerId = dynCell.ProgrammingPartnerId,
                ImplemantationPartnerId = dynCell.ImplemantationPartnerId,
                TeacherId = dynCell.TeacherId,
                CollectionStatus = dCollection.Status,
                InstanceId = dynCell.InstanceId,
                Latitude = dynCell.Latitude,
                longitude = dynCell.longitude,
                Donors = dynCell.Donors,
                NonEducationPartner = dynCell.NonEducationPartner,
                Remarks = dynCell.Remarks,
                CampId = dynCell.CampId,
                ParaName = dynCell.ParaName,
                BlockId = dynCell.BlockId,
                UpdatedBy = dCollection.UpdatedBy,
                ApproveDate = dCollection.LastUpdated

            });
            var filterFacilityRaw = ApplyFilter(facilityWithCollectionStatusData.AsQueryable(), model).ToList();

            return filterFacilityRaw;
        }

        public List<FacilityRawViewModel> FacilityRawAsync(long instanceId,out List<FacilityDynamicCell> baseDynamicCellData,out List<FacilityDataCollectionStatus> dataCollectionStatus)
        {
            baseDynamicCellData =  _context.FacilityDynamicCells.Where(a => a.InstanceId == instanceId).ToList();
            var facilityRaw = baseDynamicCellData.GroupBy(a => a.FacilityId).Select(f => new FacilityRawViewModel
            {
                Id = f.Key,
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
                InstanceId = instanceId,

                Latitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Latitude).Select(h => h.Value).FirstOrDefault(),
                longitude = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Longitude).Select(h => h.Value).FirstOrDefault(),
                Donors = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Donors).Select(h => h.Value).FirstOrDefault(),
                NonEducationPartner = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.NonEducationPartner).Select(h => h.Value).FirstOrDefault(),
                Remarks = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Remarks).Select(h => h.Value).FirstOrDefault(),
                CampId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Camp).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),
                ParaName = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.ParaName).Select(h => h.Value).FirstOrDefault(),
                BlockId = f.Where(g => g.EntityDynamicColumnId == FacilityFixedIndicators.Block).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault()
            }).ToList();

            dataCollectionStatus =  _context.FacilityDataCollectionStatuses
                .Where(a => a.InstanceId == instanceId).ToList();

            var facilityWithCollectionStatusData = facilityRaw.Join(dataCollectionStatus, dynCell => dynCell.Id, dCollection => dCollection.FacilityId
            , (dynCell, dCollection) => new FacilityRawViewModel
            {
                Id = dynCell.Id,
                FacilityName = dynCell.FacilityName,
                FacilityCode = dynCell.FacilityCode,
                UpazilaId = dynCell.UpazilaId,
                UnionId = dynCell.UnionId,
                TargetedPopulation = dynCell.TargetedPopulation,
                FacilityType = dynCell.FacilityType,
                FacilityStatus = dynCell.FacilityStatus,
                ProgrammingPartnerId = dynCell.ProgrammingPartnerId,
                ImplemantationPartnerId = dynCell.ImplemantationPartnerId,
                TeacherId = dynCell.TeacherId,
                CollectionStatus = dCollection.Status,
                InstanceId = dynCell.InstanceId,
                Latitude = dynCell.Latitude,
                longitude = dynCell.longitude,
                Donors = dynCell.Donors,
                NonEducationPartner = dynCell.NonEducationPartner,
                Remarks = dynCell.Remarks,
                CampId = dynCell.CampId,
                ParaName = dynCell.ParaName,
                BlockId = dynCell.BlockId,
                UpdatedBy = dCollection.UpdatedBy,
                ApproveDate = dCollection.LastUpdated

            }).ToList();
           
            return facilityWithCollectionStatusData;
        }

        public async Task<PagedResponse<FacilityViewModel>> GetAllByViewId(FacilityByViewIdQueryModel model)
        {
            var baseDynamicCellData = await _context.FacilityDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var filterFacilityRaw = await FacilityRawAsync(baseDynamicCellData, model);

            var total = filterFacilityRaw.Count();
            var facilityData = filterFacilityRaw.OrderBy(a => a.Id).Skip(model.Skip()).Take(model.PageSize).ToList();
            var facilityIds = facilityData.Select(a => a.Id).ToList();

            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();

            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var userData = await _context.Users.ToListAsync();
            var upazilaData = await _context.Upazilas.ToListAsync();
            var unionData = await _context.Unions.ToListAsync();
            var campData = await _context.Camps.Where(a => facilityCampIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => facilityBlockIds.Contains(a.Id)).ToListAsync();

            var espData = await _context.EducationSectorPartners.Where(a => espIds.Contains(a.Id)).ToListAsync();
            
            var indicatorData = await _context.GridViewDetails
                .Include(a => a.EntityDynamicColumn)
                .ThenInclude(a => a.ColumnList)
                .ThenInclude(a => a.ListItems)
                .Where(a => a.GridViewId == model.ViewId).ToListAsync();

            var listItemData = new List<ListItem>();

            indicatorData.ForEach(a =>
            {
                if (a.EntityDynamicColumn.ColumnList != null)
                {
                    listItemData.AddRange(a.EntityDynamicColumn.ColumnList.ListItems);
                }
            });

            var entityColumnIds = indicatorData.Select(a => a.EntityDynamicColumnId).ToList();
            var dynamicCellData = baseDynamicCellData.Where(a => a.InstanceId == model.InstanceId
                && facilityIds.Contains(a.FacilityId)
                && entityColumnIds.Contains(a.EntityDynamicColumnId)
                ).ToList();

            var combineAllData = (from f in facilityData
                                  join uno in unionData on f.UnionId equals uno.Id
                                  join upa in upazilaData on f.UpazilaId equals upa.Id

                                  join c in campData on f.CampId equals c.Id into ca
                                  from bcmp in ca.DefaultIfEmpty()
                                  join b in blockData on f.BlockId equals b.Id into ba
                                  from blk in ba.DefaultIfEmpty()

                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id
                                  join tu in userData on f.TeacherId equals tu.Id into t
                                  from teadchrData in t.DefaultIfEmpty()

                                  join ii in indicatorData on model.ViewId equals ii.GridViewId
                                  join dc in dynamicCellData on new { FacilityId = f.Id, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.FacilityId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()

                                  join u in userData on f.UpdatedBy equals u.Id into def3
                                  from approvedByUser in def3.DefaultIfEmpty()

                                  select new FacilityRawViewModel
                                  {
                                      Id = f.Id,
                                      FacilityName = f.FacilityName,
                                      FacilityCode = f.FacilityCode,
                                      BlockId = blk?.Id,
                                      BlockName = blk?.Name,
                                      UpazilaId = upa.Id,
                                      UpazilaName = upa.Name,
                                      UnionId = uno.Id,
                                      UnionName = uno.Name,
                                      CampId = f.CampId,
                                      CampName = bcmp?.Name,
                                      CampSSID = bcmp?.SSID,
                                      Donors = f.Donors,
                                      Latitude = f.Latitude,
                                      longitude = f.longitude,
                                      NonEducationPartner = f.NonEducationPartner,

                                      ParaName = f.ParaName,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,
                                      TeacherId = f.TeacherId,
                                      TeacherName = teadchrData?.FullName,
                                      TeacherEmail = teadchrData?.Email,
                                      FacilityType = f.FacilityType,
                                      FacilityStatus = f.FacilityStatus,
                                      TargetedPopulation = f.TargetedPopulation,

                                      Remarks = f.Remarks,


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
                                      CollectionStatus = f.CollectionStatus,
                                      ApproveDate = f.ApproveDate,
                                      ApproveByUserEmail = approvedByUser?.Email,
                                      ApproveByUserName = approvedByUser?.FullName,
                                      ApproveByUserPhone = approvedByUser?.PhoneNumber
                                  }).ToList();

            var facility = SetFacilityViewModel(combineAllData);

            facility.ForEach(_modelToIndicatorConverter.ReplaceFacilityFixedIndicatorIdsWithValues);
            facility.ForEach(f => f.Properties = f.Properties.OrderBy(x => x.ColumnOrder).ToList());
            facility = facility.OrderBy(x => x.FacilityCode).ToList();

            return new PagedResponse<FacilityViewModel>(facility, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<FacilityViewModel>> GetAllByInstanceId(FacilityByViewIdQueryModel model)
        {
            var columnIds = await _context.InstanceIndicators.Where(a => a.InstanceId == model.InstanceId)
                .Select(a => a.EntityDynamicColumnId).ToListAsync();

            if (columnIds.Count == 0)
            {
                throw new InstanceHasNoIndicatorException();
            }

            var baseDynamicCellData = await _context.FacilityDynamicCells.Where(a => a.InstanceId == model.InstanceId).ToListAsync();

            var filterFacilityRaw = await FacilityRawAsync(baseDynamicCellData, model);

            var total = filterFacilityRaw.Count();
            var facilityData = filterFacilityRaw.OrderBy(a => a.Id).Skip(model.Skip()).Take(model.PageSize).ToList();
            var facilityIds = facilityData.Select(a => a.Id).ToList();


            var facilityCampIds = facilityData.Select(a => (int)a.CampId).ToList();
            var facilityBlockIds = facilityData.Select(a => (int)a.BlockId).ToList();

            var ipIds = facilityData.Select(a => a.ImplemantationPartnerId).ToList();
            var espIds = facilityData.Select(a => a.ProgrammingPartnerId).ToList().Union(ipIds);

            var userData = await _context.Users.ToListAsync();
            var upazilaData = await _context.Upazilas.ToListAsync();
            var unionData = await _context.Unions.ToListAsync();
            var campData = await _context.Camps.Where(a => facilityCampIds.Contains(a.Id)).ToListAsync();
            var blockData = await _context.Blocks.Where(a => facilityBlockIds.Contains(a.Id)).ToListAsync();

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
            var dynamicCellData = baseDynamicCellData.Where(a => a.InstanceId == model.InstanceId
                && facilityIds.Contains(a.FacilityId)
                && entityColumnIds.Contains(a.EntityDynamicColumnId)
                ).ToList();

            var combineAllData = (from f in facilityData
                                  join uno in unionData on f.UnionId equals uno.Id
                                  join upa in upazilaData on f.UpazilaId equals upa.Id

                                  join c in campData on f.CampId equals c.Id into ca
                                  from bcmp in ca.DefaultIfEmpty()
                                  join b in blockData on f.BlockId equals b.Id into ba
                                  from blk in ba.DefaultIfEmpty()

                                  join pp in espData on f.ProgrammingPartnerId equals pp.Id
                                  join ip in espData on f.ImplemantationPartnerId equals ip.Id
                                  join tu in userData on f.TeacherId equals tu.Id into t
                                  from teadchrData in t.DefaultIfEmpty()

                                  join ii in indicatorData on f.InstanceId equals ii.InstanceId
                                  join dc in dynamicCellData on new { FacilityId = f.Id, EntityDynamicColumnId = ii.EntityDynamicColumnId } equals new { dc.FacilityId, dc.EntityDynamicColumnId } into def2
                                  from dynCellData in def2.DefaultIfEmpty()
                                  join edcli in listItemData on ii.EntityDynamicColumn.ColumnListId equals edcli.ColumnListId into def1
                                  from liData in def1.DefaultIfEmpty()


                                  join u in userData on f.UpdatedBy equals u.Id into def3
                                  from approvedByUser in def3.DefaultIfEmpty()

                                  select new FacilityRawViewModel
                                  {
                                      Id = f.Id,
                                      FacilityName = f.FacilityName,
                                      FacilityCode = f.FacilityCode,
                                      BlockId = blk?.Id,
                                      BlockName = blk?.Name,
                                      UpazilaId = upa.Id,
                                      UpazilaName = upa.Name,
                                      UnionId = uno.Id,
                                      UnionName = uno.Name,
                                      CampId = f.CampId,
                                      CampName = bcmp?.Name,
                                      CampSSID = bcmp?.SSID,
                                      Donors = f.Donors,
                                      Latitude = f.Latitude,
                                      longitude = f.longitude,
                                      NonEducationPartner = f.NonEducationPartner,

                                      ParaName = f.ParaName,
                                      ProgrammingPartnerId = pp.Id,
                                      ProgrammingPartnerName = pp.PartnerName,
                                      ImplemantationPartnerId = ip.Id,
                                      ImplemantationPartnerName = ip.PartnerName,
                                      TeacherId = f.TeacherId,
                                      TeacherName = teadchrData?.FullName,
                                      TeacherEmail = teadchrData?.Email,
                                      FacilityType = f.FacilityType,
                                      FacilityStatus = f.FacilityStatus,
                                      TargetedPopulation = f.TargetedPopulation,

                                      Remarks = f.Remarks,


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
                                      CollectionStatus = f.CollectionStatus,
                                      ApproveDate = f.ApproveDate,
                                      ApproveByUserEmail = approvedByUser?.Email,
                                      ApproveByUserName = approvedByUser?.FullName,
                                      ApproveByUserPhone = approvedByUser?.PhoneNumber
                                  }).ToList();

            var facility = SetFacilityViewModel(combineAllData);

            facility.ForEach(_modelToIndicatorConverter.ReplaceFacilityFixedIndicatorIdsWithValues);
            facility.ForEach(f => f.Properties = f.Properties.OrderBy(x => x.ColumnOrder).ToList());
            facility = facility.OrderBy(x => x.FacilityCode).ToList();

            return new PagedResponse<FacilityViewModel>(facility, total, model.PageNo, model.PageSize);

        }

        public async Task<PagedResponse<FacilityViewModel>> GetAllWithValue(FacilityQueryModel facilityQueryModel)
        {
            var facilityListToGetIds = await GetPaginatedFacility(facilityQueryModel);
            var facilityIds = facilityListToGetIds.Select(a => a.Id).ToList();
            var total = facilityListToGetIds.Count();

            var facilityRaw = _facilityUserConditionFactory.GetFacilityUserCondition()
                                  .ApplyCondition(GetCollection(facilityQueryModel.InstanceId), facilityQueryModel.InstanceId);
            if (!string.IsNullOrEmpty(facilityQueryModel.SearchText))
                facilityRaw = facilityRaw.Where(a => a.PropertiesValue.Contains(facilityQueryModel.SearchText));


            var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id)).ToListAsync();

            var facility = SetFacilityViewModel(facilityData);
            return new PagedResponse<FacilityViewModel>(facility, total, facilityQueryModel.PageNo, facilityQueryModel.PageSize);
        }
        public async Task<PagedResponse<FacilityViewModel>> GetFacilityByIndicator(FacilityQueryModel facilityQueryModel, List<long> indicators)
        {
            var facilityListToGetIds = await GetPaginatedFacility(facilityQueryModel);
            var facilityIds = facilityListToGetIds.Select(a => a.Id).ToList();
            var total = facilityListToGetIds.Count();

            var facilityRaw = _facilityUserConditionFactory.GetFacilityUserCondition()
                                  .ApplyCondition(GetCollection(facilityQueryModel.InstanceId), facilityQueryModel.InstanceId);

            var facilityData = await facilityRaw.Where(a => facilityIds.Contains(a.Id) && indicators.Contains(a.EntityColumnId)).ToListAsync();
            var facility = SetFacilityViewModel(facilityData);
            return new PagedResponse<FacilityViewModel>(facility, total, facilityQueryModel.PageNo, facilityQueryModel.PageSize);
        }

        public async Task<PagedResponse<FacilityIndicatorWithValue>> GetFacilityIndicatorWithValue(FacilityQueryModel facilityQueryModel, List<long> indicators)
        {
            var facilityListToGetIds = await GetPaginatedFacility(facilityQueryModel);
            var facilityIds = facilityListToGetIds.Select(a => a.Id).ToList();
            var total = facilityListToGetIds.Count();

            var data = await (from f in _context.FacilityView
                              join camp in _context.Camps on f.CampId equals camp.Id into def1
                              from c in def1.DefaultIfEmpty()
                              join cellValue in _context.FacilityDynamicCells on new { f.InstanceId, FacilityId = f.Id } equals new { cellValue.InstanceId, cellValue.FacilityId }
                              join cell in _context.EntityDynamicColumn on cellValue.EntityDynamicColumnId equals cell.Id
                              join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                              join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                              where f.InstanceId == facilityQueryModel.InstanceId &&
                              facilityIds.Contains(f.Id) && indicators.Contains(cellValue.EntityDynamicColumnId)
                              select new FacilityIndicatorWithValue
                              {
                                  FacilityId = f.Id,
                                  CampId = f.CampId ?? 0,
                                  CampName = c.Name ?? "",
                                  EntityDynamicColumnId = cellValue.EntityDynamicColumnId,
                                  EntityDynamicColumnName = cell.Name,
                                  Value = cellValue.Value,
                                  ProgramPartnerId = f.ProgramPartnerId,
                                  ProgramPartenrName = pp.PartnerName,
                                  ImplementationPartnerId = f.ImplementationPartnerId,
                                  ImplementationPartnerName = ip.PartnerName,
                              }).ToListAsync();


            return new PagedResponse<FacilityIndicatorWithValue>(data, total, facilityQueryModel.PageNo, facilityQueryModel.PageSize);
        }


        public new async Task<FacilityEditViewModel> GetById(long id, long instanceId)
        {
            var facilityRaw =
                await (from f in _context.FacilityView
                       join uno in _context.Unions on f.UnionId equals uno.Id
                       join upa in _context.Upazilas on f.UpazilaId equals upa.Id
                       join c in _context.Camps on f.CampId equals c.Id into ca
                       from bcmp in ca.DefaultIfEmpty()
                       join b in _context.Blocks on f.BlockId equals b.Id into ba
                       from blk in ba.DefaultIfEmpty()
                       join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                       join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                       join tu in _context.Users on f.TeacherId equals tu.Id into t
                       from teadchrData in t.DefaultIfEmpty()
                       where f.Id == id && f.InstanceId==instanceId
                       select new FacilityEditViewModel
                       {

                           Id = f.Id,
                           Name = f.Name,
                           FacilityCode = f.FacilityCode,
                           BlockId = blk.Id,
                           BlockName = blk.Name,
                           Donors = f.Donors,
                           Latitude = f.Latitude,
                           Longitude = f.Longitude,
                           NonEducationPartner = f.NonEducationPartner,
                           UpazilaId = upa.Id,
                           UpazilaName = upa.Name,
                           UnionId = uno.Id,
                           UnionName = uno.Name,
                           CampSSID = bcmp.SSID,
                           CampId = f.CampId,
                           CampName = bcmp.Name,

                           ParaName = f.ParaName,
                           ProgramPartnerId = pp.Id,
                           ProgramPartnerName = pp.PartnerName,
                           ImplementationPartnerId = ip.Id,
                           ImplementationPartnerName = ip.PartnerName,
                           TeacherId = f.TeacherId,
                           TeacherName = teadchrData.FullName,
                           TeacherEmail = teadchrData.Email,
                           TeacherPhone = teadchrData.PhoneNumber,
                           FacilityType = f.FacilityType,
                           FacilityStatus = f.FacilityStatus,
                           TargetedPopulation = f.TargetedPopulation,
                           Remarks = f.Remarks,

                       }).FirstOrDefaultAsync();
            return facilityRaw;
        }
        public async Task StartCollectionForAllFacility(long instanceId)
        {
            var facilityDataCollection = await _context.Facility
                                          .Select(a => new FacilityDataCollectionStatus
                                          {
                                              FacilityId = a.Id,
                                              InstanceId = instanceId,
                                              Status = CollectionStatus.NotCollected
                                          }).ToListAsync();
            _context.FacilityDataCollectionStatuses.AddRange(facilityDataCollection);
            await _context.SaveChangesAsync();
        }

        public async Task StartCollectionForAllFacility(long instanceId, List<long> facilityIds)
        {

            var facilityDataCollection = facilityIds
                .Select(id => new FacilityDataCollectionStatus
                {
                    FacilityId = id,
                    InstanceId = instanceId,
                    Status = CollectionStatus.NotCollected
                }).ToList();
            _context.FacilityDataCollectionStatuses.AddRange(facilityDataCollection);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Count(Specification<Facility> specification)
        {
            return await _context.Facility.Where(specification.ToExpression()).CountAsync();
        }

        public async Task<List<FacilityViewModel>> GetAllFacilitites(Expression<Func<FacilityView, bool>> filter)
        {
            var facilityRaw = (from f in _context.FacilityView.Where(filter)
                               join uno in _context.Unions on f.UnionId equals uno.Id
                               join upa in _context.Upazilas on f.UpazilaId equals upa.Id

                               join c in _context.Camps on f.CampId equals c.Id into ca
                               from bcmp in ca.DefaultIfEmpty()
                               join b in _context.Blocks on f.BlockId equals b.Id into ba
                               from blk in ba.DefaultIfEmpty()
                               join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
                               join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id
                               join tu in _context.Users on f.TeacherId equals tu.Id into t
                               from teacherData in t.DefaultIfEmpty()
                               select new FacilityViewModel()
                               {
                                   Id = f.Id,
                                   FacilityName = f.Name,
                                   FacilityCode = f.FacilityCode,
                                   UpazilaId = upa.Id,
                                   UpazilaName = upa.Name,
                                   UnionId = uno.Id,
                                   UnionName = uno.Name,
                                   FacilityStatus = f.FacilityStatus,
                                   FacilityType = f.FacilityType,
                                   TargetedPopulation = f.TargetedPopulation,
                                   BlockId = f.BlockId,
                                   BlockName = blk.Name,
                                   CampId = f.CampId,
                                   CampName = bcmp.Name,
                                   CampSSID = bcmp.SSID,
                                   Donors = f.Donors,
                                   Latitude = f.Latitude,
                                   longitude = f.Longitude,
                                   NonEducationPartner = f.NonEducationPartner,

                                   ParaName = f.ParaName,
                                   ProgrammingPartnerId = f.ProgramPartnerId,
                                   ProgrammingPartnerName = pp.PartnerName,
                                   ImplemantationPartnerId = f.ImplementationPartnerId,
                                   ImplemantationPartnerName = ip.PartnerName,
                                   TeacherId = f.TeacherId,
                                   TeacherName = teacherData.FullName,
                                   Remarks = f.Remarks
                               });


            var facilityData = await facilityRaw.ToListAsync();
            return facilityData;
        }

        public IQueryable<FacilityView> GetFacilityView()
        {
            return _context.FacilityView.AsQueryable();
        }
    }
}
