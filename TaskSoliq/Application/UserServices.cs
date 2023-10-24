using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;
using TaskSoliq.Domain.Enums;
using TaskSoliq.Infrastructure;


#pragma warning disable

namespace TaskSoliq.Application
{
    public class UserServices : IUserServices
    {
        private TurniketDbContext _turniketDb;
        private readonly IWebHostEnvironment hostEnvironment;
        IExcelDataReader reader;

        public UserServices(TurniketDbContext turniketDb, IWebHostEnvironment _hostEnvironment)
        {
            _turniketDb = turniketDb;
            this.hostEnvironment = _hostEnvironment;
        }

        /// <summary>
        /// Create user service
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        public async ValueTask<User> CreateUser(UserDTO addUser)
        {
            string filePath = String.Empty;
            if (addUser.Image != null)
            {
                string fileName = addUser.Image.FileName;
                Guid GuidId = Guid.NewGuid();
                filePath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot/images/" + GuidId + fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await addUser.Image.CopyToAsync(stream);
                }
            }

            User user = new User()
            {
                Id = 0,
                FirstName = addUser.FirstName,
                LastName = addUser.LastName,
                Age = addUser.Age,
                EmployeeCategory = (int)addUser.EmployeeCategory,
                ImageUrl = filePath,
                Status = (int)Status.Created,
                CreatedDate = DateTime.Now,
            };
            await _turniketDb.Users.AddAsync(user);
            await _turniketDb.SaveChangesAsync();

            return user;

        }

        /// <summary>
        /// Really delete user in database
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeepDeleteUser(int Id)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user != null)
            {
                _turniketDb.Remove(user);
                await _turniketDb.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete user and update status
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async ValueTask<bool> DeleteUser(int Id)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null || (int)user.Status == (int)Status.Deleted)
                return false;

            user.Status = (int)Status.Deleted;
            user.DeletedDate = DateTime.Now;
            await _turniketDb.SaveChangesAsync();
            return true;

        }

        /// <summary>
        /// GetAll users business logic
        /// </summary>
        /// <returns></returns>
        public async ValueTask<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> users = await _turniketDb.Users
                                            .Select(x => new User
                                            {
                                                Id = x.Id,
                                                FirstName = x.FirstName,
                                                LastName = x.LastName,
                                                Age = x.Age,
                                                EmployeeCategory = x.EmployeeCategory,
                                                ImageUrl = x.ImageUrl,
                                                Status = x.Status,
                                            }
                                            )
                                            .Where(x => (int)x.Status != (int)Status.Deleted)
                                            .ToListAsync();

            return users;
        }

        /// <summary>
        /// Really get all users )
        /// </summary>
        /// <returns></returns>
        public async ValueTask<IEnumerable<User>> ReallyGetAllUsers()
        {
            IEnumerable<User> users = await _turniketDb.Users.ToListAsync();

            return users;
        }

        /// <summary>
        /// Get user by id service
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async ValueTask<User> GetUserById(int Id)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null || (int)user.Status == (int)Status.Deleted)
                return null;

            return user;
        }

        /// <summary>
        /// Update User Service
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public async ValueTask<User> UpdateUser(int Id, UserDTO updatedModel)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user != null)
            {
                user.FirstName = updatedModel.FirstName;
                user.LastName = updatedModel.LastName;
                user.Age = updatedModel.Age;
                user.EmployeeCategory = (int)updatedModel.EmployeeCategory;
                user.Status = (int)Status.Updated;
                user.ModifyDate = DateTime.Now;

                await _turniketDb.SaveChangesAsync();
                return user;
            }
            return null;

        }

        public async ValueTask<string> UploadFile(IFormFile file)
        {
            try
            {
                // Check the File is received

                if (file == null)
                    return "File is Not Received...";


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
                    return "Sorry! This file is not allowed," +
                      "make sure that file having extension as either.xls or.xlsx is uploaded.";

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
                return "Created";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async ValueTask<DataTable> ExportDatabaseToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Id"),
                                        new DataColumn("FirstName"),
                                        new DataColumn("LastName"),
                                        new DataColumn("Age"),
                                        new DataColumn("EmployeeCategory"),
                                        new DataColumn("ImageUrl"),
                                        new DataColumn("Status"),
                                        new DataColumn("CreatedDate"),
                                        new DataColumn("ModifyDate"),
                                        new DataColumn("DeletedDate"),
            });

            var users = from user in _turniketDb.Users.Take(40)
                        select user;

            foreach (var user in users)
            {
                dt.Rows.Add(user.Id,
                            user.FirstName,
                            user.LastName,
                            user.Age,
                            user.EmployeeCategory,
                            user.ImageUrl,
                            user.Status,
                            user.CreatedDate,
                            user.ModifyDate,
                            user.DeletedDate
                            );
            }

            return dt;
        }
    }
}
