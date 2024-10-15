using EllipticCurve;
using System.Text;

namespace CustomEmailSender.Services
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-Api-Key";
        public const string SIGNATURE_HEADER = "X-Twilio-Email-Event-Webhook-Signature";
        public const string TIMESTAMP_HEADER = "X-Twilio-Email-Event-Webhook-Timestamp";
        private readonly string _apiKey;
        private readonly string _expectedApiKeyTwilio;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration.GetValue<string>("ApiSettings:ApiKey");
            _expectedApiKeyTwilio = configuration.GetValue<string>("SendGrid:WebHookApiKey");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(SIGNATURE_HEADER, out var signature))
            {
                var timestamp = context.Request.Headers[TIMESTAMP_HEADER];
                using var reader = new StreamReader(context.Request.Body);
                var requestBody = await reader.ReadToEndAsync();

                if(!VerifySignature(_expectedApiKeyTwilio, requestBody, signature, timestamp))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid Twilio signature.");
                    return;
                }
                var requestData = Encoding.UTF8.GetBytes(requestBody);
                context.Request.Body = new MemoryStream(requestData);

                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) || _apiKey != extractedApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: API Key is missing or invalid.");
                return;
            }
            await _next(context);
        }

        public bool VerifySignature(string verificationKey, string payload, string signature, string timestamp)
        {
            var publicKey = PublicKey.fromPem(verificationKey);

            var timestampedPayload = timestamp + payload;
            var decodedSignature = Signature.fromBase64(signature);

            return Ecdsa.verify(timestampedPayload, decodedSignature, publicKey);
        }
    }
}
