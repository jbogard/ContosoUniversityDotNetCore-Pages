using System.Threading.Tasks;
using ContosoUniversity.Pages.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Features.Departments
{
    public class DepartmentsController : Controller
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator) => _mediator = mediator;

        public ActionResult Create() 
            => View(new Create.Command());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Create.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToPageJson("/departments/index");
        }

        public async Task<ActionResult> Edit(Edit.Query query) 
            => View(await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Edit.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToPageJson("/departments/index");
        }

        public async Task<ActionResult> Delete(Delete.Query query) 
            => View(await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToPageJson("/departments/index");
        }
    }
}
