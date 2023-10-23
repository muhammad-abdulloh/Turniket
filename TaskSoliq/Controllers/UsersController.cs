using Microsoft.AspNetCore.Mvc;
using TaskSoliq.Application;
using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;
using TaskSoliq.Infrastructure;

namespace TaskSoliq.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private TurniketDbContext _turniketDb;
        private IUserServices _userServices;
        public UsersController(TurniketDbContext turniketDb, IUserServices userServices)
        {
            _turniketDb = turniketDb;
            _userServices = userServices;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<User> users = await _userServices.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateUser([FromForm] UserDTO addUser)
        {
            try
            {
                User user = await _userServices.CreateUser(addUser);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int Id)
        {
            try
            {
                User user = await _userServices.GetUserById(Id);

                if (user == null)
                    return NotFound("Manba topilmadi");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int Id, UserDTO updatedModel)
        {
            try
            {
                User user = await _userServices.UpdateUser(Id, updatedModel);

                if (user != null)
                    return Ok(user);

                return NotFound("Manba topilmadi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Patch qilishim kerak har bir propertysini update qilish uchun bir nechta api lar bo'ladi zo'r
        // get gilishim kerak top 10 taso top 5 tasi top 20 talikni chiqaruvchi 
        // yana get qilaman ohirgi turgan top 5 talik top 10 talik top 20 talikni olish uchun 
        // va yana get qilishim kerak
        // nechta son berilsa eng tepadagi shularni ko'rvoladigan
        // va nechta son berilsa eng pasidan yani eng yangilarini chiqarberadigan api chiqaraman

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                bool user = await _userServices.DeleteUser(Id);

                return user ? Ok("Manba o'chirildi") : NotFound("Manba topilmadi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeepDeleteUser(int Id)
        {
            try
            {
                bool user = await _userServices.DeepDeleteUser(Id);

                return user ? Ok("Manba tag tugi bilan o'chirildi") : NotFound("Manba topilmadi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
