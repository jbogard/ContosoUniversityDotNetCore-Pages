using System;
using System.Collections.Generic;
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
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public List<Model> Data { get; private set; }

        public async Task OnGetAsync()
            => Data = await _mediator.Send(new Query());

        public record Query : IRequest<List<Model>>
        {
        }

        public record Model
        {
            public string Name { get; init; }

            public decimal Budget { get; init; }

            public DateTime StartDate { get; init; }

            public int Id { get; init; }

            public string AdministratorFullName { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Model>();
        }

        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly SchoolContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext context, 
                IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<List<Model>> Handle(Query message, 
                CancellationToken token) => _context
                .Departments
                .ProjectTo<Model>(_configuration)
                .DecompileAsync()
                .ToListAsync(token);
        }
    }
}