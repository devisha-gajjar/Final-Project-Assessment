using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entities.Model;

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

}
