using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Infrastructure;
using ContosoUniversity.Infrastructure.Tags;
using ContosoUniversity.Pages.Instructors;
using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoUniversity
{
    internal class Program
    {
        private static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureServices((context, services) =>
                        {
                            services.AddMiniProfiler().AddEntityFramework();

                            services
                                .AddDbContext<SchoolContext>(options =>
                                    options.UseSqlServer(
                                        context.Configuration.GetConnectionString("DefaultConnection")))
                                .AddAutoMapper(typeof(Program))
                                .AddMediatR(typeof(Program))
                                .AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
                                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                                .AddHtmlTags(new TagConventions())
                                .AddRazorPages(opt =>
                                {
                                    opt.Conventions.ConfigureFilter(new DbContextTransactionPageFilter());
                                    opt.Conventions.ConfigureFilter(new ValidatorPageFilter());
                                })
                                .AddFluentValidation(
                                    cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Program>(); });

                            services.AddMvc(opt => opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider()));
                        })
                        .Configure((context, app) =>
                        {
                            app.UseMiniProfiler();

                            if (context.HostingEnvironment.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                            }
                            else
                            {
                                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                                app
                                    .UseExceptionHandler("/Error")
                                    .UseHsts();
                            }

                            app
                                .UseHttpsRedirection()
                                .UseStaticFiles()
                                .UseCookiePolicy()
                                .UseRouting()
                                .UseAuthorization()
                                .UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
                        });
                })
                .Build()
                .RunAsync();
    }
}
