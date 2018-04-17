namespace Project.Core.Models {
    public interface IEntity<TKey> {
        TKey Id { get; set; }
    }
}
