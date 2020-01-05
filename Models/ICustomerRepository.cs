using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessManagment.Models
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(int Id);
        IEnumerable<Customer> GetAllCustomers();
        Customer AddCustomer(Customer cust);
        Customer EditCustomer(Customer custChanges);
        Customer DeleteCustomer(int id);
    }
}
