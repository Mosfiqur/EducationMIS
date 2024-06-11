using AutoMapper;
using AutoMapper.Internal;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.QueryModel.Common;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Common;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class CommonService : ICommonService
    {
        private ICampRepository _campRepository;
        private IBlockRepository _blockRepository;
        private ISubBlockRepository _subBlockRepository;
        private IDistrictRepository _districtRepository;
        private IUpazilaRepository _upazilaRepository;
        private IUnionRepository _unionRepository;
        private IAgeGroupRepository _ageGroupRepository;
        private IReportingFrequencyRepository _reportingFrequencyRepository;
        public CommonService(
            ICampRepository campRepository,
            IBlockRepository blockRepository,
            ISubBlockRepository subBlockRepository ,
            IUnionRepository unionRepository,
            IUpazilaRepository upazilaRepository,
            IDistrictRepository districtRepository,
            IReportingFrequencyRepository reportingFrequencyRepository,
            IAgeGroupRepository ageGroupRepository)
        {
            _campRepository = campRepository;
            _blockRepository = blockRepository;
            _subBlockRepository = subBlockRepository;
            _unionRepository = unionRepository;
            _upazilaRepository = upazilaRepository;
            _districtRepository = districtRepository;
            _ageGroupRepository = ageGroupRepository;
            _reportingFrequencyRepository = reportingFrequencyRepository;
        }

        public async Task<PagedResponse<AgeGroupViewModel>> GetAgeGroups(BaseQueryModel model)
        {
            var ageGroups = _ageGroupRepository.GetAll();
            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                ageGroups = ageGroups.Where(x => x.Name.Contains(model.SearchText));

            var total = await ageGroups.CountAsync();
            var data = await ageGroups.Skip(model.Skip()).Take(model.PageSize)
                                      .Select(x => new AgeGroupViewModel()
                                      {
                                          Id = x.Id,
                                          Name = x.Name
                                      }).ToListAsync();

            return new PagedResponse<AgeGroupViewModel>(data, total, model.PageNo, model.PageSize);

        }

        public async Task<PagedResponse<ReportingFrequencyViewModel>> GetReportingFrequencies(BaseQueryModel model)
        {
            var reportingFrequencies = _reportingFrequencyRepository.GetAll();
            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                reportingFrequencies = reportingFrequencies.Where(x => x.Name.Contains(model.SearchText));

            var total = await reportingFrequencies.CountAsync();
            var data = await reportingFrequencies.Skip(model.Skip()).Take(model.PageSize)
                                      .Select(x => new ReportingFrequencyViewModel()
                                      {
                                          Id = x.Id,
                                          Name = x.Name
                                      }).ToListAsync();

            return new PagedResponse<ReportingFrequencyViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<BlockViewModel>> GetBlocks(BlockQueryModel model)
        {
            //var total = await _blockRepository.GetAll().CountAsync();

            var blocks = _blockRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                blocks = blocks.Where(x => x.Name.Contains(model.SearchText));

            if (model.CampId != null)
                blocks = blocks.Where(x => x.CampId == model.CampId).AsQueryable();
            var total = await blocks.CountAsync();
            var data = await blocks.Skip(model.Skip()).Take(model.PageSize)
                                            .Select(x => new BlockViewModel()
                                            {
                                                Id = x.Id,
                                                Code = x.Code,
                                                Name = x.Name,
                                                CampId = x.CampId
                                            })
                                            .ToListAsync();


            return new PagedResponse<BlockViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<CampViewModel>> GetCamps(CampQueryModel model)
        {
            //var total = await _campRepository.GetAll().CountAsync();

            var camps = _campRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                camps = camps.Where(x => x.Name.Contains(model.SearchText));

            if (model.UnionId != null)
                camps = camps.Where(x => x.UnionId == model.UnionId).AsQueryable();
            var total = await camps.CountAsync();
            var data = await camps.Skip(model.Skip()).Take(model.PageSize)
                                    .Select(x => new CampViewModel()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        NameAlias = x.NameAlias,
                                        SSID = x.SSID,
                                        UnionId = x.UnionId
                                    })
                                    .ToListAsync();


            return new PagedResponse<CampViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<DistrictViewModel>> GetDistricts(BaseQueryModel model)
        {
            //var total = await _districtRepository.GetAll().CountAsync();

            var districts = _districtRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                districts = districts.Where(x => x.Name.Contains(model.SearchText));
            var total = await districts.CountAsync();
            var data = await districts.Skip(model.Skip()).Take(model.PageSize)
                                            .Select(x => new DistrictViewModel()
                                            {
                                                Id = x.Id,
                                                Name = x.Name
                                            })
                                            .ToListAsync();

            return new PagedResponse<DistrictViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<SubBlockViewModel>> GetSubBlocks(SubBlockQueryModel model)
        {
            //var total = await _subBlockRepository.GetAll().CountAsync();

            var subBlocks = _subBlockRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                subBlocks = subBlocks.Where(x => x.Name.Contains(model.SearchText));

            if (model.BlockId != null)
                subBlocks = subBlocks.Where(x => x.BlockId == model.BlockId).AsQueryable();
            var total = await subBlocks.CountAsync();
            var data = await subBlocks.Skip(model.Skip()).Take(model.PageSize)
                                            .Select(x => new SubBlockViewModel()
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                BlockId = x.BlockId
                                            })
                                            .ToListAsync();

            return new PagedResponse<SubBlockViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<UnionViewModel>> GetUnions(UnionQueryModel model)
        {
            //var total = await _unionRepository.GetAll().CountAsync();

            var unions = _unionRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                unions = unions.Where(x => x.Name.Contains(model.SearchText));

            if (model.UpazilaId != null)
                unions = unions.Where(x => x.UpazilaId == model.UpazilaId).AsQueryable();
            var total = await unions.CountAsync();
            var data = await unions.Skip(model.Skip()).Take(model.PageSize)
                                            .Select(x => new UnionViewModel()
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                UpazilaId = x.UpazilaId
                                            })
                                            .ToListAsync();

            return new PagedResponse<UnionViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<UpazilaViewModel>> GetUpazilas(UpazilaQueryModel model)
        {
            //var total = await _upazilaRepository.GetAll().CountAsync();

            var upazilas = _upazilaRepository.GetAll();

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                upazilas = upazilas.Where(x => x.Name.Contains(model.SearchText));

            if (model.DistrictId != null)
                upazilas = upazilas.Where(x => x.DistrictId == model.DistrictId).AsQueryable();
            var total = await upazilas.CountAsync();
            var data = await upazilas.Skip(model.Skip()).Take(model.PageSize)
                                            .Select(x => new UpazilaViewModel()
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                DistrictId = x.DistrictId
                                            })
                                            .ToListAsync();

            return new PagedResponse<UpazilaViewModel>(data, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<CampBlockSubBlockViewModel>> GetCampsWithBlockSubBlock(BaseQueryModel model)
        {
            var total = await _campRepository.GetAll().CountAsync();
            var campBlockSubBlocks = await _campRepository.GetAll()
                                                    .Include(x => x.Blocks)
                                                    .ThenInclude(y => y.SubBlocks)
                                                    .Skip(model.Skip()).Take(model.PageSize)
                                                    .Select(x => new CampBlockSubBlockViewModel()
                                                    {
                                                        Camp = new CampViewModel()
                                                        {
                                                            Id = x.Id,
                                                            Name = x.Name,
                                                            NameAlias = x.NameAlias,
                                                            SSID = x.SSID,
                                                            UnionId = x.UnionId
                                                        },
                                                        Blocks = x.Blocks.Select(x => new BlockViewModel() {
                                                            Id = x.Id,
                                                            Code = x.Code,
                                                            Name = x.Name,
                                                            CampId = x.CampId
                                                        }).ToList(),
                                                        SubBlocks = x.Blocks.SelectMany(y => y.SubBlocks,(y,z) => new SubBlockViewModel()
                                                        {
                                                            BlockId = z.BlockId,
                                                            Id = z.Id,
                                                            Name = z.Name
                                                        }).ToList()
                                                        //SubBlocks = x.Blocks.Select(y => new SubBlockViewModel()
                                                        //{
                                                        //    Id = y.SubBlocks.Select(z => z.Id).FirstOrDefault(),
                                                        //    Name = y.SubBlocks.Select(z => z.Name).FirstOrDefault(),
                                                        //    BlockId = y.SubBlocks.Select(z => z.BlockId).FirstOrDefault()
                                                        //}).ToList()
                                                    })
                                                    //.Select(x => x.Blocks.Select(x => x.SubBlocks))
                                                    .ToListAsync();

            return new PagedResponse<CampBlockSubBlockViewModel>(campBlockSubBlocks, total, model.PageNo, model.PageSize);
        }
    }
}
