using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Stripe;
using Stripe.Checkout;

namespace owl_and_elk_press_BE.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/payment");

        group.MapPost("/create-checkout-session", (CheckoutRequest request, IStripeService stripeService) =>
        {
            string checkoutUrl = stripeService.CreateCheckoutSession(request.PriceId);
            return Results.Ok(new { url = checkoutUrl });
        }).RequireCors("AllowFrontend");
    }

    public record CheckoutRequest(string PriceId)
    { 
        public string PriceId { get; init; } = !string.IsNullOrEmpty(PriceId)
            ? PriceId
            : throw new ArgumentException("Price ID is required.", nameof(PriceId));
    }
}