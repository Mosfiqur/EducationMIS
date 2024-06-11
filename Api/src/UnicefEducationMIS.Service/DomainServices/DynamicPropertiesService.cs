using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace UnicefEducationMIS.Service.DomainServices
{
    public class DynamicPropertiesService : IDynamicPropertiesService
    {
        private readonly IDynamicColumnRepositories _dynamicPropertiesRepositories;
        private readonly IListDataTypeRepository _columnListRepository;
        private readonly IMapper _mapper;

        public DynamicPropertiesService(IDynamicColumnRepositories dynamicPropertiesRepositories, IMapper mapper, IListDataTypeRepository columnListRepository)
        {
            _dynamicPropertiesRepositories = dynamicPropertiesRepositories;
            _mapper = mapper;
            _columnListRepository = columnListRepository;
        }

        public async Task<EntityDynamicColumn> SaveDynamicColumn(DynamicColumnViewModel dynamicColumnViewModel)
        {
            var isAlreadyExists = await _dynamicPropertiesRepositories.GetAll()
                .AnyAsync(x => dynamicColumnViewModel.ColumnName.ToLower().Trim() == x.Name.ToLower().Trim());
            if (isAlreadyExists)
            {
                throw new DomainException(Messages.ColumnNameAlreadyExists);
            }
            
            var entityDynamicColumn = _mapper.Map<EntityDynamicColumn>(dynamicColumnViewModel);

            if (dynamicColumnViewModel.ColumnList != null && dynamicColumnViewModel.ColumnListId != null)
            {
                var columnList = await _columnListRepository.Save(entityDynamicColumn.ColumnList);
                entityDynamicColumn.ColumnListId = columnList.Id;
            }

            return await _dynamicPropertiesRepositories.SaveDynamicColumn(entityDynamicColumn);
        }

        public Task<EntityDynamicColumn> SaveDynamicColumns(List<DynamicColumnViewModel> columns)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumn(DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            return await _dynamicPropertiesRepositories.GetPaginatedColumn(dynamicPropertiesBySearchModel);
        }
        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedNumericColumn(DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            return await _dynamicPropertiesRepositories.GetPaginatedNumericColumn(dynamicPropertiesBySearchModel);
        }
        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumnForIndicator(DynamicPropertiesByInstanceIdQueryParam model)
        {
            return await _dynamicPropertiesRepositories.GetPaginatedColumnForIndicator(model);
        }

        public async Task<DynamicColumnViewModel> GetById(long id)
        {
            var data =await _dynamicPropertiesRepositories.GetAll()
                .Include(a => a.ColumnList)
                .ThenInclude(a=>a.ListItems)
                .Where(a=>a.Id==id)
                .Select(DynamicColumnViewModel.FromModel)
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task DeleteDynamicColumn(long entityDynamicColumnId)
        {
            var entityDynamicColumn = await _dynamicPropertiesRepositories.GetAll().Where(x => x.Id == entityDynamicColumnId).FirstOrDefaultAsync();
            entityDynamicColumn.IsDeleted = true;
            await _dynamicPropertiesRepositories.Update(entityDynamicColumn);
        }
    }
}