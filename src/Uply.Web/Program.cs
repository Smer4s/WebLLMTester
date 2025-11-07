using Uply.Domain;
using Uply.Infrastructure;
using Uply.Web.Filters;

namespace Uply.Web;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddDomain(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(o => o.Filters.Add<ApiExceptionFilter>());

        var app = builder.Build();

        app.Environment.IsDevelopment();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
