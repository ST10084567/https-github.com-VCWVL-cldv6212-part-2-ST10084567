using CLOUD_STORAGE_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CLOUD_STORAGE_2.Controllers
{
    public class SightingController : Controller
    {
        private readonly TableStorageService _tableStorageService;
        private readonly QueueService _queueService;

        public SightingController(TableStorageService tableStorageService, QueueService queueService)
        {
            _tableStorageService = tableStorageService;
            _queueService = queueService;
        }

        // Action to display all sightings
        public async Task<IActionResult> Index()
        {
            var sightings = await _tableStorageService.GetAllSightingsAsync();
            return View(sightings);
        }

        public async Task<IActionResult> Register()
        {
            var jacketOwners = await _tableStorageService.GetAllJacketOwnersAsync();
            var jackets = await _tableStorageService.GetAllJacketsAsync();

            // Check for null or empty lists
            if (jacketOwners == null || jacketOwners.Count == 0)
            {
                // Handle the case where no jacket owners are found
                ModelState.AddModelError("", "No jacket owners found. Please add jacket owners first.");
                return View(); // Or redirect to another action
            }

            if (jackets == null || jackets.Count == 0)
            {
                // Handle the case where no jackets are found
                ModelState.AddModelError("", "No jackets found. Please add jackets first.");
                return View(); // Or redirect to another action
            }

            ViewData["JacketOwners"] = jacketOwners;
            ViewData["Jackets"] = jackets;

            return View();
        }

        // Action to handle the form submission and register the sighting
        [HttpPost]
        public async Task<IActionResult> Register(Sighting sighting)
        {
            if (ModelState.IsValid)
            {
                // TableService
                sighting.Sighting_Date = DateTime.SpecifyKind(sighting.Sighting_Date, DateTimeKind.Utc);
                sighting.PartitionKey = "SightingsPartition";
                sighting.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddSightingAsync(sighting);

                // MessageQueue
                string message = $"New sighting by Jacket Owner {sighting.JacketOwner_ID} of Jacket {sighting.Jacket_ID} at {sighting.Sighting_Location} on {sighting.Sighting_Date}";
                await _queueService.SendMessageAsync(message);

                return RedirectToAction("Index");
            }
            else
            {
                // Log model state errors
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            // Reload jacket owners and jackets lists if validation fails
            var jacketOwners = await _tableStorageService.GetAllJacketOwnersAsync();
            var jackets = await _tableStorageService.GetAllJacketsAsync();
            ViewData["JacketOwners"] = jacketOwners;
            ViewData["Jackets"] = jackets;

            return View(sighting);
        }
    }
}
