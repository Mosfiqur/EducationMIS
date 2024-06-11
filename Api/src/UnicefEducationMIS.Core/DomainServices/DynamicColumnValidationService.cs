using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public class DynamicColumnValidationService
    {
        public static bool ValidateColumn(IndicatorSelectViewModel indicator, string value,out string returnValue)
        {
            bool isValid = true;
            returnValue = value;
            if (indicator.ColumnDataType == ColumnDataType.Boolean)
            {
                isValid = value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase)
                    || value.Equals("No", StringComparison.InvariantCultureIgnoreCase);
                returnValue = value;
            }
            else if(indicator.ColumnDataType == ColumnDataType.Datetime)
            {
                DateTime checkValue;
                isValid = DateTime.TryParse(value,out checkValue);
                returnValue = checkValue.ToString(ObjectExtension.DateTimeWithoutSecondFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if(indicator.ColumnDataType == ColumnDataType.Decimal)
            {
                decimal checkValue;
                isValid = decimal.TryParse(value, out checkValue);
                returnValue = checkValue.ToString();
            }
            else if(indicator.ColumnDataType == ColumnDataType.Integer)
            {
                int checkValue;
                isValid = int.TryParse(value, out checkValue);
                returnValue = checkValue.ToString();
            }
            else if(indicator.ColumnDataType== ColumnDataType.List)
            {
                var splitedValue = value.Split(',').Select(a=>a.Trim().ToLower()).ToList();
                if(indicator.IsMultivalued.HasValue && !indicator.IsMultivalued.Value && splitedValue.Count > 1)
                {
                    return false;
                }

                var listItemValues = new List<int>();
                foreach (var item in splitedValue)
                {
                    var listItem=indicator.ListItems.Where(a => a.Title.ToLower() == item).FirstOrDefault();
                    if (listItem != null)
                        listItemValues.Add(listItem.Value);
                    else
                    {
                        isValid = false;
                        break;
                    }
                }
                returnValue = string.Join(',', listItemValues);
            }
            return isValid;
        }
    }
}
