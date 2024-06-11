using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Web.Configurations;

namespace UnicefEducationMIS.Web.Seeders
{
    public class FrameworkSeeder
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SeedDataFilesConfigurations _fileConfig;
        private readonly IAgeGroupRepository _ageGroupRepository;
        private readonly IReportingFrequencyRepository _reportingFrequencyRepository;

        public FrameworkSeeder(IWebHostEnvironment hostEnv, IOptions<SeedDataFilesConfigurations> fileConfig,
            IAgeGroupRepository ageGroupRepository,IReportingFrequencyRepository reportingFrequencyRepository)
        {
            _hostEnvironment = hostEnv;
            _fileConfig = fileConfig.Value;
            _ageGroupRepository = ageGroupRepository;
            _reportingFrequencyRepository = reportingFrequencyRepository;

        }

        private List<T> ReadData<T>(string path)
        {
            List<T> result = new List<T>();
            using (var file = File.OpenText(GetFullPath(path)))
            {
                result.AddRange(JsonConvert.DeserializeObject<List<T>>(file.ReadToEnd()));
            }
            return result;
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, _fileConfig.DataRoot, path);
        }

        public void Seed()
        {
            SeedAgeGroup();
            SeedReportingFrequency();
        }

        private void SeedReportingFrequency()
        {
            var seedReportingFrequencies = ReadData<ReportingFrequency>(_fileConfig.ReportingFrequencyFileName);
            var all = _reportingFrequencyRepository.GetAll().ToList();
            var newIds = seedReportingFrequencies.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _reportingFrequencyRepository.InsertRange(seedReportingFrequencies.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }

        private void SeedAgeGroup()
        {
            var seedAgeGroups = ReadData<AgeGroup>(_fileConfig.AgeGroupFileName);
            var all = _ageGroupRepository.GetAll().ToList();
            var newIds = seedAgeGroups.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _ageGroupRepository.InsertRange(seedAgeGroups.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
    }
}
