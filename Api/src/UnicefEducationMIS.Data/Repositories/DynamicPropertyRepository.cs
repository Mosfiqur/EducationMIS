using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class DynamicPropertyRepository : BaseRepository<EntityDynamicColumn, long>, IDynamicColumnRepositories
    {
        private readonly IMapper _mapper;
        public DynamicPropertyRepository(UnicefEduDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<EntityDynamicColumn> SaveDynamicColumn(EntityDynamicColumn entityDynamicColumn)
        {
            var lastData = await _context.EntityDynamicColumn.Where(a => a.Id == entityDynamicColumn.Id).FirstOrDefaultAsync();

            if (lastData != null)
            {
                bool isDataExist = IsDataExistInDynamicCell(entityDynamicColumn, lastData);
                //lastData.EntityTypeId = entityDynamicColumn.EntityTypeId;
                lastData.Name = entityDynamicColumn.Name;
                lastData.NameInBangla = entityDynamicColumn.NameInBangla;
                if (!isDataExist)
                {
                    lastData.ColumnListId = entityDynamicColumn.ColumnListId;
                    lastData.ColumnType = entityDynamicColumn.ColumnType;
                    lastData.TargetedPopulation = entityDynamicColumn.TargetedPopulation;
                }

                _context.Update(lastData);
                await _context.SaveChangesAsync();
                return entityDynamicColumn;
            }
            _context.Add(entityDynamicColumn);
            await _context.SaveChangesAsync();

            entityDynamicColumn.ColumnList = 
            await _context.ListDataType.Include(x => x.ListItems)
                .Where(x => x.Id == entityDynamicColumn.ColumnListId).FirstOrDefaultAsync();

            return entityDynamicColumn;
        }

        private bool IsDataExistInDynamicCell(EntityDynamicColumn entityDynamicColumn, EntityDynamicColumn lastData)
        {
            bool isDataExist = false;
            if (entityDynamicColumn.EntityTypeId == EntityType.Beneficiary)
            {
                isDataExist = _context.BeneficiaryDynamicCells.Where(a => a.EntityDynamicColumnId == lastData.Id).Any();
            }
            else if (entityDynamicColumn.EntityTypeId == EntityType.Facility)
            {
                isDataExist = _context.FacilityDynamicCells.Where(a => a.EntityDynamicColumnId == lastData.Id).Any();
            }
            else if (entityDynamicColumn.EntityTypeId == EntityType.User)
            {
                isDataExist = _context.UserDynamicCells.Where(a => a.EntityDynamicColumnId == lastData.Id).Any();
            }
            else if (entityDynamicColumn.EntityTypeId == EntityType.Budget)
            {
                isDataExist = _context.BudgetDynamicCells.Where(a => a.EntityDynamicColumnId == lastData.Id).Any();
            }
            else if (entityDynamicColumn.EntityTypeId == EntityType.Target)
            {
                isDataExist = _context.TargetDynamicCells.Where(a => a.EntityDynamicColumnId == lastData.Id).Any();
            }

            return isDataExist;
        }

        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumn(DynamicPropertiesBySearchParamQueryModel model)
        {
            // var total = await _context.EntityDynamicColumn.Where(a => a.EntityTypeId == model.EntityTypeId).CountAsync();

            IQueryable<EntityDynamicColumn> entityDynamicColumns = _context.EntityDynamicColumn
                                                .Where(x => x.EntityTypeId == model.EntityTypeId && x.IsDeleted != true);

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                entityDynamicColumns = entityDynamicColumns.Where(x => x.Name.Contains(model.SearchText));
            var total = await entityDynamicColumns.CountAsync();

            var paginatedColumn = await entityDynamicColumns.Select(x => new DynamicPropertiesViewModel
            {
                Id = x.Id,
                Name = x.Name,
                NameInBangla = x.NameInBangla,
                ColumnDataType = x.ColumnType,
                Description = x.Description,
                IsMultiValued = x.IsMultiValued,
                ListObject = x.ColumnList,
                ListItems = x.ColumnList.ListItems
            })
            .Skip(model.Skip())
            .Take(model.PageSize)
            .ToListAsync();

            return new PagedResponse<DynamicPropertiesViewModel>(paginatedColumn, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedNumericColumn(DynamicPropertiesBySearchParamQueryModel model)
        {
            // var total = await _context.EntityDynamicColumn.Where(a => a.EntityTypeId == model.EntityTypeId).CountAsync();

            IQueryable<EntityDynamicColumn> entityDynamicColumns = _context.EntityDynamicColumn
                                                .Where(x => x.EntityTypeId == model.EntityTypeId
                                                && (x.ColumnType == ColumnDataType.Decimal || x.ColumnType == ColumnDataType.Integer));

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                entityDynamicColumns = entityDynamicColumns.Where(x => x.Name.Contains(model.SearchText));
            var total = await entityDynamicColumns.CountAsync();

            var paginatedColumn = await entityDynamicColumns.Select(x => new DynamicPropertiesViewModel
            {
                Id = x.Id,
                Name = x.Name,
                NameInBangla = x.NameInBangla,
                ColumnDataType = x.ColumnType,
                Description = x.Description,
                IsMultiValued = x.IsMultiValued,
                ListObject = x.ColumnList,
                ListItems = x.ColumnList.ListItems
            })
            .Skip(model.Skip())
            .Take(model.PageSize)
            .ToListAsync();

            return new PagedResponse<DynamicPropertiesViewModel>(paginatedColumn, total, model.PageNo, model.PageSize);
        }


        public async Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumnForIndicator(DynamicPropertiesByInstanceIdQueryParam model)
        {
            var alreadyAddedEntityColumnIds = await _context.InstanceIndicators.Where(a => a.InstanceId == model.InstanceId).Select(a => a.EntityDynamicColumnId)
                                                .ToListAsync();


            IQueryable<EntityDynamicColumn> entityDynamicColumns = _context.EntityDynamicColumn
                                                .Where(x => x.EntityTypeId == model.EntityTypeId
                                                && !(x.IsAutoCalculated ?? false)
                                                && !alreadyAddedEntityColumnIds.Contains(x.Id));

            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                entityDynamicColumns = entityDynamicColumns.Where(x => x.Name.Contains(model.SearchText));

            var total = await entityDynamicColumns.CountAsync();

            var paginatedColumn = await entityDynamicColumns.Select(x => new DynamicPropertiesViewModel
            {
                Id = x.Id,
                Name = x.Name,
                NameInBangla = x.NameInBangla,
                ColumnDataType = x.ColumnType,
                Description = x.Description,
                IsMultiValued = x.IsMultiValued,
                ListObject = x.ColumnList,
                ListItems = x.ColumnList.ListItems
            }).Skip(model.Skip())
              .Take(model.PageSize)
              .ToListAsync();

            return new PagedResponse<DynamicPropertiesViewModel>(paginatedColumn, total, model.PageNo, model.PageSize);
        }
    }
}
