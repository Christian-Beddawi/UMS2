using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using NodaTime;
using NpgsqlTypes;

namespace UMS.WebAPI.DTO;

public class CreateCourse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id;
    
    [Required(ErrorMessage = "Course Name is required!")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Max number of students is required!")]
    public int MaxStudentsNumber { get; set; }
    
    [Required(ErrorMessage = "Date range should be specified is required!")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "Date range should be specified is required!")]
    public DateTime EndDate { get; set; }
    
    //DateInterval
}
