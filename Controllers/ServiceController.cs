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
    [Route("api/v{version:apiVersion}/Service")]
    [ApiVersion("1.0")]
    [EnableCors("MyCors")]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllServices()
        {
            List<Service> Services = _unitOfWork.Service.GetAll().ToList();

            if (Services == null && Services.Count < 0)
                return NotFound();

            if (Services != null || Services.Count < 0)
            {
                foreach (var item in Services)
                {
                    item.Store = _unitOfWork.Store.GetById(item.StoreId);
                    item.Category = _unitOfWork.Category.GetById(item.CategoryId);
                    item.ServiceBanner = _unitOfWork.ServiceBanner.GetAllByServiceId(item.ServiceId).ToList();
                }
            }

            return Ok(Services);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetServiceById(int ServiceId)
        {
            var Service = _unitOfWork.Service.GetById(ServiceId);

            if (Service == null)
                return NotFound();

            Service.Store = _unitOfWork.Store.GetById(Service.StoreId);
            Service.Category = _unitOfWork.Category.GetById(Service.CategoryId);

            return Ok(Service);
        }

        [HttpGet]
        [Route("searchbytext")]
        public IActionResult GetAllByText(string SearchKey)
        {
            List<Service> Services = _unitOfWork.Service.GetAllByText(SearchKey).ToList();

            if (Services == null && Services.Count < 0)
                return NotFound();

            if (Services != null && Services.Count < 0)
            {
                foreach (var item in Services)
                {
                    item.Store = _unitOfWork.Store.GetById(item.StoreId);
                    item.Category = _unitOfWork.Category.GetById(item.CategoryId);
                }
            }

            return Ok(Services);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult InsertService(Service Service)
        {
            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Service.Status);

            if (_unitOfWork.Service.GetAll().Where(n => n.Title == Service.Title).FirstOrDefault() != null)
                return ValidationProblem("Service Name Already Exists, Please Enter Different Service Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Service is In-Valid");

            Service.ServiceCode = "LBC2402BPMS" + _unitOfWork.Service.GetMaxServiceId();
            Service.DateCreated = DateTime.Now;
            Service.DateUpdated = DateTime.Now;
            _unitOfWork.Service.Add(Service);
            _unitOfWork.Complete();
            return Ok(Service);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateService([FromBody] Service Service)
        {
            Service ServiceToUpdate = _unitOfWork.Service.GetById(Service.ServiceId);

            if (ServiceToUpdate == null)
                return NotFound();

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Service.Status);

            if (_unitOfWork.Service.GetAll().Where(n => n.ServiceId != Service.ServiceId && n.Title == Service.Title).FirstOrDefault() != null)
                return ValidationProblem("Service Name Already Exists, Please Enter Different Service Name");

            if (!result)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Service is In-Valid");

            ServiceToUpdate.Title = Service.Title;
            ServiceToUpdate.Description = Service.Description;
            ServiceToUpdate.Price = Service.Price;
            ServiceToUpdate.Discount = Service.Discount;
            ServiceToUpdate.Status = Service.Status;
            ServiceToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.Service.Update(ServiceToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(Service);
        }
    }
}