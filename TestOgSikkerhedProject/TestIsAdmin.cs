using Bunit.TestDoubles;
using Bunit;
using TestOgSikkerhed.Components.Pages;

public class TestIsAdmin
{
    [Fact]
    public void IsAdminTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("test", AuthorizationState.Authorized); // Simulate logged-in user
        authContext.SetRoles("Admin"); // Assign "Admin" role

        // Act
        var cut = ctx.RenderComponent<Home>();
        var myObject = cut.Instance;

        // Assert
        Assert.True(myObject._isAuthenticated);
        Assert.True(myObject._isAdmin);
    }
}
