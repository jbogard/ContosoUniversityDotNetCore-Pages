using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;
using Index = ContosoUniversity.Pages.Courses.Index;

namespace ContosoUniversity.IntegrationTests.Features.Courses
{
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_return_all_courses()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
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
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = NextCourseNumber(),
                Title = "History 101"
            };
            await InsertAsync(englishDept, historyDept, english, history);

            var result = await SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBeGreaterThanOrEqualTo(2);
        }
    }
}