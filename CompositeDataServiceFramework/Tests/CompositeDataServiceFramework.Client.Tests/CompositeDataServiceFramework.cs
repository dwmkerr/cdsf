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

        readonly string testUserName1 = "Test User 1";
        readonly string testUserName2 = "Test User 2";
        readonly string testPassword = "Test Password";
        readonly string testRoleName = "Test Role";

        private Role testRole1;
        private User testUser1;

        [TestMethod]
        public void CanResetTestData()
        {
            //  Delete each test role.
            var roles = (from r in dataService.Roles.Expand("Users") where r.Name == testRoleName select r).ToList();
            foreach (var role in roles)
            {
              foreach (var user in role.Users)
                dataService.DeleteObject(user);
              dataService.DeleteObject(role);
            }
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
        public void CanAddUserLinkedToRole()
        {
          //  Try and get the role.
          testRole1 = (from r in dataService.Roles where r.Name == testRoleName select r).First();

            //  Create the user.
            testUser1 = new User()
            {
                Username = testUserName1,
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
            var bouncedUser1 = (from r in dataService.Users where r.Username == testUserName1 select r).First();

            //  Check the result.
            Assert.AreEqual(testUser1.Username, bouncedUser1.Username);
            Assert.AreEqual(testUser1.Password, bouncedUser1.Password);
            Assert.AreEqual(testUser1.Role.Name, bouncedUser1.Role.Name);
        }

        [TestMethod]
        public void CanAddUserToRole()
        {
          //  Try and get the role.
          testRole1 = (from r in dataService.Roles where r.Name == testRoleName select r).First();

          //  Create a new user.
          User newUser = new User()
          {
            Username = testUserName2, 
            Password = "PasswordUserToRole"
          };
          
          //  Add a user to the role.
          dataService.AddRelatedObject(testRole1, "Users", newUser);
          
          //  Save the changes.
          dataService.SaveChanges();
          dataService =
             new CompositeDataServiceContainer(new Uri("http://localhost:53282/CompositeDataService.svc"));

          //  Try and get the user.
          var bouncedUser1 = (from r in dataService.Users.Expand("Role") where r.Username == testUserName2 select r).First();

          //  Check the result.
          Assert.AreEqual(newUser.Username, bouncedUser1.Username);
          Assert.AreEqual(newUser.Password, bouncedUser1.Password);
          Assert.AreEqual(testRoleName, bouncedUser1.Role.Name);
        }

        [TestMethod]
        public void CanGetUserSpecificProperty()
        {
          //  Try and get a user password.
          var user = (from u in dataService.Users where u.Username == testUserName1 && u.Password == testPassword select u).First();

          Assert.AreEqual(user.Password, testPassword);
        }
        
        [TestMethod]
        public void CanDeleteUser()
        {
          //  Try and get the user.
          testUser1 = (from r in dataService.Users where r.Username == testUserName1 select r).First();

            //  Delete the user.
            dataService.DeleteObject(testUser1);

            //  Save the changes.
            dataService.SaveChanges();

            //  Make sure there is no user.
            Assert.IsTrue((from r in dataService.Users where r.Username == testUserName1 select r).FirstOrDefault() == null);
        }

        [TestMethod]
        public void CanDeleteRole()
        {
          //  Delete each test role.
          var roles = (from r in dataService.Roles.Expand("Users") where r.Name == testRoleName select r).ToList();
          foreach (var role in roles)
          {
            foreach (var user in role.Users)
              dataService.DeleteObject(user);
            dataService.DeleteObject(role);
          }
          dataService.SaveChanges();
        }
    }
}
