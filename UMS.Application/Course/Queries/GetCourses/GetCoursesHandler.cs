using System.Reflection.Metadata.Ecma335;
using MediatR;
using UMS.Persistence;

namespace UMS.Application.Entities.Course.Queries.GetCourses;

public class GetCoursesHandler : IRequestHandler<GetCoursesQuery,List<Domain.Models.Course>>
{
    private readonly umsContext _context = new umsContext();
    public async Task<List<Domain.Models.Course>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        return _context.Courses.ToList();
        throw new NotImplementedException();
    }
}