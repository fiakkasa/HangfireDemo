using Swashbuckle.AspNetCore.SwaggerGen;

namespace HangfireDemo.Extensions;

public static class SwaggerExtensions
{
    public static SwaggerGenOptions MapTypeSpan(this SwaggerGenOptions options)
    {
        options.MapType<TimeSpan>(() =>
            new()
            {
                Type = "string",
                Format = "time",
                Example = OpenApiAnyFactory.CreateFromJson("\"00:01:02.0000000\"")
            }
        );

        return options;
    }
}
