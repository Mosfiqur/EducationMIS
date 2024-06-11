using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Specifications;

namespace UnicefEducationMIS.Core.Interfaces
{
    public interface ICountable<TModel>
    {   
        Task<int> Count(Specification<TModel> specification);
    }
}
