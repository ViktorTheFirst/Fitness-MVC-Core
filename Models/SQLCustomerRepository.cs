using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Models
{
    public class SQLCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;

        public SQLCustomerRepository(AppDbContext context)
        {
            this.context = context;
        }


    
        public Customer AddCustomer(Customer cust)
        {
            context.Customers.Add(cust);
            context.SaveChanges(); //saves the changes on the customer table on SQL
            return cust; 
        }

        public Customer DeleteCustomer(int id)
        {
            Customer customer = context.Customers.Find(id);
            if(customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
            return customer;
        }

        public Customer EditCustomer(Customer custChanges)
        {
            var picture = context.Customers.Attach(custChanges);
            //we have to tell EntityFrameWork that the entity we attach is modified
            picture.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return custChanges;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return context.Customers;
        }

        public Customer GetCustomer(int Id)
        {
            return context.Customers.Find(Id);
        }
    }
}
