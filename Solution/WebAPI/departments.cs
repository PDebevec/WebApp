namespace WebAPI
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
    }
}
