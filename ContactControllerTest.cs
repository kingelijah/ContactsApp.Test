using ContactsApp.Contracts;
using ContactsApp.Controllers;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactApp.Test
{
    public class ContactControllerTest
    {
        private Mock<IContactRepository> _mockRepository;
        public ContactControllerTest()
        {
            _mockRepository = new Mock<IContactRepository>();
        }
        [Fact]
        public async void GetContact_ReturnsAViewResult_WithContact()
        {
            var contactList = GetTestContacts();
            _mockRepository.Setup(p => p.GetContact(1)).ReturnsAsync(contactList[1]);
            ContactController emp = new ContactController(_mockRepository.Object);

            var result = await emp.GetContact(1);
            Assert.True(result.Value.Equals(contactList[1]));
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetContacts_ReturnsAViewResult_WithAListOfContacts()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetContacts())
                .ReturnsAsync(GetTestContacts());
            ContactController emp = new ContactController(_mockRepository.Object);

            // Act
            var result =  emp.GetContacts().GetAwaiter().GetResult();
            // Assert
            
            Assert.Equal(GetTestContacts().Count, result.Value.Count());
            Assert.NotNull(result.Value);

        }

        [Fact]
        public async Task CreateContact_ReturnsBadRequest_GivenEmptyModel()
        {
            // Arrange & Act
            var controller = new ContactController(_mockRepository.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.PostContact(contact: null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Contact>>(result);
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateContact_ReturnsSuccess_GivenvalidModel()
        {
            // Arrange & Act
           
            var contactList = GetTestContacts();

            _mockRepository.Setup(p => p.AddContact(contactList[1])).ReturnsAsync(contactList[1]);

            var controller = new ContactController(_mockRepository.Object);

            // Act
            var result = await controller.PostContact(contactList[1]);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Contact>>(result);
            Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        }

        [Fact]
        public async Task UpdateContact_ReturnsSuccess_GivenvalidModel()
        {
            // Arrange
            var contactList = GetTestContacts();

            _mockRepository.Setup(repo => repo.UpdateContact(It.IsAny<Contact>())).ReturnsAsync(1);
            var controller = new ContactController(_mockRepository.Object);

            // Act
            var result = await controller.UpdateContact(contactList[1]);

            // Assert
            var actionResult = Assert.IsType<ActionResult<int>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);

        }

        [Fact]
        public async Task DeleteContact_ReturnsSuccess_GivenvalidModel()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeleteContact(It.IsAny<int>())).Verifiable();
            var controller = new ContactController(_mockRepository.Object);

            // Act
            controller.DeleteContact(3);

            // Assert
            _mockRepository.Verify();
        }

        private List<Contact> GetTestContacts()
        {
            var contacts = new List<Contact>();
            contacts.Add(new Contact()
            {
                Id = 1,
                FirstName = "Lionel",
                LastName = "Messi",
                PhoneNumber = "0904787",
                Email = "lionelmessi@gmail.com"
            });
            contacts.Add(new Contact()
            {
                Id = 2,
                FirstName = "Andres",
                LastName = "Iniesta",
                PhoneNumber = "090348976",
                Email = "andresiniesta@gmail.com"
            });
            return contacts;
        }

    }
}