using Azure;
using Azure.Data.Tables;
using CLOUD_STORAGE_2.Models;
using CLOUD_STORAGE_2.Models;
using System.Net.Sockets;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly TableClient _jacketTableClient;
    private readonly TableClient _jacketOwnerTableClient;
    private readonly TableClient _sightingTableClient;

    public TableStorageService(string connectionString)
    {
        _jacketTableClient = new TableClient(connectionString, "Jacket");
        _jacketOwnerTableClient = new TableClient(connectionString, "JacketOwner");
        _sightingTableClient = new TableClient(connectionString, "Sighting");
    }

    public async Task<List<Jacket>> GetAllJacketsAsync()
    {
        var jackets = new List<Jacket>();

        await foreach (var jacket in _jacketTableClient.QueryAsync<Jacket>())
        {
            jackets.Add(jacket);
        }
        return jackets;
    }

    public async Task AddJacketAsync(Jacket jacket)
    {
        // Ensure PartitionKey and RowKey are set
        if (string.IsNullOrEmpty(jacket.PartitionKey) || string.IsNullOrEmpty(jacket.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _jacketTableClient.AddEntityAsync(jacket);
        }
        catch (RequestFailedException ex)
        {
            // Handle exception as necessary, for example log it or rethrow
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteJacketAsync(string partitionKey, string rowKey)
    {
        await _jacketTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<Jacket?> GetJacketAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _jacketTableClient.GetEntityAsync<Jacket>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // Handle not found
            return null;
        }
    }

    public async Task<List<JacketOwner>> GetAllJacketOwnersAsync()
    {
        var jacketOwners = new List<JacketOwner>();

        await foreach (var jacketOwner in _jacketOwnerTableClient.QueryAsync<JacketOwner>())
        {
            jacketOwners.Add(jacketOwner);
        }

        return jacketOwners;
    }

    public async Task AddJacketOwnerAsync(JacketOwner jacketOwner)
    {
        if (string.IsNullOrEmpty(jacketOwner.PartitionKey) || string.IsNullOrEmpty(jacketOwner.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _jacketOwnerTableClient.AddEntityAsync(jacketOwner);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteJacketOwnerAsync(string partitionKey, string rowKey)
    {
        await _jacketOwnerTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<JacketOwner?> GetJacketOwnerAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _jacketOwnerTableClient.GetEntityAsync<JacketOwner>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task AddSightingAsync(Sighting sighting)
    {
        if (string.IsNullOrEmpty(sighting.PartitionKey) || string.IsNullOrEmpty(sighting.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _sightingTableClient.AddEntityAsync(sighting);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding sighting to Table Storage", ex);
        }
    }

    public async Task<List<Sighting>> GetAllSightingsAsync()
    {
        var sightings = new List<Sighting>();

        await foreach (var sighting in _sightingTableClient.QueryAsync<Sighting>())
        {
            sightings.Add(sighting);
        }

        return sightings;
    }
}




