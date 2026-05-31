namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        ICustomerRepository CustomerRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInvoiceLineRepository InvoiceLineRepository { get; }
        IOperationRepository OperationRepository { get; }
        IOperationTypeRepository OperationTypeRepository { get; }
    }
}