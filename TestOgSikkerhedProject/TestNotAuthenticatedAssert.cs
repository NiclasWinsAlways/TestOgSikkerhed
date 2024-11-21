using Bunit.TestDoubles;
using Bunit;
using TestOgSikkerhed.Components.Pages;

public class TestNotAuthenticatedAssert
{
    [Fact]
    public void NotAuthenticatedTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetNotAuthorized(); // Simulate not logged in

        // Act
        var cut = ctx.RenderComponent<Home>();
        var myObject = cut.Instance;

        // Assert
        Assert.False(myObject._isAuthenticated);
        Assert.False(myObject._isAdmin); // User is not authenticated and not an admin
    }
}
