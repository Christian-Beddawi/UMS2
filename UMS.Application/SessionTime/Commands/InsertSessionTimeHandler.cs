using MediatR;
using UMS.Persistence;

namespace UMS.Application.Entities.SessionTime.Commands;

public class InsertSessionTimeHandler :IRequestHandler<InsertSessionTimeCommand,string>
{
    private readonly umsContext _context = new umsContext();
    public async Task<string> Handle(InsertSessionTimeCommand request, CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync();
        return "Session Added Successfully!!";
    }
}