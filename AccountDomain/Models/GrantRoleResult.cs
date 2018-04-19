using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Account.Models {
    public class GrantRoleResult {

        private GrantRoleResult() {
            Succeded = true;
            Errors = new List<string>();
        }

        private GrantRoleResult(IEnumerable<string> errors) {
            Succeded = false;
            Errors = errors;
        }

        public bool Succeded { get; }

        public IEnumerable<string> Errors { get; }


        public static GrantRoleResult Success => new GrantRoleResult();
        public static GrantRoleResult Failed(IEnumerable<string> errors) => new GrantRoleResult(errors);

    }
}
