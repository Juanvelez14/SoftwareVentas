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
    public interface IProductsService
    {
        public Task<Response<Product>> CreateAsync(Product model);

        public Task<Response<Product>> DeleteteAsync(int id);

        public Task<Response<Product>> EditAsync(Product model);

        public Task<Response<PaginationResponse<Product>>> GetListAsync(PaginationRequest request);

        public Task<Response<Product>> GetOneAsync(int id);

        public Task<Response<Product>> ToggleAsync(ToggleProductStatusRequest request);
    }

    public class ProductsService : IProductsService
    {
        private readonly DataContext _context;

        public ProductsService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Product>> CreateAsync(Product model)
        {
            try
            {
                Product product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                    Discount = model.Discount,
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(product, "Producto creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }

        }

        public async Task<Response<Product>> DeleteteAsync(int id)
        {
            try
            {
                Response<Product> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Products.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(null, "Producto eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Product>> EditAsync(Product model)
        {
            try
            {
                _context.Products.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(model, "Producto actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Product>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Product> query = _context.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Product> list = await PagedList<Product>.ToPagedListAsync(query, request);

                PaginationResponse<Product> result = new PaginationResponse<Product>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Product>>.MakeResponseSuccess(result, "Productos obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Product>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Product>> GetOneAsync(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(s => s.idProduct == id);

                if (product is null)
                {
                    return ResponseHelper<Product>.MakeResponseFail("El Producto con el id indicado no existe");
                }

                return ResponseHelper<Product>.MakeResponseSuccess(product);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }
        public async Task<Response<Product>> ToggleAsync(ToggleProductStatusRequest request)
        {
            try
            {
                Response<Product> response = await GetOneAsync(request.ProductId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                Product product = response.Result;

                //product.IsHidden = request.Hide;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(null, "Producto actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }



    }
}
