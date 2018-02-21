using System;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContosoUniversity.Infrastructure
{
    public class DbContextTransactionPageFilter : IAsyncPageFilter
    {
        private readonly SchoolContext _dbContext;

        public DbContextTransactionPageFilter(SchoolContext dbContext) => _dbContext = dbContext;

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            try
            {
                await _dbContext.BeginTransactionAsync();

                var actionExecuted = await next();
                if (actionExecuted.Exception != null && !actionExecuted.ExceptionHandled)
                {
                    _dbContext.RollbackTransaction();

                }
                else
                {
                    await _dbContext.CommitTransactionAsync();

                }
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}