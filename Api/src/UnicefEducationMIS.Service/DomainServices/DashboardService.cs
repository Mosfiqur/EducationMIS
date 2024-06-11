using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Specifications;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Dashboard;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IUserService _userService;
        private readonly IBudgetFrameworkRepository _budgetFrameworkRepository;
        private readonly IScheduleService _scheduleService;
        private readonly ICampRepository _campRepository;
        private readonly ICampCoordinateRepository _campCoordinateRepository;
        private readonly IEducationSectorPartnerRepository _espRepository;
        private readonly IUnionRepository _unionRepository;
        private readonly ITargetFrameworkRepository _targetFrameworkRepository;
        private readonly IBeneficiaryDataCollectionStatusRepository _beneficiaryDataCollectionStatusRepository;

        public DashboardService(
                IBeneficiaryRepository beneficiaryRepository,
                IFacilityRepository facilityRepository,
                IUserService userService,
                IBudgetFrameworkRepository budgetFrameworkRepository, 
                IScheduleService scheduleService, 
                ICampRepository campRepository, 
                ICampCoordinateRepository campCoordinateRepository, 
                IEducationSectorPartnerRepository espRepository, 
                IUnionRepository unionRepository, ITargetFrameworkRepository targetFrameworkRepository
            ,IBeneficiaryDataCollectionStatusRepository beneficiaryDataCollectionStatusRepository)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _facilityRepository = facilityRepository;
            _userService = userService;
            _budgetFrameworkRepository = budgetFrameworkRepository;
            _scheduleService = scheduleService;
            _campRepository = campRepository;
            _campCoordinateRepository = campCoordinateRepository;
            _espRepository = espRepository;
            _unionRepository = unionRepository;
            _targetFrameworkRepository = targetFrameworkRepository;
            _beneficiaryDataCollectionStatusRepository = beneficiaryDataCollectionStatusRepository;
        }

        public async Task<TotalCountsViewModel> GetTotalCounts()
        {
            var maxBenInstanceId = await _scheduleService.GetMaxBeneficiaryInstanceId();
            var maxFacInstanceId = await _scheduleService.GetMaxFacilityInstanceId();

            var beneficiaryCount = await _beneficiaryRepository.Count(
                x => x.BeneciaryDataCollectionStatuses.Any(
                    y => y.InstanceId == maxBenInstanceId && y.Status != CollectionStatus.Deleted && y.Status != CollectionStatus.Inactivated)
                );
            var facilityCount = await _facilityRepository.Count(
                x=> x.FacilityDataCollectionStatus.Any(
                    y => y.InstanceId == maxFacInstanceId && y.Status != CollectionStatus.Deleted)
                );
            var teacherCount = await _userService.Count(new TeacherUserSpecification());
            var projectCount = await _budgetFrameworkRepository.Count(Specification<BudgetFramework>.All);


            return new TotalCountsViewModel
            {
                TotalBeneficiries = beneficiaryCount,
                TotalFacilities = facilityCount,
                TotalProjects = projectCount,
                TotalTeachers = teacherCount
            };
        }

        public async Task<List<GapMapViewModel>> GetGapMaps(GapMapQueryModel query)
        {
            var maxBenInstanceId = await _scheduleService.GetMaxBeneficiaryInstanceId();
            var dateRanges =
            query.AgeGroupIds.Select(x => new DateRange(x)).ToList();
            
            Expression<Func<TargetFramework, bool>> filter = x =>
                 (query.CampIds.Contains((int)x.CampId)) &&
                 (query.AgeGroupIds.Select(x => (int)x).Contains(x.AgeGroupId)) &&
                 (query.Genders.Contains(x.Gender)) &&
                 (query.Years.Contains(x.StartYear) || query.Years.Contains(x.EndYear));

            Expression<Func<BeneficiaryView, bool>> outReachFilter = x =>
                     (query.Genders.Contains(x.Sex)) && x.InstanceId == maxBenInstanceId;

            var q =
                    _campRepository.GetAll()
                        .Include(x => x.CampCoordinates)
                        .Select(x => new
                        {
                            CampId = x.Id,
                            CampName = x.Name,
                            ShapeCoordinates = x.CampCoordinates.Select(coordinate => new ShapeCoordinate()
                            {
                                Latitude = coordinate.Latitude,
                                Longitude = coordinate.Longitude,
                                SequenceNo = coordinate.SequenceNumber
                            }).ToList()
                        })
                        .ToList();


            var outReach = await _beneficiaryRepository.GetBeneficiaryView()
                .Where(outReachFilter)
                .ToListAsync();
            //var bdcs =await _beneficiaryDataCollectionStatusRepository.GetAll().Where(a => a.InstanceId == maxBenInstanceId).ToListAsync();
            //outReach = (from o in outReach
            //            join s in bdcs on o.Id equals s.BeneficiaryId
            //            where s.Status == CollectionStatus.Approved
            //            select o).ToList();

            var list = outReach.GroupBy(g => g.BeneficiaryCampId).Select(ben => new
            {
                CampId = ben.Key,
                DateOfBirths = ben.ToList()
            }).ToList();

            var campWithCoordinates = (
                from cc in q
                join ben in list on cc.CampId equals ben.CampId into benGroup
                from beneficiary in benGroup.DefaultIfEmpty()
                select new GapMapViewModel
                {
                    CampId = cc.CampId,
                    CampName = cc.CampName,
                    ShapeCoordinates = cc.ShapeCoordinates,
                    Outreach = beneficiary?.DateOfBirths.Count(date => dateRanges.Contains(date.DateOfBirth)) ?? 0
                }).ToList();

            var data =
                campWithCoordinates
                    .GroupJoin(_targetFrameworkRepository.GetAll().Where(filter),
                        camp => camp.CampId,
                        fw => fw.CampId,
                        (camp, fwGroup) => new { camp, fwGroup })
                    .SelectMany(@t1 => @t1.fwGroup.DefaultIfEmpty(),
                        (@t1, framework) => new GapMapViewModel
                        {
                            CampId = @t1.camp.CampId,
                            PeopleInNeed = framework?.PeopleInNeed ?? 0,
                            Target = framework?.Target ?? 0
                        })
                    .ToList();

            var targets =
                data.GroupBy(x => x.CampId)
                    .Select(g => new GapMapViewModel()
                    {
                        CampId = g.Key,
                        PeopleInNeed = g.Sum(x => x.PeopleInNeed),
                        Target = g.Sum(x => x.Target)
                    }).ToList();


            var result = campWithCoordinates
                .GroupJoin(targets, camp => camp.CampId, target => target.CampId, (camp, t) => new
                {
                    camp,
                    t
                })
                .SelectMany(x => x.t.DefaultIfEmpty(), (camp, target) => new GapMapViewModel()
                {
                    CampId = camp.camp.CampId,
                    CampName = camp.camp.CampName,
                    ShapeCoordinates = camp.camp.ShapeCoordinates.OrderBy(x => x.SequenceNo).ToList(),
                    Outreach = camp.camp.Outreach,
                    Target = target.Target,
                    PeopleInNeed = target.PeopleInNeed,

                }).ToList();
            // Exclude the unselected camp from calculating the outreach, gap, and target
            result.Where(x => !query.CampIds.Contains(x.CampId)).ToList()
                .ForEach(x => x.Outreach = 0);

            result.ForEach(x =>
            {
                x.Gap = x.Target - x.Outreach;
            });

            result.AddGapColorGradients();
            return result;
        }

        public async Task<List<LcMapViewModel>> GetLcCoordinates(LcCoordinateQueryModel query)
        {
            
            Expression<Func<FacilityView, bool>> filter = facility =>
                query.SelectedCamps.Contains(facility.CampId.Value) &&
                query.SelectedIPs.Contains(facility.ImplementationPartnerId) &&
                query.SelectedPPs.Contains(facility.ProgramPartnerId) &&
                query.SelectedTPs.Contains(facility.TargetedPopulation) &&
                query.SelectedUpazilas.Contains(facility.UpazilaId) &&
                query.SelectedUnions.Contains(facility.UnionId);



            List<LcMapViewModel> lcMaps;

            lcMaps = await GetRealData(filter);
            //lcMaps = await GetRealData(filter);

            lcMaps.ForEach(m => m.CampCoordinates = m.CampCoordinates.OrderBy(x => x.SequenceNo).ToList());
            lcMaps.ForEach(x => x.CalculateRadius());

            return lcMaps;
        }

        private async Task<List<LcMapViewModel>> GetRealData(Expression<Func<FacilityView, bool>> filter)
        {
            var campCoordsQuery = await (
                from camp in _campRepository.GetAll()
                join coord in _campCoordinateRepository.GetAll() on camp.Id equals coord.CampId
                select new
                {
                    CampId = camp.Id,
                    CampName = camp.Name,
                    CampLat = coord.Latitude,
                    CampLng = coord.Longitude,
                    Sequence = coord.SequenceNumber
                }).ToListAsync();


            var campCoords =
                campCoordsQuery.GroupBy(x => new { x.CampId, x.CampName })
                .Select(g => new LcMapViewModel
                {
                    CampId = g.Key.CampId,
                    CampName = g.Key.CampName,
                    CampCoordinates = g.Select(x => new ShapeCoordinate
                    {
                        Latitude = x.CampLat,
                        Longitude = x.CampLng,
                        SequenceNo = x.Sequence
                    }).OrderBy(x => x.SequenceNo).ToList()
                }).ToList();

            
            var lcBeneficiaries = await (
                from facility in _facilityRepository.GetFacilityView()
                    .Where(filter)
                    .Where(x => !string.IsNullOrEmpty(x.Latitude) && !string.IsNullOrEmpty(x.Longitude))
                join pp in _espRepository.GetAll() on facility.ProgramPartnerId equals pp.Id
                join ip in _espRepository.GetAll() on facility.ImplementationPartnerId equals ip.Id
                join un in _unionRepository.GetAll() on facility.UnionId equals un.Id
                join up in _unionRepository.GetAll() on facility.UpazilaId equals up.Id
                join beneficiary in _beneficiaryRepository.GetBeneficiaryView() on facility.Id equals beneficiary.FacilityId
                    into benefGroup
                from b in benefGroup.DefaultIfEmpty()

                select new
                {
                    facility.CampId,
                    FacilityId = facility.Id,
                    FacilityName = facility.Name,
                    facility.FacilityCode,
                    facility.Latitude,
                    facility.Longitude,
                    facility.TargetedPopulation,
                    IpName = ip.PartnerName,
                    PpName = pp.PartnerName,
                    UpazilaName = up.Name,
                    UnionName = un.Name,
                    Beneficiary = b.Id
                }).ToListAsync();


            var learningCenters =
            lcBeneficiaries.GroupBy(x => new
            {
                x.CampId,
                x.FacilityId,
                x.FacilityName,
                x.FacilityCode,
                x.Latitude,
                x.Longitude,
                x.TargetedPopulation,
                x.IpName,
                x.PpName,
                x.UpazilaName,
                x.UnionName
            })
            .Select(g => new LearningCenterViewModel
            {
                CampId = (int)g.Key.CampId,
                FacilityId = g.Key.FacilityId,
                FacilityName = g.Key.FacilityName,
                FacilityCode = g.Key.FacilityCode,
                Position = new Coordinate(g.Key.Latitude, g.Key.Longitude)
                {
                    Latitude = decimal.TryParse(g.Key.Latitude, out var val) ? val : 0,
                    Longitude = decimal.TryParse(g.Key.Longitude, out var val2) ? val2 : 0
                },

                TargetedPopulation = g.Key.TargetedPopulation,
                IpName = g.Key.IpName,
                PpName = g.Key.PpName,
                UpazilaName = g.Key.UpazilaName,
                UnionName = g.Key.UnionName,
                NumberOfBeneficiaries = g.Count(x => x.Beneficiary > 0)
            })
            .Where(x => x.Position.IsValid() && x.Position.WithinTheBound())
            .GroupBy(x => x.CampId)
            .Select(g => new LcMapViewModel
            {
                CampId = g.Key,
                LearningCenters = g.ToList()
            })
            .ToList();

            var result = (
            from c in campCoords
            join lc in learningCenters on c.CampId equals lc.CampId into lcGroup
            from l in lcGroup.DefaultIfEmpty()
            select new LcMapViewModel
            {
                CampId = c.CampId,
                CampName = c.CampName,
                LearningCenters = l?.LearningCenters ?? new List<LearningCenterViewModel>(),
                CampCoordinates = c.CampCoordinates
            }).ToList();

            result.ForEach(x => x.LearningCenters.ForEach(y => y.CampName = x.CampName));
            return result;
        }

        private async Task<List<LcMapViewModel>> GetDummyData(Expression<Func<FacilityView, bool>> filter)
        {
            throw new NotImplementedException();
            
            //var camps = await _campRepository.GetAll()
            //    .Include(x => x.CampCoordinates)
            //    .ToListAsync();



            //var campList = new List<Camp>();

            //for (int i = 0; i < camps.Count; i++)
            //{
            //    var facilities = new List<Facility>();


            //    var camp = camps[i];

            //    var minLat = camp.CampCoordinates.Select(x => x.Latitude).Min();
            //    var minLng = camp.CampCoordinates.Select(x => x.Longitude).Min();

            //    var maxLat = camp.CampCoordinates.Select(x => x.Latitude).Max();
            //    var maxLng = camp.CampCoordinates.Select(x => x.Longitude).Max();

            //    var center = new Coordinate()
            //    {
            //        Latitude = (minLat + (maxLat - minLat) / 2),
            //        Longitude = (minLng + (maxLng - minLng) / 2)
            //    };




            //    var rand = new Random();

            //    for (int j = 1; j < 10; j++)
            //    {
            //        var item = new Facility
            //        {
            //            Name = "Facilit " + (j + 1 * i + 1).ToString(),
            //            CampId = (j + 1 * i + 1),
            //            TargetedPopulation = j > 5 ? TargetedPopulation.Host_Communities : TargetedPopulation.Refugee_Communities,
            //        };

            //        var rand1 = (decimal)rand.NextDouble() * (maxLat - minLat) + minLat;
            //        var rand2 = (decimal)rand.NextDouble() * (maxLng - minLng) + minLng;


            //        item.Latitude = ((decimal)rand.NextDouble() * (maxLat - minLat) + minLat).ToString();
            //        item.Longitude = ((decimal)rand.NextDouble() * (maxLng - minLng) + minLng).ToString();
            //        facilities.Add(item);
            //    }

            //    facilities.ForEach(f =>
            //    {
            //        camp.Facilities.Add(f);
            //    });

            //    campList.Add(camp);
            //}


            //var list =
            //camps.SelectMany(camp =>
            //{
            //    var rand2 = new Random();
            //    return
            //    camp.Facilities.Select(facility => new LearningCenterViewModel
            //    {
            //        CampId = camp.Id,
            //        CampName = camp.Name,
            //        FacilityId = facility.Id,
            //        FacilityName = facility.Name,
            //        NumberOfBeneficiaries = rand2.Next(50, 400),
            //        TargetedPopulation = facility.TargetedPopulation,
            //        Position = new Coordinate
            //        {
            //            Latitude = decimal.Parse(facility.Latitude),
            //            Longitude = decimal.Parse(facility.Longitude)
            //        }
            //    }).ToList();
            //}).ToList();


            //return campList.Select(camp =>
            // {
            //     var rand2 = new Random();
            //     return
            //     new LcMapViewModel
            //     {

            //         CampCoordinates = camp.CampCoordinates.Select(x =>
            //                 new ShapeCoordinate
            //                 {
            //                     Latitude = x.Latitude,
            //                     Longitude = x.Longitude,
            //                     SequenceNo = x.SequenceNumber
            //                 }).ToList(),
            //         LearningCenters = camp.Facilities
            //         .Where(filter.Compile().Invoke)
            //         .Select(facility => new LearningCenterViewModel
            //         {
            //             CampId = camp.Id,
            //             CampName = camp.Name,
            //             FacilityId = facility.Id,
            //             FacilityName = facility.Name,
            //             NumberOfBeneficiaries = rand2.Next(50, 400),
            //             TargetedPopulation = facility.TargetedPopulation,
            //             Position = new Coordinate
            //             {
            //                 Latitude = decimal.Parse(facility.Latitude),
            //                 Longitude = decimal.Parse(facility.Longitude)
            //             }
            //         }).ToList()
            //     };
            // }).ToList();

        }

        

    }
}
