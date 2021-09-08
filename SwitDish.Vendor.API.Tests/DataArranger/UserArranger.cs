using SwitDish.DataModel_OLD.Models;
using System;
using System.Linq;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public class UserArranger : DataArranger<User>
    {
        private readonly SwitDishDatabaseContext dbContext;

        private readonly User user;

        public UserArranger(SwitDishDatabaseContext dbContext, User user, Behaviour createFlag = Behaviour.TakeOwnership)
            : base(dbContext, user, createFlag)
        {
            this.dbContext = dbContext;
            this.user = user;
        }

        public static User DefaultTestData()
        {
            return new User
            {
                UserId = 1,
                FirstName = "Niyi",
                LastName = "Michael",
                Username = "sb2@gmail.com",
                Password = "Test",
                Email = "sb2@gmail.com",
                Phone = "09087833333",
                Gender = "Male",
                Title = "Mr",
            };
        }

        public static User CreateTestData(string firstName, string lastName, string userName, string email)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Username = userName,
                Password = "TestPass",
                Email = email,
                Phone = "09087833333",
                Gender = "Male",
                Title = "Mr",
                Active = true
            };
        }

        public bool ExistsByBusinessKey()
        {
            var userEntity = this.dbContext.Users.Where(d=> d.UserId.Equals(this.user))
                .FirstOrDefault();

            if (userEntity == null)
            {
                return false;
            }

            return Convert.ToInt32(userEntity.UserId) != 0;
        }
    }
}
