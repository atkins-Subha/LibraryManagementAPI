using LibraryManagement.BUSINESS.Services;
using LibraryManagement.MODELS.Constants;
using LibraryManagement.MODELS.DTOS;
using LibraryManagement.MODELS.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            Response response = new Response();
            try
            {
                var result = await _categoryService.GetAllCategoryService();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }

        //[Authorize(Roles = "Librarian")]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryRequest request)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                response.statuscode = Constants.FailCode;
                response.message = "CategoryName is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _categoryService.CreateCategoryService(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }

        }
        //[Authorize(Roles = "Librarian")]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int bookCategoryId)
        {
            Response response = new Response();
            if (bookCategoryId == 0)
            {
                response.statuscode = Constants.FailCode;
                response.message = "bookCategoryId is Required!";
                return Ok(response);
            }
            try
            {
                var result = await _categoryService.DeleteCategoryService(bookCategoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ControllerContext.ActionDescriptor.ControllerName + " -> " + ControllerContext.ActionDescriptor.ActionName + " -> {err}", ex.Message);
                response.statuscode = Constants.ErrorCode;
                response.message = ex.Message;
                return Ok(response);
            }
        }

    }
}
