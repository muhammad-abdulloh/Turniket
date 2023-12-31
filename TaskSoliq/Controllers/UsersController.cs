﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskSoliq.Application;
using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;
using TaskSoliq.Domain.External;

namespace TaskSoliq.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
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
            catch (Exception ex)
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
        public async Task<IActionResult> UpdateUser([FromRoute] int Id, [FromForm] UserDTO updatedModel)
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

        /// <summary>
        /// Upload Excel File
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async ValueTask<IActionResult> ImportExcelToDataBase([FromForm] ExcelAndImage files)
        {
            try
            {
                var result = await _userServices.UploadFile(files);

                if (result == "Created")
                    return Ok("Muvaffaqiyati EXCEL ma'lumotlari bazaga saqlandi");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Export database to excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async ValueTask<IActionResult> ExportDataBaseToExcel()
        {
            try
            {
                DataTable result = await _userServices.ExportDatabaseToExcel();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(result);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }

}
