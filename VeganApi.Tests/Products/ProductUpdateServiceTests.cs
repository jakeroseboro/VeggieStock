using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeganAPI.Models.Products;
using VeganAPI.Models.Products.Subclasses;
using Xunit;

namespace VeganApi.Tests.Products;

public class ProductUpdateServiceTests
{
    private readonly IProductUpdateService _updateService;
    private Product _product = new Product();
    private IList<Product> _products = new List<Product>();
    private readonly Mock<IMongoProductSource> _source = new Mock<IMongoProductSource>();
    private readonly Mock<IProductMongoSink> _sink = new Mock<IProductMongoSink>();
    private Guid Id1 = Guid.NewGuid();

    public ProductUpdateServiceTests()
    {
        _updateService = new ProductUpdateService(_sink.Object,_source.Object);
    }
    
    
    [Fact]
    public async void QueryServiceShouldGetCorrectProductWithParams()
    {
        GivenSinkReturnsProduct();
        var result = await _updateService.UpdateProduct(new ProductUpdateOptions
        {
            Id = Guid.NewGuid(),
            Sighting = new Sighting
            {
                SpottedBy = "jake"
            }
        }, CancellationToken.None);
        result.Value.Should().NotBeNull();
        AssertSourceCall();
    }

    private void GivenSinkReturnsProduct()
    {
        _source.Setup(x => x.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_product);
        _sink.Setup(x =>
                x.UpdateProduct(It.IsAny<Product>(), It.IsAny<ProductUpdateOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_product);
    }

    private void AssertSourceCall()
    {
        _sink.Verify(x => x.UpdateProduct(It.IsAny<Product>(), It.IsAny<ProductUpdateOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}