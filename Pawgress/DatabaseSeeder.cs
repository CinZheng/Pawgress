using System;
using System.Collections.Generic;
using Pawgress.Data;
using Pawgress.Models;
using BCrypt.Net;

public class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        try
        {
            Console.WriteLine("Starting database seeding...");

            // Ensure the database is created
            Console.WriteLine("Ensuring database is created...");
            context.Database.EnsureCreated();

            // Clear existing data
            Console.WriteLine("Clearing existing data...");
            context.Users.RemoveRange(context.Users);
            context.TrainingPaths.RemoveRange(context.TrainingPaths);
            context.DogProfiles.RemoveRange(context.DogProfiles);
            context.Libraries.RemoveRange(context.Libraries);
            context.Lessons.RemoveRange(context.Lessons);
            context.Quizzes.RemoveRange(context.Quizzes);
            context.UserProgress.RemoveRange(context.UserProgress);
            context.SaveChanges();
            Console.WriteLine("Existing data cleared.");

            // Create test users
            Console.WriteLine("Creating test users...");
            var users = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "admin",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "testuser",
                    Email = "user@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = "User",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
            Console.WriteLine("Test users created.");

            // Create dog profiles
            Console.WriteLine("Creating dog profiles...");
            var dogProfiles = new List<DogProfile>
            {
                new DogProfile
                {
                    DogProfileId = Guid.NewGuid(),
                    Name = "Max",
                    Breed = "Golden Retriever",
                    DateOfBirth = DateTime.UtcNow.AddYears(-2),
                    Image = "https://example.com/dog1.jpg",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                },
                new DogProfile
                {
                    DogProfileId = Guid.NewGuid(),
                    Name = "Luna",
                    Breed = "German Shepherd",
                    DateOfBirth = DateTime.UtcNow.AddYears(-1),
                    Image = "https://example.com/dog2.jpg",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            context.DogProfiles.AddRange(dogProfiles);
            context.SaveChanges();
            Console.WriteLine("Dog profiles created.");

            // Create user-dog relationships
            Console.WriteLine("Creating user-dog relationships...");
            var userDogProfiles = new List<User_DogProfile>
            {
                new User_DogProfile
                {
                    UserId = users[1].UserId,
                    DogProfileId = dogProfiles[0].DogProfileId,
                    StartDate = DateTime.UtcNow.AddMonths(-6),
                    EndDate = DateTime.UtcNow.AddYears(10),
                    IsFavorite = true,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            context.UserDogProfiles.AddRange(userDogProfiles);
            context.SaveChanges();
            Console.WriteLine("User-dog relationships created.");

            // Create lessons
            Console.WriteLine("Creating lessons...");
            var lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Name = "Basic Obedience",
                    Text = "Learn the fundamentals of dog obedience training",
                    MarkdownContent = "# Basic Obedience\n\nIn this lesson, we'll cover:\n- Sit command\n- Stay command\n- Come command",
                    Tag = "Beginner",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                },
                new Lesson
                {
                    Id = Guid.NewGuid(),
                    Name = "Advanced Commands",
                    Text = "Master advanced dog training techniques",
                    MarkdownContent = "# Advanced Commands\n\nIn this lesson, we'll cover:\n- Heel command\n- Leave it command\n- Place command",
                    Tag = "Advanced",
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            context.Lessons.AddRange(lessons);
            context.SaveChanges();
            Console.WriteLine("Lessons created.");

            // Create quizzes
            Console.WriteLine("Creating quizzes...");
            var quizzes = new List<Quiz>
            {
                new Quiz
                {
                    Id = Guid.NewGuid(),
                    Name = "Basic Obedience Quiz",
                    Description = "Test your knowledge of basic commands",
                    QuizQuestions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            QuizQuestionId = Guid.NewGuid(),
                            QuestionText = "What is the first command you should teach a puppy?",
                            CorrectAnswer = "Sit",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new QuizQuestion
                        {
                            QuizQuestionId = Guid.NewGuid(),
                            QuestionText = "How long should you hold the 'stay' command initially?",
                            CorrectAnswer = "5 seconds",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        }
                    },
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                }
            };

            context.Quizzes.AddRange(quizzes);
            context.SaveChanges();
            Console.WriteLine("Quizzes created.");

            // Create training paths
            Console.WriteLine("Creating training paths...");
            var trainingPath = new TrainingPath
            {
                TrainingPathId = Guid.NewGuid(),
                Name = "Complete Dog Training Course",
                Description = "A comprehensive course covering basic to advanced training",
                TrainingPathItems = new List<TrainingPathItemOrder>
                {
                    new TrainingPathItemOrder
                    {
                        TrainingPathItemId = lessons[0].Id,
                        Order = 1,
                        TrainingPathItem = lessons[0]
                    },
                    new TrainingPathItemOrder
                    {
                        TrainingPathItemId = quizzes[0].Id,
                        Order = 2,
                        TrainingPathItem = quizzes[0]
                    },
                    new TrainingPathItemOrder
                    {
                        TrainingPathItemId = lessons[1].Id,
                        Order = 3,
                        TrainingPathItem = lessons[1]
                    }
                },
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            context.TrainingPaths.Add(trainingPath);
            context.SaveChanges();
            Console.WriteLine("Training paths created.");

            // Create user progress
            Console.WriteLine("Creating user progress...");
            var userProgress = new List<UserProgress>
            {
                new UserProgress
                {
                    UserProgressId = Guid.NewGuid(),
                    UserId = users[1].UserId, // test user
                    TrainingPathId = trainingPath.TrainingPathId,
                    ItemId = lessons[0].Id,
                    ItemType = "Lesson",
                    IsCompleted = true,
                    CompletedDate = DateTime.UtcNow.AddDays(-1),
                    Score = 0,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new UserProgress
                {
                    UserProgressId = Guid.NewGuid(),
                    UserId = users[1].UserId, // test user
                    TrainingPathId = trainingPath.TrainingPathId,
                    ItemId = quizzes[0].Id,
                    ItemType = "Quiz",
                    IsCompleted = true,
                    CompletedDate = DateTime.UtcNow,
                    Score = 85,
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.UserProgress.AddRange(userProgress);
            context.SaveChanges();
            Console.WriteLine("User progress created.");

            Console.WriteLine("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while seeding the database:");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}
