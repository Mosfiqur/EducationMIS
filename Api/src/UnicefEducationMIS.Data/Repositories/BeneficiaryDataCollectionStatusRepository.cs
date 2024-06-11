using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class BeneficiaryDataCollectionStatusRepository : BaseRepository<BeneficiaryDataCollectionStatus, long>, IBeneficiaryDataCollectionStatusRepository
    {
        private const string noRecordsFound = "No records found.";
        private const string noScheduleInstanceRecordFound = "NO scheduleInstance record found.";

        public BeneficiaryDataCollectionStatusRepository(UnicefEduDbContext context) : base(context)
        {
        }

        public async Task ApproveBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //TODO: beneficiary new schema
            //var lstDataCollection = await _context.BeneciaryDataCollectionStatuses
            //                        .Where(a => a.InstanceId == model.InstanceId && model.BeneficiaryIds.Contains(a.BeneficiaryId))
            //                        .ToListAsync();
            //if (lstDataCollection.Count == 0)
            //{
            //    throw new RecordNotFound(noRecordsFound);
            //}

            //foreach (BeneficiaryDataCollectionStatus item in lstDataCollection)
            //{
            //    item.Status = CollectionStatus.Approved;
            //}
            //_context.BeneciaryDataCollectionStatuses.UpdateRange(lstDataCollection);

            //var isNotApprovedExist = await _context.BeneciaryDataCollectionStatuses
            //    .AnyAsync(a => a.InstanceId == model.InstanceId
            //    && a.Status != CollectionStatus.Approved
            //    && !model.BeneficiaryIds.Contains(a.BeneficiaryId));

            //if (!isNotApprovedExist)
            //{
            //    var scheduleInstance = _context.ScheduleInstances.Where(a => a.Id == model.InstanceId).FirstOrDefault();
            //    if (scheduleInstance == null)
            //    {
            //        throw new RecordNotFound(noScheduleInstanceRecordFound);
            //    }
            //    scheduleInstance.Status = InstanceStatus.Completed;
            //    _context.ScheduleInstances.Update(scheduleInstance);
            //}

            //await _context.SaveChangesAsync();
        }
        public async Task RemoveDeactivateBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //TODO: beneficiary new schema
            //var deactivateData = await _context.DeleteInactiveBeneficiary
            //    .Where(a => a.InstanceId == model.InstanceId && model.BeneficiaryIds.Contains(a.BeneficiaryId)).ToListAsync();
            //_context.DeleteInactiveBeneficiary.RemoveRange(deactivateData);
            //await _context.SaveChangesAsync();

        }
        public async Task<List<int>> BeneficiaryDeletedBy(BeneficiaryApprovalViewModel model)
        {
            //TODO: beneficiary new schema

            //var userList=await _context.DeleteInactiveBeneficiary
            //    .Where(a => a.InstanceId == model.InstanceId && model.BeneficiaryIds.Contains(a.BeneficiaryId))
            //    .Select(a=>a.CreatedBy)
            //    .ToListAsync();
            //return userList;

            return new List<int>();
        }
        public async Task ApproveDeactiveBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //TODO: beneficiary new schema
            //var lstDataCollection = await _context.BeneciaryDataCollectionStatuses
            //                        .Where(a => a.InstanceId == model.InstanceId && model.BeneficiaryIds.Contains(a.BeneficiaryId))
            //                        .ToListAsync();

            //foreach (BeneficiaryDataCollectionStatus item in lstDataCollection)
            //{
            //    item.Status = CollectionStatus.Approved;
            //}
            //_context.BeneciaryDataCollectionStatuses.UpdateRange(lstDataCollection);

            //var isNotApprovedExist = await _context.BeneciaryDataCollectionStatuses
            //    .AnyAsync(a => a.InstanceId == model.InstanceId
            //    && a.Status != CollectionStatus.Approved
            //    && !model.BeneficiaryIds.Contains(a.BeneficiaryId));

            //if (!isNotApprovedExist)
            //{
            //    var scheduleInstance = _context.ScheduleInstances.Where(a => a.Id == model.InstanceId).FirstOrDefault();
            //    if (scheduleInstance == null)
            //    {
            //        throw new RecordNotFound(noScheduleInstanceRecordFound);
            //    }
            //    scheduleInstance.Status = InstanceStatus.Completed;
            //    _context.ScheduleInstances.Update(scheduleInstance);
            //}

            //var deactivateBeneficiaryHistory = await _context.BeneficiaryDeactivationHistory
            //    .Where(a => model.BeneficiaryIds.Contains(a.BeneficiaryId) && a.InstanceId == model.InstanceId).ToListAsync();
            //if (deactivateBeneficiaryHistory.Count > 0)
            //{
            //    foreach (var item in deactivateBeneficiaryHistory)
            //    {
            //        item.IsApproved = true;
            //    }
            //    _context.BeneficiaryDeactivationHistory.UpdateRange(deactivateBeneficiaryHistory);


            //    var deactivateBeneficiary = await _context.Beneficiary
            //    .Where(a => model.BeneficiaryIds.Contains(a.Id)).ToListAsync();
            //    if (deactivateBeneficiary.Count > 0)
            //    {
            //        foreach (Beneficiary item in deactivateBeneficiary)
            //        {
            //            item.IsActive = false;
            //        }
            //        _context.Beneficiary.UpdateRange(deactivateBeneficiary);
            //    }


            //}

            //await _context.SaveChangesAsync();
        }

    }
}
