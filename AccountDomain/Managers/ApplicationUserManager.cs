using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Project.Account.Models;
using Project.Account.Services;

namespace Project.Account.Managers {
    public interface IApplicationUserManager {
        Task<UserInfo> FindByEmailAsync(string email);
        Task<UserInfo> FindByIdAsync(string userId);
        Task<UserInfo> FindByNameAsync(string userName);
    }

    public class ApplicationUserManager : UserManager<UserInfo>, IApplicationUserManager {
        public ApplicationUserManager(IDataProtectionProvider dataProtectionProvider, IUserStore<UserInfo> store)
            : base(store) {
            UserValidator = new UserValidator<UserInfo>(this) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<UserInfo> {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<UserInfo> {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
            if (dataProtectionProvider != null) {
                UserTokenProvider =
                    new DataProtectorTokenProvider<UserInfo>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
