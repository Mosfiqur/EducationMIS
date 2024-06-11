using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace UnicefEducationMIS.Core.Specifications
{
    internal sealed class InitialSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }
}
