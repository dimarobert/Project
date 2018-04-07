using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;

namespace Project.Account.DAL {
    public class AccountDbContext : IdentityDbContext<UserInfo> {

        static AccountDbContext() {
            Database.SetInitializer<AccountDbContext>(null);
        }

        public AccountDbContext()
            : base("name=DefaultConnection", throwIfV1Schema: false) {
        }

    }
}
