using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Courses;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses
{
    [Collection(nameof(SliceFixture))]
    public class EditTests
    {
        private readonly SliceFixture _fixture;

        public EditTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_query_for_command()
        {
            var adminId = await _fixture.SendAsync(new CreateEdit.Command
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

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = _fixture.NextCourseNumber(),
                Title = "English 101"
            };
            await _fixture.InsertAsync(dept, course);

            var result = await _fixture.SendAsync(new Edit.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.Id.ShouldBe(dept.Id);
            result.Title.ShouldBe(course.Title);
        }

        [Fact]
        public async Task Should_edit()
        {
            var adminId = await _fixture.SendAsync(new CreateEdit.Command
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
            var newDept = new Department
            {
                Name = "English",
                InstructorId = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = _fixture.NextCourseNumber(),
                Title = "English 101"
            };
            await _fixture.InsertAsync(dept, newDept, course);

            Edit.Command command = default;

            await _fixture.ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                command = new Edit.Command
                {
                    Id = course.Id,
                    Credits = 5,
                    Title = "English 202",
                    Department = await ctxt.Departments.FindAsync(newDept.Id)
                };

                await mediator.Send(command);
            });

            var edited = await _fixture.FindAsync<Course>(course.Id);

            edited.ShouldNotBeNull();
            edited.DepartmentId.ShouldBe(newDept.Id);
            edited.Credits.ShouldBe(command.Credits.GetValueOrDefault());
            edited.Title.ShouldBe(command.Title);
        }
    }
}