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
    [Route("api/v{version:apiVersion}/store")]
    [ApiVersion("1.0")]
    [EnableCors("MyCors")]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public StoreController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllStores()
        {
            var stores = _unitOfWork.Store.GetAll();
            return Ok(stores);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetStoreById(int StoreId)
        {
            var Store = _unitOfWork.Store.GetById(StoreId);

            if (Store == null)
                return NotFound();

            return Ok(Store);
        }

        [HttpGet]
        [Route("getbyzipcode")]
        public IActionResult GetStoreByZipCode(string ZipCode)
        {
            var Store = _unitOfWork.Store.GetAllByZipCode(ZipCode);

            if (Store == null)
                return NotFound();

            return Ok(Store);
        }

        [HttpGet]
        [Route("searchbytext")]
        public IActionResult GetAllByText(string SearchKey)
        {
            var Store = _unitOfWork.Store.GetAllByText(SearchKey);

            if (Store == null)
                return NotFound();

            return Ok(Store);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult InsertStore([FromForm] Store StoreModel)
        {
            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(StoreModel.Status);

            if (_unitOfWork.Store.GetAll().Where(n => n.StoreName == StoreModel.StoreName).FirstOrDefault() != null)
                return ValidationProblem("Store Name Already Exists, Please Enter Different Store Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Store is In-Valid");

            if (StoreModel.ImageFile != null)
            {
                DeleteImage(StoreModel.Banner);
                StoreModel.Banner = SaveImage(StoreModel.ImageFile);
            }

            StoreModel.DateCreated = DateTime.Now;
            StoreModel.DateUpdated = DateTime.Now;
            _unitOfWork.Store.Add(StoreModel);
            _unitOfWork.Complete();
            return Ok(StoreModel);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateStore([FromForm] Store StoreModel)
        {
            Store storeToUpdate = _unitOfWork.Store.GetById(StoreModel.StoreId);

            if (storeToUpdate == null)
                return NotFound();

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(StoreModel.Status);

            if (_unitOfWork.Store.GetAll().Where(n => n.StoreId != StoreModel.StoreId && n.StoreName == StoreModel.StoreName).FirstOrDefault() != null)
                return ValidationProblem("Store Name Already Exists, Please Enter Different Store Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Store is In-Valid");

            if (StoreModel.ImageFile != null)
            {
                DeleteImage(StoreModel.Banner);
                StoreModel.Banner = SaveImage(StoreModel.ImageFile);
            }

            if (storeToUpdate.Banner != "nobanner.png" && StoreModel.Banner == "nobanner.png")
                StoreModel.Banner = storeToUpdate.Banner;


            storeToUpdate.StoreName = StoreModel.StoreName;
            storeToUpdate.Description = StoreModel.Description;
            storeToUpdate.Address = StoreModel.Address;
            storeToUpdate.ContactDetail = storeToUpdate.ContactDetail;
            storeToUpdate.Status = StoreModel.Status;
            storeToUpdate.Banner = StoreModel.Banner;
            storeToUpdate.DateUpdated = DateTime.Now;
            storeToUpdate.CreatedBy = StoreModel.CreatedBy;

            _unitOfWork.Store.Update(storeToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(StoreModel);
        }

        [NonAction]
        public string SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}