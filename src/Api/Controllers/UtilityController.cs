using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Used for upgrading existing data
/// </summary>
namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase {
        private ModelDataContext context;

        public UtilityController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<string> Get() {
            return "Done";
        }
    }
}
