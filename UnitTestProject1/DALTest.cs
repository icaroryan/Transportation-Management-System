﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Transportation_Management_System;

namespace UnitTestProject1
{
    [TestClass]
    public class DALTest
    {
        // Test if a new user is successfully created
        [TestMethod]
        public void CreateUserNormalTest()
        {
            bool passed;
            User usr = new User("BubuFirstName", "bubuLastName", "bubuUsername", "pass", "email@gmail.com", UserRole.Buyer);

            DAL db = new DAL();
            try
            {
                db.CreateUser(usr);
                passed = true;
            }
            catch (Exception)
            {
                // Fail if an exception is thrown
                passed = false;
            }

            Assert.IsTrue(passed);
        }

        // Test if an alert message is displayed when inserting an existent user
        [TestMethod]
        public void CreateUserException()
        {
            DAL db = new DAL();
            User usr = new User("jijiFirstName", "jijiLastName", "jiji123", "pass", "email@gmail.com", UserRole.Buyer);

            // Insert the first user normaly
            db.CreateUser(usr);

            // Insert another user with the same username
            usr = new User("babaFirstName", "babaLastName", "jiji123", "pass", "email@gmail.com", UserRole.Buyer);
            Assert.ThrowsException<ArgumentException>(() => db.CreateUser(usr));

        }



        // Test if a new client is successfully created
        [TestMethod]
        public void CreateClientNormalTest()
        {
            bool passed;
            Client client = new Client("Xixibubu");

            DAL db = new DAL();
            try
            {
                db.CreateClient(client);
                passed = true;
            }
            catch (Exception)
            {
                // Fail if an exception is thrown
                passed = false;
            }

            Assert.IsTrue(passed);
        }

        // Test if an alert message is displayed when inserting an existent client
        [TestMethod]
        public void CreateClientException()
        {
            DAL db = new DAL();
            Client client = new Client("Jujuba");

            // Insert the first client normaly
            db.CreateClient(client);

            // Insert the same client again with the same username
            Assert.ThrowsException<ArgumentException>(() => db.CreateClient(client));

        }
    }
}
