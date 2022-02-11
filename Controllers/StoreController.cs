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
        public StoreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult InsertStore(Store Store)
        {
            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Store.Status);

            if (_unitOfWork.Store.GetAll().Where(n => n.StoreName == Store.StoreName).FirstOrDefault() != null)
                return ValidationProblem("Store Name Already Exists, Please Enter Different Store Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Store is In-Valid");

            Store.DateCreated = DateTime.Now;
            Store.DateUpdated = DateTime.Now;
            _unitOfWork.Store.Add(Store);
            _unitOfWork.Complete();
            return Ok(Store);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateStore([FromBody] Store Store)
        {
            Store storeToUpdate = _unitOfWork.Store.GetById(Store.StoreId);

            if (storeToUpdate == null)
                return NotFound();

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Store.Status);

            if (_unitOfWork.Store.GetAll().Where(n => n.StoreId != Store.StoreId && n.StoreName == Store.StoreName).FirstOrDefault() != null)
                return ValidationProblem("Store Name Already Exists, Please Enter Different Store Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Store is In-Valid");

            storeToUpdate.StoreName = Store.StoreName;
            storeToUpdate.Description = Store.Description;
            storeToUpdate.Address = Store.Address;
            storeToUpdate.ContactDetail = storeToUpdate.ContactDetail;
            storeToUpdate.Status = Store.Status;
            storeToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.Store.Update(storeToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(Store);
        }
    }
}