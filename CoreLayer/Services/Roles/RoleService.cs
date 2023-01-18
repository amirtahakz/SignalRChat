using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Roles
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
