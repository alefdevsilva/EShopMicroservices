using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync())
            return;

        // Marten UPSERT will cater for existing records
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
    {
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone X",
            Description = "Description",
            ImageFile = "product-1.png",
            Price = 950.00M,
            Category = new List<string>{ "Smart Phone" }
        },
         new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung S24",
            Description = "Description",
            ImageFile = "product-1.png",
            Price = 850.00M,
            Category = new List<string>{ "Smart Phone" }
        }
    };
}
