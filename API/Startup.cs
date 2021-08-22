using API.Data;
using API.Data.Repositories;
using API.Services;
using API.Services.ThirdParty;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API
{
  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>(options => options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
      services.AddControllers();

      services.AddScoped<IDateTimeService, DateTimeService>();
      services.AddScoped<IOrderRepository, OrderRepository>();
      services.AddScoped<ICoffeeService, CoffeeService>();
      AddOpenWeatherIntegration(services);

      services.AddHttpClient();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    private void AddOpenWeatherIntegration(IServiceCollection services)
    {
      OpenWeatherConfig openWeatherConfig = new();
      _config.GetSection("OpenWeather").Bind(openWeatherConfig);
      services.AddSingleton(openWeatherConfig);
      services.AddScoped<IOpenWeatherService, OpenWeatherService>();
    }
  }
}
