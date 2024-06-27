using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using WebDoAn.dbs;
using WebDoAn.Models;
using WebDoAn.Service.Admin.Users.Dto;

namespace WebDoAn.Service.Admin.Users
{
	public class UserService : IUserService
	{
		private readonly DoAnDbContext _db;
		public INotyfService _notyfService;

		public UserService(DoAnDbContext db, INotyfService notyfService)
		{
			_db = db;
			_notyfService = notyfService;
		}

		public async Task<bool> Create(User user)
		{
			var isUser = _db.user.Where(x => x.PhoneNumber.ToLower() == user.PhoneNumber.ToLower()).ToList();
			if (isUser.Count() == 0)
			{
				var date = DateTime.Now;
				var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
				user.CreateTime = datecustom;

				_db.user.Add(user);
				await _db.SaveChangesAsync();
				_notyfService.Success("Đăng kí thành công!");
				return true;
			}
			else
			{
				_notyfService.Warning("Số điện thoại đã được đăng kí! Vui lòng thay đổi");
				return false;
			}
		}

		public List<User> GetAll(GetInput input)
		{
			if (input.Name == "all")
			{
				var list = _db.user.OrderBy(x => x.Id).ToList();
				return list;
			}
			else
			{
				var list = _db.user.Where(x => x.FullName.ToLower().Contains(input.Name.ToLower())).OrderBy(x => x.Id).ToList();
				return list;
			}
		}

		public async Task<List<User>> GetAllUser()
		{
			var listuser = await _db.user.OrderBy(x => x.Id).ToListAsync();
			return listuser;
		}

		public async Task<User> GetUserById(int id)
		{
			var user = await _db.user.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
			{
				User users = new User();
				_notyfService.Error("Không thấy người dùng này");
				return users;
			}
			else
			{
				return user;
			}
		}

		public async Task<bool> UpdateStatus(User user)
		{
			DateTime cate = _db.user.Where(x => x.Id == user.Id).Select(x => x.CreateTime).FirstOrDefault();
            var phoneold = _db.user.Where(x => x.Id == user.Id).Select(x => x.PhoneNumber).FirstOrDefault();
			var password = _db.user.Where(x => x.Id == user.Id).Select(x => x.Password).FirstOrDefault();
            var emailold = _db.user.Where(x => x.Id == user.Id).Select(x => x.Email).FirstOrDefault();
			var fullnameole = _db.user.Where(x => x.Id == user.Id).Select(x => x.FullName).FirstOrDefault();
			var gtinhold = _db.user.Where(x => x.Id == user.Id).Select(x => x.Gender).FirstOrDefault();

            // Tao moi ngay cập nhật
            var date = DateTime.Now;
			var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
			user.UpdateTime = datecustom;

			user.CreateTime = cate;
			user.Password = password;
			user.PhoneNumber = phoneold;
			user.Email = emailold;
			user.Gender = gtinhold;
			user.FullName = fullnameole;

			_db.user.Update(user);
			await _db.SaveChangesAsync();
			_notyfService.Success("Cập nhật thành công");
			return true;
		}

		public async Task<bool> UpdateUser(User user)
		{
			var isUser = _db.user.Where(x => user.Id != x.Id && x.PhoneNumber.ToLower() == user.PhoneNumber.ToLower()).ToList();
			if (isUser.Count == 0)
			{
				DateTime cate = _db.user.Where(x => x.Id == user.Id).Select(x => x.CreateTime).FirstOrDefault();

				// Tao moi ngay cập nhật
				var date = DateTime.Now;
				var datecustom = DateTime.SpecifyKind(date, DateTimeKind.Utc);
				user.UpdateTime = datecustom;

				user.CreateTime = cate;
				_db.user.Update(user);
				await _db.SaveChangesAsync();
				_notyfService.Success("Cập nhật thành công");
				return true;
			}
			else
			{
				_notyfService.Error("Tên loại sản phẩm này đã tồn tại, vui lòng thay đổi tên !");
				return false;
			}
		}
	}
}
