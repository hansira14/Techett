using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IQueryable<User> GetUsers(bool isSuperAdmin = false)
        {
            var query = this.GetDbSet<User>()
                           .Include(u => u.Team);
                           
            if (!isSuperAdmin)
            {
                return query.Where(u => (bool)u.IsActive);
            }
            
            return query;
        }

        public User GetById(int id) 
        {
            return this.GetDbSet<User>()
                       .Include(u => u.Team)
                       .FirstOrDefault(x => x.UserId == id);
        }

        public bool UserExists(int userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetById(user.UserId);
            if (existingUser != null)
            {
                this.GetDbSet<User>().Update(user);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteUser(User user)
        {
            this.GetDbSet<User>().Remove(user);
            UnitOfWork.SaveChanges();
        }

    }
}
