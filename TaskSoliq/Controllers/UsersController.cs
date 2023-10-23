using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TaskSoliq.Domain;
using TaskSoliq.Infrastructure;

namespace TaskSoliq.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private TurniketDbContext _turniketDb;
        public UsersController(TurniketDbContext turniketDb)
        {
            _turniketDb = turniketDb;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _turniketDb.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(UserDTO addUser)
        {
            var user = new User()
            {
                Id = 0,
                FirstName = addUser.FirstName,
                LastName = addUser.LastName,
                Age = addUser.Age,
                IsEmployee = addUser.IsEmployee,
                ImageUrl = "Upload/images/binarsa.jpg"
            };
            await _turniketDb.Users.AddAsync(user);
            await _turniketDb.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int Id)
        {
            var user = await _turniketDb.Users.FindAsync(Id);

            if (user == null)
                return NotFound("Ma'lumot mavjud emas");

            return Ok(user);
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int Id, UserDTO updatedModel)
        {
            var user = await _turniketDb.Users.FindAsync(Id);

            if (user != null)
            {
                user.FirstName = updatedModel.FirstName;
                user.LastName = updatedModel.LastName;
                user.Age = updatedModel.Age;
                user.IsEmployee = updatedModel.IsEmployee;

                await _turniketDb.SaveChangesAsync(); 
                return Ok(user);
            }
               
            return NotFound("Manba topilmadi");
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
            // delete qilishda uni databasedan o'chirmaslik kerak statusini o'zgartirishim kerak
            // va delete qilinganda statusi o'zgaradi 
            // get qiganda va get all qiganda ko'rinmaydi bu eng zo'r yo'li
            // delete user bo'ladi va deeep delete user bo'ladi

            var user = await _turniketDb.Users.FindAsync(Id);
            if (user != null)
            {
                _turniketDb.Remove(user);
                await _turniketDb.SaveChangesAsync();
                return Ok(user);
            }

            return NotFound("Manba topilmadi");
        }
    }
}
