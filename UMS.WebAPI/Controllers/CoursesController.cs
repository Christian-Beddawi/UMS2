using System.Web.Http.OData;
using AutoMapper;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.ClassEnrollment.Commands;
using UMS.Application.Entities.Course.Commands.InsertCourse;
using UMS.Application.Entities.Course.Queries.GetCourses;
using UMS.Application.Entities.TeacherPerCoursePerSession.Commands;
using UMS.Domain.Models;
using UMS.WebAPI.DTO;
using User = Keycloak.Net.Models.Users.User;

namespace UMS.WebAPI.Controllers;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CoursesController> _logger;
    
    public CoursesController(IMediator mediator, IMapper mapper, ILogger<CoursesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    // GET
    [HttpGet("GetCourses")]
    [EnableQuery]
    public async Task<IActionResult> GetCourse()
    {
        //NpgsqlRange<LocalDate> range = default;
        //Console.WriteLine("Date range : "+range);
        _logger.LogInformation("CoursesController Get - this is a nice message to test the logs", DateTime.UtcNow);
        return Ok(await _mediator.Send(new GetCoursesQuery()));
    }
    
    //POST
    [HttpPost("AddCourse")]
    //[TypeFilter(typeof(UserAuthorizationFilter))]
    public async Task<IActionResult> InsertCourse([FromHeader] int userId,[FromBody] CreateCourse c)
    {
        Course course = _mapper.Map<Course>(c);
        return Ok(await _mediator.Send(new InsertCourseCommand(course,userId)));
    }
}