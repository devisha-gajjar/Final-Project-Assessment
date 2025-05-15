using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Entities.ViewModel;

public class AddCourseViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[A-Za-z0-9]+(?:[ .-][A-Za-z0-9]+)*$", ErrorMessage = "Course name can only contain alphabets,numbers and hyphen (-), dot (.)")]
    public string CourseName { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    public int DeptId { get; set; }
    public List<SelectListItem>? DepartmentList { get; set; }

    [Required(ErrorMessage = "Content is required.")]
    [StringLength(200, ErrorMessage = "Content cannot exceed 200 characters.")]
    [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage = "Content can only contain letters.")]
    public string Content { get; set; } = null!;

    [Required(ErrorMessage = "Credit selection is required.")]
    [Range(1, 6, ErrorMessage = "Credit must be between 1 and 6")]
    public int Credit { get; set; }
}


