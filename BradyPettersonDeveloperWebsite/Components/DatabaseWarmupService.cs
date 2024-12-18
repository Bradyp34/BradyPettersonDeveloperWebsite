using BradyPettersonDeveloperWebsite.Models;
using Microsoft.EntityFrameworkCore;



namespace BradyPettersonDeveloperWebsite.Components {
    public class DatabaseWarmUpService {
        private readonly AppDbContext _context;

        public DatabaseWarmUpService (AppDbContext context) {
            _context = context;
        }

        public async Task WarmUpAsync () {
            try {
                // Attempt a simple database query to ensure connection is established
                await _context.Database.OpenConnectionAsync();
                await _context.Database.CloseConnectionAsync();
            }
            catch (Exception ex) {
                Console.WriteLine($"Database warm-up failed: {ex.Message}");
            }
        }
    }
}
