using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Core;
using Inventory.DataAccess;

namespace Inventory.Remote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchProductController : ControllerBase
    {
        private readonly ProductSearchEngine searchEngine;

        public SearchProductController(ProductSearchEngine searchEngine)
        {
            this.searchEngine = searchEngine;
        }

        [HttpGet]
        public IEnumerable<ProductModel> Get(string search)
        {
            return searchEngine.SearchResults(search).Take(50);
        }


    }
}
