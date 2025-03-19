
using LibraryManagement.BUSINESS.Services;
using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Models;

namespace LibraryManagement.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureBusinessService(this IServiceCollection services)
        {
            //2. Dependency Injection
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserDetailsService, UserDetailsService>();
            services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IBookDetailsService, BookDetailsService>();
            services.AddScoped<IBookDetailsRepository, BookDetailsRepository>();

            services.AddScoped<IBookAlertsService, BookAlertsService>();
            services.AddScoped<IBookAlertsRepository, BookAlertsRepository>();

            services.AddScoped<IBookBorrowService, BookBorrowService>();
            services.AddScoped<IBookBorrowRepository, BookBorrowRepository>();

            services.AddScoped<IBookReturnService, BookReturnService>();
            services.AddScoped<IBookReturnRepository, BookReturnRepository>();

            services.AddScoped<IBookReservationService, BookReservationService>();
            services.AddScoped<IBookReservationRepository, BookReservationRepository>();

            services.AddScoped<IFineService, FineService>();
            services.AddScoped<IFineRepository, FineRepository>();

            services.AddScoped<IFineDetailsService, FineDetailsService>();
            services.AddScoped<IFineDetailsRepository, FineDetailsRepository>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IStatusRepository, StatusRepository>();

            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            services.AddScoped<ISubscriptionDetailsService, SubscriptionDetailsService>();
            services.AddScoped<ISubscriptionDetailsRepository, SubscriptionDetailsRepository>();
        }



    }
}
