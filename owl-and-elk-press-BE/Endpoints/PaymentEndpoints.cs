using Microsoft.AspNetCore.Mvc.Infrastructure;
using Stripe;
using Stripe.Checkout;

namespace owl_and_elk_press_BE.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/payment");

        group.MapPost("/create-checkout-session", (CheckoutRequest request) =>
        {
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = "https://owlandelkpress.com{CHECKOUT_SESSION_ID}",
                CancelUrl = "https://owlandelkpress.com",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = request.PriceId,
                        Quantity = 1,
                    },
                },
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Results.Ok(new { url = session.Url });
        }).RequireCors("AllowFrontend");
    }

    public record CheckoutRequest(string PriceId)
    { 
        public string PriceId { get; init; } = !string.IsNullOrEmpty(PriceId)
            ? PriceId
            : throw new ArgumentException("Price ID is required.", nameof(PriceId));
    }
}