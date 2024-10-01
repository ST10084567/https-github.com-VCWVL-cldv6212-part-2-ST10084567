using Microsoft.AspNetCore.Mvc;

namespace CLOUD_STORAGE_2.Controllers
{
    public class TestController : Controller
    {
        // GET: /Test/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Test/GetJson
        [HttpGet]
        public IActionResult GetJson()
        {
            var testData = new
            {
                Id = 1,
                Name = "Test Data",
                Description = "This is a sample JSON response"
            };

            return Json(testData);
        }

        // GET: /Test/Status
        [HttpGet]
        public IActionResult Status()
        {
            return Ok("The API is working correctly.");
        }

        // GET: /Test/CustomStatus/{statusCode}
        [HttpGet]
        public IActionResult CustomStatus(int statusCode)
        {
            return StatusCode(statusCode, $"Custom status code: {statusCode}");
        }
    }
}

