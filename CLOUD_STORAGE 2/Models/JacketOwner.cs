using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace CLOUD_STORAGE_2.Models
{
    public class JacketOwner : ITableEntity
    {
        [Key]
        public int JacketOwner_Id { get; set; }  // Ensure this property exists and is populated
        public string? JacketOwner_Name { get; set; }  // Ensure this property exists and is populated
        public string? email { get; set; }
        public string? password { get; set; }

        // ITableEntity implementation
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
