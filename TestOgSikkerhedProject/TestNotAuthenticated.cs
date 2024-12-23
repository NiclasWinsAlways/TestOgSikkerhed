using Bunit.TestDoubles;
using Bunit;
using TestOgSikkerhed.Components.Pages;

public class TestNotAuthenticated
{
    [Fact]
    public void NotAuthenticatedMarkupTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetNotAuthorized(); // Simulate not logged in

        // Act
        var cut = ctx.RenderComponent<Home>();

        // Assert
        cut.MarkupMatches("<div>Du er IKKE logget ind (from code)</div>");
    }
}
