using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Project.Account.Models;
using Project.Account.Services;

namespace Project.Account.Managers {
    public class ApplicationUserManager : UserManager<UserInfo> {
        public ApplicationUserManager(IDataProtectionProvider dataProtectionProvider, IUserStore<UserInfo> store)
            : base(store) {
            UserValidator = new CustomUserValidator(this) {
                // the simpler version: [a-zA-Z0-9\._]*@?[a-zA-Z0-9\._]*
                RequireUserNameRegex = @"([a-zA-Z0-9]+)([\._]?[a-zA-Z0-9]+)*(@([a-zA-Z0-9]+)([\._]?[a-zA-Z0-9]{2,})*)?",
                RequireUserNameRegexErrorMessage = "The Username can only contain alphanumeric characters, dots, underscores and at sign.",
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
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

    internal class CustomUserValidator : UserValidator<UserInfo> {

        readonly UserManager<UserInfo, string> manager;

        Regex userNameRegex;
        string requireUserNameRegex;
        public string RequireUserNameRegex {
            get {
                return requireUserNameRegex;
            }
            set {
                requireUserNameRegex = value;
                requireUserNameRegex = requireUserNameRegex.TrimStart('^').TrimEnd('$');
                requireUserNameRegex = $"^{requireUserNameRegex}$";
                userNameRegex = new Regex(requireUserNameRegex, RegexOptions.Compiled);
            }
        }

        public string RequireUserNameRegexErrorMessage { get; set; }

        public CustomUserValidator(UserManager<UserInfo, string> manager) : base(manager) {
            this.manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(UserInfo item) {
            var result = await base.ValidateAsync(item);

            if (!result.Succeeded)
                return result;

            if (userNameRegex != null)
                result = CheckUserNameRegex(item);

            return result;
        }

        private IdentityResult CheckUserNameRegex(UserInfo item) {
            if (userNameRegex.IsMatch(item.UserName))
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(RequireUserNameRegexErrorMessage ?? "The User Name did not pass the validation process.");
        }
    }
}
