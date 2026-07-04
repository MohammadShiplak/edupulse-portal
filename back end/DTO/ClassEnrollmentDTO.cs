namespace School_Project_API.DTO
{
    public class ClassEnrollmentDTO
    {

       public int StudentId { get; set; }

        public string StudentName { get; set; } 

        public int ClassId { get; set; }    


        public string ClassName { get; set; }   


        public int CurrentEnrollment { get; set; }

        public int Capacity { get; set; }

        public DateTime EnrolledAt {  get; set; }   



    }
}
