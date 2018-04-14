using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Account.Models;
using Project.Core.DbContext;

namespace Project.Account.DAL {

    public interface IAccountDbContext : IDbContext {
        IDbSet<UserInfo> Users { get; set; }
    }

    public class AccountDbContext : IdentityDbContext<UserInfo>, IAccountDbContext {

        static AccountDbContext() {
            Database.SetInitializer<AccountDbContext>(null);
        }

        public AccountDbContext()
            : base("name=DefaultConnection", throwIfV1Schema: false) {
        }

    }
}
