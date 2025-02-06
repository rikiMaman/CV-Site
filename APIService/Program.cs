using GitHubIntegration;

namespace APIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var folderPath = builder.Configuration["folderPath"];

            var token = builder.Configuration["GitHubToken"];
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<GitHubIntegrationOptions>(builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)));
            builder.Services.AddGitHubIntegration(options => builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)).Bind(options));
            //builder.Services.AddMemoryCache();
            builder.Services.AddMemoryCache();

            var app = builder.Build();

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
