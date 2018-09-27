using System.Linq;

namespace ContosoUniversity.IntegrationTests.Features.Instructors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Pages.Instructors;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_list_instructor_with_details()
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
                Id = NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = NextCourseNumber()
            };

            await InsertAsync(englishDept, english101, english201);

            var instructor1Id = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                SelectedCourses = new[] { english101.Id.ToString(), english201.Id.ToString() },
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Austin",
            });

            var instructor2Id = await SendAsync(new CreateEdit.Command
            {
                OfficeAssignmentLocation = "Houston",
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
            });

            var student1 = new Student
            {
                FirstMidName = "Cosmo",
                LastName = "Kramer",
                EnrollmentDate = DateTime.Today,
            };
            var student2 = new Student
            {
                FirstMidName = "Elaine",
                LastName = "Benes",
                EnrollmentDate = DateTime.Today
            };

            await InsertAsync(student1, student2);

            var enrollment1 = new Enrollment { StudentID = student1.Id, CourseID = english101.Id };
            var enrollment2 = new Enrollment { StudentID = student2.Id, CourseID = english101.Id };

            await InsertAsync(enrollment1, enrollment2);

            var result = await SendAsync(new Index.Query { Id = instructor1Id, CourseId = english101.Id });

            result.ShouldNotBeNull();

            result.Instructors.ShouldNotBeNull();
            result.Instructors.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.Instructors.Select(i => i.ID).ShouldContain(instructor1Id);
            result.Instructors.Select(i => i.ID).ShouldContain(instructor2Id);

            result.Courses.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);

            result.Enrollments.ShouldNotBeNull();
            result.Enrollments.Count.ShouldBe(2);
        }

    }
}