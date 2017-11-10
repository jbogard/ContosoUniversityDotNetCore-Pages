using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Features.Instructors
{
    public class InstructorsController : Controller
    {
        private readonly IMediator _mediator;

        public InstructorsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query) 
            => View(await _mediator.Send(query));

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(Details.Query query)
            => View(await _mediator.Send(query));

        public async Task<IActionResult> Create() 
            => View(nameof(CreateEdit), await _mediator.Send(new CreateEdit.Query()));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEdit.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<IActionResult> Edit(CreateEdit.Query query) 
            => View(nameof(CreateEdit), await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEdit.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }
        public async Task<IActionResult> Delete(Delete.Query query) 
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
