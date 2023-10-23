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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(TurniketDbContext turniketDb, IUserServices userServices, IWebHostEnvironment webHostEnvironment)
        {
            _turniketDb = turniketDb;
            _userServices = userServices;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Get All Users in Database 
        /// but not show change status deleted items :)
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// geta all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async ValueTask<IActionResult> ReallyGetAllUsers()
        {
            try
            {
                IEnumerable<User> users = await _userServices.ReallyGetAllUsers();

                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update Date
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// dont Delete date but change status :)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Really delete data 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        // Upload Excel file

        /// <summary>
        /// Upload Excel File
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> Upload(CancellationToken ct)
        {

            return Ok();
        }


    }
}
