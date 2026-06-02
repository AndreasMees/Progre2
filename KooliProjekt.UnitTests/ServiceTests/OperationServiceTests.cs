using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OperationServiceTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly OperationService _service;

        public OperationServiceTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Operations).Returns(_operationRepositoryMock.Object);
            _service = new OperationService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var operations = new List<Operation>
            {
                new Operation { Id = 1, Date = DateTime.Now, Status = OperationStatus.Pending },
                new Operation { Id = 2, Date = DateTime.Now, Status = OperationStatus.Completed }
            };
            var expectedResult = new PagedResult<Operation>
            {
                Results = operations,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _operationRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _operationRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_operation_when_found()
        {
            int id = 1;
            var expectedOperation = new Operation { Id = id, Date = DateTime.Now, Status = OperationStatus.Pending };

            _operationRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedOperation);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _operationRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_operation()
        {
            var operation = new Operation { Id = 0, Date = DateTime.Now, Status = OperationStatus.Pending };
            var savedOperation = new Operation { Id = 1, Date = DateTime.Now, Status = OperationStatus.Pending };

            _operationRepositoryMock.Setup(x => x.Add(operation)).Returns(savedOperation);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(operation);

            Assert.True(result);
            _operationRepositoryMock.Verify(x => x.Add(operation), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_operation()
        {
            var operation = new Operation { Id = 1, Date = DateTime.Now, Status = OperationStatus.Completed };

            _operationRepositoryMock.Setup(x => x.Update(operation)).Returns(operation);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(operation);

            Assert.True(result);
            _operationRepositoryMock.Verify(x => x.Update(operation), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_operation_when_found()
        {
            int id = 1;
            var operation = new Operation { Id = id, Date = DateTime.Now, Status = OperationStatus.Pending };

            _operationRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(operation);
            _operationRepositoryMock.Setup(x => x.Remove(operation)).Returns(operation);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _operationRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _operationRepositoryMock.Verify(x => x.Remove(operation), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_return_false_when_operation_not_found()
        {
            int id = 99;
            _operationRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync((Operation)null);

            var result = await _service.Delete(id);

            Assert.False(result);
            _operationRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _operationRepositoryMock.Verify(x => x.Remove(It.IsAny<Operation>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}