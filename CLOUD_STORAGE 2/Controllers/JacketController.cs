using Microsoft.AspNetCore.Mvc;
using CLOUD_STORAGE_2.Models;
using CLOUD_STORAGE_2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;


namespace CLOUD_STORAGE_2.Controllers
{

    public class JacketController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableStorageService _tableStorageService;

        public JacketController(BlobService blobService, TableStorageService tableStorageService)
        {
            _blobService = blobService;
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult AddJacket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddJacket(Jacket jacket, IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
                jacket.ImageUrl = imageUrl;
            }

            if (ModelState.IsValid)
            {
                jacket.PartitionKey = "JacketPartition";
                jacket.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddJacketAsync(jacket);
                return RedirectToAction("Index");
            }
            return View(jacket);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJacket(string partitionKey, string rowKey, Jacket jacket)
        {
            if (jacket != null && !string.IsNullOrEmpty(jacket.ImageUrl))
            {
                // Delete the associated blob image
                await _blobService.DeleteBlobAsync(jacket.ImageUrl);
            }
            // Delete Table entity
            await _tableStorageService.DeleteJacketAsync(partitionKey, rowKey);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var jacket = await _tableStorageService.GetAllJacketsAsync();
            return View(jacket);
        }
    }
}
