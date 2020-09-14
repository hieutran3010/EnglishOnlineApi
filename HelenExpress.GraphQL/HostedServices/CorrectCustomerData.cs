using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelenExpress.GraphQL.HostedServices
{
    public class CorrectCustomerData : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public CorrectCustomerData(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();
            var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var customerRepository = scopedUnitOfWork.GetRepository<Customer>();
            var billRepository = scopedUnitOfWork.GetRepository<Bill>();

            // Replace all bad character in phone number
            var customers = await customerRepository.GetQueryable(false)
                .ToArrayAsync(cancellationToken: stoppingToken);

            var invalidContents = new[] {"khongco", "secapnhat", "kobiet", "khôngcó", "khôngco"};

            try
            {
                foreach (var customer in customers)
                {
                    customer.Name = customer.Name?.Trim().Replace("-", string.Empty);
                    if (string.IsNullOrWhiteSpace(customer.Phone))
                    {
                        customer.Phone = null;
                    }
                    else
                    {
                        customer.Phone = Regex.Replace(customer.Phone, "[^0-9]*", "").TrimStart(new[] {'0'});
                    }

                    if (customer.Address != null &&
                        invalidContents.Contains(customer.Address.ToLower().Trim().Replace(" ", string.Empty)))
                    {
                        customer.Address = null;
                    }

                    customerRepository.Update(customer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                // delete un-phone customers
                var noPhoneCustomers = await customerRepository.GetQueryable(false)
                    .Where(c => string.IsNullOrWhiteSpace(c.Phone)).ToArrayAsync(stoppingToken);
                foreach (var noPhoneCustomer in noPhoneCustomers)
                {
                    await this.TransferRelatedBills(billRepository, noPhoneCustomer);
                }
                customerRepository.RemoveRange(noPhoneCustomers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            // grouped by phone
            try
            {
                var groupedByPhone = await customerRepository.GetQueryable(false)
                    .Where(c => c.Phone != null)
                    .OrderBy(c => c.Phone)
                    .ToListAsync(stoppingToken);
                var browseData = groupedByPhone.GroupBy(c => c.Phone)
                    .ToDictionary(t => t.Key, t => t.ToList());

                foreach (var customer in browseData)
                {
                    var relatedCustomers = customer.Value;
                    if (relatedCustomers.Count <= 1)
                    {
                        continue;
                    }

                    // keep the first customer, then remove the rest
                    var firstCustomer = relatedCustomers.FirstOrDefault();
                    relatedCustomers.Remove(firstCustomer);

                    foreach (var relatedCustomer in relatedCustomers)
                    {
                        await this.TransferRelatedBills(billRepository, relatedCustomer, firstCustomer);
                    }

                    customerRepository.RemoveRange(relatedCustomers);
                }

                await scopedUnitOfWork.SaveChangesAsync(stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task TransferRelatedBills(IRepository<Bill> billRepository, Customer relatedCustomer,
            Customer replaceCustomer = null)
        {
            // set all related bills to the first cus
            var senderBills = await billRepository.GetQueryable(false)
                .Where(b => b.SenderId == relatedCustomer.Id)
                .ToListAsync();
            foreach (var senderBill in senderBills)
            {
                senderBill.SenderId = replaceCustomer?.Id;
                billRepository.Update(senderBill);
            }

            var receiverBills = await billRepository.GetQueryable(false)
                .Where(b => b.ReceiverId == relatedCustomer.Id)
                .ToListAsync();
            foreach (var receiverBill in receiverBills)
            {
                receiverBill.ReceiverId = replaceCustomer?.Id;
                billRepository.Update(receiverBill);
            }
        }
    }
}