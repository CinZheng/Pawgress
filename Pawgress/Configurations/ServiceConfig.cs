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
            services.AddScoped<BaseService<Note>>();

            // specific services
            services.AddScoped<UserService>();
            services.AddScoped<TrainingPathService>();
            services.AddScoped<NoteService>();
            services.AddScoped<FolderService>();
            services.AddScoped<QuizService>();
            services.AddScoped<LessonService>();
            services.AddScoped<DogProfileService>(); 
            services.AddScoped<DogSensorData>(); 
            return services;
        }
    }
}
