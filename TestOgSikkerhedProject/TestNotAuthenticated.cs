using Bunit;
using Bunit.TestDoubles;
using TestOgSikkerhed.Components.Pages;

namespace TestOgSikkerhedProject
{
    public class TestNotAuthenticated
    {
        [Fact]
        public void NotAuthenticatedTest()
            
        {
            //Arrange 
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetRoles("Admin");

            // Act
            var cut = ctx.RenderComponent<Home>();
            var myObject = cut.Instance;

            // Assert
            cut.MarkupMatches("<div>Du er IKKE logget ind (from code)</div>");
            //Assert.Equal
        }
    }
}