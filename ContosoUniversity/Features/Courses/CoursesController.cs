using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Features.Courses
{
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator) => _mediator = mediator;

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _mediator.Send(new Index.Query());
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(Details.Query query)
        {
            var course = await _mediator.Send(query);

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<IActionResult> Delete(Delete.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }
    }
}
