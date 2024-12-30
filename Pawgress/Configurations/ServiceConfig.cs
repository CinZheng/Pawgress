using Pawgress.Models;
using Pawgress.Services;

namespace Pawgress.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // generic services
            services.AddScoped<BaseService<DogProfile>>();
            services.AddScoped<BaseService<Lesson>>();
            services.AddScoped<BaseService<Library>>();
            services.AddScoped<BaseService<Quiz>>();

            // specific services
            services.AddScoped<UserService>();
            services.AddScoped<TrainingPathService>();
            services.AddScoped<NoteService>();
            services.AddScoped<FolderService>();
            services.AddScoped<QuizService>();
            services.AddScoped<LessonService>();

            return services;
        }
    }
}
