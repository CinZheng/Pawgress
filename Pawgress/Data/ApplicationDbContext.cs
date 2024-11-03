using Microsoft.EntityFrameworkCore;
using Pawgress.Models;

namespace Pawgress.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DogProfile> DogProfiles { get; set; }
        public DbSet<TrainingPath> TrainingPaths { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<QuizPage> QuizPages { get; set; }

        public DbSet<User_DogProfile> UserDogProfiles { get; set; }
        public DbSet<User_TrainingPath> UserTrainingPaths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // N - N relation tussen User DogProfile
            modelBuilder.Entity<User_DogProfile>()
                .HasKey(ud => new { ud.UserId, ud.DogProfileId });

            modelBuilder.Entity<User_DogProfile>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.DogProfiles)
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<User_DogProfile>()
                .HasOne(ud => ud.DogProfile)
                .WithMany(d => d.UserDogProfiles)
                .HasForeignKey(ud => ud.DogProfileId);

            // N - N relation User en TrainingPath
            modelBuilder.Entity<User_TrainingPath>()
                .HasKey(ut => new { ut.UserId, ut.TrainingPathId });

            modelBuilder.Entity<User_TrainingPath>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.TrainingPaths)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<User_TrainingPath>()
                .HasOne(ut => ut.TrainingPath)
                .WithMany(tp => tp.Users)
                .HasForeignKey(ut => ut.TrainingPathId);

            // Folder - self relational mapping
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentFolderId);

            // 1 - N relatie tussen DogProfile en Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.DogProfile)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DogProfileId);

            // 1 - N relatie tussen User en Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);

            // 1 - N relatie tussen TrainingPath en Lesson
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.TrainingPath)
                .WithMany(tp => tp.Lessons)
                .HasForeignKey(l => l.TrainingPathId);

            // 1 - N relatie tussen Lesson en Page
            modelBuilder.Entity<Page>()
                .HasOne(p => p.Lesson)
                .WithMany(l => l.Pages)
                .HasForeignKey(p => p.LessonId);

            // 1 - N Quiz en QuizPage
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne()
                .HasForeignKey(qp => qp.QuizId);
        }
    }
}
