using Domain.Entities;

namespace Application.Interfaces
{
    public interface IActionService
    {
        IEnumerable<string> GetAllowedActions(CardDetails card);
    }
}
