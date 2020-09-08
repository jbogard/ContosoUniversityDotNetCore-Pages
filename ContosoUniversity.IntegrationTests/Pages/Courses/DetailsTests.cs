using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;
using Details = ContosoUniversity.Pages.Courses.Details;

namespace ContosoUniversity.IntegrationTests.Pages.Courses
{
    [Collection(nameof(SliceFixture))]
    public class DetailsTests
    {
        private readonly SliceFixture _fixture;

        public DetailsTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_query_for_details()
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

            var result = await _fixture.SendAsync(new Details.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }
    }
}