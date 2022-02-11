using Last.Bench.Coder.Beauty.World.Entity;
using Last.Bench.Coder.Beauty.World.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Last.Bench.Coder.Beauty.World.Utility;
using Last.Bench.Coder.Beauty.World.Entity.Enums;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace Last.Bench.Coder.Beauty.World.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/admin")]
    [ApiVersion("1.0")]
    [EnableCors("MyCors")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtAuth _jwtAuth;
        public AdminController(IUnitOfWork unitOfWork, IJwtAuth jwtAuth)
        {
            _unitOfWork = unitOfWork;
            _jwtAuth = jwtAuth;
        }

        [HttpPost]
        [Route("token")]
        [AllowAnonymous]
        public IActionResult AuthenticateAdmin(AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
                return Unauthorized();

            var admin = _unitOfWork.Admin.Login(model.UserId, model.Password);

            if (admin == null)
                return Unauthorized();

            var token = _jwtAuth.Authentication(model.UserId);

            AuthenticateResponse response = new AuthenticateResponse();
            response.UserId = admin.FullName;
            response.EmailId = admin.EmailId;
            response.Role = admin.Role;
            response.Token = token;

            return Ok(response);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetAllAdmins()
        {
            List<Admin> Admins = _unitOfWork.Admin.GetAll().ToList();

            if (Admins != null && Admins.Count > 0)
            {
                foreach (var item in Admins)
                {
                    item.Store = _unitOfWork.Store.GetById(item.StoreId);
                }
            }

            return Ok(Admins);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetAdminById(int AdminId)
        {
            var Admin = _unitOfWork.Admin.GetById(AdminId);

            if (Admin == null)
                return NotFound();

            Admin.Store = _unitOfWork.Store.GetById(Admin.StoreId);
            return Ok(Admin);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string EmailId, string Password)
        {
            var Admin = _unitOfWork.Admin.Login(EmailId, Password);

            if (Admin == null)
                return NotFound();

            return Ok(Admin);
        }

        [HttpGet]
        [Route("searchbytext")]
        public IActionResult GetAllByText(string SearchKey)
        {
            List<Admin> Admins = _unitOfWork.Admin.GetAllByText(SearchKey).ToList();

            if (Admins == null && Admins.Count > 0)
                return NotFound();

            if (Admins != null && Admins.Count > 0)
            {
                foreach (var item in Admins)
                {
                    item.Store = _unitOfWork.Store.GetById(item.StoreId);
                }
            }

            return Ok(Admins);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult InsertAdmin(Admin Admin)
        {
            bool resultStatus = false;
            resultStatus = EnumData.TryParseEnum<eStatus>(Admin.Status);

            bool resultRole = false;
            resultRole = EnumData.TryParseEnum<eRole>(Admin.Role);

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(Admin.Status);

            if (_unitOfWork.Admin.GetAll().Where(n => n.EmailId == Admin.EmailId && n.PhoneNo == Admin.PhoneNo).FirstOrDefault() != null)
                return ValidationProblem("Admin EmailId or PhoneNo Already Exists, Please Enter Different Admin FullName or PhoneNo");

            if (!resultStatus)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Admin is In-Valid");

            if (!resultRole)
                return ValidationProblem("Please Specify Valid Role, Given Role for the Admin is In-Valid");

            MailAddress address = new MailAddress(Admin.EmailId);

            Admin.LoginId = address.User;
            Admin.DateCreated = DateTime.Now;
            Admin.DateUpdated = DateTime.Now;
            _unitOfWork.Admin.Add(Admin);
            _unitOfWork.Complete();
            return Ok(Admin);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateAdmin([FromBody] Admin Admin)
        {
            Admin AdminToUpdate = _unitOfWork.Admin.GetById(Admin.AdminId);

            if (AdminToUpdate == null)
                return NotFound();

            bool resultStatus = false;
            resultStatus = EnumData.TryParseEnum<eStatus>(Admin.Status);

            bool resultRole = false;
            resultRole = EnumData.TryParseEnum<eRole>(Admin.Role);

            if (_unitOfWork.Admin.GetAll().Where(n => n.AdminId != Admin.AdminId && n.EmailId == Admin.EmailId).FirstOrDefault() != null)
                return ValidationProblem("Admin EmailId Already Exists, Please Enter Different Admin EmailId");

            if (_unitOfWork.Admin.GetAll().Where(n => n.AdminId != Admin.AdminId && n.PhoneNo == Admin.PhoneNo).FirstOrDefault() != null)
                return ValidationProblem("Admin Phone No Already Exists, Please Enter Different Admin Phone No");

            if (!resultStatus)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Admin is In-Valid");


            if (!resultRole)
                return ValidationProblem("Please Specify Valid Role, Given Role for the Admin is In-Valid");

            AdminToUpdate.FullName = Admin.FullName;
            AdminToUpdate.Password = Admin.Password;
            AdminToUpdate.Status = AdminToUpdate.Status;
            AdminToUpdate.Role = Admin.Role;
            AdminToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.Admin.Update(AdminToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(Admin);
        }
    }
}