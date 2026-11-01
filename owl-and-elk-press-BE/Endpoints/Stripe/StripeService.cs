using System.Collections.Generic;
using Stripe.Checkout;

namespace owl_and_elk_press_BE.Endpoints;

public interface IStripeService
{
    string CreateCheckoutSession(string priceId);
}

public class StripeService : IStripeService
{
    public string CreateCheckoutSession(string priceId)
    {
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = "https://owlandelkpress.com{CHECKOUT_SESSION_ID}",
            CancelUrl = "https://owlandelkpress.com",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions { Price = priceId, Quantity = 1 }
            }
        };

        var service = new SessionService();
        Session session = service.Create(options);
        return session.Url;
    }
}