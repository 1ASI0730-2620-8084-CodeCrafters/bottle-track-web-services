using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BottleTrack.API.Tests.Integration;

public class AuthenticationEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory factory;

    public AuthenticationEndpointsTests(ApiFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task SignIn_WithRegisteredUser_ReturnsToken()
    {
        HttpClient client = factory.CreateClient();

        var organization = new { businessName = "CodeCrafters EIRL", taxId = "20123456789", email = "contact@codecrafters.pe", phone = "999888777", address = "Lima" };
        HttpResponseMessage organizationResponse = await client.PostAsJsonAsync("/api/v1/organizations", organization);
        Assert.Equal(HttpStatusCode.Created, organizationResponse.StatusCode);
        JsonElement organizationBody = await organizationResponse.Content.ReadFromJsonAsync<JsonElement>();
        string organizationId = organizationBody.GetProperty("id").GetString()!;

        var user = new { organizationId = organizationId, firstName = "Angel", lastName = "Crispin", email = "angel@codecrafters.pe", password = "Password123", role = "administrator" };
        HttpResponseMessage userResponse = await client.PostAsJsonAsync("/api/v1/users", user);
        Assert.Equal(HttpStatusCode.Created, userResponse.StatusCode);

        var credentials = new { email = "angel@codecrafters.pe", password = "Password123" };
        HttpResponseMessage signInResponse = await client.PostAsJsonAsync("/api/v1/tokens", credentials);

        Assert.Equal(HttpStatusCode.OK, signInResponse.StatusCode);
        JsonElement signInBody = await signInResponse.Content.ReadFromJsonAsync<JsonElement>();
        string token = signInBody.GetProperty("token").GetString()!;
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task SignIn_WithWrongPassword_ReturnsUnauthorized()
    {
        HttpClient client = factory.CreateClient();

        var organization = new { businessName = "CodeCrafters Logistics", taxId = "20999999999", email = "ops@codecrafters.pe", phone = "999000111", address = "Callao" };
        HttpResponseMessage organizationResponse = await client.PostAsJsonAsync("/api/v1/organizations", organization);
        JsonElement organizationBody = await organizationResponse.Content.ReadFromJsonAsync<JsonElement>();
        string organizationId = organizationBody.GetProperty("id").GetString()!;

        var user = new { organizationId = organizationId, firstName = "Maria", lastName = "Pezo", email = "maria@codecrafters.pe", password = "Password123", role = "fleet_supervisor" };
        await client.PostAsJsonAsync("/api/v1/users", user);

        var credentials = new { email = "maria@codecrafters.pe", password = "WrongPassword" };
        HttpResponseMessage signInResponse = await client.PostAsJsonAsync("/api/v1/tokens", credentials);

        Assert.Equal(HttpStatusCode.Unauthorized, signInResponse.StatusCode);
    }
}
