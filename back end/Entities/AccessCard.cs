using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    public class AccessCard
    {

        public int Id { get; set; }

        public string SerialNo { get; set; }

        public DateTime ExpirationDate { get; set; }


        public Student Student { get; set; }

    }
}
