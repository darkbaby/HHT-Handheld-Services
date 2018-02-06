using FSBT_HHT_Services.Models;
using FSBT_HHT_Services.Modules;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace FSBT_HHT_Services.Controllers
{
    public class StocktakingsController : ApiController
    {
        private DBModule _db = new DBModule();

        public StocktakingsController()
        {
            APIConstant.Instance.Init();
        }

        [HttpGet]
        public IEnumerable<Stocktaking> GetAllStocktakings()
        {
            return _db.GetAllStocktaking();
        }

        [HttpPost]
        public IHttpActionResult InsertStocktaking(Stocktaking s)
        {
            bool returnValue = _db.InsertStocktaking(s);
            if (returnValue)
            {
                return new MyActionResult("S", HttpStatusCode.OK, Request);
            }
            else
            {
                return new MyActionResult("F", HttpStatusCode.OK, Request);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateStocktaking(Stocktaking s)
        {
            bool returnValue = _db.UpdateStocktaking(s);
            if (returnValue)
            {
                return new MyActionResult("S", HttpStatusCode.OK, Request);
            }
            else
            {
                return new MyActionResult("F", HttpStatusCode.OK, Request);
            }
        }
    }
}
