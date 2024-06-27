using WebDoAn.Models;
using WebDoAn.Service.Admin.Users.Dto;

namespace WebDoAn.Service.Admin.Users
{
	public interface IUserService
	{
		public Task<List<User>> GetAllUser();
		public List<User> GetAll(GetInput input);
		public Task<bool> UpdateUser(User user);
		public Task<bool> UpdateStatus(User user);
		public Task<bool> Create(User user);
		public Task<User> GetUserById(int id);

	}
}
