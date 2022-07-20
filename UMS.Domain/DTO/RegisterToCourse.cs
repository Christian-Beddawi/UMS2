using System.ComponentModel.DataAnnotations;

namespace UMS.WebAPI.DTO;

public class RegisterToCourse
{
    [Required(ErrorMessage = "Teacher name is required!")]
    public int TeacherId { get; set; }
    
    [Required(ErrorMessage = "Course name is required!")]
    public string CourseName { get; set; }

    [Required(ErrorMessage = "Start Time is required!")]
    public DateTime StartTime { get; set; }
    
    [Required(ErrorMessage = "End Time is required!")]
    public DateTime EndTime { get; set; }
}