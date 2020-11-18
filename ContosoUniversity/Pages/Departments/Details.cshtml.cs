using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using DelegateDecompiler.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments
{
    public class Details : PageModel
    {
        private readonly IMediator _mediator;

        public Model Data { get; private set; }

        public Details(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public record Query : IRequest<Model>
        {
            public int Id { get; init; }
        }

        public record Model
        {
            public string Name { get; init; }

            public decimal Budget { get; init; }

            public DateTime StartDate { get; init; }

            public int Id { get; init; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Model>();
        }
        
        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<Model> Handle(Query message, 
                CancellationToken token) => 
                _context.Departments
                .Where(m => m.Id == message.Id)
                .ProjectTo<Model>(_configuration)
                .DecompileAsync()
                .SingleOrDefaultAsync(token);
        }
    }
}