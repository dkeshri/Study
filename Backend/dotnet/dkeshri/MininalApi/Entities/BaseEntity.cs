namespace MininalApi.Entities
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
