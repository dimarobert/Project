using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Account.Models {
    public class RoleInfo : IdentityRole<string, UserRoleInfo> {

        public RoleInfo() {
            Id = Guid.NewGuid().ToString();
        }

    }

    public class UserRoleInfo : IdentityUserRole<string> {

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleInfo Role { get; set; }

    }
}
