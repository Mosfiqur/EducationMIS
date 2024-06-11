using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class FacilityDataCollectionStatusRepository : BaseRepository<FacilityDataCollectionStatus, long>, IFacilityDataCollectionStatusRepository
    {
        public FacilityDataCollectionStatusRepository(UnicefEduDbContext context) : base(context)
        {
        }

        public async Task ApproveFacility(FacilityApprovalViewModel model)
        {
            var (facilityDataCollectionStatuses, collectionStatuses) = 
                await FacilityDataCollectionStatuses(model);

            //if (collectionStatuses.First() != CollectionStatus.Collected)
            //    throw new DomainException(Messages.CannotApproveWhichNotCollected);

            foreach (var item in facilityDataCollectionStatuses)
            {
                item.Status = CollectionStatus.Approved;
            }
            _context.FacilityDataCollectionStatuses.UpdateRange(facilityDataCollectionStatuses);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<FacilityDataCollectionStatus>, List<int>)> RecollectFacility(FacilityApprovalViewModel model)
        {
            var (facilityDataCollectionStatuses, collectionStatuses) = 
                await FacilityDataCollectionStatuses(model);

            if (collectionStatuses.First() != CollectionStatus.Collected)
                throw new DomainException(Messages.CannotRecollectWhichNotCollected);
            var users= facilityDataCollectionStatuses.Where(a => a.UpdatedBy != null).Select(a => (int)a.UpdatedBy).ToList();
            foreach (var item in facilityDataCollectionStatuses)
            {
                item.Status = CollectionStatus.Recollect;
            }
            _context.FacilityDataCollectionStatuses.UpdateRange(facilityDataCollectionStatuses);
            await _context.SaveChangesAsync();
            return (facilityDataCollectionStatuses, users);
        }

        private async Task<(List<FacilityDataCollectionStatus>, CollectionStatus[])> FacilityDataCollectionStatuses(FacilityApprovalViewModel model)
        {
            var lstDataCollection = await _context.FacilityDataCollectionStatuses
                .Where(a => a.InstanceId == model.InstanceId && model.FacilityIds.Contains(a.FacilityId))
                .ToListAsync();
            if (lstDataCollection.Count == 0)
            {
                throw new RecordNotFound();
            }

            var collectionStatus = lstDataCollection.Select(x => x.Status).Distinct();
            var collectionStatuses = collectionStatus as CollectionStatus[] ?? collectionStatus.ToArray();
            if (collectionStatuses.Count() > 1)
                throw new DomainException(Messages.MultipleStatusSelected);
            return (lstDataCollection, collectionStatuses);
        }
    }
}
