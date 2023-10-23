using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;
using TaskSoliq.Application;
using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;
using TaskSoliq.Infrastructure;
using ExcelDataReader;
using TaskSoliq.Domain.Enums;

namespace TaskSoliq.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private TurniketDbContext _turniketDb;
        private IUserServices _userServices;
        private readonly IWebHostEnvironment hostEnvironment;
        /*IExcelDataReaderreader;>*/
        IExcelDataReader reader;
         

        public UsersController(TurniketDbContext turniketDb, IUserServices userServices, IWebHostEnvironment webHostEnvironment)
        {
            _turniketDb = turniketDb;
            _userServices = userServices;
            hostEnvironment = webHostEnvironment;
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
        [HttpPost]
        public async ValueTask<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                // Check the File is received

                if (file == null)
                    return NotFound("File is Not Received...");


                // Create the Directory if it is not exist
                string dirPath = Path.Combine(hostEnvironment.WebRootPath, "ReceivedReports");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(file.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    return BadRequest("Sorry! This file is not allowed," +
                      "make sure that file having extension as either.xls or.xlsx is uploaded.");

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // USe this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // read the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension == ".xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        // Read the the Table
                        DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            User details = new User();

                            details.FirstName = serviceDetails.Rows[i][0].ToString();
                            details.LastName = serviceDetails.Rows[i][1].ToString();
                            details.Age = Convert.ToInt32(serviceDetails.Rows[i][2].ToString());
                            details.EmployeeCategory = Convert.ToInt32(serviceDetails.Rows[i][3].ToString());
                            details.ImageUrl = "wwwroot/images/rasm.jpg";
                            details.Status = (int)Status.Updated;

                            details.CreatedDate = DateTime.Now;


                            // Add the record in Database
                            await _turniketDb.Users.AddAsync(details);
                            await _turniketDb.SaveChangesAsync();
                        }
                    }
                }
                return Ok("Davay");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
