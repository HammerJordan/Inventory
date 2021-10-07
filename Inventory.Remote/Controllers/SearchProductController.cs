using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.Application.Core.Models.Product.Commands;
using Inventory.Domain.Models;
using MediatR;

namespace Inventory.Remote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductModel>> Get(string search)
        {
            var searchCommand = new ProductSearchCommand(search);

            var models = await _mediator.Send(searchCommand);
            return models;
        }


    }
}
