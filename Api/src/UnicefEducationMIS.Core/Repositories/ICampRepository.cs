using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface ICampRepository : IBaseRepository<Camp,int>
    {
        Task<List<CampCoordinate>> GetCoordinatesByCampId(int campId);
        Task<List<CampCoordinate>> GetAllCoordinates();
    }
}
