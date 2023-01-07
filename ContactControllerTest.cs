using AutoMapper;
using ContactsApp;
using ContactsApp.Controllers;
using ContactsApp.ViewModel;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactApp.Test
{
    public class ContactControllerTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IValidator<ContactViewModel>> _validator;
        public ContactControllerTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _validator = new Mock<IValidator<ContactViewModel>>();

        }
        [Fact]
        public async void GetContact_ReturnsAViewResult_WithContact()
        {
            // Arrange
            var contactList = TestHelperClass.GetTestContacts();
            var mapper = TestHelperClass.GetMapper();
            var validator = new ContactValidator();

            _unitOfWork.Setup(p => p.Contacts.GetByIdAsync(1)).ReturnsAsync(contactList[1]);
            ContactController emp = new ContactController(_unitOfWork.Object, mapper,validator);

            // Act
            var result = await emp.GetContact(1);

            // Assert
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task GetContacts_ReturnsAViewResult_WithAListOfContacts()
        {
            // Arrange
            var mapper = TestHelperClass.GetMapper();
            var validator = new ContactValidator();

            _unitOfWork.Setup(repo => repo.Contacts.GetAll())
                .Returns(TestHelperClass.GetTestContacts());
            ContactController emp = new ContactController(_unitOfWork.Object,mapper,validator);

            // Act
            var result = emp.GetContacts();
            // Assert

            Assert.Equal(TestHelperClass.GetTestContacts().Count, result.Value.Count());
            Assert.NotNull(result.Value);

        }


        [Fact]
        public async Task CreateContact_ReturnsSuccess_GivenValidModel()
        {
            // Arrange & Act

            var contactList = TestHelperClass.GetTestContacts();
            var contactListView = TestHelperClass.GetTestContactsViewModel();
            var mapper = TestHelperClass.GetMapper(); 
            var validator = new ContactValidator();
            _unitOfWork.Setup(p => p.Contacts.AddAsync(contactList[1]));

            var controller = new ContactController(_unitOfWork.Object,mapper,validator);

           
            // Act
            var result = await controller.PostContact(contactListView[1]);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ReturnsSuccess_GivenvalidModel()
        {
            // Arrange
            var mapper = TestHelperClass.GetMapper();
            var validator = new ContactValidator();
            var contactList = TestHelperClass.GetTestContacts();

            _unitOfWork.Setup(repo => repo.Contacts.Remove(contactList[1]));
            var controller = new ContactController(_unitOfWork.Object,mapper,validator);

            // Act
            await controller.DeleteContact(2);

            // Assert
            _unitOfWork.Verify();

        }

    }
}