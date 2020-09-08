using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Instructors
{
    
    [Collection(nameof(SliceFixture))]
    public class DetailsTests
    {
        private readonly SliceFixture _fixture;

        public DetailsTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_get_instructor_details()
        {
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };
            await _fixture.InsertAsync(englishDept, english101);

            var command = new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
                SelectedCourses = new[] { english101.Id.ToString() }
            };
            var instructorId = await _fixture.SendAsync(command);

            var result = await _fixture.SendAsync(new Details.Query { Id = instructorId });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(command.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(command.OfficeAssignmentLocation);
        }
    }
}