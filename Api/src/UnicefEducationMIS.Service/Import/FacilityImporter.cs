using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel.Import;

namespace UnicefEducationMIS.Service.Import
{
    //public class FacilityImporter : AbstractWorkBookImporter<FacilityImportViewModel>
    //{
    //    private readonly List<Camp> _camps;
    //    private readonly List<Block> _blocks;
    //    private readonly List<Union> _unionList;
    //    private readonly List<Upazila> _upazilList;
    //    private readonly List<EducationSectorPartner> _espList;
    //    private readonly Dictionary<string, long> _facilities;
    //    public FacilityImporter(
    //        ILogger<FacilityImportViewModel> logger, 
    //        List<ExcelCell> importColumns,           
    //        IEnumerable<EducationSectorPartner> espList,
    //        IEnumerable<Union> unionList,
    //        IEnumerable<Upazila> upazilaList,
    //        IEnumerable<Camp> camps,
    //        IEnumerable<Block> blocks, Dictionary<string, long> facilities)
    //        :base(logger, importColumns)
    //    {
    //        _facilities = facilities;

    //        _unionList= new List<Union>();
    //        _unionList.AddRange(unionList);

    //        _upazilList = new List<Upazila>();
    //        _upazilList.AddRange(upazilaList);
    //        _espList = new List<EducationSectorPartner>();
    //        _espList.AddRange(espList);
    //        _camps = new List<Camp>();
    //        _camps.AddRange(camps);
    //        _blocks = new List<Block>();
    //        _blocks.AddRange(blocks);
    //    }

        

    //    public override bool SetPropertyValue(ExcelCell column, FacilityImportViewModel entity)
    //    {

    //        if (column.IsMandatory)
    //        {
    //            return SetMandatoryColumns(column, entity);
    //        }

    //        if (column.DisplayName == FacilityExcelColumns.CampSsId.ToLower())
    //        {                
    //            var camp = _camps.SingleOrDefault(x => x.SSID == column.Value);
    //            if (camp != null)
    //            {
    //                entity.CampId = camp.Id;
    //            }
    //            if (!string.IsNullOrEmpty(column.Value) && camp == null)
    //            {
    //                return false;
    //            }

    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.BlockId.ToLower())
    //        {
    //            var block = _blocks.SingleOrDefault(x => x.Code == column.Value);
    //            if (block != null)
    //            {
    //                entity.BlockId = block.Id;
    //            }
    //            return string.IsNullOrEmpty(column.Value) || block != null;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.FacilityType.ToLower())
    //        {
    //            var facilityType = Enum.GetValues(typeof(FacilityType))
    //                .Cast<FacilityType>()
    //                .FirstOrDefault(x => x.GetDescription() == column.Value);

    //            if (facilityType.GetDescription() == null)
    //            {
    //                return false;
    //            }
    //            entity.FacilityType = facilityType;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.FacilityStatus.ToLower())
    //        {
    //            var facilityStatus = column.Value.TryParseToEnum<FacilityStatus>(out var isSuccess);

    //            if (!isSuccess)
    //            {
    //                return false;
    //            }
    //            entity.FacilityStatus = facilityStatus;
    //            return true;
    //        }

    //        var type = entity.GetType();
    //        type.GetProperty(column.ActualName).SetValue(entity, column.Value);
    //        return true;
    //    }

    //    public override Task ApplyBusinessLogic(FacilityImportViewModel entity, int rowIndex)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool SetMandatoryColumns(ExcelCell column, FacilityImportViewModel entity)
    //    {
    //        if (column.DisplayName == FacilityExcelColumns.UpazilaId.ToLower())
    //        {
    //            var upazila = _upazilList.SingleOrDefault(x => x.Name == column.Value);
    //            if (upazila == null)
    //            {
    //                return false;
    //            }
    //            entity.UpazilaId = upazila.Id;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.UnionId.ToLower())
    //        {
    //            var union = _unionList.SingleOrDefault(x => x.Name == column.Value);
    //            if (union == null)
    //            {
    //                return false;
    //            }
    //            entity.UnionId = union.Id;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.FacilityId.ToLower())
    //        {
    //            entity.FacilityCode = column.Value;
    //            _facilities.TryGetValue(column.Value, out var id);
    //            entity.Id = id;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.FacilityName.ToLower())
    //        {
    //            entity.FacilityName = column.Value;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.TargetedPopulation.ToLower())
    //        {
    //            var targetedPopulation = Enum.GetValues(typeof(TargetedPopulation))
    //                .Cast<TargetedPopulation>()
    //                .FirstOrDefault(x => x.GetDescription() == column.Value);
    //            if (targetedPopulation.NoneMatched())
    //            {
    //                return false;
    //            }

    //            entity.TargetedPopulation = targetedPopulation;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.ProgramPartner.ToLower())
    //        {
    //            var esp = _espList.SingleOrDefault(x => x.PartnerName == column.Value);
    //            if (esp == null)
    //            {
    //                return false;
    //            }
    //            entity.ProgramPartnerId = esp.Id;
    //            return true;
    //        }
    //        if (column.DisplayName == FacilityExcelColumns.ImplementationPartner.ToLower())
    //        {
    //            var esp = _espList.SingleOrDefault(x => x.PartnerName == column.Value);
    //            if (esp == null)
    //            {
    //                return false;
    //            }
    //            entity.ImplementationPartnerId = esp.Id;
    //            return true;
    //        }

    //        return true;
    //    }
    //}





}
