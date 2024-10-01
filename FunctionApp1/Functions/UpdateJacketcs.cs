using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

[FunctionName("UpdateJacket")]
public static async Task<IActionResult> UpdateJacket(
    [HttpTrigger(AuthorizationLevel.Function, "put", Route = "jacket/{ownerID}/{jacketID}")] HttpRequest req,
    [Table("JacketTable", Connection = "AzureWebJobsStorage")] CloudTable jacketTable,
    string ownerID, string jacketID)
{
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    var updatedJacket = JsonConvert.DeserializeObject<Jacket>(requestBody);

    var retrieveOperation = TableOperation.Retrieve<JacketEntity>(ownerID, jacketID);
    var retrievedResult = await jacketTable.ExecuteAsync(retrieveOperation);

    var existingJacket = retrievedResult.Result as JacketEntity;
    if (existingJacket == null)
    {
        return new NotFoundResult();
    }

    existingJacket.Brand = updatedJacket.Brand;
    existingJacket.Size = updatedJacket.Size;
    existingJacket.Color = updatedJacket.Color;

    var updateOperation = TableOperation.Replace(existingJacket);
    await jacketTable.ExecuteAsync(updateOperation);

    return new OkObjectResult(existingJacket);
}

