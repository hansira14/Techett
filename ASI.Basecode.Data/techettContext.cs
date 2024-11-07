using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class techettContext : DbContext
    {
        public techettContext()
        {
        }

        public techettContext(DbContextOptions<techettContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleVersion> ArticleVersions { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Update> Updates { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=techett;User Id=sa;Password=YourStrongPassword!;MultipleActiveResultSets=true;TrustServerCertificate=True;Trusted_Connection=True;integrated security=false");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.ArticleId).HasColumnName("articleID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Articles__create__3A4CA8FD");
            });

            modelBuilder.Entity<ArticleVersion>(entity =>
            {
                entity.HasKey(e => e.VersionId)
                    .HasName("PK__ArticleV__E772A4B08B14B4E4");

                entity.Property(e => e.VersionId).HasColumnName("versionID");

                entity.Property(e => e.ArticleId).HasColumnName("articleID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.VersionDate)
                    .HasColumnName("versionDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VersionedBy).HasColumnName("versionedBy");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.ArticleVersions)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArticleVe__artic__45BE5BA9");

                entity.HasOne(d => d.VersionedByNavigation)
                    .WithMany(p => p.ArticleVersions)
                    .HasForeignKey(d => d.VersionedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArticleVe__versi__47A6A41B");
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.Property(e => e.AssignmentId).HasColumnName("assignmentID");

                entity.Property(e => e.AssignedBy).HasColumnName("assignedBy");

                entity.Property(e => e.AssignedOn)
                    .HasColumnName("assignedOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AssignedTo).HasColumnName("assignedTo");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.HasOne(d => d.AssignedByNavigation)
                    .WithMany(p => p.AssignmentAssignedByNavigations)
                    .HasForeignKey(d => d.AssignedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assignmen__assig__395884C4");

                entity.HasOne(d => d.AssignedToNavigation)
                    .WithMany(p => p.AssignmentAssignedToNavigations)
                    .HasForeignKey(d => d.AssignedTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assignmen__assig__3864608B");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assignmen__ticke__01142BA1");
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachment");

                entity.Property(e => e.AttachmentId).HasColumnName("attachmentID");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("filename");

                entity.Property(e => e.Filesize).HasColumnName("filesize");

                entity.Property(e => e.Filetype)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("filetype");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("source");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.UploadedBy).HasColumnName("uploadedBy");

                entity.Property(e => e.UploadedOn)
                    .HasColumnName("uploadedOn")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attachmen__ticke__503BEA1C");

                entity.HasOne(d => d.UploadedByNavigation)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.UploadedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attachmen__uploa__51300E55");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("commentID");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasColumnName("comment");

                entity.Property(e => e.CommentedOn)
                    .HasColumnName("commentedOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__ticket__14270015");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__userID__2FCF1A8A");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");

                entity.Property(e => e.AgentId).HasColumnName("agentID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Remarks).HasColumnName("remarks");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.FeedbackAgents)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__agentI__339FAB6E");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__ticket__22751F6C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FeedbackUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__userID__32AB8735");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotifId)
                    .HasName("PK__Notifica__F2770D061E8BE9E0");

                entity.Property(e => e.NotifId).HasColumnName("notifID");

                entity.Property(e => e.AgentId).HasColumnName("agentID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsRead).HasColumnName("isRead");

                entity.Property(e => e.IsSent).HasColumnName("isSent");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("message");

                entity.Property(e => e.SentOn).HasColumnName("sentOn");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.NotificationAgents)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__agent__42E1EEFE");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__ticke__0E6E26BF");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__userI__41EDCAC5");
            });

            modelBuilder.Entity<Preference>(entity =>
            {
                entity.Property(e => e.PreferenceId).HasColumnName("preferenceID");

                entity.Property(e => e.IsEmailingOn)
                    .IsRequired()
                    .HasColumnName("isEmailingOn")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.ViewType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("viewType")
                    .HasDefaultValueSql("('list')");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Preferences)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Preferenc__userI__3493CFA7");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                entity.Property(e => e.SessionId).HasColumnName("sessionID");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpiresOn).HasColumnName("expiresOn");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Session__userID__2EDAF651");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Teams__72E12F1BD11E4B21")
                    .IsUnique();

                entity.Property(e => e.TeamId).HasColumnName("teamID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("category");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnName("dueDate");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.ResolvedBy).HasColumnName("resolvedBy");

                entity.Property(e => e.ResolvedOn).HasColumnName("resolvedOn");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedOn).HasColumnName("updatedOn");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TicketCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tickets__created__3587F3E0");

                entity.HasOne(d => d.ResolvedByNavigation)
                    .WithMany(p => p.TicketResolvedByNavigations)
                    .HasForeignKey(d => d.ResolvedBy)
                    .HasConstraintName("FK_Tickets_Users_ResolvedBy");
            });

            modelBuilder.Entity<Update>(entity =>
            {
                entity.Property(e => e.UpdateId).HasColumnName("updateID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.TicketId).HasColumnName("ticketID");

                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updatedOn")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.Updates)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Updates__ticketI__09A971A2");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.Updates)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Updates__updated__30C33EC3");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__tmp_ms_x__AB6E6164238652F2")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fname");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("role");

                entity.Property(e => e.TeamId).HasColumnName("teamID");

                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

                entity.Property(e => e.UpdatedOn).HasColumnName("updatedOn");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Users__createdBy__3B40CD36");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK__Users__teamID__367C1819");

                entity.HasOne(d => d.UpdatedByNavigation)
                    .WithMany(p => p.InverseUpdatedByNavigation)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__Users__updatedBy__2DE6D218");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
