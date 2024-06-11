using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Web.Configurations;

namespace UnicefEducationMIS.Web.Seeders
{
    public class FacilityIndicatorSeeder
    {
        private readonly SeedDataFilesConfigurations _fileConfig;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDynamicColumnRepositories _dynamicColumnRepository;

        public FacilityIndicatorSeeder(IOptions<SeedDataFilesConfigurations> fileConfig,
            IWebHostEnvironment hostEnv, IDynamicColumnRepositories dynamicColumnRepositories)
        {
            _fileConfig = fileConfig.Value;
            _hostEnvironment = hostEnv;
            _dynamicColumnRepository = dynamicColumnRepositories;
        }
        private string GetFullPath(string path)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, _fileConfig.DataRoot, path);
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

        public void SeedFacilityIndicator()
        {
            var seedEntityDynamicColumns = ReadData<EntityDynamicColumn>(_fileConfig.EntityDynamicColumnFileName);
            var allDB = _dynamicColumnRepository.GetAll().ToList();

            //Update
            var oldIds = seedEntityDynamicColumns.Select(x => x.Id).Intersect(allDB.Select(x => x.Id)).ToList();
            var seedColumns = seedEntityDynamicColumns.Where(x => oldIds.Contains(x.Id)).ToList();
            seedColumns.ForEach(seedColumn =>
            {
                _dynamicColumnRepository.Update(seedColumn);
            });

            //Insert
            var newIds = seedEntityDynamicColumns.Select(x => x.Id).Except(allDB.Select(x => x.Id)).ToList();
            _dynamicColumnRepository.InsertRange(seedEntityDynamicColumns.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
    }
}
