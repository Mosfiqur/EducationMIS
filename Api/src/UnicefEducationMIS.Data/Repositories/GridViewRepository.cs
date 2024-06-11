using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class GridViewRepository : IGridViewRepository
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public GridViewRepository(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }

        public async Task<GridView> Add(GridView gridView)
        {
            _context.Add(gridView);
            await _context.SaveChangesAsync();
            return gridView;
        }
        public async Task<GridView> Update(GridView gridView)
        {

            var gview = await _context.GridViews.Where(a => a.Id == gridView.Id).FirstOrDefaultAsync();
            if (gview != null)
            {
                gview.Name = gridView.Name;
                _context.GridViews.Update(gview);

                var removeGridDetails = await _context.GridViewDetails.Where(a => a.GridViewId == gridView.Id).ToListAsync();
                _context.GridViewDetails.RemoveRange(removeGridDetails);

                var gridDetails = gridView.GridViewDetails.Select(a => new GridViewDetails()
                {
                    Id = 0,
                    GridViewId = gview.Id,
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    ColumnOrder = a.ColumnOrder
                }).ToList();

                _context.GridViewDetails.AddRange(gridDetails);
            }


            await _context.SaveChangesAsync();
            return gridView;
        }
        public async Task<GridView> Delete(long gridViewId)
        {
            var gridView = await _context.GridViews.Where(a => a.Id == gridViewId).FirstOrDefaultAsync();
            _context.GridViews.Remove(gridView);
            var gridViewDetails = await _context.GridViewDetails.Where(a => a.GridViewId == gridViewId).ToListAsync();
            _context.GridViewDetails.RemoveRange(gridViewDetails);

            await _context.SaveChangesAsync();
            return gridView;
        }
        public async Task<GridViewDetails> AddColumnToView(long gridViewId, long entityDynamicColumnId)
        {

            var columnOrderList = await _context.GridViewDetails.Where(a => a.GridViewId == gridViewId)
                .Select(a => a.ColumnOrder).ToListAsync();
            var nextColumnOrder = columnOrderList.Count() > 0 ? columnOrderList.Max() + 1 : 1;

            var gridDetails = new GridViewDetails()
            {
                GridViewId = gridViewId,
                ColumnOrder = nextColumnOrder,
                EntityDynamicColumnId = entityDynamicColumnId,
                
            };

            _context.GridViewDetails.Add(gridDetails) ;

            await _context.SaveChangesAsync();
            return gridDetails;
        }
        public async Task<PagedResponse<GridViewViewModel>> GetAll(GridViewQueryModel baseQueryModel)
        {
            var gridView = _context.GridViews
                .Where(a => a.Name.Contains(baseQueryModel.SearchText) && a.EntityTypeId == baseQueryModel.EntityTypeId)
                .Select(a => new GridViewViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    EntityTypeId = a.EntityTypeId

                });

            var total = await gridView.CountAsync();
            var grid = await gridView.Skip(baseQueryModel.Skip()).Take(baseQueryModel.PageSize).ToListAsync();
            return new PagedResponse<GridViewViewModel>(grid, total, baseQueryModel.PageNo, baseQueryModel.PageSize);
        }

        public async Task<GridViewModel> GetById(long id)
        {
            var gridView = await _context.GridViews

                .Where(a => a.Id == id)
                .Select(a => new GridViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    EntityTypeId = a.EntityTypeId,
                    GridViewDetails = a.GridViewDetails.Select(b => new GridDetailsViewModel()
                    {
                        Id = b.Id,
                        Name = b.EntityDynamicColumn.Name,
                        ColumnOrder = b.ColumnOrder,
                        EntityDynamicColumnId = b.EntityDynamicColumnId
                    }).ToList()

                }).FirstOrDefaultAsync();
            return gridView;
        }

        public async Task<GridViewDetails> DeleteColumnToView(long gridViewId, long entityDynamicColumnId)
        {
           var gridViewDetails = await _context.GridViewDetails.FirstOrDefaultAsync(x => x.EntityDynamicColumnId == entityDynamicColumnId &&
                                                    x.GridViewId == gridViewId);
            
            _context.GridViewDetails.Remove(gridViewDetails);
            await _context.SaveChangesAsync();
            return gridViewDetails;
        }
    }
}
