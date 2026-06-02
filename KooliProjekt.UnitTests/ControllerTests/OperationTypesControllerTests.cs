using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OperationTypesControllerTests
    {
        private readonly Mock<IOperationTypeService> _operationTypeServiceMock;
        private readonly OperationTypesController _controller;

        public OperationTypesControllerTests()
        {
            _operationTypeServiceMock = new Mock<IOperationTypeService>();
            _controller = new OperationTypesController(_operationTypeServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            var page = 1;
            var pagedResult = new PagedResult<OperationType> { Results = new List<OperationType>(), CurrentPage = 1, PageCount = 1, PageSize = 5, RowCount = 0 };
            _operationTypeServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);
            var result = await _controller.Index(page) as ViewResult;
            Assert.NotNull(result);
            var model = result.Model as OperationTypesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult, model.Data);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_operationtype_is_missing()
        {
            int id = 1;
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync((OperationType)null);
            var result = await _controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_view_with_model_when_operationtype_was_found()
        {
            int id = 1;
            var operationType = new OperationType { Id = id, Name = "Maintenance" };
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operationType);
            var result = await _controller.Details(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Details");
            Assert.Equal(operationType, result.Model);
        }

        [Fact]
        public void Create_should_return_view()
        {
            var result = _controller.Create() as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_operationtype_is_missing()
        {
            int id = 1;
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync((OperationType)null);
            var result = await _controller.Edit(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_with_model_when_operationtype_was_found()
        {
            int id = 1;
            var operationType = new OperationType { Id = id, Name = "Maintenance" };
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operationType);
            var result = await _controller.Edit(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Edit");
            Assert.Equal(operationType, result.Model);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_missing()
        {
            int? id = null;
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_operationtype_is_missing()
        {
            int id = 1;
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync((OperationType)null);
            var result = await _controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_view_with_model_when_operationtype_was_found()
        {
            int id = 1;
            var operationType = new OperationType { Id = id, Name = "Maintenance" };
            _operationTypeServiceMock.Setup(x => x.Get(id)).ReturnsAsync(operationType);
            var result = await _controller.Delete(id) as ViewResult;
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Delete");
            Assert.Equal(operationType, result.Model);
        }

        [Fact]
        public async Task Create_POST_should_return_view_when_modelstate_is_invalid()
        {
            _controller.ModelState.AddModelError("key", "error");
            var operationType = new OperationType { Name = "Maintenance" };
            var result = await _controller.Create(operationType) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(operationType, result.Model);
            _operationTypeServiceMock.Verify(x => x.Save(It.IsAny<OperationType>()), Times.Never);
        }

        [Fact]
        public async Task Create_POST_should_redirect_when_modelstate_is_valid()
        {
            var operationType = new OperationType { Name = "Maintenance" };
            _operationTypeServiceMock.Setup(x => x.Save(operationType)).Verifiable();
            var result = await _controller.Create(operationType) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _operationTypeServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_POST_should_return_notfound_when_id_mismatch()
        {
            var operationType = new OperationType { Id = 2, Name = "Maintenance" };
            var result = await _controller.Edit(1, operationType) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_POST_should_return_view_when_modelstate_is_invalid()
        {
            _controller.ModelState.AddModelError("key", "error");
            var operationType = new OperationType { Id = 1, Name = "Maintenance" };
            var result = await _controller.Edit(1, operationType) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(operationType, result.Model);
            _operationTypeServiceMock.Verify(x => x.Save(It.IsAny<OperationType>()), Times.Never);
        }

        [Fact]
        public async Task Edit_POST_should_redirect_when_modelstate_is_valid()
        {
            var operationType = new OperationType { Id = 1, Name = "Maintenance" };
            _operationTypeServiceMock.Setup(x => x.Save(operationType)).Verifiable();
            var result = await _controller.Edit(1, operationType) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _operationTypeServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_operationtype()
        {
            int id = 1;
            _operationTypeServiceMock.Setup(x => x.Delete(id)).Verifiable();
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _operationTypeServiceMock.VerifyAll();
        }
    }
}