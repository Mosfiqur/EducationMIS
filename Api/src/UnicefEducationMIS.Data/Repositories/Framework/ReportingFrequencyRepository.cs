using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class ReportingFrequencyRepository : BaseRepository<ReportingFrequency, int>, IReportingFrequencyRepository
    {
        public ReportingFrequencyRepository(UnicefEduDbContext context) : base(context)
        {
        }

    }
}