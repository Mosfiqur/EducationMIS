using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class BudgetFrameworkViewModel
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectViewModel Project { get; set; }
        public DonorViewModel Donor { get; set; }
        public decimal Amount { get; set; }
        public List<BudgetDynamicCellViewModel> DynamicCells { get; set; }


        public static Expression<Func<BudgetFramework, BudgetFrameworkViewModel>> FromModel = x =>
            new BudgetFrameworkViewModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Donor = new DonorViewModel()
                {
                    Name = x.Donor.Name,
                    Id = x.DonorId
                },
                Project = new ProjectViewModel()
                {
                    Name = x.Project.Name,
                    Id = x.ProjectId
                },
                Amount = x.Amount,
                DynamicCells = x.DynamicCells.AsQueryable()
                    .Select(BudgetDynamicCellViewModel.FromModel).ToList(),
            };

        public static Func<BudgetFrameworkViewModel, BudgetFrameworkViewModel> GroupeByEntityDynamicColumn = x =>
            new BudgetFrameworkViewModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Donor = x.Donor,
                Project = x.Project,
                Amount = x.Amount,
                DynamicCells = x.DynamicCells
                    .GroupBy(cell => new
                    {
                        FrameworkDynamicColumnId = cell.EntityDynamicColumnId,
                        cell.BudgetFrameworkId,
                        cell.ColumnName,
                        cell.DataType
                    })
                    .ToList()
                    .Select(col => new BudgetDynamicCellViewModel
                    {
                        ColumnName = col.Key.ColumnName,
                        EntityDynamicColumnId = col.Key.FrameworkDynamicColumnId,
                        BudgetFrameworkId = col.Key.BudgetFrameworkId,
                        DataType = col.Key.DataType,
                        Values = col.Select(c => c.Value).ToList()
                    }).ToList()
            };


    }
}
