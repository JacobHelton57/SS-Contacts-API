using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS_Contacts_API.Controllers;
using SS_Contacts_API.Models;

namespace SS_Contacts_APITests
{
    [TestClass]
    public class ContactsControllerTests
    {
        string path;
        public ContactsControllerTests()
        {
            string fileName = "Contacts.db";
            this.path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), @"Data\", fileName);
        }

        [TestMethod]
        public void GetAllReturnsAllContacts()
        {
            // Arrange            
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.GetAllContacts();

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<List<Contact>>));
        }

        [TestMethod]
        public void PostContactReturnsOKWithContact()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "John", Middle = "B", Last = "Doe" }};
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Post(contact);
            var contentResult = actionResult as OkNegotiatedContentResult<Contact>;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Contact>));
            Assert.AreEqual(contentResult.Content.Name, contact.Name);
        }

        [TestMethod]
        public void GetByValidIdReturnsOkWithContact()
        {
            // Arrange
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.GetContactById(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Contact>;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Contact>));
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void PutReturnsOkWithCorrectContact()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "Jim", Middle = "B", Last = "Doesnt" } };
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Put(1, contact);
            var contentResult = actionResult as OkNegotiatedContentResult<Contact>;
            

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Contact>));
            Assert.AreEqual(contact.Name, contentResult.Content.Name);
        }

        [TestMethod]
        public void DeleteReturnsOkWithCorrectId()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "John", Middle = "B", Last = "Doe" } };
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult postResult = controller.Post(contact);
            var postResultContent = postResult as OkNegotiatedContentResult<Contact>;
            IHttpActionResult deleteResult = controller.Delete(1);
            var contentResult = deleteResult as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsInstanceOfType(postResult, typeof(OkNegotiatedContentResult<Contact>));
            Assert.AreEqual(contact.Name, postResultContent.Content.Name);
            Assert.IsInstanceOfType(deleteResult, typeof(OkNegotiatedContentResult<string>));
            Assert.AreEqual("Contact 1 deleted.", contentResult.Content);
        }

        [TestMethod]
        public void GetByInvalidIdReturnsNotFound()
        {
            // Arrange
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.GetContactById(1000);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostInvalidContactReturnsBadRequest()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "John", Middle = "B", Last = "Doe" }, Phone = new List<Phone> { new Phone { Number = "123-456-7890", Type = "landline" } } };
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Post(contact);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutInvalidIdReturnsNotFound()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "Jim", Middle = "B", Last = "Doesnt" } };
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Put(1000, contact);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutInvalidContactReturnsBadRequest()
        {
            // Arrange
            Contact contact = new Contact { Name = new Name { First = "Jim", Middle = "B", Last = "Doesnt" }, Phone = new List<Phone> { new Phone { Number = "123-456-7890", Type = "landline" } } };
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Put(1, contact);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteInvalidIdReturnsNotFound()
        {
            // Arrange
            var controller = new ContactsController(path);

            // Act
            IHttpActionResult actionResult = controller.Delete(1000);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

    }
}
