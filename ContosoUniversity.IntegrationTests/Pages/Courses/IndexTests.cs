using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Courses
{
    using ContosoUniversity.Pages.Courses;

    [Collection(nameof(SliceFixture))]
    public class IndexTests
    {
        private readonly SliceFixture _fixture;

        public IndexTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_return_all_courses()
        {
            var adminId = await _fixture.SendAsync(
                new ContosoUniversity.Pages.Instructors.CreateEdit.Command
                {
                    FirstMidName = "George",
                    LastName = "Jones",
                    HireDate = DateTime.Today
                });

            var englishDept = new Department
            {
                Name = "English",
                InstructorId = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var historyDept = new Department
            {
                Name = "History",
                InstructorId = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var english = new Course
            {
                Credits = 4,
                Department = englishDept,
                Id = _fixture.NextCourseNumber(),
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = _fixture.NextCourseNumber(),
                Title = "History 101"
            };
            await _fixture.InsertAsync(
                englishDept, 
                historyDept, 
                english, 
                history);

            var result = await _fixture.SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBeGreaterThanOrEqualTo(2);

            var courseIds = result.Courses.Select(c => c.Id).ToList();
            courseIds.ShouldContain(english.Id);
            courseIds.ShouldContain(history.Id);
        }
    }
}