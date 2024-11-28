using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;
using System.Reflection.Metadata;


namespace SoftwareVentas.Services
{
    public interface ICustomerService
    {
        public Task<Response<Customer>> CreateAsync(CustomerDTO dto);

        public Task<Response<Customer>> EditAsync(CustomerDTO dto);

        public Task<Response<PaginationResponse<Customer>>> GetListAsync(PaginationRequest request);

        public Task<Response<Customer>> GetOneAsync(int id);

        public Task<Response<Customer>> DeleteAsync(int id);

    }

    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public CustomerService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Customer>> CreateAsync(CustomerDTO dto)
        {
            try
            {
                Customer customer = _converterHelper.ToCustomer(dto);

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(customer, "Cliente creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Customer>> EditAsync(CustomerDTO dto)
        {
            try
            {
                Customer? customer = await _context.Customers.FirstOrDefaultAsync(b => b.idCustomer == dto.idCustomer);

                if (customer is null)
                {
                    return ResponseHelper<Customer>.MakeResponseFail($"No existe cliente con id '{dto.idCustomer}'");
                }

                customer.Name = dto.Name;
                customer.address = dto.address;
                customer.mainPhone = dto.mainPhone;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(customer, "Cliente actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Customer>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Customer> query = _context.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Customer> list = await PagedList<Customer>.ToPagedListAsync(query, request);

                PaginationResponse<Customer> result = new PaginationResponse<Customer>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Customer>>.MakeResponseSuccess(result, "Clientes obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Customer>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Customer>> GetOneAsync(int id)
        {
            try
            {
                Customer? customer = await _context.Customers.FirstOrDefaultAsync(s => s.idCustomer == id);

                if (customer is null)
                {
                    return ResponseHelper<Customer>.MakeResponseFail("El Cliente con el id indicado no existe");
                }

                return ResponseHelper<Customer>.MakeResponseSuccess(customer);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }
        }

        // Implementación del método DeleteAsync
        public async Task<Response<Customer>> DeleteAsync(int id)
        {
            try
            {
                // Buscar el cliente por ID
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.idCustomer == id);

                // Si no se encuentra, devolver un error
                if (customer == null)
                {
                    return ResponseHelper<Customer>.MakeResponseFail("Cliente no encontrado");
                }

                // Eliminar el cliente
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync(); // Guardar los cambios

                // Devolver una respuesta exitosa
                return ResponseHelper<Customer>.MakeResponseSuccess(customer, "Cliente eliminado con éxito");
            }
            catch (Exception ex)
            {
                // Devolver un error si ocurre alguna excepción
                return ResponseHelper<Customer>.MakeResponseFail(ex, "Error al eliminar el cliente");
            }
        }
    }
}