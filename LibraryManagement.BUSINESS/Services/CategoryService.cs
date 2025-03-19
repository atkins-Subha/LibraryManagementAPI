using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.BUSINESS.Services
{
    
  public interface ICategoryService
    {

        Task<IEnumerable<Category>> GetAllCategoryService();
        Task<Category> GetCategoryByIdService(int categoryId);
        Task<Category> CreateByCategoryService(Category category);
        Task UpdateCategoryService(Category category);
        Task DeleteCategoryService(Category category);

        Task<Response> CreateCategoryService(CategoryRequest request);
        Task<Response> DeleteCategoryService(int categoryId);

    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;

        public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryService()
        {
            return await _categoryRepository.GetAllCategoryRepo();
        }

        public async Task<Category> GetCategoryByIdService(int categoryId)
        {
            return await _categoryRepository.GetCategoryByIdRepo(categoryId);
        }


        public async Task<Category> CreateByCategoryService(Category category)
        {
            return await _categoryRepository.CreateCategoryRepo(category);
        }

        public async Task UpdateCategoryService(Category category)
        {
            await _categoryRepository.UpdateCategoryRepo(category);
        }
        public async Task DeleteCategoryService(Category category)
        {
            await _categoryRepository.DeleteCategoryRepo(category);
        }

        public async Task<Response> CreateCategoryService(CategoryRequest request)
        {
            Response response = new Response();
            try
            {
                var dbCategory = _categoryRepository.GetAllCategoryRepo().Result.FirstOrDefault(p => p.CategoryName.Trim().ToLower() == request.CategoryName.Trim().ToLower());

                if ((dbCategory == null))
                {

                    var category = new Category
                    {
                        CategoryName = request.CategoryName,
                    };
                    Category newCategory =  await _categoryRepository.CreateCategoryRepo(category);

                    if (newCategory != null)
                    {
                        response.statuscode = Constants.SuccessCode;
                        response.message = Constants.categoryCreatedMsg;
                        return response;
                    }
                    response.statuscode = Constants.FailCode;
                    response.message = Constants.categoryNotCreatedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.categoryAlreadyExistsMsg;
                return response;
            }
            catch (Exception ex)
            {
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
            }
            return response;

        }


        public async Task<Response> DeleteCategoryService(int categoryId)
        {
            Response response = new Response();
            try
            {
                Category category = await _categoryRepository.GetCategoryByIdRepo(categoryId);
                if (category != null)
                {
                    await _categoryRepository.DeleteCategoryRepo(category);
                    response.statuscode = Constants.SuccessCode;
                    response.message = Constants.categoryDeletedMsg;
                    return response;
                }
                response.statuscode = Constants.FailCode;
                response.message = Constants.categoryNotDeletedMsg;
                return response;

            }
            catch (Exception ex)
            {
                response.statuscode = -1;
                response.message = ex.InnerException.Message;
                return response;
            }

            return response;
        }
    }
}
