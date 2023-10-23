using Microsoft.EntityFrameworkCore;
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
        public UserServices(TurniketDbContext turniketDb)
        {
            _turniketDb = turniketDb;
        }

        /// <summary>
        /// Create user service
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        public async ValueTask<User> CreateUser(UserDTO addUser)
        {
            User user = new User()
            {
                Id = 0,
                FirstName = addUser.FirstName,
                LastName = addUser.LastName,
                Age = addUser.Age,
                EmployeeCategory = (int)addUser.EmployeeCategory,
                ImageUrl = "Upload/images/binarsa.jpg",
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
    }
}
