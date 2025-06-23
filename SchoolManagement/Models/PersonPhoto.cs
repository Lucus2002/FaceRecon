// In Models/PersonPhoto.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementApp.Models
{
    public class PersonPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonPhotoID { get; set; }  // Note: "ID" not "Id"

        public int PersonID { get; set; }
        public Person Person { get; set; }
        public string Photopath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}