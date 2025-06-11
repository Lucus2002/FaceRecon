namespace SchoolManagementApp.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<PersonPhoto> Photos { get; set; } = new();
    }
}
