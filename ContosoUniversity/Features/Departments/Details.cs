using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Departments
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

        }

        public class QueryHandler : AsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _context;

            public QueryHandler(SchoolContext context) => _context = context;

            protected override Task<Model> HandleCore(Query message) => _context.Departments
                .FromSql(@"SELECT * FROM Department WHERE DepartmentID = {0}", message.Id)
                .ProjectTo<Model>()
                .SingleOrDefaultAsync();
        }
    }
}