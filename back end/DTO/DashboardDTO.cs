namespace School_Project_API.DTO
{
    public class DashboardDTO
    {


        public int TotalStudents { get; set; }

        public int TotalTeachers { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalCourses { get; set; }

        public int TotalClasses { get; set; }

        public int TotalSubjects { get; set; }

        // homework break down

        public int ActiveHomework { get; set; }

        public int ArchivedHomework { get; set; }

        // Today attendance 

        public int TodayPresent { get; set; }
        public int TodayAbsent { get; set; }

        public int TodayLate { get; set; }

        public List<RecentHomeworkDTO> RecentHomework
        {
            get; set;
        }

        public class RecentHomeworkDTO
        {

            public int Id { get; set; }
            public string Title { get; set; }

            public string TeacherName { get; set; }

            public string ClassName { get; set; }

            public DateTime DueDate { get; set; }

            public string Status { get; set; }

        
        }

    }
}

