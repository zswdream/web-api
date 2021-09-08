using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SwitDish.Common.Interfaces;
using SwitDish.Common.Logger;
using SwitDish.Common.Repositories;
using SwitDish.Common.Services;
using SwitDish.Customer.Services;
using SwitDish.Customer.Services.Interfaces;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace SwitDish.Customer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Enable CORS for any host
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                });
            });

            // Register Azure Application Insights Service
            services.AddApplicationInsightsTelemetry();

            // Register Azure Blob Storage Service
            services.AddSingleton(sp => { return CloudStorageAccount.Parse(this.Configuration.GetValue<string>("switdishcustomerimages-connectionstring")).CreateCloudBlobClient(); });
            services.AddScoped<IAzureBlobStorageService, AzureBlobService>();

            // Register General Services
            services.AddSingleton<INotificationService, SendGridEmailService>();

            // Register Repositories
            services.AddTransient<IRepository<DataModel.Models.ApplicationUser>,ApplicationUserRepository>();
            services.AddTransient<IRepository<DataModel.Models.Customer>,CustomerRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerBooking>,CustomerBookingRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerDeliveryAddress>,CustomerDeliveryAddressRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerSecurityQuestion>,CustomerSecurityQuestionRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerFavouriteVendor>,CustomerFavouriteVendorRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerOrder>,CustomerOrderRepository>();
            services.AddTransient<IRepository<DataModel.Models.Vendor>,VendorRepository>();
            services.AddTransient<IRepository<DataModel.Models.VendorGalleryImage>,VendorGalleryImageRepository>();
            services.AddTransient<IRepository<DataModel.Models.VendorOffer>,VendorOfferRepository>();
            services.AddTransient<IRepository<DataModel.Models.SecurityQuestion>,SecurityQuestionRepository>();
            services.AddTransient<IRepository<DataModel.Models.CustomerOrderFeedback>, CustomerOrderFeedbackRepository>();
            services.AddTransient<IRepository<DataModel.Models.Product>, ProductRepository>();
            services.AddTransient<IRepository<DataModel.Models.VendorFeedbackReaction>, VendorFeedbackReactionRepository>();
            services.AddTransient<IRepository<DataModel.Models.ProductCategory>, ProductCategoryRepository>();

            // Register Data Services
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerBookingService, CustomerBookingService>();
            services.AddTransient<ICustomerDeliveryAddressService, CustomerDeliveryAddressService>();
            services.AddTransient<ICustomerFavouriteVendorService, CustomerFavouriteVendorService>();
            services.AddTransient<ICustomerOrderService, CustomerOrderService>();
            services.AddTransient<ICustomerSecurityQuestionService, CustomerSecurityQuestionService>();
            services.AddTransient<IVendorService, VendorService>();
            services.AddTransient<IVendorGalleryImageService, VendorGalleryImageService>();
            services.AddTransient<IVendorOfferService, VendorOfferService>();
            services.AddTransient<ISecurityQuestionService, SecurityQuestionService>();
            services.AddTransient<ICustomerOrderFeedbackService, CustomerOrderFeedbackService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IVendorFeedbackReactionService, VendorFeedbackReactionService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();

            // NLog: Support aspnet-user-authtype/aspnet-user-identity
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerManager, LoggerManager>();

            // Register DbContext
            services.AddDbContext<DataModel.Models.SwitDishDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("SwitDishDatabase")));

            // Register Authentication Scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt_Auth:ValidIssuer"],
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt_Auth:SecretKey"]))
                };
            });

            // Register AutoMapper
            services.AddAutoMapper(config => 
                config.AddMaps("SwitDish.Customer.API", "SwitDish.Customer.Services", "SwitDish.Common"));

            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Register Swagger
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "SwitDish.Customer.API", Version = "v1" }));

            // Add Session Service
            services.AddSession(opt=> {
                opt.IdleTimeout = new TimeSpan(0, Configuration.GetValue<int>("SessionTimeout"), 0);
                opt.IOTimeout = new TimeSpan(0, Configuration.GetValue<int>("SessionTimeout"), 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Run all pending migrations
            UpdateDatabase(app, Configuration);

            // Enable and Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwitDish Customer API V1");
            });

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
        }

        private static void UpdateDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<SwitDish.DataModel.Models.SwitDishDbContext>();

            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                context.Database.Migrate();

                // Seed Data
                //if (configuration.GetValue<Boolean>("SeedTestData", false))
                //    Common.SeedData.SeedTestData(app);
            }
        }
    }
}
