using Microsoft.Extensions.DependencyInjection;
using Pawgress.Data;
using Pawgress.Repositories;
using Pawgress.Services;

namespace Pawgress.Config
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IDogProfileRepository, DogProfileRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<QuizService>();
            services.AddScoped<NoteService>();
            services.AddScoped<LessonService>();
            services.AddScoped<DogProfileService>();
            services.AddScoped<TrainingPathService>();
        }
    }
} 