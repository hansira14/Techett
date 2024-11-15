using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;

public class ArticleAttachmentService : IArticleAttachmentService
{
    private readonly IArticleAttachmentRepository _articleAttachmentRepository;

    public ArticleAttachmentService(IArticleAttachmentRepository articleAttachmentRepository)
    {
        _articleAttachmentRepository = articleAttachmentRepository;
    }
}