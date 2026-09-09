using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using owl_and_elk_press_BE.Endpoints;
using Xunit;

namespace owl_and_elk_press_BE.Tests;

public class FakeStripeService : IStripeService
{
    public string CreateCheckoutSession(string priceId)
    {
        return "https://stripe.com";
    }
}

public class PaymentEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public PaymentEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateCheckoutSession_WithValidPriceId_ShouldReturn200AndFakeUrl()
    {
        var client = _factory.CreateClient();
        var validPayload = new PaymentEndpoints.CheckoutRequest(PriceId : "price_12345");

        var response = await client.PostAsJsonAsync("/api/v1/payment/create-checkout-session", validPayload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var jsonResult = await response.Content.ReadFromJsonAsync<TestCheckoutResponse>();
        Assert.NotNull(jsonResult);
        Assert.Equal("https://stripe.com", jsonResult.Url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async Task CreateCheckoutSession_WithMissingPriceId_ShouldReturn400BadRequest()
    {
        var client = _factory.CreateClient();
        
        var exception = Assert.Throws<System.ArgumentException>(() =>
        {
            var invalidPayload = new PaymentEndpoints.CheckoutRequest(PriceId: "");
        });
        
        Assert.Equal("Price ID is required. (Parameter 'PriceId')", exception.Message);    
    } 
}

public record TestCheckoutResponse(string Url);
