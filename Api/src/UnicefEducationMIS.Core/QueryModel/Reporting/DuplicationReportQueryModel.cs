using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel.Reporting
{
    public class DuplicationReportQueryModel
    {
        public List<int> ProgrammingPartnerIds { get; set; }
        public List<int> ImplementingPartnerIds { get; set; }

        public DuplicationReportQueryModel()
        {
            ProgrammingPartnerIds = new List<int>();
            ImplementingPartnerIds = new List<int>();
        }
        public Expression<Func<FacilityView, bool>> ToExpression()
        {
            return x => ProgrammingPartnerIds.Contains(x.ProgramPartnerId) &&
                ImplementingPartnerIds.Contains(x.ImplementationPartnerId);
        }
    }
}
