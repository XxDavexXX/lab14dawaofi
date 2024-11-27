using lab12.Models;

namespace lab12.Requests
{
    public class StudentRequestV4
    {
        public int IdGrade { get; set; }
        public List<Student> Students { get; set; }
    }
}