using PaymentService.Application.RepositoryContract;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.Persistance.RepositoryImplementation
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly PaymentDbContext _context;
        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePaymentDetails(PaymentDetail payment)
        {
            await _context.PaymentDetails.AddAsync(payment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
