using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DATAACCESS.Repository
{
    public interface IBookReservationRepository : IRepositoryBase<BookReservation>
    {
        Task<IEnumerable<BookReservation>> GetAllBookReservationRepo();
        Task<BookReservation> GetBookReservationByIdRepo(int bookReservationId);
        Task<BookReservation> CreateBookReservationRepo(BookReservation bookReservation);
        Task UpdateBookReservationRepo(BookReservation bookReservation);
        Task DeleteBookReservationRepo(BookReservation bookReservation);
    }
    public class BookReservationRepository : RepositoryBase<BookReservation>, IBookReservationRepository
    {
        public BookReservationRepository(LibraryManagementDbContext dbcontext)
           : base(dbcontext)
        {
        }

        public async Task<IEnumerable<BookReservation>> GetAllBookReservationRepo()
        {

            return await FindAllBaseRepo()
                .Include(e => e.User)
                .Include(e => e.Book)
                .Include(e => e.Status)
            .ToListAsync();

        }

        public async Task<BookReservation> GetBookReservationByIdRepo(int bookReservationId)
        {
            return await FindByConditionBaseRepo(e => e.BookReservationId.Equals(bookReservationId))
                .Include(e => e.User)
                .Include(e => e.Book)
                .Include(e => e.Status)
            .FirstOrDefaultAsync();
        }

        public async Task<BookReservation> CreateBookReservationRepo(BookReservation bookReservation)
        {
            return await CreateBaseRepo(bookReservation);
        }
        public async Task UpdateBookReservationRepo(BookReservation bookReservation)
        {
            await UpdateBaseRepo(bookReservation);
        }

        public async Task DeleteBookReservationRepo(BookReservation bookReservation)
        {
            await DeleteBaseRepo(bookReservation);
        }
    }
}
