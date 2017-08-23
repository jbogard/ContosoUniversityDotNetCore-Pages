using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Departments
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly IMediator _mediator;

        public DepartmentsController(SchoolContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: Departments
        public async Task<IActionResult> Index() 
            => View(await _mediator.Send(new Index.Query()));

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(Details.Query query) 
            => View(await _mediator.Send(query));

        public ActionResult Create() 
            => View(new Create.Command());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Create.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<ActionResult> Edit(Edit.Query query) 
            => View(await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Edit.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<ActionResult> Delete(Delete.Query query) 
            => View(await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }
    }
}
