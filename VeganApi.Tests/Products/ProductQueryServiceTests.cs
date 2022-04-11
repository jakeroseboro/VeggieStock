using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Products;
using VeganAPI.Models.Products.Enums;
using VeganAPI.Models.Products.Subclasses;
using Xunit;
using Moq;

namespace VeganApi.Tests.Products;

public class ProductQueryServiceTests
{
    private readonly IProductQueryService _queryService;
    private Product _product = new Product();
    private IList<Product> _products = new List<Product>();
    private readonly Mock<IMongoProductSource> _source = new Mock<IMongoProductSource>();
    private Guid Id1 = Guid.NewGuid();
    private Guid Id2 = Guid.NewGuid();
    private Guid Id3 = Guid.NewGuid();

    public ProductQueryServiceTests()
    {
        _queryService = new ProductQueryService(_source.Object);
    }
    
    
    [Fact]
    public async void QueryServiceShouldGetCorrectProductWithParams()
    {
        var query = CreateQuery();
        GivenSourceReturnsProduct();
        var result = await _queryService.GetProducts(query, CancellationToken.None);
        result.Value.Should().NotBeNull();
        result.Value.Where(x => x.Name == "Bef").Should().NotBeNull();
        AssertSourceCallsWithQuery();
    }
    
    [Fact]
    public async void QueryServiceShouldGetCorrectProductWithId()
    {
        var query = CreateQuery();
        UpdateProduct();
        GivenSourceReturnsProduct();
        var result = await _queryService.GetProductById(Id1, CancellationToken.None);
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(Id1);
        AssertSourceCallById();
    }

    private ProductQueryOptions CreateQuery()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Bef"
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Chickn"
        };
        
        _products.Add(product);
        _products.Add(product2);

        return new ProductQueryOptions
        {
            Name = "Bef"
        };
    }

    private void UpdateProduct()
    {
        _product.Id = Id1;
    }

    private void GivenSourceReturnsProduct()
    {
        _source.Setup(x => x.GetProducts(It.IsAny<ProductQueryOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionResult<IList<Product>>(_products));
        
        _source.Setup(x => x.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionResult<Product>(_product));
    }

    private void AssertSourceCallsWithQuery()
    {
        _source.Verify(x => x.GetProducts(It.IsAny<ProductQueryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private void AssertSourceCallById()
    {
        _source.Verify(x => x.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}