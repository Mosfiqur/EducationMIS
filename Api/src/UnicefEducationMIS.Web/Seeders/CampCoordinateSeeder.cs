using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Web.Configurations;

namespace UnicefEducationMIS.Web.Seeders
{
    public class CampCoordinateSeeder
    {
        private readonly SeedDataFilesConfigurations _fileConfig;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ICampCoordinateRepository _campCoordinateRepository;

        public CampCoordinateSeeder(IOptions<SeedDataFilesConfigurations> fileConfig, 
            IWebHostEnvironment hostEnv, ICampCoordinateRepository campCoordinateRepository)
        {
            _fileConfig = fileConfig.Value;
            _hostEnvironment = hostEnv;
            _campCoordinateRepository = campCoordinateRepository;
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

        public void SeedCampCoordinate(bool checkStatus)
        {
            if(checkStatus == true)
            {
                var seedCampCoordinates = ReadData<CampCoordinate>(_fileConfig.CampCoordinateFileName);
                var seedCampId = seedCampCoordinates.Select(x => x.CampId).Distinct().ToList();
                
                var commonCampCoordinates = seedCampCoordinates.Where(x => seedCampId.Contains(x.CampId)).ToList();
                seedCampId.ForEach(seedCamp => {
                    var camps = _campCoordinateRepository.GetAll().Where(x => x.CampId == seedCamp).ToList();
                    _campCoordinateRepository.DeleteRange(camps);
                });

                _campCoordinateRepository.InsertRange(commonCampCoordinates).Wait();
            }
        }

    }
}
