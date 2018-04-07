using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models {
    public interface IObjectWithState {
        ModelState State { get; set; }
    }

    public abstract class ObjectWithState : IObjectWithState {

        [NotMapped]
        public ModelState State { get; set; }
    }
}
