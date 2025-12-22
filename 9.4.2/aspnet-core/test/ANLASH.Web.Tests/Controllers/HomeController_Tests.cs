using System.Threading.Tasks;
using ANLASH.Models.TokenAuth;
using ANLASH.Web.Controllers;
using Shouldly;
using Xunit;

namespace ANLASH.Web.Tests.Controllers
{
    public class HomeController_Tests: ANLASHWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}