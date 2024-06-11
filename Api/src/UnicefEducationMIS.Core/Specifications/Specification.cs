using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace UnicefEducationMIS.Core.Specifications
{
    public abstract class Specification<T>
    {
        public bool IsSatisfiedBy(T entity)
        {
            return ToExpression().Compile().Invoke(entity);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public static Specification<T> All = new InitialSpecification<T>();
    }
}
