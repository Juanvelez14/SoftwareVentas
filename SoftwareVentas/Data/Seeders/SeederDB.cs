namespace SoftwareVentas.Data.Seeders
{
    public class SeederDB
    {
        private readonly DataContext _context;

        public SeederDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync() 
        {
            
        }
    }
}
