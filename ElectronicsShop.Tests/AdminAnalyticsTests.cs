using ElectronicsShop.Controllers;
using ElectronicsShop.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ElectronicsShop.Tests
{
    public class AdminAnalyticsTests
    {
        [Fact]
        public void Can_Edit_Product()
        {
            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new Product[] {
            //    new Product {ProductID = 1, Name = "P1"},
            //    new Product {ProductID = 2, Name = "P2"},
            //    new Product {ProductID = 3, Name = "P3"},
            //    }.AsQueryable<Product>());
            Mock<ICatalogRepository> mockCategory = new Mock<ICatalogRepository>();
            Mock<IAnalyticsCalculate> mockAnalyticsCalculate = new Mock<IAnalyticsCalculate>();
            // Arrange - create the controller
            AdminForecastController target = new AdminForecastController (mock.Object, mockCategory.Object, mockAnalyticsCalculate.Object);
            // Act
            //Product p1 = GetViewModel<Product>(target.Edit(1));
            //Product p2 = GetViewModel<Product>(target.Edit(2));
            //Product p3 = GetViewModel<Product>(target.Edit(3));
            // Assert
            //Assert.Equal(1, p1.ProductID);
            //Assert.Equal(2, p2.ProductID);
            //Assert.Equal(3, p3.ProductID);
        }
    }
}
