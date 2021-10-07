using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Product.Queries;
using Inventory.Domain.Models;
using MediatR;

namespace Inventory.Application.Core.Models.Product.Commands
{
    public class ProductSearchCommand : IRequest<IEnumerable<ProductModel>>
    {
        public string Search { get; set; }

        public ProductSearchCommand(string search)
        {
            Search = search;
        }
    }

    public class ProductSearchCommandHandler : IRequestHandler<ProductSearchCommand, IEnumerable<ProductModel>>
    {
        private readonly IProductSearchQuery _searchQuery;

        public ProductSearchCommandHandler(IProductSearchQuery searchQuery)
        {
            _searchQuery = searchQuery;
        }

        public async Task<IEnumerable<ProductModel>> Handle(ProductSearchCommand request,
            CancellationToken cancellationToken)
        {
            return await _searchQuery.SearchAsync(request.Search, cancellationToken);
        }
    }
}