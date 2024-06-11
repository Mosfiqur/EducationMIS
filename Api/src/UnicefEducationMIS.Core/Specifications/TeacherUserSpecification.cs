using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Specifications
{
    public class TeacherUserSpecification : Specification<User>
    {    
        public override Expression<Func<User, bool>> ToExpression()
        {
            return user => !user.IsDeleted && user.UserRoles.Single().Role.Level.Rank == LevelRank.Teacher;
        }
    }
}
