using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContosoUniversity.Pages.Instructors;
using DelegateDecompiler;

namespace ContosoUniversity.Models
{
    public class Instructor : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column("FirstName")]
        [StringLength(50)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Computed]
        public string FullName => LastName + ", " + FirstMidName;

        public ICollection<CourseAssignment> CourseAssignments { get; private set; } = new List<CourseAssignment>();
        public OfficeAssignment OfficeAssignment { get; private set; }

        public void Handle(CreateEdit.Command message,
            IEnumerable<Course> courses)
        {
            UpdateDetails(message);

            UpdateInstructorCourses(message.SelectedCourses, courses);
        }

        public void Handle(Delete.Command message) => OfficeAssignment = null;

        private void UpdateDetails(CreateEdit.Command message)
        {
            FirstMidName = message.FirstMidName;
            LastName = message.LastName;
            HireDate = message.HireDate.GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(message.OfficeAssignmentLocation))
            {
                OfficeAssignment = null;
            }
            else if (OfficeAssignment == null)
            {
                OfficeAssignment = new OfficeAssignment
                {
                    Location = message.OfficeAssignmentLocation
                };
            }
            else
            {
                OfficeAssignment.Location = message.OfficeAssignmentLocation;
            }
        }

        private void UpdateInstructorCourses(string[] selectedCourses, IEnumerable<Course> courses)
        {
            if (selectedCourses == null)
            {
                CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHs = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (CourseAssignments.Select(c => c.CourseId));

            foreach (var course in courses)
            {
                if (selectedCoursesHs.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        CourseAssignments.Add(new CourseAssignment { Course = course, Instructor = this });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        var toRemove = CourseAssignments.Single(ci => ci.CourseId == course.Id);
                        CourseAssignments.Remove(toRemove);
                    }
                }
            }
        }
    }
}