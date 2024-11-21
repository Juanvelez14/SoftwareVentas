using SoftwareVentas.Services;

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
           
            await new UserRolesSeeder(_context, _usersService).SeedAsync();
            await new PermissionsSeeder(_context).SeedAsync();

        }
	}
}