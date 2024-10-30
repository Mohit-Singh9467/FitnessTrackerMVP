namespace FitnessApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<Goal>? Goals { get; set; }  
        public Workout? Workout { get; set; }
        public string Password { get;  set; }
    }
}
