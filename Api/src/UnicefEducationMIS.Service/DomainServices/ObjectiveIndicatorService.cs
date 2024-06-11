using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class ObjectiveIndicatorService : IObjectiveIndicatorService
    {

        private readonly IObjectiveIndicatorRepository _indicatorRepository;
        private readonly IMapper _mapper;

        public ObjectiveIndicatorService(IObjectiveIndicatorRepository indicatorRepository, IMapper mapper)
        {
            _indicatorRepository = indicatorRepository;
            _mapper = mapper;
        }

        public async Task<ObjectiveIndicatorViewModel> Create(ObjectiveIndicatorCreateViewModel model)
        {
            var entity = _mapper.Map<ObjectiveIndicator>(model);
            await _indicatorRepository.Insert(entity);
            return _mapper.Map<ObjectiveIndicatorViewModel>(entity);
        }

        public async Task Update(ObjectiveIndicatorUpdateViewModel model)
        {
            await ThrowIfNotFound(model.Id);
            var entity = _mapper.Map<ObjectiveIndicator>(model);
            await _indicatorRepository.Update(entity);
        }

        private async Task<ObjectiveIndicator> ThrowIfNotFound(long id)
        {
            var entity = await _indicatorRepository.GetById(id);
            if (entity == null)
            {
                throw new RecordNotFound(Messages.RecordNotFound);
            }
            return entity;
        }
    }
}
