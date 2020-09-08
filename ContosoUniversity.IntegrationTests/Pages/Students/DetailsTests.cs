using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using ContosoUniversity.Pages.Students;
using Shouldly;
using Xunit;
using Details = ContosoUniversity.Pages.Students.Details;

namespace ContosoUniversity.IntegrationTests.Pages.Students
{
    [Collection(nameof(SliceFixture))]
    public class DetailsTests
    {
        private readonly SliceFixture _fixture;

        public DetailsTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_get_details()
        {
            var adminId = await _fixture.SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today
            });

            var englishDept = new Department
            {
                InstructorId = adminId,
                Budget = 123m,
                Name = "English 101",
                StartDate = DateTime.Today
            };
            await _fixture.InsertAsync(englishDept);
            var deptId = englishDept.Id;

            var course1 = new Course
            {
                DepartmentId = deptId,
                Credits = 10,
                Id = _fixture.NextCourseNumber(),
                Title = "Course 1"
            };
            var course2 = new Course
            {
                DepartmentId = deptId,
                Credits = 10,
                Id = _fixture.NextCourseNumber(),
                Title = "Course 2"
            };
            await _fixture.InsertAsync(course1, course2);

            var command = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = new DateTime(2013, 1, 1)
            };
            var studentId = await _fixture.SendAsync(command);

            var enrollment1 = new Enrollment
            {
                CourseId = course1.Id,
                Grade = Grade.A,
                StudentId = studentId
            };
            var enrollment2 = new Enrollment
            {
                CourseId = course2.Id,
                Grade = Grade.F,
                StudentId = studentId
            };
            await _fixture.InsertAsync(enrollment1, enrollment2);

            var details = await _fixture.SendAsync(new Details.Query {Id = studentId});

            details.ShouldNotBeNull();
            details.FirstMidName.ShouldBe(command.FirstMidName);
            details.LastName.ShouldBe(command.LastName);
            details.EnrollmentDate.ShouldBe(command.EnrollmentDate.GetValueOrDefault());
            details.Enrollments.Count.ShouldBe(2);
        }
    }
}