using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models.Product.Queries;
using Inventory.Domain.Models;
using MediatR;
using Serilog;

namespace Application.WPF.WebScraping.ProductUpdates
{
    public class CheckProductForUpdatesCommand : IRequest<ProductModel>
    {
        public ProductModel ModelToCheck { get; private set; }

        public CheckProductForUpdatesCommand(ProductModel modelToCheck)
        {
            ModelToCheck = modelToCheck;
        }
    }

    public class CheckProductForUpdatesCommandHandler :
        IRequestHandler<CheckProductForUpdatesCommand,ProductModel>
    {
        private readonly ProductUpdateRule _productUpdateRule;
        private readonly IProductModelQuery _modelQuery;
        private readonly ProductScraper _productScraper;
        


        public CheckProductForUpdatesCommandHandler(ProductUpdateRule productUpdateRule, IProductModelQuery modelQuery, ProductScraper productScraper)
        {
            _productUpdateRule = productUpdateRule;
            _modelQuery = modelQuery;
            _productScraper = productScraper;
        }

        public async Task<ProductModel> Handle(CheckProductForUpdatesCommand request,
            CancellationToken cancellationToken)
        {
            if (!_productUpdateRule.DoesProductRequireUpdate(request.ModelToCheck))
                return await Task.FromResult(request.ModelToCheck);

            
            
            try
            {
                var updatedModel =
                    await _productScraper
                        .GetProductModelFromUrlAsync(request.ModelToCheck.URL);
                updatedModel.ID = request.ModelToCheck.ID;

               await _modelQuery.UpdateAsync(updatedModel);
               return updatedModel;
            }
            catch (Exception e)
            {
                Log.Error(e,"Unable to update product ID:{ID}, from {URL}", request.ModelToCheck.ID,
                    request.ModelToCheck.URL);
                
                await _modelQuery.UpdateAsync(request.ModelToCheck);
                return request.ModelToCheck;
            }


        }
    }



}