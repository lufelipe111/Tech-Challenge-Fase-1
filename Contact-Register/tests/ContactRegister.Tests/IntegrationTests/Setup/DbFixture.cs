using ContactRegister.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactRegister.Tests.IntegrationTests.Setup;

// TODO: Entendi esta Fixture, mas acho que não precisa. Tudo pode ser configurado no WebApplicationFactory
public class DbFixture : IDisposable
{
    private readonly AppDbContext _context;
    public readonly string DatabaseName = $"Context-{Guid.NewGuid()}";
    public readonly string ConnectionString;
    private bool _disposed;

    public DbFixture()
    {
        ConnectionString = $"Server=localhost;Database={DatabaseName};User Id=sa;Password=Password123;TrustServerCertificate=True;";

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(ConnectionString);

        _context = new AppDbContext(builder.Options);

        _context.Database.Migrate();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Database.EnsureDeleted();
            }

            _disposed = true;
        }
    }
}
