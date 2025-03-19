using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.MODELS.Constants
{
    public static class Constants
    {
        //Users
        public const string userCreatedMsg = "User created successfully!";
        public const string userNotCreatedMsg = "Unable to create user";
        public const string userUpdatedMsg = "User updated successfully!";
        public const string userNotUpdatedMsg = "Unable to update user";
        public const string userDeletedMsg = "User deleted successfully!";
        public const string userNotDeletedMsg = "Unable to delete user details!";
        public const string userAlreadyExistsMsg = "User already exists, unable to create new user!";

        //Users Details
        public const string userDetailsCreatedMsg = "User Details created successfully!";
        public const string userDetailsNotCreatedMsg = "Unable to create user details";
        public const string userDetailsUpdatedMsg = "User Details updated successfully!";
        public const string userDetailsNotUpdatedMsg = "Unable to update user details";
        public const string userDetailsDeletedMsg = "User Details deleted successfully!";
        public const string userDetailsNotDeletedMsg = "Unable to delete user details!";
        public const string userDetailsAlreadyExistsMsg = "User Details already exists, unable create new user details!";
        public const string userDetailsNotExitsMsg = "User Details not available, unable to update user details!";

        //Books
        public const string bookCreatedMsg = "Book created successfully!";
        public const string bookNotCreatedMsg = "Unable to create book";
        public const string bookUpdatedMsg = "Book updated successfully!";
        public const string bookNotUpdatedMsg = "Unable to update book";
        public const string bookDeletedMsg = "Book deleted successfully!";
        public const string bookNotDeletedMsg = "Unable to delete book details!";
        public const string bookIdInvalidMsg = "BookId is invalid!";

        //BooksDetails
        public const string booksDetailsCreatedMsg = "Book details created successfully!";
        public const string booksDetailsNotCreatedMsg = "Unable to create book details";
        public const string booksDetailsUpdatedMsg = "Book details updated successfully!";
        public const string booksDetailsNotUpdatedMsg = "Unable to update book details";
        public const string booksDetailsDeletedMsg = "Book details deleted successfully!";
        public const string booksDetailsNotDeletedMsg = "Unable to delete book details!";

        //Fine
        public const string fineCreatedMsg = "Fine created successfully!";
        public const string fineNotCreatedMsg = "Unable to create fine";
        public const string fineUpdatedMsg = "Fine updated successfully!";
        public const string ffineNotUpdatedMsg = "Unable to update fine!";
        public const string fineDeletedMsg = "Fine deleted successfully!";
        public const string fineNotDeletedMsg = "Unable to delete fine!";
        public const string fineAlreadyExistsMsg = "Fine already exists, unable to create new fine!";

        //Fine Details
        public const string fineDetailsCreatedMsg = "Fine Details created successfully!";
        public const string fineDetailsNotCreatedMsg = "Unable to create fine details!";
        public const string fineDetailsUpdatedMsg = "Fine Details updated successfully!";
        public const string ffineDetailsNotUpdatedMsg = "Unable to update fine details!";
        public const string fineDetailsDeletedMsg = "Fine Details deleted successfully!";
        public const string fineDetailsNotDeletedMsg = "Unable to delete fine details!";

        //Roles
        public const string roleCreatedMsg = "Role created successfully!";
        public const string roleNotCreatedMsg = "Unable to create role other than Admin, Librarian & User!";
        public const string roleAlreadyExistsMsg = "Role already exists, create new role!";
        public const string roleDeletedMsg = "Role deleted successfully!";
        public const string roleNotDeletedMsg = "Unable to delete role details!";

        //Book Category

        public const string categoryCreatedMsg = "Category created successfully!";
        public const string categoryNotCreatedMsg = "Unable to create category!";
        public const string categoryAlreadyExistsMsg = "Category already exists, create new category!";
        public const string categoryDeletedMsg = "Category deleted successfully!";
        public const string categoryNotDeletedMsg = "Unable to delete category details!";

        //Book Status
        public const string statusCreatedMsg = "Status created successfully!";
        public const string statusNotCreatedMsg = "Unable to create status other than Borrowed, Damaged, Lost, Overdue & Returned!";
        public const string statusAlreadyExistsMsg = "Status already exists, create new status!";
        public const string statusDeletedMsg = "Status deleted successfully!";
        public const string statusNotDeletedMsg = "Unable to delete status details!";

        //Subscription 
        public const string subscriptionCreatedMsg = "Subscription created successfully!";
        public const string subscriptionNotCreatedMsg = "Unable to create subscription other than Basic, Premium!";
        public const string subscriptionAlreadyExistsMsg = "Subscription already exists, create new subscription!";
        public const string subscriptionDeletedMsg = "Subscription deleted successfully!";
        public const string subscriptionNotDeletedMsg = "Unable to delete subscription details!";

        //Subscription Details
        public const string subscriptionDetailsCreatedMsg = "Subscription details created successfully!";
        public const string subscriptionDetailsNotCreatedMsg = "Unable to create subscription details!";
        public const string subscriptionDetailsAlreadyExistsMsg = "Subscription details already exists, create new subscription details!";
        public const string subscriptionDetailsDeletedMsg = "Subscription details deleted successfully!";
        public const string subscriptionDetailsNotDeletedMsg = "Unable to delete subscription details!";

        //BookAlerts
        public const string bookAlertsCreatedMsg = "Book alerts created successfully!";
        public const string bookAlertsNotCreatedMsg = "Unable to create alerts details";
        public const string bookAlertsUpdatedMsg = "Alert details updated successfully!";
        public const string bookAlertsNotUpdatedMsg = "Unable to update alerts details";
        public const string bookAlertsDeletedMsg = "Alert details deleted successfully!";
        public const string bookAlertsNotDeletedMsg = "Unable to delete alerts details!";


        //BookBorrow
        public const string bookBorrowCreatedMsg = "Book borrow details created successfully!";
        public const string bookBorrowNotCreatedMsg = "Unable to create book borrow details";
        public const string bookBorrowUpdatedMsg = "Book borrow details updated successfully!";
        public const string bookBorrowNotUpdatedMsg = "Unable to update book borrow details";
        public const string bookBorrowDeletedMsg = "Book borrow details deleted successfully!";
        public const string bookBorrowNotDeletedMsg = "Unable to delete book borrow details!";

        //BookReturn
        public const string bookReturnCreatedMsg = "Book return details created successfully!";
        public const string bookReturnNotCreatedMsg = "Unable to create book return details";
        public const string bookReturnUpdatedMsg = "Book return details updated successfully!";
        public const string bookReturnNotUpdatedMsg = "Unable to update book return details";
        public const string bookReturnDeletedMsg = "Book return details deleted successfully!";
        public const string bookReturnNotDeletedMsg = "Unable to delete book return details!";

        //BookReservation
        public const string bookReservationCreatedMsg = "Book reservation details created successfully!";
        public const string bookReservationNotCreatedMsg = "Unable to create book reservation details";
        public const string bookReservationUpdatedMsg = "Book reservation details updated successfully!";
        public const string bookReservationNotUpdatedMsg = "Unable to update book reservation details";
        public const string bookReservationDeletedMsg = "Book reservation details deleted successfully!";
        public const string bookReservationNotDeletedMsg = "Unable to delete book reservation details!";





        public const string UnauthorizedUserMsg = "Unauthorized User!";

        public const int SuccessCode = 1;

        public const int ErrorCode = -1;

        public const int FailCode = 0;

        public const string SuccessMsg = "Success";

        public const string FailMsg = "Failure";

        public const string NoDataFoundMsg = "No Data Found!";

        public const string AuthenticateUserMsg = "Authenticated User";

        public const string AdminRole = "Admin";

        public const string LibrarianRole = "Librarian";

        public const string MemberRole = "Member";
    }
}
