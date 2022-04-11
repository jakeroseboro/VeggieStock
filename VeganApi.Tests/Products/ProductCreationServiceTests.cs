using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VeganAPI.Configuration;
using VeganAPI.Models.Products;
using VeganAPI.Models.Products.Enums;
using VeganAPI.Models.Products.Subclasses;
using Xunit;
using Moq;

namespace VeganApi.Tests.Products;

public class ProductCreationServiceTests : IDisposable
{
    private readonly IProductCreationService _creationService;
    private static NewProduct _newProduct = new NewProduct();
    private static Product _product = new Product();
    private IList<Guid> _ids;
    private Mock<IProductMongoSink> sink = new Mock<IProductMongoSink>();

    public ProductCreationServiceTests()
    {
        _creationService = new ProductCreationService(sink.Object);
    }
    
    public void Dispose()
    {
    }

    [Fact]
    public async void CreationServiceShouldCreateProduct()
    {
        CreateNewProduct();
        ConvertToNewProduct();
        GivenSinkReturnsProduct();
        var product = await _creationService.CreateProduct(_newProduct, CancellationToken.None);
        var value = product?.Value;
        value.Should().NotBeNull();
        AssertCorrectProduct(value);
    }

    private void AssertCorrectProduct(Product product)
    {
        _newProduct.Brand.Should().BeEquivalentTo(product.Brand);
        _newProduct.Images.Should().BeEquivalentTo(product.Images);
        _newProduct.Name.Should().BeEquivalentTo(product.Name);
        _newProduct.Sighting.Should().BeEquivalentTo(product.Sightings[0]);
        _newProduct.CreatedBy.Should().BeEquivalentTo(product.CreatedBy);
    }

    private void CreateNewProduct()
    {
        _newProduct.Brand = "Gardein";
        _newProduct.Images = new List<string> {"Base64String"};
        _newProduct.Name = "Mandarin Orange Chik'n";
        _newProduct.Sighting = new Sighting
        {
            City = "Pensacola",
            Seen = DateTime.Now,
            SpottedBy = "jakeroseboro@gmail.com",
            State = "Fl",
            Store = new Store
            {
                Name = "Walmart",
                Logo = "Base64Img"
            }
        };
        _newProduct.Type = ProductType.Vegan;
        _newProduct.CoverImage = "Base64Img";
        _newProduct.CreatedBy = "jakeroseboro@gmail.com";
    }

    private void ConvertToNewProduct()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            CoverImage = _newProduct.CoverImage,
            Brand = _newProduct.Brand,
            Type = _newProduct.Type,
            CreatedBy = _newProduct.CreatedBy,
            Images = _newProduct.Images,
            Name = _newProduct.Name,
            IsDeleted = false,
            LastSpotted = DateTime.Now,
            Sightings = new List<Sighting> {_newProduct.Sighting},
            ZipCodes = new List<int> {_newProduct.Sighting.ZipCode}
        };

        _product = product;
    }

    public void GivenSinkReturnsProduct()
    {
        sink.Setup(x => x.CreateProduct(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionResult<Product>(_product));
    }
    
}