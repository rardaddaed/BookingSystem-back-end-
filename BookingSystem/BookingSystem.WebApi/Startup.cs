using Autofac;
using BookingSystem.Application.BookingLevelBL.Queries;
using BookingSystem.Application.Infrastructure.SignalR;
using BookingSystem.Core.Constants;
using BookingSystem.Persistence;
using BookingSystem.WebApi.Filters.Exceptions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO.Compression;
using System.Reflection;

namespace BookingSystem.WebApi
{
  public partial class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      Log.Information($"Database: {Configuration.GetConnectionString(Constants.BS_DB_NAME)}");

      services.AddRouting(options => options.LowercaseUrls = true);

      #region CORS

      var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();

      services.AddCors(options => options.AddPolicy("AllowedOrigins", builder =>
      {
        builder.AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
          .WithOrigins(allowedOrigins)
          .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
      }));

      #endregion CORS

      #region global exception filter, fluent validation

      services.AddMvc(options =>
        {
          options.Filters.Add(typeof(GlobalExceptionFilter));
          options.ModelValidatorProviders.Clear();
          options.EnableEndpointRouting = false;
        })
        .AddFluentValidation();

      #endregion global exception filter, fluent validation

      services.AddDbContext<BSDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString(Constants.BS_DB_NAME),
          o => o.MigrationsAssembly(typeof(BSDbContext).Namespace)));

      services.AddSignalR();
      services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
      services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
      services.AddMemoryCache();
      services.AddMediatR(typeof(GetAllBookingLevelsQuery));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem.WebApi", Version = "v1" });
      });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.Register(_ => Configuration).As<IConfiguration>();
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (!env.IsProduction())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingSystem.WebApi v1"));
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseCors("AllowedOrigins");
      app.UseResponseCompression();
      app.UseSerilogRequestLogging();
      app.UseMvc();

      // If the app calls UseStaticFiles, place UseStaticFiles before UseRouting.
      // It's important that you place the Authentication and Authorization middleware between UseRouting and UseEndPoints .
      // Any middleware that appears after the UseRouting() call will know which endpoint will run eventually.
      // Any middleware that appears before the UseRouting() call won't know which endpoint will run eventually.
      app.UseRouting();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<BookingHub>("/booking-hub");
      });
    }
  }
}