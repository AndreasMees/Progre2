namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInvoiceLineRepository InvoiceLineRepository { get; }
        IOperationRepository OperationRepository { get; }
        IOperationTypeRepository OperationTypeRepository { get; }

        ICustomerRepository Customers { get; }
        IVehicleRepository Vehicles { get; }
        IInvoiceRepository Invoices { get; }
        IInvoiceLineRepository InvoiceLines { get; }
        IOperationRepository Operations { get; }
        IOperationTypeRepository OperationTypes { get; }

        Task BeginTransaction();
        Task Commit();
        Task Rollback();
        Task<int> SaveChangesAsync();
    }
}