using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Features.PermissionManagement.Roles;

namespace Core.Features.Users.GetRoles;
public record GetRolesResponse(IList<Role> Roles);