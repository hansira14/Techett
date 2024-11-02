using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ArticleViewModel
{
    public int ArticleId { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
    
    public int CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class ArticleDetailViewModel
{
    public ArticleViewModel Article { get; set; }
    public IEnumerable<ArticleVersionViewModel> Versions { get; set; }
}

public class ArticleVersionViewModel
{
    public int VersionId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime VersionDate { get; set; }
    public string VersionedByName { get; set; }
} 