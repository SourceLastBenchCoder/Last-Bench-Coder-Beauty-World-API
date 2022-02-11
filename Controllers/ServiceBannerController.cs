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
    [Route("api/v{version:apiVersion}/servicebnr")]
    [ApiVersion("1.0")]
    [EnableCors("MyCors")]
    [Authorize]
    public class ServiceBannerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceBannerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllServiceBanners()
        {
            var ServiceBanners = _unitOfWork.ServiceBanner.GetAll();
            return Ok(ServiceBanners);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetServiceBannerById(int ServiceBannerId)
        {
            var ServiceBanner = _unitOfWork.ServiceBanner.GetById(ServiceBannerId);

            if (ServiceBanner == null)
                return NotFound();

            return Ok(ServiceBanner);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult InsertServiceBanner(ServiceBanner ServiceBanner)
        {
            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(ServiceBanner.Status);

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the ServiceBanner is In-Valid");

            ServiceBanner.DateCreated = DateTime.Now;
            ServiceBanner.DateUpdated = DateTime.Now;
            _unitOfWork.ServiceBanner.Add(ServiceBanner);
            _unitOfWork.Complete();
            return Ok(ServiceBanner);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateServiceBanner([FromBody] ServiceBanner ServiceBanner)
        {
            ServiceBanner ServiceBannerToUpdate = _unitOfWork.ServiceBanner.GetById(ServiceBanner.ServiceBannerId);

            if (ServiceBannerToUpdate == null)
                return NotFound();

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(ServiceBanner.Status);

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the ServiceBanner is In-Valid");

            ServiceBannerToUpdate.BannerUrl = ServiceBanner.BannerUrl;
            ServiceBannerToUpdate.ServiceId = ServiceBanner.ServiceId;
            ServiceBannerToUpdate.IsDefault = ServiceBanner.IsDefault;
            ServiceBannerToUpdate.Status = ServiceBanner.Status;
            ServiceBannerToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.ServiceBanner.Update(ServiceBannerToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(ServiceBanner);
        }
    }
}