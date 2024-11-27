using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftwareVentas.Data;

namespace SoftwareVentas.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasRolesAsync();
        Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasCustomersAsync();
        Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasEmployeesAsync();
        //public Task<IEnumerable<SelectListItem>> GetComboSections();
    }
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasRolesAsync()
        {
            List<SelectListItem> list = await _context.Roles.Select(r => new SelectListItem
            {
                Text = r.RoleName,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });

            return list;
        }


        public async Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasCustomersAsync()
        {
            List<SelectListItem> list = await _context.Customers.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.idCustomer.ToString()
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSoftwareVentasEmployeesAsync()
        {
            List<SelectListItem> list = await _context.Employees.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });

            return list;
        }
        //public async Task<IEnumerable<SelectListItem>> GetComboSections()
        //{
        //    List<SelectListItem> list = await _context.Sections.Select(s => new SelectListItem
        //    {
        //        Text = s.Name,
        //        Value = s.Id.ToString()
        //    }).ToListAsync();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "[Seleccione una sección]",
        //        Value = "0"
        //    });

        //    return list;
        //}

    }
}
