using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompositeDataServiceFramework.Client.Tests.CompositeDataService;

namespace CompositeDataServiceFramework.Client.Tests
{
    [TestClass]
    public class CompositeDataServiceFramework
    {
        private static CompositeDataServiceContainer dataService =
            new CompositeDataServiceContainer(new Uri("http://localhost:53282/CompositeDataService.svc"));

        readonly string testUserName = "Test User";
        readonly string testPassword = "Test Password";
        readonly string testRoleName = "Test Role";

        private Role testRole1;
        private User testUser1;

        [TestMethod]
        public void CanResetTestData()
        {
            //  Delete each test user.
            var users = (from u in dataService.Users where u.Username == testUserName select u).ToList();
            foreach (var user in users)
                dataService.DeleteObject(user);
            dataService.SaveChanges();

            //  Delete each test role.
            var roles = (from r in dataService.Roles where r.Name == testRoleName select r).ToList();
            foreach (var role in roles)
                dataService.DeleteObject(role);
            dataService.SaveChanges();
        }

        [TestMethod]
        public void CanAddUserRole()
        {
            //  Create the user role.
            testRole1 = new Role()
            {
                Name = testRoleName
            };

            //  Add the user role.
            dataService.AddToRoles(testRole1);

            //  Save the changes.
            dataService.SaveChanges();

            //  Try and get the role.
            var bouncedRole1 = (from r in dataService.Roles where r.Name == testRoleName select r).First();

            //  Have we added the role.
            Assert.AreEqual(testRole1.Name, bouncedRole1.Name);
        }

        [TestMethod]
        public void CanAddUserToRole()
        {
            //  Create the user.
            testUser1 = new User()
            {
                Username = testUserName,
                Password = testPassword,
                Role = testRole1
            };

            //  Add the user.
            dataService.AddToUsers(testUser1);

            //  Set the role.
            dataService.SetLink(testUser1, "Role", testUser1.Role);

            //  Save the changes.
            dataService.SaveChanges();

            //  Try and get the user.
            var bouncedUser1 = (from r in dataService.Users where r.Username == testUserName select r).First();

            //  Check the result.
            Assert.AreEqual(testUser1.Username, bouncedUser1.Username);
            Assert.AreEqual(testUser1.Password, bouncedUser1.Password);
            Assert.AreEqual(testUser1.Role.Name, bouncedUser1.Role.Name);
        }
        
        [TestMethod]
        public void CanDeleteUser()
        {
            //  Delete the user.
            dataService.DeleteObject(testUser1);

            //  Save the changes.
            dataService.SaveChanges();

            //  Make sure there is no user.
            Assert.IsTrue((from r in dataService.Users where r.Username == testUserName select r).FirstOrDefault() == null);
        }

        [TestMethod]
        public void CanDeleteRole()
        {
            //  Delete the user.
            dataService.DeleteObject(testRole1);

            //  Save the changes.
            dataService.SaveChanges();

            //  Make sure there is no role.
            Assert.IsTrue((from r in dataService.Roles where r.Name == testRoleName select r).FirstOrDefault() == null);
        }
    }
}
