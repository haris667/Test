using Mirra.Domain.Models;

namespace Mirra.Presentation.Services
{
    public interface ITimeFormatter
    {
        string Format(TimeData time);
        bool TryParse(string input, out TimeData result);
    }
}
