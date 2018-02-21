using ContosoUniversity.Pages.Departments;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class EditTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_edit_department_details()
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
            await InsertAsync(dept);

            var query = new Edit.Query
            {
                Id = dept.Id
            };

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.Administrator.Id.ShouldBe(adminId);
        }

        [Fact]
        public async Task Should_edit_department()
        {
            var adminId = await SendAsync(new Pages.Instructors.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });

            var admin2Id = await SendAsync(new Pages.Instructors.CreateEdit.Command
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
            await InsertAsync(dept);

            Edit.Command command = null;
            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                var admin2 = await FindAsync<Instructor>(admin2Id);

                command = new Edit.Command
                {
                    Id = dept.Id,
                    Name = "English",
                    Administrator = admin2,
                    StartDate = DateTime.Today.AddDays(-1),
                    Budget = 456m
                };

                await mediator.Send(command);
            });

            var result = await ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == dept.Id).Include(d => d.Administrator).SingleOrDefaultAsync());

            result.Name.ShouldBe(command.Name);
            result.Administrator.Id.ShouldBe(command.Administrator.Id);
            result.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            result.Budget.ShouldBe(command.Budget.GetValueOrDefault());
        }
    }
}