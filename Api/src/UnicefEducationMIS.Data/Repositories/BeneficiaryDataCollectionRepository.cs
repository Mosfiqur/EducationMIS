using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Data.Factory;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Repositories
{
    public class BeneficiaryDataCollectionRepository : BaseRepository<BeneficiaryDynamicCell, long>, IBeneficiaryDataCollectionRepository
    {
        private readonly BeneficiaryUserConditionFactory _beneficiaryUserConditionFactory;
        public BeneficiaryDataCollectionRepository(UnicefEduDbContext context, BeneficiaryUserConditionFactory beneficiaryUserConditionFactory) : base(context)
        {
            _beneficiaryUserConditionFactory = beneficiaryUserConditionFactory;
        }

        public IQueryable<BeneficiaryRawViewModel> GetDeactivateBeneficiaryData(SubmittedBeneficiaryQueryModel model)
        {
            return null;
            //TODO: beneficiary new schema
            //var query = from b in _context.Beneficiary
            //            join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
            //            join f in _context.Facility on b.FacilityId equals f.Id
            //            join bdh in _context.BeneficiaryDeactivationHistory on
            //            b.Id equals bdh.BeneficiaryId
            //            where bdh.IsApproved == false
            //            && bdh.InstanceId == model.InstanceId
            //            select new BeneficiaryRawViewModel
            //            {
            //                BeneficiaryName = b.Name,
            //                UnhcrId = b.UnhcrId,
            //                Sex = b.Sex,
            //                LevelOfStudy = b.LevelOfStudy,
            //                Disabled = b.Disabled,
            //                BeneficiaryCampId = b.BeneficiaryCampId,
            //                BeneficiaryCampName = bcmp.Name,
            //                DateOfBirth = b.DateOfBirth,
            //                EnrollmentDate = b.EnrollmentDate,

            //                EntityId = b.Id,
            //                FacilityId = b.FacilityId,
            //                FacilityName = f.Name,
            //                ProgrammingPartnerId = f.ProgramPartnerId,
            //                ImplemantationPartnerId = f.ImplementationPartnerId
            //            };

            //query = ApplyFilter(query, model);
            //query = _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //             .ApplyCondition(query);

            //return query;
        }
        public IQueryable<BeneficiaryRawViewModel> GetSubmittedData(SubmittedBeneficiaryQueryModel model)
        {
            return null; 
            //TODO: beneficiary new schema
            //var query = from cs in _context.BeneciaryDataCollectionStatuses
            //            join b in _context.Beneficiary on cs.BeneficiaryId equals b.Id
            //            join bcmp in _context.Camps on b.BeneficiaryCampId equals bcmp.Id
            //            join blk in _context.Blocks on b.BlockId equals blk.Id
            //            join sblk in _context.SubBlocks on b.SubBlockId equals sblk.Id
            //            join f in _context.Facility on b.FacilityId equals f.Id
            //            // join fcmp in _context.Camps on f.CampId equals fcmp.Id

            //            join c in _context.Camps on f.CampId equals c.Id into ca
            //            from fcmp in ca.DefaultIfEmpty()


            //            join ii in _context.InstanceIndicators on cs.InstanceId equals ii.InstanceId

            //            join edc in _context.EntityDynamicColumn on
            //             new { EntityTypeId = EntityType.Beneficiary, Id = ii.EntityDynamicColumnId } equals new { edc.EntityTypeId, edc.Id }
            //            join cl in _context.ListDataType on edc.ColumnListId equals cl.Id into def
            //            from columnListData in def.DefaultIfEmpty()
            //            join li in _context.ListItems on columnListData.Id equals li.ColumnListId into def1
            //            from listItemData in def1.DefaultIfEmpty()
            //            join dn in _context.BeneficiaryDynamicCells
            //             on new { BeneficiaryId = b.Id, ii.InstanceId, ii.EntityDynamicColumnId }
            //                 equals new { dn.BeneficiaryId, dn.InstanceId, dn.EntityDynamicColumnId } into group1
            //            from dynamicCellData in group1.DefaultIfEmpty()

            //            where
            //           (model.CollectionStatus == null || cs.Status == model.CollectionStatus) 
            //           && cs.InstanceId == model.InstanceId
            //            select new BeneficiaryRawViewModel
            //            {
            //                EntityId = b.Id,
            //                BeneficiaryName = b.Name,
            //                UnhcrId = b.UnhcrId,
            //                FCNId = b.FCNId,
            //                FatherName = b.FatherName,
            //                MotherName = b.MotherName,
            //                DateOfBirth = b.DateOfBirth,
            //                EnrollmentDate = b.EnrollmentDate,
            //                Disabled = b.Disabled,
            //                LevelOfStudy = b.LevelOfStudy,
            //                Sex = b.Sex,

            //                FacilityId = f.Id,
            //                FacilityName = f.Name,

            //                FacilityCampId = fcmp.Id,
            //                FacilityCampName = fcmp.Name,
            //                BeneficiaryCampId = bcmp.Id,
            //                BeneficiaryCampName = bcmp.Name,
            //                BlockId = blk.Id,
            //                BlockName = blk.Name,
            //                SubBlockId = sblk.Id,
            //                SubBlockName = sblk.Name,

            //                EntityColumnId = edc.Id,
            //                EntityColumnName = edc.Name,
            //                ColumnListId = edc.ColumnListId,
            //                ColumnListName = columnListData.Name,
            //                DataType = edc.ColumnType,
            //                ListItemId = listItemData.Id,
            //                ListItemTitle = listItemData.Title,
            //                ListItemValue = listItemData.Value,
            //                PropertiesId = dynamicCellData.Id,
            //                PropertiesValue = dynamicCellData.Value,

            //                IsActive = b.IsActive,
            //                IsApproved = b.IsApproved,

            //                CollectionStatus = cs.Status
            //            };

            //return query;
        }

        private IQueryable<BeneficiaryRawViewModel> ApplyFilter(IQueryable<BeneficiaryRawViewModel> rawQuery, SubmittedBeneficiaryQueryModel model)
        {
            //var rawQuery = beneficiaryRaws;
            if (!string.IsNullOrEmpty(model.Filter.SearchText))
            {
                rawQuery = rawQuery.Where(a => a.BeneficiaryName.Contains(model.Filter.SearchText)
                || a.UnhcrId.Contains(model.Filter.SearchText));
            }
            if (!string.IsNullOrEmpty(model.Filter.DateOfBirth.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(model.Filter.DateOfBirth.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(model.Filter.DateOfBirth.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.DateOfBirth >= filterStartDate && a.DateOfBirth <= filterEndDate);
            }

            if (!string.IsNullOrEmpty(model.Filter.EnrolmentDate.StartDate))
            {
                DateTime filterStartDate = DateTime.ParseExact(model.Filter.EnrolmentDate.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime filterEndDate = DateTime.ParseExact(model.Filter.EnrolmentDate.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                rawQuery = rawQuery.Where(a => a.EnrollmentDate >= filterStartDate && a.EnrollmentDate <= filterEndDate);
            }

            if (model.Filter.Sex != null)
            {
                rawQuery = rawQuery.Where(a => a.Sex == model.Filter.Sex);
            }

            if (model.Filter.LevelOfStudy != null)
            {
                rawQuery = rawQuery.Where(a => a.LevelOfStudy == model.Filter.LevelOfStudy);
            }

            if (model.Filter.Disable != null)
            {
                rawQuery = rawQuery.Where(a => a.Disabled == model.Filter.Disable);
            }

            if (model.Filter.Camps.Count > 0)
            {
                var campIds = model.Filter.Camps.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => campIds.Contains(a.BeneficiaryCampId));
            }
            if (model.Filter.Facilities.Count > 0)
            {
                var facilityIds = model.Filter.Facilities.Select(a => a.Id).ToList();
                rawQuery = rawQuery.Where(a => facilityIds.Contains(a.FacilityId));
            }

            return rawQuery;
        }
        public async Task<List<long>> GetPaginatedBeneficiaryId(SubmittedBeneficiaryQueryModel model)
        {
            return new List<long>();
            //TODO: beneficiary new schema
            //var beneficiary = (
            //    from b in _context.Beneficiary
            //    join f in _context.Facility on b.FacilityId equals f.Id
            //    join bdcs in _context.BeneciaryDataCollectionStatuses on new { model.InstanceId, BeneficiaryId = b.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
            //    where bdcs.Status == CollectionStatus.Collected
            //    select new BeneficiaryRawViewModel
            //    {
            //        EntityId = b.Id,
            //        FacilityId = b.FacilityId,
            //        ProgrammingPartnerId = f.ProgramPartnerId,
            //        ImplemantationPartnerId = f.ImplementationPartnerId,

            //        BeneficiaryName = b.Name,
            //        UnhcrId = b.UnhcrId,
            //        Sex = b.Sex,
            //        LevelOfStudy = b.LevelOfStudy,
            //        Disabled = b.Disabled,
            //        BeneficiaryCampId = b.BeneficiaryCampId,
            //        DateOfBirth = b.DateOfBirth,
            //        EnrollmentDate = b.EnrollmentDate,

            //    });

            //beneficiary = ApplyFilter(beneficiary, model);


            //var ids = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //             .ApplyCondition(beneficiary).Select(a => a.EntityId).Skip(model.Skip())
            //             .Take(model.PageSize).ToListAsync();

            ////var ids =await _context.BeneciaryDataCollectionStatuses
            ////         .Where(cs => cs.Status == CollectionStatus.Collected && cs.InstanceId == model.InstanceId)
            ////         .Skip(model.Skip())
            ////         .Take(model.PageSize)
            ////         .Select(a=>a.BeneficiaryId)
            ////         .ToListAsync();


            //return ids;
        }

        public async Task<List<long>> GetSubmittedBeneficiaryId(SubmittedBeneficiaryQueryModel model, int skip, int take)
        {
            return new List<long>();
            //TODO: beneficiary new schema
            //var beneficiary = (
            //    from b in _context.Beneficiary
            //    join f in _context.Facility on b.FacilityId equals f.Id
            //    join bdcs in _context.BeneciaryDataCollectionStatuses on new { model.InstanceId, BeneficiaryId = b.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
            //    where (model.CollectionStatus==null || bdcs.Status == model.CollectionStatus)
            //    && bdcs.InstanceId == model.InstanceId
            //    select new BeneficiaryRawViewModel
            //    {
            //        EntityId = b.Id,
            //        FacilityId = b.FacilityId,
            //        ProgrammingPartnerId = f.ProgramPartnerId,
            //        ImplemantationPartnerId = f.ImplementationPartnerId,

            //        BeneficiaryName = b.Name,
            //        UnhcrId = b.UnhcrId,
            //        Sex = b.Sex,
            //        LevelOfStudy = b.LevelOfStudy,
            //        Disabled = b.Disabled,
            //        BeneficiaryCampId = b.BeneficiaryCampId,
            //        DateOfBirth = b.DateOfBirth,
            //        EnrollmentDate = b.EnrollmentDate,

            //    });

            //beneficiary = ApplyFilter(beneficiary, model);


            //var ids = await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //             .ApplyCondition(beneficiary).OrderBy(a=>a.BeneficiaryName)
            //             .Select(a => a.EntityId).Skip(skip)
            //             .Take(take).ToListAsync();

            //return ids;
        }

        public async Task<int> GetTotal(SubmittedBeneficiaryQueryModel model)
        {
            return 0;
            //TODO: beneficiary new schema
            //var beneficiary = (
            //    from b in _context.Beneficiary
            //    join f in _context.Facility on b.FacilityId equals f.Id
            //    join bdcs in _context.BeneciaryDataCollectionStatuses on new { InstanceId = model.InstanceId, BeneficiaryId = b.Id } equals new { bdcs.InstanceId, bdcs.BeneficiaryId }
            //    where (model.CollectionStatus==null || bdcs.Status == model.CollectionStatus) //CollectionStatus.Collected
            //    select new BeneficiaryRawViewModel
            //    {
            //        EntityId = b.Id,
            //        FacilityId = b.FacilityId,
            //        ProgrammingPartnerId = f.ProgramPartnerId,
            //        ImplemantationPartnerId = f.ImplementationPartnerId,


            //        BeneficiaryName = b.Name,
            //        UnhcrId = b.UnhcrId,
            //        Sex = b.Sex,
            //        LevelOfStudy = b.LevelOfStudy,
            //        Disabled = b.Disabled,
            //        BeneficiaryCampId = b.BeneficiaryCampId,
            //        DateOfBirth = b.DateOfBirth,
            //        EnrollmentDate = b.EnrollmentDate,

            //    });

            //beneficiary = ApplyFilter(beneficiary, model);

            //return await _beneficiaryUserConditionFactory.GetBeneficiaryUserCondition()
            //             .ApplyCondition(beneficiary).CountAsync();

        }

    }
}
