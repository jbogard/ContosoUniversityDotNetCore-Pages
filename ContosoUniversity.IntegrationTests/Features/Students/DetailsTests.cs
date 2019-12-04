using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using ContosoUniversity.Pages.Students;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using Details = ContosoUniversity.Pages.Students.Details;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class DetailsTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_details()
        {
            var adminId = await SendAsync(new CreateEdit.Command
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
            await InsertAsync(englishDept);
            var deptId = englishDept.Id;

            var course1 = new Course
            {
                DepartmentId = deptId,
                Credits = 10,
                Id = NextCourseNumber(),
                Title = "Course 1"
            };
            var course2 = new Course
            {
                DepartmentId = deptId,
                Credits = 10,
                Id = NextCourseNumber(),
                Title = "Course 2"
            };
            await InsertAsync(course1, course2);

            var command = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = new DateTime(2013, 1, 1)
            };
            var studentId = await SendAsync(command);

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
            await InsertAsync(enrollment1, enrollment2);

            var details = await SendAsync(new Details.Query {Id = studentId});

            details.ShouldNotBeNull();
            details.FirstMidName.ShouldBe(command.FirstMidName);
            details.LastName.ShouldBe(command.LastName);
            details.EnrollmentDate.ShouldBe(command.EnrollmentDate.GetValueOrDefault());
            details.Enrollments.Count.ShouldBe(2);
        }
    }
}