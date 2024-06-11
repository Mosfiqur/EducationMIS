using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class ModelToIndicatorConverter : IModelToIndicatorConverter
    {
        public IEnumerable<DynamicCellViewModel> FacilityToDynamicCell(CreateFacilityViewModel model)
        {
            var cells = new List<DynamicCellViewModel>();
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Code, model.FacilityCode));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Name, model.Name));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.TargetedPopulation, (int)model.TargetedPopulation));
            if (model.FacilityStatus.HasValue)
                cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Status, (int)model.FacilityStatus));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Latitude, model.Latitude));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Longitude, model.Longitude));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Donors, model.Donors));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.NonEducationPartner, model.NonEducationPartner));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.ProgramPartner, model.ProgramPartnerId.ToString()));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.ImplementationPartner, model.ImplementationPartnerId.ToString()));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Remarks, model.Remarks));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Upazila, model.UpazilaId.ToString()));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Union, model.UnionId.ToString()));
            if (model.CampId.HasValue)
                cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Camp, model.CampId.ToString()));
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.ParaName, model.ParaName));
            if (model.BlockId.HasValue)
                cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Block, model.BlockId.ToString()));
            if (model.TeacherId.HasValue)
                cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Teacher, model.TeacherId.ToString()));
            if (model.FacilityType.HasValue)
                cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Type, (int)model.FacilityType));
            return cells;
        }

        public IEnumerable<DynamicCellViewModel> AssignTeacherToDynamicCell(AssignTeacherViewModel model)
        {
            var cells = new List<DynamicCellViewModel>();
            cells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Teacher, model.TeacherId.ToString()));
            return cells;
        }

        public void ReplaceFacilityFixedIndicatorIdsWithValues(FacilityViewModel model)
        {
            model.Properties.ForEach(indicator =>
            {
                switch (indicator.EntityColumnId)
                {
                    case FacilityFixedIndicators.ImplementationPartner:
                        indicator.Values = model.ImplemantationPartnerName.AsList();
                        break;
                    case FacilityFixedIndicators.ProgramPartner:
                        indicator.Values = model.ProgrammingPartnerName.AsList();
                        break;
                    case FacilityFixedIndicators.Block:
                        indicator.Values = model.BlockName.AsList();
                        break;
                    case FacilityFixedIndicators.Camp:
                        indicator.Values = model.CampName.AsList();
                        break;
                    case FacilityFixedIndicators.Union:
                        indicator.Values = model.UnionName.AsList();
                        break;
                    case FacilityFixedIndicators.Upazila:
                        indicator.Values = model.UpazilaName.AsList();
                        break;
                    case FacilityFixedIndicators.Teacher:
                        indicator.Values = model.TeacherEmail.AsList();
                        break;
                }
            });
        }

        public CreateFacilityViewModel DynamicCellToCreateFacilityViewModels(FacilityDynamicCellAddViewModel model)
        {
            var result = new CreateFacilityViewModel();
            model.DynamicCells.ForEach(cell =>
            {
                result.Id = model.FacilityId;
                result.InstanceId = model.InstanceId;
                switch (cell.EntityDynamicColumnId)
                {
                    case FacilityFixedIndicators.Code:
                        result.FacilityCode = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.Name:
                        result.Name = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.TargetedPopulation:
                        result.TargetedPopulation = cell.Value.ToEnum<TargetedPopulation>();
                        break;
                    case FacilityFixedIndicators.Status:
                        result.FacilityStatus = cell.Value.ToEnum<FacilityStatus>();
                        break;
                    case FacilityFixedIndicators.Latitude:
                        result.Latitude = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.Longitude:
                        result.Longitude = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.Donors:
                        result.Donors = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.NonEducationPartner:
                        result.NonEducationPartner = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.ProgramPartner:
                        result.ProgramPartnerId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.ImplementationPartner:
                        result.ImplementationPartnerId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.Remarks:
                        result.Remarks = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.Upazila:
                        result.UpazilaId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.Union:
                        result.UnionId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.Camp:
                        result.CampId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.ParaName:
                        result.ParaName = cell.Value.FirstOrDefault();
                        break;
                    case FacilityFixedIndicators.Block:
                        result.BlockId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.Teacher:
                        result.TeacherId = int.Parse(cell.Value.FirstOrDefault() ?? "0");
                        break;
                    case FacilityFixedIndicators.Type:
                        result.FacilityType = cell.Value.ToEnum<FacilityType>();
                        break;
                }
            });

            return result;
        }

        public void ReplaceBeneficiaryFixedIndicatorIdsWithValues(List<BeneficiaryViewModel> beneficiaries)
        {
            beneficiaries.ForEach(ben =>
            {
                var facilityProp = ben.Properties.FirstOrDefault(a => a.EntityColumnId == BeneficairyFixedColumns.FacilityId);
                if (facilityProp != null)
                    facilityProp.Values = new List<string> { ben.FacilityName };

                var campProp = ben.Properties.FirstOrDefault(a => a.EntityColumnId == BeneficairyFixedColumns.CampId);
                if (campProp != null)
                    campProp.Values = new List<string> { ben.BeneficiaryCampName };

                var blockProp = ben.Properties.FirstOrDefault(a => a.EntityColumnId == BeneficairyFixedColumns.BlockId);
                if (blockProp != null)
                    blockProp.Values = new List<string> { ben.BlockName };

                var subblockProp = ben.Properties.FirstOrDefault(a => a.EntityColumnId == BeneficairyFixedColumns.SubBlockId);
                if (subblockProp != null)
                    subblockProp.Values = new List<string> { ben.SubBlockName };
            });
        }
    }
}
