using System.ComponentModel.DataAnnotations;

namespace CLOUD_STORAGE_2.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        // Relationship to Jacket entity (one-to-many)
        public virtual ICollection<Jacket> Jackets { get; set; }

        // Adding metadata properties like created and modified timestamps
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
    
