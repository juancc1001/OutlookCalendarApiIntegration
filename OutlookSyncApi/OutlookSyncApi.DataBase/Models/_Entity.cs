

namespace ProySuministros.DataAccess.Layer.Models
{
    public class _Entity
    {
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }

        public _Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
        public _Entity(string id)
        {
            Id = id;
        }
    }
}
