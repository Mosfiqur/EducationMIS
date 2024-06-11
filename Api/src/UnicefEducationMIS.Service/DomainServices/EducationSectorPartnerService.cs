using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class EducationSectorPartnerService : IEducationSectorPartnerService
    {

        private readonly IEducationSectorPartnerRepository _repository;
        private readonly IMapper _mapper;
        public EducationSectorPartnerService(IEducationSectorPartnerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;            
        }

        public async Task<IEnumerable<EduSectorPartnerViewModel>> GetAll()
        {
            var data = await _repository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<EduSectorPartnerViewModel>>(data);
        }
    }
}
