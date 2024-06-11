using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class ListDataTypeRepository : IListDataTypeRepository
    {
        private readonly UnicefEduDbContext _context;
        private readonly IMapper _mapper;

        public ListDataTypeRepository(UnicefEduDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Delete(long id)
        {
            var listDataType = _context.ListDataType.Where(x => x.Id == id).FirstOrDefault();
            _context.ListDataType.Remove(listDataType);
            await _context.SaveChangesAsync();
        }

        public async Task<ListDataType> GetById(long id)
        {
            return await _context.ListDataType
                .Include(x => x.ListItems)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<PagedResponse<ListDataTypeViewModel>> GetPaginatedList(BaseQueryModel baseQueryModel)
        {
            var total = _context.ListDataType.Count();
            var columnLists = _context.ListDataType.Include(x => x.ListItems).AsQueryable();
            if (!(string.IsNullOrWhiteSpace(baseQueryModel.SearchText)))
            {
                columnLists = columnLists.Where(x => x.Name.Contains(baseQueryModel.SearchText));
            }
            
            var paginatedColumn = await columnLists.Skip(baseQueryModel.Skip())
                                      .Take(baseQueryModel.PageSize)
                                      .ToListAsync();


            var data = new List<ListDataTypeViewModel>();
            foreach (var item in paginatedColumn)
            {
                var result = new ListDataTypeViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    
                };
                result.ListItems = new List<ListItemViewModel>();
                foreach (var listItem in item.ListItems)
                {
                    result.ListItems.Add(
                    new ListItemViewModel()
                    {
                        Id = listItem.Id,
                        Title = listItem.Title,
                        Value = listItem.Value
                    });
                        
                    
                }
                data.Add(result);
            }
            return new PagedResponse<ListDataTypeViewModel>(data, total, baseQueryModel.PageNo, baseQueryModel.PageSize);
        }

        public async Task<ListDataType> Save(ListDataType columnList)
        {
            _context.ListDataType.Add(columnList);
            await _context.SaveChangesAsync();
            return _context.ListDataType.Where(x => x.Id == columnList.Id).FirstOrDefault();
        }

        public async Task Update(ListDataType columnList)
        {
            var idsForTheList =await _context.EntityDynamicColumn.Where(a => a.ColumnListId == columnList.Id).Select(a => a.Id).ToListAsync();
            bool isListExistInBeneficairyData =await _context.BeneficiaryDynamicCells.AnyAsync(a => idsForTheList.Contains(a.EntityDynamicColumnId));
            bool isListExistInFacilityData =await _context.FacilityDynamicCells.AnyAsync(a => idsForTheList.Contains(a.EntityDynamicColumnId));

            if(isListExistInBeneficairyData || isListExistInFacilityData)
            {
                throw new DomainException(Messages.ListAlreadyUsedException);
            }

            var OldcolumnList = _context.ListDataType.Where(x => x.Id == columnList.Id)
                .Include(x => x.ListItems)
                .FirstOrDefault();
            if(OldcolumnList != null)
            {
                OldcolumnList.Name = columnList.Name;
                OldcolumnList.ListItems.ToList().ForEach(x => OldcolumnList.ListItems.Remove(x));
                columnList.ListItems.ToList().ForEach(x => OldcolumnList.ListItems.Add(x));
            }
            _context.ListDataType.Update(OldcolumnList);
            await _context.SaveChangesAsync();
        }
    }
}
