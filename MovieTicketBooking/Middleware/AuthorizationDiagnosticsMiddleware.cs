namespace MovieTicketBooking.Middleware;

public class AuthorizationDiagnosticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationDiagnosticsMiddleware> _logger;

    public AuthorizationDiagnosticsMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationDiagnosticsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "\n========== AUTHORIZATION PROCESS ==========\n"
        );

        await _next(context);

        _logger.LogInformation(
            "Final Response Status: {StatusCode}",
            context.Response.StatusCode
        );
    }
}


