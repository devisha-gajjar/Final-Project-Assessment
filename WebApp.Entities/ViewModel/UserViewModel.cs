namespace WebApp.Entities.ViewModel;

public class UserViewModel
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime EnrollDate { get; set; }
    public DateTime RegisterDate { get; set; }
    public string PhoneNumber { get; set; }
    public int TotalCourse { get; set; } = 0;
    public int TotalCredits { get; set; } = 0;
}
