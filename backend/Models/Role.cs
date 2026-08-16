namespace whm.Models
{
    public class Role
    {
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }=string.Empty;
        public string Role_Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }=true;
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
        public List<Users>User { get; set; } = new List<Users>();
    }
}
