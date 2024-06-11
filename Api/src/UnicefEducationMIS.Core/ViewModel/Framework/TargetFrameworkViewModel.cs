using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetFrameworkViewModel
    {
        public long Id { get; set; }
        public int? CampId { get; set; }
        public string CampName { get; set; }
        public Gender Gender { get; set; }
        public int AgeGroupId { get; set; }
        public string AgeGroupName { get; set; }
        public int PeopleInNeed { get; set; }
        public int Target { get; set; }
        public int StartYear { get; set; }
        public Month StartMonth { get; set; }
        public int EndYear { get; set; }
        public Month EndMonth { get; set; }
        public int UpazilaId { get; set; }
        public int UnionId { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }

        public List<TargetDynamicCellViewModel> DynamicCells { get; set; }

        public static Expression<Func<TargetFramework, TargetFrameworkViewModel>> FromModel = x => new TargetFrameworkViewModel
            {
                Id = x.Id,
                CampId = x.CampId,
                CampName = x.Camp.Name,
                Gender = x.Gender,
                AgeGroupId = x.AgeGroupId,
                AgeGroupName = x.AgeGroup.Name,
                PeopleInNeed = x.PeopleInNeed,
                Target = x.Target,
                StartYear = x.StartYear,
                StartMonth = x.StartMonth,
                EndYear = x.EndYear,
                EndMonth = x.EndMonth,
                UnionId = x.UnionId,
                UpazilaId = x.UpazilaId,
                TargetedPopulation = x.TargetedPopulation,

                DynamicCells = x.DynamicCells.AsQueryable()
                    .Select(TargetDynamicCellViewModel.FromModel).ToList(),
            };

        public static Func<TargetFrameworkViewModel, TargetFrameworkViewModel> GroupByEntityDynamicColumn = x => new TargetFrameworkViewModel
        {
            Id = x.Id,
            CampId = x.CampId,
            CampName = x.CampName,
            Gender = x.Gender,
            AgeGroupId = x.AgeGroupId,
            AgeGroupName = x.AgeGroupName,
            PeopleInNeed = x.PeopleInNeed,
            Target = x.Target,
            StartYear = x.StartYear,
            StartMonth = x.StartMonth,
            EndYear = x.EndYear,
            EndMonth = x.EndMonth,
            UnionId = x.UnionId,
            UpazilaId = x.UpazilaId,
            TargetedPopulation = x.TargetedPopulation,
            DynamicCells = x.DynamicCells
                .GroupBy(cell => new
                {
                    EntityDynamicColumnId = cell.EntityDynamicColumnId,
                    cell.TargetFrameworkId,
                    cell.ColumnName,
                    cell.DataType
                })
                .ToList()
                .Select(col => new TargetDynamicCellViewModel()
                {
                    ColumnName = col.Key.ColumnName,
                    EntityDynamicColumnId = col.Key.EntityDynamicColumnId,
                    DataType = col.Key.DataType,
                    TargetFrameworkId = col.Key.TargetFrameworkId,
                    Values = col.Select(c => c.Value).ToList()
                })
                .ToList()
        };
    }
}
