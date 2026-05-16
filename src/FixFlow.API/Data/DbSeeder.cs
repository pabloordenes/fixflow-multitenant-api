using FixFlow.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixFlow.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Aplicar migraciones pendientes automáticamente al arrancar (ideal para Docker)
            await context.Database.MigrateAsync();

            // Si ya existe un Tenant, asumimos que la base de datos ya fue poblada y salimos
            if (await context.Tenants.AnyAsync())
            {
                return;
            }

            // 1. Crear el Tenant Piloto
            var vektorTenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "TenantEx",
                TaxId = "76.123.456-7", // RUT Ficticio de empresa
                Industry = "Servicios",
                Plan = "Piloto",
                IsActive = true
            };

            await context.Tenants.AddAsync(vektorTenant);

            // 2. Crear el Usuario Administrador por defecto
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Pablo Ordenes",
                Email = "admin@fixflow.cl",
                // Hasheamos la contraseña "FixFlow2026!" con BCrypt
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("FixFlow2026!"),
                Role = "Admin",
                TenantId = vektorTenant.Id, // Vinculamos el usuario al Tenant
                Tenant = vektorTenant
            };

            await context.Users.AddAsync(adminUser);

            // Guardar los cambios en la base de datos
            await context.SaveChangesAsync();
        }
    }
}
