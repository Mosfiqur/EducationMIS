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
    public class GridViewService : IGridViewService
    {
        private readonly IGridViewRepository _gridViewRepository;
        private readonly IMapper _mapper;
        public GridViewService(IGridViewRepository gridViewRepository, IMapper mapper)
        {
            _gridViewRepository = gridViewRepository;
            _mapper = mapper;
        }

        public async Task<GridView> Add(GridViewViewModel gridView)
        {
            var beneficiaryGridView = _mapper.Map<GridView>(gridView);
            //foreach (var item in gridView.GridViewDetails)
            //{
            //    beneficiaryGridView.GridViewDetails.Add(_mapper.Map<GridViewDetails>(item));
            //}
            return await _gridViewRepository.Add(beneficiaryGridView);
        }

        public async Task<GridView> Update(GridViewViewModel gridView)
        {
            var beneficiaryGridView = _mapper.Map<GridView>(gridView);
            //foreach (var item in gridView.GridViewDetails)
            //{
            //    beneficiaryGridView.GridViewDetails.Add(_mapper.Map<GridViewDetails>(item));
            //}
            return await _gridViewRepository.Update(beneficiaryGridView);
        }
        public async Task<GridView> Delete(long gridViewId)
        {
            return await _gridViewRepository.Delete(gridViewId);
        }

        public async Task<PagedResponse<GridViewViewModel>> GetAll(GridViewQueryModel baseQueryModel)
        {
            return await _gridViewRepository.GetAll(baseQueryModel);
        }

        public async Task<GridViewModel> GetById(long id)
        {
            return await _gridViewRepository.GetById(id);
        }

        public async Task<GridViewDetails> AddColumnToView(long gridViewId, long entityDynamicColumnId)
        {
            return await _gridViewRepository.AddColumnToView(gridViewId,entityDynamicColumnId);
        }

        public async Task<GridViewDetails> DeleteColumnToView(long gridViewId, long entityDynamicColumnId)
        {
            return await _gridViewRepository.DeleteColumnToView(gridViewId, entityDynamicColumnId);
        }
    }
}
