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
    public interface IEmployeeService
    {
        public Task<Response<Employee>> CreateAsync(EmployeeDTO dto);
        Task<Response<bool>> DeleteAsync(int id);
        public Task<Response<Employee>> EditAsync(EmployeeDTO dto);

        public Task<Response<PaginationResponse<Employee>>> GetListAsync(PaginationRequest request);

        public Task<Response<Employee>> GetOneAsync(int id);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public EmployeeService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Employee>> CreateAsync(EmployeeDTO dto)
        {
            try
            {
                Employee employee = _converterHelper.ToEmployee(dto);

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return ResponseHelper<Employee>.MakeResponseSuccess(employee, "Empleado creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Employee>.MakeResponseFail(ex);
            }
        }

        public Task<Response<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<Employee>> EditAsync(EmployeeDTO dto)
        {
            try
            {
                Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == dto.Id);

                if (employee is null)
                {
                    return ResponseHelper<Employee>.MakeResponseFail($"No existe empleado con id '{dto.Id}'");
                }

                employee.Name = dto.Name;
                employee.RoleId = dto.RoleId;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return ResponseHelper<Employee>.MakeResponseSuccess(employee, "Empleado actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Employee>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Employee>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Employee> query = _context.Employees.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(e => e.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Employee> list = await PagedList<Employee>.ToPagedListAsync(query, request);

                PaginationResponse<Employee> result = new PaginationResponse<Employee>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Employee>>.MakeResponseSuccess(result, "Empleados obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Employee>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Employee>> GetOneAsync(int id)
        {
            try
            {
                Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

                if (employee is null)
                {
                    return ResponseHelper<Employee>.MakeResponseFail("El empleado con el id indicado no existe");
                }

                return ResponseHelper<Employee>.MakeResponseSuccess(employee);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Employee>.MakeResponseFail(ex);
            }
        }
    }
}
