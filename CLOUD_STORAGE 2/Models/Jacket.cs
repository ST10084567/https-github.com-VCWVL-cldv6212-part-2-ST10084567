using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLOUD_STORAGE_2.Models
{
    public class Jacket : ITableEntity
    {
        [Key]
        public int Jacket_Id { get; set; }  // Ensure this property exists and is populated
        public string? Jacket_Name { get; set; }  // Ensure this property exists and is populated
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }

}
