using advance_csharp.dto.Request.Product;
using advance_csharp.dto.Response.Product;
using advance_csharp.service.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace advance_csharp.test
{
    [TestClass]
    public class ProductServiceTest
    {
        private readonly IProductService productService;


        public ProductServiceTest(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// GetApplicationProductList happy case request
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetApplicationProductListTestAsync()
        {
            ProductGetListRequest request = new()
            {
                PageIndex = 1,
                PageSize = 10,
                Name = string.Empty,
                Category = string.Empty,
                PriceFrom = string.Empty,
                PriceTo = string.Empty,
            };
            ProductGetListResponse response = await productService.GetApplicationProductList(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data.Count > 0);
        }

        /// <summary>
        /// GetApplicationProductListWithName happy case request
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetApplicationProductListWithNameTestAsync()
        {
            ProductGetListRequest request = new()
            {
                PageIndex = 1,
                PageSize = 10,
                Name = "Product 40183",
                Category = string.Empty,
                PriceFrom = string.Empty,
                PriceTo = string.Empty,
            };
            ProductGetListResponse response = await productService.GetApplicationProductList(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data.Count > 0);
        }

        /// <summary>
        /// GetApplicationProductListWithCategory happy case request
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetApplicationProductListWithCategoryTestAsync()
        {
            ProductGetListRequest request = new()
            {
                PageIndex = 1,
                PageSize = 10,
                Name = string.Empty,
                Category = "Cloth",
                PriceFrom = string.Empty,
                PriceTo = string.Empty,
            };
            ProductGetListResponse response = await productService.GetApplicationProductList(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data.Count > 0);
        }

        /// <summary>
        /// GetApplicationProductListWithPriceFromTo happy case request
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetApplicationProductListWithPriceFromToTestAsync()
        {
            ProductGetListRequest request = new()
            {
                PageIndex = 1,
                PageSize = 10,
                Name = string.Empty,
                Category = string.Empty,
                PriceFrom = "1000000",
                PriceTo = "1100000",
            };
            ProductGetListResponse response = await productService.GetApplicationProductList(request);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data.Count > 0);
        }
    }
}