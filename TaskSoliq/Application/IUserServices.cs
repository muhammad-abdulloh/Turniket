using TaskSoliq.Domain.DTOs;
using TaskSoliq.Domain.Entities;

namespace TaskSoliq.Application
{
    public interface IUserServices
    {
        public ValueTask<IEnumerable<User>> GetAllUsers();
        public ValueTask<IEnumerable<User>> ReallyGetAllUsers();
        public ValueTask<User> CreateUser(UserDTO addUser);
        public ValueTask<User> GetUserById(int Id);
        public ValueTask<User> UpdateUser(int Id, UserDTO updatedModel);
        public ValueTask<bool> DeleteUser(int Id);
        public ValueTask<bool> DeepDeleteUser(int Id);
    }
}
