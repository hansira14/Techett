using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories;

public class AttachmentRepository : BaseRepository, IAttachmentRepository
{
    public AttachmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}