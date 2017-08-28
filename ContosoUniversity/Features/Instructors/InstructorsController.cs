using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Instructors
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly IMediator _mediator;

        public InstructorsController(SchoolContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(Index.Query query) 
            => View(await _mediator.Send(query));

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(Details.Query query)
            => View(await _mediator.Send(query));

        public async Task<IActionResult> Create()
        {
            var model = await _mediator.Send(new CreateEdit.Query());

            return View(nameof(CreateEdit), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEdit.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<IActionResult> Edit(CreateEdit.Query query)
        {
            var model = await _mediator.Send(query);

            return View(nameof(CreateEdit), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEdit.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }
        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Instructor instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.Id == id);

            var departments = await _context.Departments
                .Where(d => d.Id == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
