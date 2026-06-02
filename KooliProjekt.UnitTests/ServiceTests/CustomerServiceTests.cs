using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Customers).Returns(_customerRepositoryMock.Object);
            _service = new CustomerService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            var page = 1;
            var pageSize = 10;
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test 1", Email = "test1@test.com" },
                new Customer { Id = 2, Name = "Test 2", Email = "test2@test.com" }
            };
            var expectedResult = new PagedResult<Customer>
            {
                Results = customers,
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = 2
            };

            _customerRepositoryMock.Setup(x => x.List(page, pageSize, null)).ReturnsAsync(expectedResult);

            var result = await _service.List(page, pageSize, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.RowCount);
            _customerRepositoryMock.Verify(x => x.List(page, pageSize, null), Times.Once);
        }

        [Fact]
        public async Task Get_should_return_customer_when_found()
        {
            int id = 1;
            var expectedCustomer = new Customer { Id = id, Name = "Test", Email = "test@test.com" };

            _customerRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(expectedCustomer);

            var result = await _service.Get(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _customerRepositoryMock.Verify(x => x.Get(id), Times.Once);
        }

        [Fact]
        public async Task Save_should_add_new_customer()
        {
            var customer = new Customer { Id = 0, Name = "New Customer", Email = "new@test.com" };
            var savedCustomer = new Customer { Id = 1, Name = "New Customer", Email = "new@test.com" };

            _customerRepositoryMock.Setup(x => x.Add(customer)).Returns(savedCustomer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Save(customer);

            Assert.True(result);
            _customerRepositoryMock.Verify(x => x.Add(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_should_update_existing_customer()
        {
            var customer = new Customer { Id = 1, Name = "Updated Customer", Email = "updated@test.com" };

            _customerRepositoryMock.Setup(x => x.Update(customer)).Returns(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Update(customer);

            Assert.True(result);
            _customerRepositoryMock.Verify(x => x.Update(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_remove_customer_when_found()
        {
            int id = 1;
            var customer = new Customer { Id = id, Name = "Test", Email = "test@test.com" };

            _customerRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(customer);
            _customerRepositoryMock.Setup(x => x.Remove(customer)).Returns(customer);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.Delete(id);

            Assert.True(result);
            _customerRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _customerRepositoryMock.Verify(x => x.Remove(customer), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_return_false_when_customer_not_found()
        {
            int id = 99;
            _customerRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync((Customer)null);

            var result = await _service.Delete(id);

            Assert.False(result);
            _customerRepositoryMock.Verify(x => x.Get(id), Times.Once);
            _customerRepositoryMock.Verify(x => x.Remove(It.IsAny<Customer>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}