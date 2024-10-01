using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLOUD_STORAGE_2.Models
{
    public class Sighting : ITableEntity
    {
        [Key]
        public int Sighting_Id { get; set; }

        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Introduce validation sample
        [Required(ErrorMessage = "Please select a jacket owner.")]
        public int JacketOwner_ID { get; set; } // FK to the JacketOwner who made the sighting

        [Required(ErrorMessage = "Please select a jacket.")]
        public int Jacket_ID { get; set; } // FK to the Jacket being sighted

        [Required(ErrorMessage = "Please select the date.")]
        public DateTime Sighting_Date { get; set; }

        [Required(ErrorMessage = "Please enter the location.")]
        public string? Sighting_Location { get; set; }
    }
}
