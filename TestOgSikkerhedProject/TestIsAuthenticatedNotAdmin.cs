using Bunit.TestDoubles;
using Bunit;
using TestOgSikkerhed.Components.Pages;

public class TestIsAuthenticatedNotAdmin
{
    [Fact]
    public void IsAuthenticatedNotAdminTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("test", AuthorizationState.Authorized); // Simulate logged-in user
        // No roles assigned

        // Act
        var cut = ctx.RenderComponent<Home>();
        var myObject = cut.Instance;

        // Assert
        Assert.True(myObject._isAuthenticated);
        Assert.False(myObject._isAdmin); // User is authenticated but not an admin
    }
}
