using SoftwareVentas.BLL;
using SoftwareVentas.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace SoftwareVentas.Data.Seeders
{
    public class SeederDB
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public SeederDB(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }


        public async Task SeedAsync()
        {
            //await new SectionsSeeder(_context).SeedAsync();
            //await new PermissionsSeeder(_context).SeedAsync();
            //await new UserRolesSeeder(_context, _usersService).SeedAsync();
        }
    }
}
}
