using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using University.Data.Context;
using University.Data.Models;
using University.Data.Models.Entities.User;
using University.Data.Models.User;

namespace University.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UniDbContext _uniDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UserController(IConfiguration configuration, UniDbContext storeDbContext)
        {
            _uniDbContext = storeDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        ///////////////////////////////////////////// Post Methods For Products /////////////////////////////////////////////////////

        // POST api/<UserController> --> Register form
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RegisterStudent([FromForm] Student student)
        {
            var userWithSameId = _uniDbContext.Students.Where(u => u.Id == student.Id).SingleOrDefault();
            if (userWithSameId != null)
            {
                return BadRequest("User with same email already exists");
            }
            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            var studentObj = new Student()
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Phone = student.Phone,
                Address = student.Address,
                Role = "Student",
                Password = SecurePasswordHasherHelper.Hash(student.Password),
                ConfirmPassword = SecurePasswordHasherHelper.Hash(student.Password),
            };

            if (studentObj.Images != null)
            {
                var filestream = new FileStream(filepath, FileMode.Create);
                student.Images.CopyTo(filestream);
            }

            studentObj.ImageUrl = filepath.Remove(0, 7);
            _uniDbContext.Students.Add(studentObj);
            _uniDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // POST api/<UserController> --> Register form
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RegisterTeacher([FromForm] Teacher teacher)
        {
            var userWithSameId = _uniDbContext.Teachers.Where(u => u.Id == teacher.Id).SingleOrDefault();
            if (userWithSameId != null)
            {
                return BadRequest("User with same email already exists");
            }
            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            var teacherObj = new Teacher()
            {
                TeacherId = teacher.TeacherId,
                Name = teacher.Name,
                Phone = teacher.Phone,
                Address = teacher.Address,
                Role = "Teacher",
                Password = SecurePasswordHasherHelper.Hash(teacher.Password),
                ConfirmPassword = SecurePasswordHasherHelper.Hash(teacher.Password),
            };

            if (teacherObj.Images != null)
            {
                var filestream = new FileStream(filepath, FileMode.Create);
                teacher.Images.CopyTo(filestream);
            }

            teacherObj.ImageUrl = filepath.Remove(0, 7);
            _uniDbContext.Teachers.Add(teacherObj);
            _uniDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // POST api/<UserController> --> Register form
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RegisterAdmin([FromForm] Admin admin)
        {
            var userWithSameId = _uniDbContext.Admins.Where(u => u.Id == admin.Id).SingleOrDefault();
            if (userWithSameId != null)
            {
                return BadRequest("User with same email already exists");
            }
            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            var adminObj = new Admin()
            {
                AdminId = admin.AdminId,
                Name = admin.Name,
                Phone = admin.Phone,
                Address = admin.Address,
                Role = admin.Role,
                Password = SecurePasswordHasherHelper.Hash(admin.Password),
                ConfirmPassword = SecurePasswordHasherHelper.Hash(admin.Password),
            };

            if (adminObj.Images != null)
            {
                var filestream = new FileStream(filepath, FileMode.Create);
                admin.Images.CopyTo(filestream);
            }

            adminObj.ImageUrl = filepath.Remove(0, 7);
            _uniDbContext.Admins.Add(adminObj);
            _uniDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
