using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Last.Bench.Coder.Beauty.World.Utility;
using Last.Bench.Coder.Beauty.World.Entity.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Last.Bench.Coder.Beauty.World.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/category")]
    [ApiVersion("1.0")]
    [EnableCors("MyCors")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllCategorys()
        {
            var Categorys = _unitOfWork.Category.GetAll();
            return Ok(Categorys);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetCategoryById(int CategoryId)
        {
            var Category = _unitOfWork.Category.GetById(CategoryId);

            if (Category == null)
                return NotFound();

            return Ok(Category);
        }

        [HttpGet]
        [Route("searchbytext")]
        public IActionResult GetAllByText(string SearchKey)
        {
            var Category = _unitOfWork.Category.GetAllByText(SearchKey);

            if (Category == null)
                return NotFound();

            return Ok(Category);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult InsertCategory(Category Category)
        {
            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Category.Status);

            if (_unitOfWork.Category.GetAll().Where(n => n.Title == Category.Title).FirstOrDefault() != null)
                return ValidationProblem("Category Name Already Exists, Please Enter Different Category Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Category is In-Valid");

            Category.DateCreated = DateTime.Now;
            Category.DateUpdated = DateTime.Now;
            _unitOfWork.Category.Add(Category);
            _unitOfWork.Complete();
            return Ok(Category);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateCategory([FromBody] Category Category)
        {
            Category CategoryToUpdate = _unitOfWork.Category.GetById(Category.CategoryId);

            if (CategoryToUpdate == null)
                return NotFound();

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Category.Status);

            if (_unitOfWork.Category.GetAll().Where(n => n.CategoryId != Category.CategoryId && n.Title == Category.Title).FirstOrDefault() != null)
                return ValidationProblem("Category Name Already Exists, Please Enter Different Category Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Category is In-Valid");

            CategoryToUpdate.Title = Category.Title;
            CategoryToUpdate.Description = Category.Description;
            CategoryToUpdate.Banner = Category.Banner;
            CategoryToUpdate.Status = Category.Status;
            CategoryToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.Category.Update(CategoryToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(Category);
        }
    }
}