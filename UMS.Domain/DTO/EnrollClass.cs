using System.ComponentModel.DataAnnotations;

namespace UMS.WebAPI.DTO;

public class EnrollClass
{
    [Required(ErrorMessage = "Student id is required!")]
    public int StudentId { get; set; }
 
    [Required(ErrorMessage = "ClassId name required!")]
    public string ClassName { get; set; }

    [Required(ErrorMessage = "Teacher name is required!")]
    public string TeacherName { get; set; }
    
    [Required(ErrorMessage = "Start Time is required!")]
    public DateTime StartTime { get; set; }
    
    [Required(ErrorMessage = "End Time is required!")]
    public DateTime EndTime { get; set; }

}