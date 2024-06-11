using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class DataApprovalService : IDataApprovalService
    {

        private readonly IBeneficiaryDataCollectionStatusRepository _benefCollectionStatusRepo;
        private readonly IFacilityDataCollectionStatusRepository _facilityCollectionStatusRepo;
        private readonly IBeneficiaryDataCollectionRepository _beneficiaryDataCollectionRepository;
        private readonly IFacilityDataCollectionRepository _facilityDataCollectionRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly INotificationService _notificationService;

        public DataApprovalService(IBeneficiaryDataCollectionStatusRepository benefCollectionStatusRepo,
            IFacilityDataCollectionStatusRepository facilityCollectionStatusRepo,
            IBeneficiaryDataCollectionRepository beneficiaryDataCollectionRepository,
            IFacilityDataCollectionRepository facilityDataCollectionRepository,
            IBeneficiaryRepository beneficiaryRepository,
            INotificationService notificationService,
            IScheduleInstanceRepository scheduleInstanceRepository,
            IFacilityRepository facilityRepository)
        {
            _benefCollectionStatusRepo = benefCollectionStatusRepo;
            _facilityCollectionStatusRepo = facilityCollectionStatusRepo;
            _beneficiaryDataCollectionRepository = beneficiaryDataCollectionRepository;
            _facilityDataCollectionRepository = facilityDataCollectionRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _notificationService = notificationService;

            _scheduleInstanceRepository = scheduleInstanceRepository;
            _facilityRepository = facilityRepository;
        }

        public async Task ApproveBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //await _benefCollectionStatusRepo.ApproveBeneficiary(model);
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var lstDataCollection = await _benefCollectionStatusRepo.GetAll()
                                    .Where(a => a.InstanceId == model.InstanceId
                                    && model.BeneficiaryIds.Contains(a.BeneficiaryId))
                                    .ToListAsync();
            if (lstDataCollection.Count == 0)
            {
                throw new RecordNotFound(Messages.RecordNotFound);
            }

            foreach (BeneficiaryDataCollectionStatus item in lstDataCollection)
            {
                item.Status = CollectionStatus.Approved;
            }
            await _benefCollectionStatusRepo.UpdateRange(lstDataCollection);

        }

        public async Task ApproveInactiveBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //await _benefCollectionStatusRepo.ApproveDeactiveBeneficiary(model);
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var data = await _benefCollectionStatusRepo.GetAll()
                            .Where(a => model.BeneficiaryIds.Contains(a.BeneficiaryId)
                            && a.Status == CollectionStatus.Requested_Inactive
                            ).ToListAsync();
            if (data.Count == 0)
            {
                throw new RecordNotFound("No inactivate request found.");
            }
            data.ForEach(a =>
            {
                a.Status = CollectionStatus.Inactivated;
            });

            await _benefCollectionStatusRepo.UpdateRange(data);
        }

        public async Task RecollectBeneficiary(BeneficiaryApprovalViewModel model)
        {
            //TODO: beneficiary new schema
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var beneficiaryData = await _beneficiaryRepository.GetBeneficiaryBasicData(model.BeneficiaryIds, model.InstanceId);
            var items =
            _benefCollectionStatusRepo.GetAll()
                .Where(x => x.InstanceId == model.InstanceId 
                && model.BeneficiaryIds.Contains(x.BeneficiaryId))
                .ToList();

            if (items.Count == 0)
            {
                throw new RecordNotFound("No collected record found.");
            }
            var notCollectedStatusExist = items.Any(a => a.Status != CollectionStatus.Collected && a.Status != CollectionStatus.Requested_Inactive);
            if (notCollectedStatusExist)
            {
                throw new DomainException(Messages.CannotRecollectWhichNotCollected);
            }

            var users = items.Where(a => a.UpdatedBy != null).Select(a => (int)a.UpdatedBy).ToList();
            var schedule = await _scheduleInstanceRepository.GetById(model.InstanceId);
            items.ForEach(a =>
            {
                a.ScheduleInstance = null;
                a.Status = CollectionStatus.Recollect;
            });

            await _benefCollectionStatusRepo.UpdateRange(items);

            var notificationId = (int)NotificationTypeEnum.Recollect_Beneficiary;
            List<Notification> notifications = new List<Notification>();

            foreach (var item in items)
            {
                var beneficiary = beneficiaryData.Where(a => a.Id == item.BeneficiaryId).FirstOrDefault();
                var notification = new Notification()
                {
                    Data = @"{""InstanceId"":" + model.InstanceId + @",""BeneficiaryId"":" + item.BeneficiaryId + "}",
                    Details = $"Recollect beneficiary [{beneficiary.Name}] data of instance [{schedule.Title}]. ",
                    IsActed = false,
                    IsDeleted = false,
                    NotificationTypeId = notificationId,
                };
                notifications.Add(notification);
            }
            await _notificationService.Save(users, notifications);



        }

        public async Task ApproveFacility(FacilityApprovalViewModel model)
        {
            await _facilityCollectionStatusRepo.ApproveFacility(model);
            _beneficiaryRepository.AutoUpdateFacilityIndicators(model);
        }

        public async Task RecollectFacility(FacilityApprovalViewModel model)
        {
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var (items,users) = await _facilityCollectionStatusRepo.RecollectFacility(model);

            var schedule = await _scheduleInstanceRepository.GetById(model.InstanceId);
            var facilityData = await _facilityRepository.GetFacilityView()
                .Where(a => model.FacilityIds.Contains(a.Id) && a.InstanceId == model.InstanceId).ToListAsync();

            var notificationId = (int)NotificationTypeEnum.Recollect_Facility;
            List<Notification> notifications = new List<Notification>();

            foreach (var item in items)
            {
                var facility = facilityData.Where(a => a.Id == item.FacilityId).FirstOrDefault();
                var notification = new Notification()
                {
                    Data = @"{""InstanceId"":" + model.InstanceId + @",""FacilityId"":" + item.FacilityId + "}",
                    Details = $"Recollect facility [{facility.Name}] data of instance [{schedule.Title}]. ",
                    IsActed = false,
                    IsDeleted = false,
                    NotificationTypeId = notificationId,
                };
                notifications.Add(notification);
            }
            await _notificationService.Save(users, notifications);
        }

        public async Task<PagedResponse<BeneficiaryDataForApproval>> GetSubmittedBeneficiaries(SubmittedBeneficiaryQueryModel model)
        {
            var totalCollected = await _beneficiaryDataCollectionRepository.GetTotal(model);  //await beneficiaryRaw.GroupBy(a => a.EntityId).CountAsync();
            var deactivatedBeneficiaryQueryable = _beneficiaryDataCollectionRepository.GetDeactivateBeneficiaryData(model);
            var totalDeactivate = (model.CollectionStatus == null || model.CollectionStatus == Core.ValueObjects.CollectionStatus.Collected)
                                                  ? await deactivatedBeneficiaryQueryable.CountAsync()
                                                  : 0;

            var total = totalCollected + totalDeactivate;

            var subtructTotalCollected = (model.PageNo * model.PageSize) - totalCollected < 0 ?
                                            0 : (model.PageNo * model.PageSize) - totalCollected;
            int collectedDataTake = subtructTotalCollected <= model.PageSize ?
                                          model.PageSize - subtructTotalCollected : 0;
            int collectedDataSkip = model.Skip();
            int deactivateDataTake = subtructTotalCollected > 0 ?
                                        model.PageSize - collectedDataTake : 0;
            int deactivateDataSkip = subtructTotalCollected - model.PageSize <= 0 ?
                                        0 : subtructTotalCollected - model.PageSize;

            //var totalRecordsNeed = model.PageNo * model.PageSize;
            //var cTake = totalRecordsNeed <= totalCollected ? model.PageSize : 0;
            //var cSkip= model.Skip();

            //var dTake= totalRecordsNeed > totalCollected ?




            var beneficiaryIds = await _beneficiaryDataCollectionRepository.GetSubmittedBeneficiaryId(model, collectedDataSkip, collectedDataTake);
            var beneficiaryRaw = _beneficiaryDataCollectionRepository.GetSubmittedData(model);

            var beneficiaryData = await beneficiaryRaw.Where(a => beneficiaryIds.Contains(a.EntityId))
                //.Skip(model.Skip())
                //.Take(model.PageSize)
                .ToListAsync();

            var data = SetBeneficiaryViewModel(beneficiaryData);

            List<BeneficiaryViewModel> deactivateData = new List<BeneficiaryViewModel>();
            if (model.CollectionStatus == null || model.CollectionStatus == Core.ValueObjects.CollectionStatus.Collected)
            {
                var deactivateRaw = await deactivatedBeneficiaryQueryable.OrderBy(a => a.BeneficiaryName).Skip(deactivateDataSkip).Take(deactivateDataTake).ToListAsync();
                deactivateData = SetDeactivatedBeneficiaryViewModel(deactivateRaw);
            }

            BeneficiaryDataForApproval beneficiaryDataForApproval = new BeneficiaryDataForApproval()
            {
                CollectedData = data,
                DeactivateData = deactivateData
            };
            List<BeneficiaryDataForApproval> lstBeneficiaryData = new List<BeneficiaryDataForApproval>();
            lstBeneficiaryData.Add(beneficiaryDataForApproval);

            return await Task.FromResult(new PagedResponse<BeneficiaryDataForApproval>(lstBeneficiaryData, total, model.PageNo, model.PageSize));
        }

        private List<BeneficiaryViewModel> SetBeneficiaryViewModel(List<BeneficiaryRawViewModel> beneficiaryRawData)
        {
            var beneficiary = beneficiaryRawData.GroupBy(v =>
                        new
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
                            v.ImplemantationPartnerName
                        })
                        .Select(g => new BeneficiaryViewModel
                        {
                            EntityId = g.Key.EntityId,
                            UnhcrId = g.Key.UnhcrId,
                            FCNId = g.Key.FCNId,
                            BeneficiaryName = g.Key.BeneficiaryName,
                            FacilityId = g.Key.FacilityId,
                            FacilityName = g.Key.FacilityName,
                            BeneficiaryCampId = g.Key.BeneficiaryCampId,
                            BeneficiaryCampName = g.Key.BeneficiaryCampName,
                            FacilityCampId = g.Select(h => h.FacilityCampId).FirstOrDefault(),
                            FacilityCampName = g.Select(h => h.FacilityCampName).FirstOrDefault(),
                            BlockId = g.Key.BlockId,
                            BlockName = g.Key.BlockName,
                            SubBlockId = g.Key.SubBlockId,
                            SubBlockName = g.Key.SubBlockName,
                            ProgrammingPartnerId = g.Key.ProgrammingPartnerId,
                            ProgrammingPartnerName = g.Key.ProgrammingPartnerName,
                            ImplemantationPartnerId = g.Key.ImplemantationPartnerId,
                            ImplemantationPartnerName = g.Key.ImplemantationPartnerName,
                            IsActive = g.Select(h => h.IsActive).FirstOrDefault(),
                            IsApproved = g.Select(h => h.IsApproved).FirstOrDefault(),
                            CollectionStatus = g.Select(h => h.CollectionStatus).FirstOrDefault(),
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
                                           Values = m.GroupBy(n => n.PropertiesId).Select(b => b.Select(a => a.PropertiesValue).FirstOrDefault()).ToList(),
                                           ListItem = m.GroupBy(n => n.ListItemId).Select(c => new ListItemViewModel()
                                           {
                                               Id = (int)c.Key,
                                               Title = c.Select(o => o.ListItemTitle).FirstOrDefault(),
                                               Value = (int)c.Select(o => o.ListItemValue).FirstOrDefault()
                                           }).ToList()
                                       }).ToList()
                        }).ToList();
            return beneficiary;
        }

        private List<BeneficiaryViewModel> SetDeactivatedBeneficiaryViewModel(List<BeneficiaryRawViewModel> beneficiaryRawData)
        {
            var beneficiary = beneficiaryRawData.GroupBy(v =>
                        new
                        {
                            v.EntityId,
                            v.UnhcrId,
                            v.BeneficiaryName,
                            v.FCNId,
                            v.FacilityName,
                            v.FacilityId,
                            v.BeneficiaryCampId,
                            v.BeneficiaryCampName

                        })
                        .Select(g => new BeneficiaryViewModel
                        {
                            EntityId = g.Key.EntityId,
                            UnhcrId = g.Key.UnhcrId,
                            FCNId = g.Key.FCNId,
                            BeneficiaryName = g.Key.BeneficiaryName,
                            FacilityId = g.Key.FacilityId,
                            FacilityName = g.Key.FacilityName,
                            BeneficiaryCampId = g.Key.BeneficiaryCampId,
                            BeneficiaryCampName = g.Key.BeneficiaryCampName,

                        }).ToList();
            return beneficiary;
        }


        private List<FacilityViewModel> ConvertToFacilityViewModel(List<FacilityRawViewModel> facilityRaw)
        {
            var facility = facilityRaw.GroupBy(v =>
                        new
                        {
                            v.Id
                            ,
                            v.FacilityName,
                        })
                        .Select(g => new FacilityViewModel
                        {
                            Id = g.Key.Id,
                            FacilityName = g.Key.FacilityName,
                            FacilityCode = g.Select(x => x.FacilityCode).FirstOrDefault(),
                            CampName = g.Select(x => x.CampName).FirstOrDefault(),
                            CollectionStatus = g.Select(x => x.CollectionStatus).FirstOrDefault(),
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
                                           Values = m.GroupBy(n => n.PropertiesId).Select(o => o.Select(o => o.PropertiesValue).FirstOrDefault()).ToList(),
                                           ListItem = m.Where(n => n.ListItemId != null).GroupBy(n => n.ListItemId).Select(o => new ListItemViewModel()
                                           {
                                               Id = (long)o.Key,
                                               Title = o.Select(o => o.ListItemTitle).FirstOrDefault(),
                                               Value = (int)o.Select(o => o.ListItemValue).FirstOrDefault()
                                           }).ToList()
                                       }).ToList()
                        }).ToList();
            return facility;
        }

        public async Task<PagedResponse<FacilityViewModel>> GetSubmittedFacilities(SubmittedFacilityQueryModel model)
        {
            var facilityIds = await _facilityDataCollectionRepository.GetPaginatedFacilityId(model);
            var rawData = _facilityDataCollectionRepository.GetSubmittedData(model);

            var facilities = await rawData.Where(a => facilityIds.Contains(a.Id)).ToListAsync();

            var total = await _facilityDataCollectionRepository.GetTotal(model); //await rawData.GroupBy(x => x.Id).CountAsync();
            var mapped = ConvertToFacilityViewModel(facilities);
            var result = new PagedResponse<FacilityViewModel>(mapped, total, model.PageNo, model.PageSize);
            return await Task.FromResult(result);
        }



    }
}
