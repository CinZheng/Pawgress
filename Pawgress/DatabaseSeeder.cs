using Pawgress.Data;
using Pawgress.Models;

public class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // user ids
        var AdminId1 = Guid.NewGuid();
        var UserId1 = Guid.NewGuid();
        var UserId2 = Guid.NewGuid();
        var UserId3 = Guid.NewGuid();
        var UserId4 = Guid.NewGuid();
        var UserId5 = Guid.NewGuid();

        var TrainingPathId1 = Guid.NewGuid(); // 3 lessen, 2 quizzes
        var TrainingPathId2 = Guid.NewGuid(); // 2 lessen, 1 quiz
        var TrainingPathId3 = Guid.NewGuid(); // Nog niet toegewezen

        // dog ids
        var DogId1 = Guid.NewGuid();
        var DogId2 = Guid.NewGuid();
        var DogId3 = Guid.NewGuid();
        var DogId4 = Guid.NewGuid();
        var DogId5 = Guid.NewGuid();

        // seed users
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { UserId = AdminId1, Username = "Admin", Email = "admin@example.com", Role = "Admin", PasswordHash = "hashedpassword" },
                new User { UserId = UserId1, Username = "User1", Email = "user1@example.com", Role = "User", PasswordHash = "hashedpassword" },
                new User { UserId = UserId2, Username = "User2", Email = "user2@example.com", Role = "User", PasswordHash = "hashedpassword" },
                new User { UserId = UserId3, Username = "User3", Email = "user3@example.com", Role = "User", PasswordHash = "hashedpassword" },
                new User { UserId = UserId4, Username = "User4", Email = "user4@example.com", Role = "User", PasswordHash = "hashedpassword" },
                new User { UserId = UserId5, Username = "User5", Email = "user5@example.com", Role = "User", PasswordHash = "hashedpassword" }
            );
            context.SaveChanges();
        }



        // seed doggies
        if (!context.DogProfiles.Any())
        {
            context.DogProfiles.AddRange(
                new DogProfile { DogProfileId = DogId1, Name = "Nacho", Breed = "Labrador Retriever", DateOfBirth = new DateTime(2022, 10, 14) },
                new DogProfile { DogProfileId = DogId2, Name = "Ollie", Breed = "Labrador Retriever", DateOfBirth = new DateTime(2022, 11, 7) },
                new DogProfile { DogProfileId = DogId3, Name = "Pebbles", Breed = "Labrador Retriever", DateOfBirth = new DateTime(2022, 11, 15) },
                new DogProfile { DogProfileId = DogId4, Name = "Danu", Breed = "Duitse Herder", DateOfBirth = new DateTime(2023, 1, 7) },
                new DogProfile { DogProfileId = DogId5, Name = "Boris", Breed = "Labrador/ Golden Retriever", DateOfBirth = new DateTime(2023, 04, 19) }
            );
            context.SaveChanges();
        }

        // seed trajecten
        if (!context.TrainingPaths.Any())
        {
            context.TrainingPaths.AddRange(
                new TrainingPath { TrainingPathId = TrainingPathId1, Name = "Traject 1", Description = "Omschrijving van traject 1." },
                new TrainingPath { TrainingPathId = TrainingPathId2, Name = "Traject 2", Description = "Omschrijving van traject 2." },
                new TrainingPath { TrainingPathId = TrainingPathId3, Name = "Traject 3", Description = "Omschrijving van traject 3." }
            );
            context.SaveChanges();
        }

        // seed lessons
        if (!context.Lessons.Any())
        {
            context.Lessons.AddRange(
                new Lesson { LessonId = Guid.NewGuid(), Name = "Les 1: Leiding geven", TrainingPathId = TrainingPathId1 },
                new Lesson { LessonId = Guid.NewGuid(), Name = "Les 2: Intuigen", TrainingPathId = TrainingPathId1 },
                new Lesson { LessonId = Guid.NewGuid(), Name = "Les 3: Indraaien zijstraten", TrainingPathId = TrainingPathId1 },
                new Lesson { LessonId = Guid.NewGuid(), Name = "Les 4: Richtingcommando's", TrainingPathId = TrainingPathId2 },
                new Lesson { LessonId = Guid.NewGuid(), Name = "Les 5: Stoppen voor stoepen", TrainingPathId = TrainingPathId2 }
            );
            context.SaveChanges();
        }

        // seed quizzes
        if (!context.Quizzes.Any())
        {
            context.Quizzes.AddRange(
                new Quiz { QuizId = Guid.NewGuid(), QuizName = "Quiz 1", TrainingPathId = TrainingPathId1 },
                new Quiz { QuizId = Guid.NewGuid(), QuizName = "Quiz 2", TrainingPathId = TrainingPathId1 },
                new Quiz { QuizId = Guid.NewGuid(), QuizName = "Quiz 3", TrainingPathId = TrainingPathId2 }
            );
            context.SaveChanges();
        }

        // seed folders
        if (!context.Folders.Any())
        {
            context.Folders.AddRange(
                new Folder { FolderId = Guid.NewGuid(), Name = "Folder 1", Description = "Lege folder 1", ParentFolderId = null },
                new Folder { FolderId = Guid.NewGuid(), Name = "Folder 2", Description = "Lege folder 2", ParentFolderId = null },
                new Folder { FolderId = Guid.NewGuid(), Name = "Folder 3", Description = "Lege folder 3", ParentFolderId = null },
                new Folder { FolderId = Guid.NewGuid(), Name = "Folder 4", Description = "Lege folder 4", ParentFolderId = null }
            );
            context.SaveChanges();
        }

        // seed notes
        if (!context.Notes.Any())
        {
            context.Notes.AddRange(
                new Note { NoteId = Guid.NewGuid(), DogProfileId = DogId1, UserId = UserId1, Tag = "Gezondheid", Date = DateTime.Now, Description = "Gezondheidscontrole uitgevoerd." },
                new Note { NoteId = Guid.NewGuid(), DogProfileId = DogId2, UserId = UserId2, Tag = "Gedrag", Date = DateTime.Now.AddDays(-10), Description = "Verbeterd gedrag opgemerkt." }
            );
            context.SaveChanges();
        }

        // seed user - dogprofile relation
        if (!context.UserDogProfiles.Any())
        {
            context.UserDogProfiles.AddRange(
                new User_DogProfile { UserId = UserId1, DogProfileId = DogId1, StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now },
                new User_DogProfile { UserId = UserId2, DogProfileId = DogId2, StartDate = DateTime.Now.AddYears(-1), EndDate = DateTime.Now.AddMonths(-3) }
            );
            context.SaveChanges();
        }

        // seed user - trainingpath relation 
        if (!context.UserTrainingPaths.Any())
        {
            context.UserTrainingPaths.AddRange(
                new User_TrainingPath { UserId = UserId1, TrainingPathId = TrainingPathId1, Progress = "50%", Status = "Active", StartDate = DateTime.Now.AddMonths(-2), CompletionDate = DateTime.MinValue },
                new User_TrainingPath { UserId = UserId2, TrainingPathId = TrainingPathId2, Progress = "30%", Status = "Active", StartDate = DateTime.Now.AddMonths(-1), CompletionDate = DateTime.MinValue }
            );
            context.SaveChanges();
        }

        // 1 bilbiotheek
        if (!context.Libraries.Any())
        {
            context.Libraries.Add(
                new Library
                {
                    LibraryId = Guid.NewGuid(),
                    Name = "Main Library",
                    Lessons = new List<Lesson>(), // Leeg voor nu
                    Quizzes = new List<Quiz>(),   // Leeg voor nu
                }
            );
            context.SaveChanges();
        }
        context.SaveChanges();
        Console.WriteLine("Seeding complete!");
    }
}
