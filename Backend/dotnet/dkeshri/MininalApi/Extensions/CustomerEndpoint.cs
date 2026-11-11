using Microsoft.EntityFrameworkCore;
using MininalApi.Data;
using MininalApi.Dtos;
using MininalApi.Entities;

namespace MininalApi.Extensions
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers")
            .WithTags("Customers");

            // GET: /api/customers
            group.MapGet("/", async (AppDbContext dbContext) =>
            {
                var customers = await dbContext.Customers
                .Include(c => c.Address)
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync();
                return Results.Ok(customers);
            });

            group.MapGet("/{id:int}", async (int id, AppDbContext dbContext) =>
            {
                var customer = await dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            });

            group.MapPost("/", async (CustomerDto customer, AppDbContext dbContext) =>
            {
                Customer newCustomer = new Customer
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address
                };
                newCustomer.CreatedDate = DateTime.UtcNow;
                newCustomer.LastModifiedDate = DateTime.UtcNow;
                dbContext.Customers.Add(newCustomer);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/customers/{newCustomer.Id}", newCustomer);
            });
        }
    }
}
