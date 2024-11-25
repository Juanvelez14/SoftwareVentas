using Humanizer;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using System.Net;
using System.Reflection.Metadata;

namespace SoftwareVentas.Helpers
{
    public interface IConverterHelper
    {
        public Customer ToCustomer(CustomerDTO dto);
        //Customer ToCustomer(Customer dto);
        public CustomerDTO ToCustomerDTO(Customer result);
        Role ToRole(RoleDTO dto);
        public Task<RoleDTO> ToRoleDTOAsync(Role role);
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
        public Employee ToEmployee(EmployeeDTO dto);
        public EmployeeDTO ToEmployeeDTO(Employee result);
        public Sale ToSale(SaleForCreationDTO dto);
        public SaleDTO ToSaleDTO(Sale result);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _context;

        public ConverterHelper(ICombosHelper combosHelper)
        {
            _combosHelper = combosHelper;
        }

        public Customer ToCustomer(CustomerDTO dto)
        {
            return new Customer
            {
                Name = dto.Name,
                idCustomer = dto.idCustomer,
                address = dto.address,
                mainPhone = dto.mainPhone,
            };
        }

       // public Customer ToCustomer(Customer dto)
        //{
          //  throw new NotImplementedException();
        //}

        public CustomerDTO ToCustomerDTO(Customer customer)
        {
            return new CustomerDTO
            {
                Name = customer.Name,
                idCustomer = customer.idCustomer,
                address = customer.address,
                mainPhone = customer.mainPhone,
            };

        }

        public Employee ToEmployee(EmployeeDTO dto)
        {
            return new Employee
            {
                Id = dto.Id,
                Name = dto.Name,
                RoleId = dto.RoleId
            };
        }

        public EmployeeDTO ToEmployeeDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                RoleId = employee.RoleId
            };
        }

        public Role ToRole(RoleDTO dto)
        {
            return new Role
            {
                Id = dto.Id,
                RoleName = dto.RoleName
            };
        }

        public async Task<RoleDTO> ToRoleDTOAsync(Role role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Descripcion = p.Descripcion,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id),
            }).ToListAsync();

            return new RoleDTO
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Permissions = permissions,
            };
        }

        public Sale ToSale(SaleForCreationDTO dto)
        {
            return new Sale
            {
                SaleDate = dto.SaleDate,
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId
            };
        }

        public SaleDTO ToSaleDTO(Sale sale)
        {
            return new SaleDTO
            {
                Id = sale.Id,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                EmployeeId = sale.EmployeeId
            };
        }

        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id.ToString(),
                Document = dto.Document,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                RoleId = dto.RoleId,
                PhoneNumber = dto.PhoneNumber,
            };
        }

        public async Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true)
        {
            return new UserDTO
            {
                Id = isNew ? Guid.NewGuid() : Guid.Parse(user.Id),
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = await _combosHelper.GetComboSoftwareVentasRolesAsync(),
                RoleId = user.RoleId,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}