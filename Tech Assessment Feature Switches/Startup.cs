using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Tech_Assessment_Feature_Switches.Models;

namespace Tech_Assessment_Feature_Switches
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
            services.AddDbContext<FeatureAccessContext>(opt =>
                                               opt.UseInMemoryDatabase("FeatureAccessList"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech_Assessment_Feature_Switches", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech_Assessment_Feature_Switches v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<FeatureAccessContext>();
                // Seed the database.
                AddTestData(context);
            }
        }

        private static void AddTestData(FeatureAccessContext context)
        {
            var firstRecord = new FeatureAccess
            {
                Email = "testUser1",
                FeatureName = "Export",
                Enable = true
            };
        
            context.FeatureAccess.Add(firstRecord);
        
            var secondRecord = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Export",
                Enable = true
            };
        
            context.FeatureAccess.Add(secondRecord);

            var thirdRecord = new FeatureAccess
            {
                Email = "testUser1",
                FeatureName = "Import",
                Enable = true
            };
        
            context.FeatureAccess.Add(thirdRecord);
        
            var fourthRecord = new FeatureAccess
            {
                Email = "testUser2",
                FeatureName = "Delete",
                Enable = false
            };
        
            context.FeatureAccess.Add(fourthRecord);

            var fifthRecord = new FeatureAccess
            {
                Email = "testUser3",
                FeatureName = "Delete",
                Enable = false
            };
        
            context.FeatureAccess.Add(fifthRecord);
        
            context.SaveChanges();
        }
    }
}
