using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Core;
using SoftwareVentas.Core.Pagination;
using SoftwareVentas.Data;
using SoftwareVentas.Data.Entities;
using SoftwareVentas.DTOs;
using SoftwareVentas.Helpers;

namespace SoftwareVentas.Services
{
    // Interfaz ISaleService
    public interface ISaleService
    {
        public Task<Response<Sale>> CreateAsync(SaleForCreationDTO dto);
        Task<Response<bool>> DeleteAsync(int id);
        public Task<Response<Sale>> EditAsync(SaleDTO dto);

        public Task<Response<PaginationResponse<Sale>>> GetListAsync(PaginationRequest request);

        public Task<Response<Sale>> GetOneAsync(int id);
    }

    // Clase SaleService
    public class SaleService : ISaleService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public SaleService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        // Crear una venta
        public async Task<Response<Sale>> CreateAsync(SaleForCreationDTO dto)
        {
            try
            {
                Sale sale = _converterHelper.ToSale(dto);

                await _context.Sales.AddAsync(sale);
                await _context.SaveChangesAsync();

                return ResponseHelper<Sale>.MakeResponseSuccess(sale, "Venta creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Sale>.MakeResponseFail(ex);
            }
        }

        // Eliminar una venta
        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                Sale? sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

                if (sale == null)
                {
                    return ResponseHelper<bool>.MakeResponseFail($"No existe venta con id '{id}'");
                }

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();

                return ResponseHelper<bool>.MakeResponseSuccess(true, "Venta eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<bool>.MakeResponseFail(ex);
            }
        }

        // Editar una venta
        public async Task<Response<Sale>> EditAsync(SaleDTO dto)
        {
            try
            {
                Sale? sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (sale is null)
                {
                    return ResponseHelper<Sale>.MakeResponseFail($"No existe venta con id '{dto.Id}'");
                }

                sale.SaleDate = dto.SaleDate;
                sale.CustomerId = dto.CustomerId;
                sale.EmployeeId = dto.EmployeeId;

                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();

                return ResponseHelper<Sale>.MakeResponseSuccess(sale, "Venta actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Sale>.MakeResponseFail(ex);
            }
        }

        // Obtener lista de ventas con paginación
        public async Task<Response<PaginationResponse<Sale>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Sale> query = _context.Sales.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.CustomerId.ToString().Contains(request.Filter) || s.EmployeeId.ToString().Contains(request.Filter));
                }

                PagedList<Sale> list = await PagedList<Sale>.ToPagedListAsync(query, request);

                PaginationResponse<Sale> result = new PaginationResponse<Sale>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Sale>>.MakeResponseSuccess(result, "Ventas obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Sale>>.MakeResponseFail(ex);
            }
        }

        // Obtener una venta específica por ID
        public async Task<Response<Sale>> GetOneAsync(int id)
        {
            try
            {
                Sale? sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);

                if (sale is null)
                {
                    return ResponseHelper<Sale>.MakeResponseFail("La venta con el id indicado no existe");
                }

                return ResponseHelper<Sale>.MakeResponseSuccess(sale);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Sale>.MakeResponseFail(ex);
            }
        }
    }
}
