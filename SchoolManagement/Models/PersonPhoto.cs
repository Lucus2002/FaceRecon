namespace SchoolManagementApp.Models
{
    public class PersonPhoto
    {
        public int PersonPhotoID { get; set; }
        public int PersonID { get; set; }
        public Person Person { get; set; }
        public string Photopath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
