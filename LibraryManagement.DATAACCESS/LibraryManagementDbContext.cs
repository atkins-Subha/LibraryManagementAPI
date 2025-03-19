using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LibraryManagement.DATAACCESS
{
    public class LibraryManagementDbContext : DbContext
    {
        public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) : base(options) { }

        public DbSet<Role> RoleMaster { get; set; }
        public DbSet<Category> CategoryMaster { get; set; }
        public DbSet<Status> StatusMaster { get; set; }
        public DbSet<User> UserMaster { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Book> BookMaster { get; set; }
        public DbSet<BookDetails> BookDetails { get; set; }
        public DbSet<BookAlerts> BookAlerts { get; set; }
        public DbSet<BookBorrow> BookBorrow { get; set; }
        public DbSet<BookReturn> BookReturn { get; set; }
        public DbSet<BookReservation> BookReservation { get; set; }
        public DbSet<Subscription> SubscriptionMaster { get; set; }
        public DbSet<SubscriptionDetails> SubscriptionDetails { get; set; }
        public DbSet<Fine> FineMaster { get; set; }
        public DbSet<FineDetails> FineDetails { get; set; }
    }
}
