using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Departments;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Departments
{
    
    [Collection(nameof(SliceFixture))]
    public class EditTests
    {
        private readonly SliceFixture _fixture;

        public EditTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_get_edit_department_details()
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
            await _fixture.InsertAsync(dept);

            var query = new Edit.Query
            {
                Id = dept.Id
            };

            var result = await _fixture.SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.Administrator.Id.ShouldBe(adminId);
        }

        [Fact]
        public async Task Should_edit_department()
        {
            var adminId = await _fixture.SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today
            });

            var admin2Id = await _fixture.SendAsync(new CreateEdit.Command
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
            await _fixture.InsertAsync(dept);

            Edit.Command command = null;
            await _fixture.ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                var admin2 = await _fixture.FindAsync<Instructor>(admin2Id);

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

            var result = await _fixture.ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == dept.Id).Include(d => d.Administrator).SingleOrDefaultAsync());

            result.Name.ShouldBe(command.Name);
            result.Administrator.Id.ShouldBe(command.Administrator.Id);
            result.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            result.Budget.ShouldBe(command.Budget.GetValueOrDefault());
        }
    }
}