using ContentApi.DataAccess;
using ContentApi.DataAccess.Repositories;
using ContentApi.Core.Abstractions.Repositories;
using ContentApi.WebHost.Mapping;
using ContentApi.DataAccess.Data;

namespace ContentApi.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<DatabaseSettings>(
                builder.Configuration.GetSection("ContentStoreDatabase"));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(ContentRepository<>));
            builder.Services.AddScoped<DataContext>();
            builder.Services.AddScoped<IDbInitializer, ContentInitializer>();
            builder.Services.AddScoped<IContentMapping, ContentMapping>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider.GetRequiredService<IDbInitializer>;
                services.Invoke().InitializeDb();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}