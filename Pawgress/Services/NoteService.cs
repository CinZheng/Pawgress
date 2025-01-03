using Pawgress.Data;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class NoteService : BaseService<Note>
    {
        public NoteService(ApplicationDbContext context) : base(context) { }
    }
}
