using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Courses;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    using static SliceFixture;

    public class CreateTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_create_new_course()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today
            });

            var dept = new Department
            {
                Name = "History",
                InstructorId = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };


            Create.Command command = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                await ctxt.Departments.AddAsync(dept);
                command = new Create.Command
                {
                    Credits = 4,
                    Department = dept,
                    Number = NextCourseNumber(),
                    Title = "English 101"
                };
                await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == command.Number).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DepartmentId.ShouldBe(dept.Id);
            created.Credits.ShouldBe(command.Credits);
            created.Title.ShouldBe(command.Title);
        }
    }
}