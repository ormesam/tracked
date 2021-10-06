using Api.Utility;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Used for upgrading existing data
/// </summary>
namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase {
        private DbFactory dbFactory;

        public UtilityController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult<string> Get() {
            return "Done";
        }
    }
}
