using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class ListDataTypeService : IListDataTypeService
    {
        private readonly IListDataTypeRepository _listDataTypeRepository;
        private readonly IMapper _mapper;

        public ListDataTypeService(IListDataTypeRepository listDataTypeRepo,IMapper mapper)
        {
            _listDataTypeRepository = listDataTypeRepo;
            _mapper = mapper;
        }

        public async Task Delete(long columnListId)
        {
            await _listDataTypeRepository.Delete(columnListId);
        }

        public async Task<PagedResponse<ListDataTypeViewModel>> GetAll(BaseQueryModel baseQueryModel)
        {
            return await _listDataTypeRepository.GetPaginatedList(baseQueryModel);
        }

        public async Task<ListDataType> Add(ListDataTypeCreateViewModel createColumnListView)
        {
            var columnList = _mapper.Map<ListDataType>(createColumnListView);
            return await _listDataTypeRepository.Save(columnList);
        }

        public async Task Update(ListDataTypeUpdateViewModel aListDataType)
        {
            var entity = _mapper.Map<ListDataType>(aListDataType);
            await _listDataTypeRepository.Update(entity);
        }

        public async Task<ListDataTypeViewModel> GetById(long id)
        {
            return _mapper.Map<ListDataTypeViewModel>(await _listDataTypeRepository.GetById(id));
        }
    }
}
