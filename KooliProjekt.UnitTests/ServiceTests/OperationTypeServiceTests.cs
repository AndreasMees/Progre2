using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OperationTypeServiceTests
    {
        private readonly Mock<IOperationTypeRepository> _operationTypeRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly OperationTypeService _service;

        public OperationTypeServiceTests()
        {
            _operationTypeRepositoryMock = new Mock<IOperationTypeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.OperationTypes).Returns(_operationTypeRepositoryMock.Object);
            _service = new OperationTypeService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var operationTypes = new List<OperationType>
            {
                new OperationType { Id = 1, Name = "Oil Change" },
                new OperationType { Id = 2, Name = "Brake Service" }
            };
            var expectedResult = new PagedResult<OperationType>
            {
                Results = operationTypes,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _operationTypeRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _operationTypeRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_operationType_when_found()
        {
            int id = 1;
            var expectedOperationType = new OperationType { Id = id, Name = "Oil Change" };

            _operationTypeRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedOperationType);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _operationTypeRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_operationType()
        {
            var operationType = new OperationType { Id = 0, Name = "Tire Service" };
            var savedOperationType = new OperationType { Id = 1, Name = "Tire Service" };

            _operationTypeRepositoryMock.Setup(x => x.Add(operationType)).Returns(savedOperationType);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(operationType);

            Assert.True(result);
            _operationTypeRepositoryMock.Verify(x => x.Add(operationType), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_operationType()
        {
            var operationType = new OperationType { Id = 1, Name = "Updated Service" };

            _operationTypeRepositoryMock.Setup(x => x.Update(operationType)).Returns(operationType);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(operationType);

            Assert.True(result);
            _operationTypeRepositoryMock.Verify(x => x.Update(operationType), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_operationType_when_found()
        {
            int id = 1;
            var operationType = new OperationType { Id = id, Name = "Oil Change" };

            _operationTypeRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(operationType);
            _operationTypeRepositoryMock.Setup(x => x.Remove(operationType)).Returns(operationType);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _operationTypeRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _operationTypeRepositoryMock.Verify(x => x.Remove(operationType), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}