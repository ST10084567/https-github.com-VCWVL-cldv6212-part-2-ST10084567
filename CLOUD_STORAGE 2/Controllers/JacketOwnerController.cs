using CLOUD_STORAGE_2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLOUD_STORAGE_2.Controllers
{
    public class JacketOwnerController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public JacketOwnerController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var jacketOwner = await _tableStorageService.GetAllJacketOwnersAsync();
            return View(jacketOwner);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JacketOwner jacketOwner)
        {
            jacketOwner.PartitionKey = "JacketOwnerPartition";
            jacketOwner.RowKey = Guid.NewGuid().ToString();

            await _tableStorageService.AddJacketOwnerAsync(jacketOwner);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _tableStorageService.DeleteJacketAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var jacketOwner = await _tableStorageService.GetJacketOwnerAsync(partitionKey, rowKey);
            if (jacketOwner == null)
            {
                return NotFound();
            }
            return View(jacketOwner);
        }
    }
}
