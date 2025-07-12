using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using OrderStateMachine.Database.Mapping;

namespace OrderStateMachine
{
    public class OrderStateDBContext : SagaDbContext
    {
        public OrderStateDBContext(DbContextOptions<OrderStateDBContext> options):base(options)
        {
        }
        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                return new ISagaClassMap[]
                {
                    new OrderStateMap()
                };
            }
        }

    }
}
