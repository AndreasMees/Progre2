namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceLineRepository invoiceLineRepository,
            IOperationRepository operationRepository,
            IOperationTypeRepository operationTypeRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            VehicleRepository = vehicleRepository;
            InvoiceRepository = invoiceRepository;
            InvoiceLineRepository = invoiceLineRepository;
            OperationRepository = operationRepository;
            OperationTypeRepository = operationTypeRepository;
        }

        public ICustomerRepository CustomerRepository { get; private set; }
        public IVehicleRepository VehicleRepository { get; private set; }
        public IInvoiceRepository InvoiceRepository { get; private set; }
        public IInvoiceLineRepository InvoiceLineRepository { get; private set; }
        public IOperationRepository OperationRepository { get; private set; }
        public IOperationTypeRepository OperationTypeRepository { get; private set; }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}