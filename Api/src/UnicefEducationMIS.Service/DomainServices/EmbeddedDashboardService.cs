using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class EmbeddedDashboardService : IEmbeddedDashboardService
    {
        private readonly IMapper _mapper;
        private readonly IEmbeddedDashboardRepository _embaddedDashboardRepository;

        public EmbeddedDashboardService(IEmbeddedDashboardRepository embaddedDashboardRepository
            , IMapper mapper)
        {
            _embaddedDashboardRepository = embaddedDashboardRepository;
            _mapper = mapper;
        }

        public async Task Save(EmbeddedDashboardViewModel embeddedDashboardViewModel)
        {
            var embeddedDashboard = _mapper.Map<EmbeddedDashboard>(embeddedDashboardViewModel);
            await _embaddedDashboardRepository.Insert(embeddedDashboard);
        }

        public async Task Update(EmbeddedDashboardViewModel embeddedDashboardViewModel)
        {
            var embeddedDashboard = _mapper.Map<EmbeddedDashboard>(embeddedDashboardViewModel);
            await _embaddedDashboardRepository.Update(embeddedDashboard);
        }
        public async Task Delete(int id)
        {
            await _embaddedDashboardRepository.Delete(id);
        }

        public async Task<List<EmbeddedDashboardViewModel>> GetAll()
        {
            var data = await _embaddedDashboardRepository.GetAll().ToListAsync();
            return  _mapper.Map<List<EmbeddedDashboardViewModel>>(data);
        }

    }
}
