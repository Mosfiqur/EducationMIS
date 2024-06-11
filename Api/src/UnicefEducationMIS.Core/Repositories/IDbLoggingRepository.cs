using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IDbLoggingRepository
    {
        Task Insert(LogEntry log);
    }
}
