using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetProductReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsAllProducts))
                .Options;

            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productProvider = new ProductProvider(dbContext, null, mapper);

            var product = await productProvider.GetProductsAsync();

            Assert.True(product.IsSuccess);
            Assert.True(product.products.Any());

            Assert.Null(product.ErrorMessage);
        }

        //[Fact]
        //public async Task GetProductReturnsProductUsingValidId()
        //{
        //    var options = new DbContextOptionsBuilder<ProductsDbContext>()
        //        .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidId))
        //        .Options;

        //    var dbContext = new ProductsDbContext(options); 

        //    var productProfile = new ProductProfile();
        //    var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
        //    var mapper = new Mapper(configuration);

        //    var productProvider = new ProductProvider(dbContext, null, mapper);

        //    var product = await productProvider.GetProductAsync(1);

        //    Assert.True(product.IsSuccess);
        //    Assert.NotNull(product.product);
        //    Assert.True(product.product.Id == 1);
        //    Assert.Null(product.ErrorMessage);
        //}
        [Fact]
        public async Task GetProductReturnsProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInValidId))
                .Options;

            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productProvider = new ProductProvider(dbContext, null, mapper);

            var product = await productProvider.GetProductAsync(-1);

            Assert.False(product.IsSuccess);
            Assert.Null(product.product);
            Assert.NotNull(product.ErrorMessage);
        }
        private void CreateProducts(ProductsDbContext dbContext)
        {
           
           for (int i=1;i<=10;i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id=i,
                    Name=Guid.NewGuid().ToString(),
                    Inventory=i+10,
                    Price=(decimal)(i*3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
