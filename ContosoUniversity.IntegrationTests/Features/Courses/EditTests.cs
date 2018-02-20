namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    using System;
    using System.Threading.Tasks;
    using Pages.Courses;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class EditTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_query_for_command()
        {
            var adminId = await SendAsync(new Pages.Instructors.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });

            var dept = new Department
            {
                Name = "History",
                InstructorID = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            await InsertAsync(dept, course);

            var result = await SendAsync(new Edit.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.Id.ShouldBe(dept.Id);
            result.Title.ShouldBe(course.Title);
        }

        [Fact]
        public async Task Should_edit()
        {
            var adminId = await SendAsync(new Pages.Instructors.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });

            var dept = new Department
            {
                Name = "History",
                InstructorID = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var newDept = new Department
            {
                Name = "English",
                InstructorID = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            await InsertAsync(dept, newDept, course);

            Edit.Command command = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                newDept = await ctxt.Departments.FindAsync(dept.Id);
                command = new Edit.Command
                {
                    Id = course.Id,
                    Credits = 5,
                    Department = newDept,
                    Title = "English 202"
                };
                await mediator.Send(command);
            });

            var edited = await FindAsync<Course>(course.Id);

            edited.ShouldNotBeNull();
            edited.DepartmentID.ShouldBe(newDept.Id);
            edited.Credits.ShouldBe(command.Credits.GetValueOrDefault());
            edited.Title.ShouldBe(command.Title);
        }
    }
}