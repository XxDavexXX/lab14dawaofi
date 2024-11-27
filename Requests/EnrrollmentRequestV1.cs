namespace lab12.Requests
{
    public class EnrrollmentRequestV1
    {
        public int IdStudent { get; set; }
        public List<int> CourseIDs { get; set; }

    }
}
