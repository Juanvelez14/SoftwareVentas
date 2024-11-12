using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.Helpers;
using SoftwareVentas.Requests;
using System.Collections;
using System.Collections.Generic;

namespace SoftwareVentas.Services
{
    public interface ICustomerService
    {
        public Task<Response<Customer>> CreateAsync(Customer model);

        public Task<Response<Customer>> DeleteteAsync(int id);

        public Task<Response<Customer>> EditAsync(Customer model);

        public Task<Response<PaginationResponse<Customer>>> GetListAsync(PaginationRequest request);

        public Task<Response<Customer>> GetOneAsync(int id);

        public Task<Response<Customer>> ToggleAsync(ToggleProductStatusRequest request);
    }

    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Customer>> CreateAsync(Customer model)
        {
            try
            {
                Customer customer = new Customer
                {
                    Name = model.Name,
                    address = model.address,
                    mainPhone = model.mainPhone,
                };

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(customer, "Cliente creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }

        }

        public async Task<Response<Customer>> DeleteteAsync(int id)
        {
            try
            {
                Response<Customer> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Customers.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(null, "Cliente eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Customer>> EditAsync(Customer model)
        {
            try
            {
                _context.Customers.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(model, "Cliente actualizada con éxito");
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

                return ResponseHelper<PaginationResponse<Customer>>.MakeResponseSuccess(result, "Clientes obtenidos con éxito");
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

        public async Task<Response<Customer>> ToggleAsync(ToggleProductStatusRequest request)
        {
            try
            {
                Response<Customer> response = await GetOneAsync(request.ProductId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                Customer customer = response.Result;

                //product.IsHidden = request.Hide;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return ResponseHelper<Customer>.MakeResponseSuccess(null, "Cliente actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Customer>.MakeResponseFail(ex);
            }
        }

    }
}
