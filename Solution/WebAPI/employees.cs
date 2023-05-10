namespace WebAPI
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int JobId { get; set; }
        public decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }

        public Job? Job { get; set; }
        public Employee? Manager { get; set; }
        public Department? Department { get; set; }
    }
}
