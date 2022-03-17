using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
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

        //Post api --> Login
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            var Student = _uniDbContext.Students.FirstOrDefault(u => u.StudentId == login.Id);
            if (Student == null)
            {
                return NotFound();
            }

            var Teacher = _uniDbContext.Teachers.FirstOrDefault(u => u.TeacherId == login.Id);
            if (Teacher == null)
            {
                return NotFound();
            }

            var Admin = _uniDbContext.Admins.FirstOrDefault(u => u.AdminId == login.Id);
            if (Admin == null)
            {
                return NotFound();
            }
            //-----------------------------------------------------------------------------------
            if (!SecurePasswordHasherHelper.Verify(login.Password, Student.Password))
            {
                return Unauthorized();
            }

            if (!SecurePasswordHasherHelper.Verify(login.Password, Teacher.Password))
            {
                return Unauthorized();
            }

            if (!SecurePasswordHasherHelper.Verify(login.Password, Admin.Password))
            {
                return Unauthorized();
            }
            //------------------------------------------------------------------------------------

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, login.Id),
               new Claim(ClaimTypes.Email, login.Id),
               new Claim(ClaimTypes.Role,Student.Role),
               new Claim(ClaimTypes.Role,Teacher.Role),
               new Claim(ClaimTypes.Role,Admin.Role)
            };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                student_id = Student.StudentId,
                teacher_id = Teacher.TeacherId,
                admin_id = Admin.AdminId
            });
        }
    }
}
