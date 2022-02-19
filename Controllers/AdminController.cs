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
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminController(IUnitOfWork unitOfWork, IJwtAuth jwtAuth, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _jwtAuth = jwtAuth;
            this._hostEnvironment = hostEnvironment;
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
            response.UserUniqueId = admin.AdminId;
            response.UserId = admin.FullName;
            response.EmailId = admin.EmailId;
            response.Role = admin.Role;
            response.Banner = admin.Banner;
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
        public IActionResult InsertAdmin([FromForm] Admin AdminModel)
        {
            bool resultStatus = false;
            resultStatus = EnumData.TryParseEnum<eStatus>(AdminModel.Status);

            bool resultRole = false;
            resultRole = EnumData.TryParseEnum<eRole>(AdminModel.Role);

            bool result = false;
            result = EnumData.TryParseEnum<eStatus>(AdminModel.Status);

            if (_unitOfWork.Admin.GetAll().Where(n => n.EmailId == AdminModel.EmailId).FirstOrDefault() != null)
                return ValidationProblem("Admin EmailId Already Exists, Please Enter different Email Id");

            if (_unitOfWork.Admin.GetAll().Where(n => n.PhoneNo == AdminModel.PhoneNo).FirstOrDefault() != null)
                return ValidationProblem("Admin Phone Number Already Exists, Please Enter different Phone Number");

            if (!resultStatus)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Admin is In-Valid");

            if (!resultRole)
                return ValidationProblem("Please Specify Valid Role, Given Role for the Admin is In-Valid");

            if (AdminModel.ImageFile != null)
            {
                DeleteImage(AdminModel.Banner);
                AdminModel.Banner = SaveImage(AdminModel.ImageFile);
            }

            MailAddress address = new MailAddress(AdminModel.EmailId);

            AdminModel.LoginId = address.User;
            AdminModel.DateCreated = DateTime.Now;
            AdminModel.DateUpdated = DateTime.Now;
            _unitOfWork.Admin.Add(AdminModel);
            _unitOfWork.Complete();
            return Ok(AdminModel);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateAdmin([FromForm] Admin AdminModel)
        {
            Admin AdminToUpdate = _unitOfWork.Admin.GetById(AdminModel.AdminId);

            if (AdminToUpdate == null)
                return NotFound();

            bool resultStatus = false;
            if (AdminModel.Status != null)
                resultStatus = EnumData.TryParseEnum<eStatus>(AdminModel.Status);
            else
                resultStatus = true;

            bool resultRole = false;
            if (AdminModel.Role != null)
                resultRole = EnumData.TryParseEnum<eRole>(AdminModel.Role);
            else
                resultRole = true;

            if (_unitOfWork.Admin.GetAll().Where(n => n.AdminId != AdminModel.AdminId && n.EmailId == AdminModel.EmailId).FirstOrDefault() != null)
                return ValidationProblem("Admin EmailId Already Exists, Please Enter Different Admin EmailId");

            if (_unitOfWork.Admin.GetAll().Where(n => n.AdminId != AdminModel.AdminId && n.PhoneNo == AdminModel.PhoneNo).FirstOrDefault() != null)
                return ValidationProblem("Admin Phone No Already Exists, Please Enter Different Admin Phone No");

            if (!resultStatus)
                return ValidationProblem("Please Specify Valid Status, Given Status for the Admin is In-Valid");


            if (!resultRole)
                return ValidationProblem("Please Specify Valid Role, Given Role for the Admin is In-Valid");

            if ((AdminToUpdate.Banner != "nobanner.png" && AdminModel.Banner == "nobanner.png") || AdminModel.Banner == null)
                AdminModel.Banner = AdminToUpdate.Banner;

            if (AdminModel.ImageFile != null)
            {
                DeleteImage(AdminModel.Banner);
                AdminModel.Banner = SaveImage(AdminModel.ImageFile);
            }

            AdminToUpdate.FullName = AdminModel.FullName != null ? AdminModel.FullName : AdminToUpdate.FullName;
            AdminToUpdate.Password = AdminModel.Password != null ? AdminModel.Password : AdminToUpdate.Password;
            AdminToUpdate.Status = AdminModel.Status != null ? AdminModel.Status : AdminToUpdate.Status;
            AdminToUpdate.Role = AdminModel.Role != null ? AdminModel.Role : AdminToUpdate.Role;
            AdminToUpdate.Banner = AdminModel.Banner;
            AdminToUpdate.DateUpdated = DateTime.Now;

            _unitOfWork.Admin.Update(AdminToUpdate);
            int i = _unitOfWork.Complete();
            return Ok(AdminModel);
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