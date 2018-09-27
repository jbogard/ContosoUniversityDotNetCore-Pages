using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Infrastructure.Tags;
using ContosoUniversity.Pages.Instructors;
using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
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
            services.AddMiniProfiler().AddEntityFramework();

            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));
            services.AddScoped(
                typeof(IPipelineBehavior<,>), 
                typeof(TransactionBehavior<,>));
            services.AddScoped(
                typeof(IPipelineBehavior<,>), 
                typeof(LoggingBehavior<,>));

            services.AddHtmlTags(new TagConventions());

            services.AddMvc(opt =>
                {
                    opt.Filters.Add(typeof(DbContextTransactionPageFilter));
                    opt.Filters.Add(typeof(ValidatorPageFilter));
                    opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiniProfiler();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
