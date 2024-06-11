using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Data.Factory;

namespace UnicefEducationMIS.Data.Repositories
{

    public class FacilityDataCollectionRepository : BaseRepository<FacilityDynamicCell, long>,
        IFacilityDataCollectionRepository
    {
        public FacilityDataCollectionRepository(UnicefEduDbContext context) : base(context)
        {
        }

        public IQueryable<FacilityRawViewModel> GetSubmittedData(SubmittedFacilityQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<long>> GetPaginatedFacilityId(SubmittedFacilityQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotal(SubmittedFacilityQueryModel model)
        {
            throw new NotImplementedException();
        }
    }

    //public class FacilityDataCollectionRepository : BaseRepository<FacilityDynamicCell, long>, IFacilityDataCollectionRepository
    //{
    //    private readonly FacilityUserConditionFactory _facilityUserConditionFactory;
    //    public FacilityDataCollectionRepository(UnicefEduDbContext context, FacilityUserConditionFactory facilityUserConditionFactory) : base(context)
    //    {
    //        _facilityUserConditionFactory = facilityUserConditionFactory;
    //    }

    //    public IQueryable<FacilityRawViewModel> GetSubmittedData(SubmittedFacilityQueryModel model)
    //    {
    //        var query = from collectionStatus in _context.FacilityDataCollectionStatuses
    //                    join f in _context.Facility on collectionStatus.FacilityId equals f.Id
    //                    join ii in _context.InstanceIndicators on collectionStatus.InstanceId equals ii.InstanceId

    //                    join c in _context.Camps on f.CampId equals c.Id into ca
    //                    from bcmp in ca.DefaultIfEmpty()
    //                    join b in _context.Blocks on f.BlockId equals b.Id into ba
    //                    from blk in ba.DefaultIfEmpty()
    //                        //join pa in _context.Paras on f.ParaId equals pa.Id
    //                        //into p from para in p.DefaultIfEmpty()
    //                    join pp in _context.EducationSectorPartners on f.ProgramPartnerId equals pp.Id
    //                    join ip in _context.EducationSectorPartners on f.ImplementationPartnerId equals ip.Id

    //                    join edc in _context.EntityDynamicColumn on
    //                     new { EntityTypeId = EntityType.Facility, Id = ii.EntityDynamicColumnId } equals new { edc.EntityTypeId, edc.Id }
    //                    join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
    //                    from columnListData in def.DefaultIfEmpty()
    //                    join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
    //                    from listItemData in def1.DefaultIfEmpty()
    //                    join dn in _context.FacilityDynamicCells
    //                     on new { FacilityId = f.Id, ii.InstanceId, ii.EntityDynamicColumnId }
    //                         equals new { dn.FacilityId, dn.InstanceId, dn.EntityDynamicColumnId } into group1
    //                    from dynamicCellData in group1.DefaultIfEmpty()

    //                    where ( model.CollectionStatus==null || collectionStatus.Status == model.CollectionStatus )
    //                    && collectionStatus.InstanceId == model.InstanceId
    //                    select new FacilityRawViewModel
    //                    {
    //                        Id = f.Id,
    //                        FacilityName = f.Name,
    //                        FacilityCode = f.FacilityCode,
    //                        BlockId = blk.Id,
    //                        BlockName = blk.Name,
    //                        CampId = f.CampId,
    //                        CampName = bcmp.Name,
    //                        ParaId = f.ParaId,
    //                        ParaName = f.ParaName,
    //                        ProgrammingPartnerId = pp.Id,
    //                        ProgrammingPartnerName = pp.PartnerName,
    //                        ImplemantationPartnerId = ip.Id,
    //                        ImplemantationPartnerName = ip.PartnerName,
    //                        TeacherId = f.TeacherId,
    //                        //TeacherName = teadchrData.FullName,
    //                        InstanceId = ii.InstanceId,
    //                        //CollectionStatus = fdcs.Status,
    //                        FacilityType = f.FacilityType,
    //                        FacilityStatus = f.FacilityStatus,
    //                        TargetedPopulation = f.TargetedPopulation,
    //                        //NoOfLfEstablishedInRohingyaCommunity = f.NoOfLfEstablishedInRohingyaCommunity,
    //                        //NoOfClassroomsInLfInRohingyaCommunity = f.NoOfClassroomsInLfInRohingyaCommunity,
    //                        //NoOfShiftsInLfInRohingyaCommunity = f.NoOfShiftsInLfInRohingyaCommunity,
    //                        //NoOfFunctionalCommunityLatrinesNearLF = f.NoOfFunctionalCommunityLatrinesNearLF,
    //                        //NoOfLatrinesEstablishedForBoys = f.NoOfLatrinesEstablishedForBoys,
    //                        //NoOfLatrinesEstablishedEorGirls = f.NoOfLatrinesEstablishedForGirls,
    //                        //IDofTheLatrinesAccessibleToTheLC = f.IDofTheLatrinesAccessibleToTheLC,
    //                        //NoOfHandwashingStationEstablished = f.NoOfHandwashingStationEstablished,
    //                        //NoOfFemaleRohingyaLearningFacilitatorsTrained = f.NoOfFemaleRohingyaLearningFacilitatorsTrained,
    //                        //NoOfMaleRohingyaLearningFacilitatorsTrained = f.NoOfMaleRohingyaLearningFacilitatorsTrained,
    //                        //NoOfFemaleHcLearningFacilitatorsTrained = f.NoOfFemaleHcLearningFacilitatorsTrained,
    //                        //NoOfMaleHcLearningFacilitatorsTrained = f.NoOfMaleHcLearningFacilitatorsTrained,
    //                        //NoOfDrrAwarenessSessionsConductedInHc = f.NoOfDrrAwarenessSessionsConductedInHc,
    //                        //NoOfRohingyaLearningFacilityEducationCommitteesEstablishedInRohingyaCamps = f.NoOfRohingyaLearningFacilityEducationCommitteesEstablishedInRohingyaCamps,
    //                        //NoOfFemaleRohingyaCaregiversSensitizedOnCYRPAP = f.NoOfFemaleRohingyaCaregiversSensitizedOnCYRPAP,
    //                        //NoOfMaleRohingyaCaregiversSensitizedOnCYRPAP = f.NoOfMaleRohingyaCaregiversSensitizedOnCYRPAP,
    //                        //NoOfFemaleHcCaregiversSensitizedOnCYRPAP = f.NoOfFemaleHcCaregiversSensitizedOnCYRPAP,
    //                        //NoOfMaleHcCaregiversSensitizedOnCYRPAP = f.NoOfMaleHcCaregiversSensitizedOnCYRPAP,
    //                        //NoOfRohingyaGirlsAged3_24YearsOldEngagedInSCI = f.NoOfRohingyaGirlsAged3_24YearsOldEngagedInSCI,
    //                        //NoOfRohingyaBoysAged3_24YearsOldEngagedInSCI = f.NoOfRohingyaBoysAged3_24YearsOldEngagedInSCI,
    //                        //NoOfChildrenBenefittingFromFood = f.NoOfChildrenBenefittingFromFood,
    //                        //NoOfChildrenBenefittingFromSchool_Classroom_ToiletRehabilitation = f.NoOfChildrenBenefittingFromSchool_Classroom_ToiletRehabilitation,
    //                        //OtherRapidIntervention = f.OtherRapidIntervention,
    //                        Remarks = f.Remarks,
    //                        CollectionStatus=collectionStatus.Status,

    //                        EntityColumnId = edc.Id,
    //                        EntityColumnName = edc.Name,
    //                        ColumnListId = edc.ColumnListId,
    //                        ColumnListName = columnListData.Name,
    //                        DataType = edc.ColumnType,
    //                        ListItemId = listItemData.Id,
    //                        ListItemTitle = listItemData.Title,
    //                        ListItemValue = listItemData.Value,
    //                        PropertiesId = dynamicCellData.Id,
    //                        PropertiesValue = dynamicCellData.Value
    //                    };
    //        return query;
    //    }
    //    private IQueryable<FacilityRawViewModel> ApplyFilter(IQueryable<FacilityRawViewModel> rawQuery, SubmittedFacilityQueryModel model)
    //    {
    //        //var rawQuery = beneficiaryRaws;
    //        if (!string.IsNullOrEmpty(model.Filter.SearchText))
    //        {
    //            rawQuery = rawQuery.Where(a => a.FacilityName.Contains(model.Filter.SearchText)
    //            || a.FacilityCode.Contains(model.Filter.SearchText));
    //        }
    //        if (model.Filter.UpazilaId != null)
    //        {

    //            rawQuery = rawQuery.Where(a => a.UpazilaId == model.Filter.UpazilaId);
    //        }

    //        if (model.Filter.UnionId != null)
    //        {

    //            rawQuery = rawQuery.Where(a => a.UnionId == model.Filter.UnionId);
    //        }

    //        if (model.Filter.TargetedPopulation != null)
    //        {
    //            rawQuery = rawQuery.Where(a => a.TargetedPopulation == model.Filter.TargetedPopulation);
    //        }

    //        if (model.Filter.FacilityType != null)
    //        {
    //            rawQuery = rawQuery.Where(a => a.FacilityType == model.Filter.FacilityType);
    //        }

    //        if (model.Filter.FacilityStatus != null)
    //        {
    //            rawQuery = rawQuery.Where(a => a.FacilityStatus == model.Filter.FacilityStatus);
    //        }

    //        if (model.Filter.ProgramPartner.Count > 0)
    //        {
    //            var ppIds = model.Filter.ProgramPartner.Select(a => a.Id).ToList();
    //            rawQuery = rawQuery.Where(a => ppIds.Contains(a.ProgrammingPartnerId));
    //        }
    //        if (model.Filter.ImplementationPartner.Count > 0)
    //        {
    //            var ipIds = model.Filter.ImplementationPartner.Select(a => a.Id).ToList();
    //            rawQuery = rawQuery.Where(a => ipIds.Contains(a.ImplemantationPartnerId));
    //        }
    //        if (model.Filter.Teachers.Count > 0)
    //        {
    //            var tIds = model.Filter.Teachers.Select(a => a.Id).ToList();
    //            rawQuery = rawQuery.Where(a => tIds.Contains((int)a.TeacherId));
    //        }
    //        return rawQuery;
    //    }
    //    public async Task<List<long>> GetPaginatedFacilityId(SubmittedFacilityQueryModel model)
    //    {
    //        var facility = (
    //           from f in _context.Facility
    //           join bdcs in _context.FacilityDataCollectionStatuses on new { model.InstanceId, FacilityId = f.Id } equals new { bdcs.InstanceId, bdcs.FacilityId }
    //           where (model.CollectionStatus==null||bdcs.Status == model.CollectionStatus)//CollectionStatus.Collected //&& bdcs.InstanceId == model.InstanceId
    //           select new FacilityRawViewModel
    //           {
    //               Id = f.Id,
    //               FacilityName = f.Name,
    //               FacilityCode = f.FacilityCode,
    //               UpazilaId = f.UpazilaId,
    //               UnionId = f.UnionId,
    //               TargetedPopulation = f.TargetedPopulation,
    //               FacilityType = f.FacilityType,
    //               FacilityStatus = f.FacilityStatus,
    //               ProgrammingPartnerId = f.ProgramPartnerId,
    //               ImplemantationPartnerId = f.ImplementationPartnerId,
    //               TeacherId = f.TeacherId
    //           });
    //        facility = ApplyFilter(facility, model);
    //        var ids= await _facilityUserConditionFactory.GetFacilityUserCondition()
    //                     .ApplyCondition(facility).OrderBy(a => a.FacilityCode).Select(a=>a.Id).Skip(model.Skip())
    //                     .Take(model.PageSize).ToListAsync();


    //        //var ids = await _context.FacilityDataCollectionStatuses
    //        //         .Where(cs => cs.Status == CollectionStatus.Collected && cs.InstanceId == model.InstanceId)
    //        //         .Skip(model.Skip())
    //        //         .Take(model.PageSize)
    //        //         .Select(a => a.FacilityId)
    //        //         .ToListAsync();
    //        return ids;
    //    }

    //    public async Task<int> GetTotal(SubmittedFacilityQueryModel model)
    //    {
    //        //return await _context.FacilityDataCollectionStatuses.CountAsync(a => a.InstanceId == instanceId && a.Status==CollectionStatus.Collected);

    //        var facility = (
    //           from f in _context.Facility
    //           join bdcs in _context.FacilityDataCollectionStatuses on new { InstanceId= model.InstanceId, FacilityId = f.Id } equals new { bdcs.InstanceId, bdcs.FacilityId }
    //           where (model.CollectionStatus==null || bdcs.Status == model.CollectionStatus)//CollectionStatus.Collected //&& bdcs.InstanceId == model.InstanceId
    //           select new FacilityRawViewModel
    //           {
    //               Id = f.Id,
    //               FacilityName = f.Name,
    //               FacilityCode = f.FacilityCode,
    //               UpazilaId = f.UpazilaId,
    //               UnionId = f.UnionId,
    //               TargetedPopulation = f.TargetedPopulation,
    //               FacilityType = f.FacilityType,
    //               FacilityStatus = f.FacilityStatus,
    //               ProgrammingPartnerId = f.ProgramPartnerId,
    //               ImplemantationPartnerId = f.ImplementationPartnerId,
    //               TeacherId = f.TeacherId
    //           });
    //        facility = ApplyFilter(facility, model);

    //        return await _facilityUserConditionFactory.GetFacilityUserCondition()
    //                     .ApplyCondition(facility).CountAsync();
    //    }
    //}
}
