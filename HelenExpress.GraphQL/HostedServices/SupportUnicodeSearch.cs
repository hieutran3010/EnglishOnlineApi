using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.GraphQL.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelenExpress.GraphQL.HostedServices
{
    public class SupportUnicodeSearch : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public SupportUnicodeSearch(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();
            var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var customerRepo = scopedUnitOfWork.GetRepository<Customer>();
            var billRepo = scopedUnitOfWork.GetRepository<Bill>();

            var customers = await customerRepo.GetQueryable(false)
                .Where(c => string.IsNullOrWhiteSpace(c.NameNonUnicode)).ToListAsync(cancellationToken: stoppingToken);
            foreach (var customer in customers)
            {
                customer.NameNonUnicode = customer.Name?.RemoveUnicode();
            }

            var bills = await billRepo.GetQueryable(false)
                .Where(b => string.IsNullOrWhiteSpace(b.SenderNameNonUnicode) ||
                            string.IsNullOrWhiteSpace(b.ReceiverNameNonUnicode))
                .ToListAsync(cancellationToken: stoppingToken);
            foreach (var bill in bills)
            {
                bill.SenderNameNonUnicode = bill.SenderName?.RemoveUnicode();
                bill.ReceiverNameNonUnicode = bill.ReceiverName?.RemoveUnicode();
            }

            await scopedUnitOfWork.SaveChangesAsync(stoppingToken);
        }
    }
}