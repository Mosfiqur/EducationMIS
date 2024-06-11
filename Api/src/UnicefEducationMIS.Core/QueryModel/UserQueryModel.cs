using System.Collections.Generic;


namespace UnicefEducationMIS.Core.QueryModel
{
    public class UserQueryModel: BaseQueryModel
    {
        public UserQueryModel()
        {
            RoleIds = new List<int>();
            EspIds = new List<int>();
        }
        public List<int> RoleIds { get; set; }
        public List<int> EspIds { get; set; }
        public int UserId { get; set; }
    }
}
