namespace lab12.Models
{
    public class Grade
    {
        public int GradeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; } = true;
    }
}
