using Microsoft.EntityFrameworkCore;
using Pawgress.Models;

namespace Pawgress.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DogProfile> DogProfiles { get; set; }
        public DbSet<DogSensorData> DogSensorData { get; set; }
        public DbSet<TrainingPath> TrainingPaths { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> Questions { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<TrainingPathItem> TrainingPathItems { get; set; }

        public DbSet<User_DogProfile> UserDogProfiles { get; set; }
        public DbSet<User_TrainingPath> UserTrainingPaths { get; set; }
        public DbSet<DogSensorData> DogSensorDatas { get; set; }
        public DbSet<TrainingPathItemOrder> TrainingPathItemOrders { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // N - N relation between User and DogProfile
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

            // N - N relation between User and TrainingPath
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
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 - N relationship between DogProfile and Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.DogProfile)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DogProfileId);

            // 1 - N relationship between User and Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<QuizQuestion>()
                .HasOne<Quiz>()
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 - N relationship between DogProfile and DogSensorData
            modelBuilder.Entity<DogSensorData>()
                .HasOne(ds => ds.DogProfile)
                .WithMany(dp => dp.DogSensorDatas)
                .HasForeignKey(ds => ds.DogProfileId);

            // Favorite Dog
            modelBuilder.Entity<User_DogProfile>()
                .Property(ud => ud.IsFavorite)
                .HasDefaultValue(false);

            modelBuilder.Entity<TrainingPathItemOrder>()
                .HasKey(tpio => new { tpio.TrainingPathId, tpio.TrainingPathItemId });

            modelBuilder.Entity<TrainingPathItemOrder>()
                .HasOne(tpio => tpio.TrainingPath)
                .WithMany(tp => tp.TrainingPathItems)
                .HasForeignKey(tpio => tpio.TrainingPathId);

            modelBuilder.Entity<TrainingPathItemOrder>()
                .HasOne(tpio => tpio.TrainingPathItem)
                .WithMany(tpi => tpi.TrainingPaths)
                .HasForeignKey(tpio => tpio.TrainingPathItemId);

            // Order is required
            modelBuilder.Entity<TrainingPathItemOrder>()
                .Property(tpio => tpio.Order)
                .IsRequired();

            // Configure discriminator for polymorphic TrainingPathItem
            modelBuilder.Entity<TrainingPathItem>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Lesson>("Lesson")
                .HasValue<Quiz>("Quiz");

            modelBuilder.Entity<TrainingPathItem>()
                .Property(t => t.Id)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            // Configure many-to-many between Library and Lesson
            modelBuilder.Entity<LibraryLesson>()
                .HasKey(ll => new { ll.LibraryId, ll.LessonId });

            modelBuilder.Entity<LibraryLesson>()
                .HasOne(ll => ll.Library)
                .WithMany(l => l.LibraryLessons)
                .HasForeignKey(ll => ll.LibraryId);

            modelBuilder.Entity<LibraryLesson>()
                .HasOne(ll => ll.Lesson)
                .WithMany(l => l.LibraryLessons)
                .HasForeignKey(ll => ll.LessonId);

            // Configure many-to-many between Library and Quiz
            modelBuilder.Entity<LibraryQuiz>()
                .HasKey(lq => new { lq.LibraryId, lq.QuizId });

            modelBuilder.Entity<LibraryQuiz>()
                .HasOne(lq => lq.Library)
                .WithMany(l => l.LibraryQuizzes)
                .HasForeignKey(lq => lq.LibraryId);

            modelBuilder.Entity<LibraryQuiz>()
                .HasOne(lq => lq.Quiz)
                .WithMany(q => q.LibraryQuizzes)
                .HasForeignKey(lq => lq.QuizId);

            modelBuilder.Entity<UserProgress>()
                .HasKey(up => up.UserProgressId);

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.TrainingPath)
                .WithMany()
                .HasForeignKey(up => up.TrainingPathId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
