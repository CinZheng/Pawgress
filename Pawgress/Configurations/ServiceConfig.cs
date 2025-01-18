using Pawgress.Models;
using Pawgress.Services;
using Pawgress.Repositories;

namespace Pawgress.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {

            // repository registrations
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDogProfileRepository, DogProfileRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            // specific services
            services.AddScoped<UserService>();
            services.AddScoped<TrainingPathService>();
            services.AddScoped<NoteService>();
            services.AddScoped<QuizService>();
            services.AddScoped<LessonService>();
            services.AddScoped<DogProfileService>();
            services.AddScoped<DogSensorDataService>();
            return services;
        }
    }
}
