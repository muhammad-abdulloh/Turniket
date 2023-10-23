using Microsoft.EntityFrameworkCore;
using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;
using TaskSoliq.Domain.Enums;
using TaskSoliq.Infrastructure;

namespace TaskSoliq.Application
{
    public class UserServices : IUserServices
    {
        private TurniketDbContext _turniketDb;
        public UserServices(TurniketDbContext turniketDb)
        {
            _turniketDb = turniketDb;
        }

        public async ValueTask<User> CreateUser(UserDTO addUser)
        {
            User user = new User()
            {
                Id = 0,
                FirstName = addUser.FirstName,
                LastName = addUser.LastName,
                Age = addUser.Age,
                EmployeeCategory = (EmployeeCategory)addUser.EmployeeCategory,
                ImageUrl = "Upload/images/binarsa.jpg",
                Status = Status.Created
            };
            await _turniketDb.Users.AddAsync(user);
            await _turniketDb.SaveChangesAsync();

            return user;

        }

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

        public async ValueTask<bool> DeleteUser(int Id)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null || user.Status == Status.Deleted)
                return false;

            user.Status = Status.Deleted;
            await _turniketDb.SaveChangesAsync();
            return true;

        }

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
                                            .Where(x => x.Status != Status.Deleted)
                                            .ToListAsync();

            return users;
        }

        public async ValueTask<User> GetUserById(int Id)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null || user.Status == Status.Deleted)
                return null;

            return user;
        }

        public async ValueTask<User> UpdateUser(int Id, UserDTO updatedModel)
        {
            User user = await _turniketDb.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user != null)
            {
                user.FirstName = updatedModel.FirstName;
                user.LastName = updatedModel.LastName;
                user.Age = updatedModel.Age;
                user.EmployeeCategory = (EmployeeCategory)updatedModel.EmployeeCategory;
                user.Status = Status.Updated;

                await _turniketDb.SaveChangesAsync();
                return user;
            }
            return null;

        }
    }
}
