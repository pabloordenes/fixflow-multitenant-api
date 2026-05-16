using FixFlow.API.Entities;
using FixFlow.API.Entities.WorkOrderModels;
using FixFlow.API.Services.Tenant;
using FixFlow.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FixFlow.API.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderProduct> WorkOrderProducts { get; set; }
    public DbSet<WorkOrderAttachment> WorkOrderAttachments { get; set; }
    public DbSet<WorkOrderHistory> WorkOrderHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- 1. RELACIONES GLOBALES Y RESTRICCIONES (Restrict) ---
        modelBuilder.Entity<User>().HasOne(u => u.Tenant).WithMany(t => t.Users).HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Customer>().HasOne(c => c.Tenant).WithMany().HasForeignKey(c => c.TenantId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Product>().HasOne(p => p.Tenant).WithMany(t => t.Products).HasForeignKey(p => p.TenantId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkOrder>().HasOne(w => w.Tenant).WithMany(t => t.WorkOrders).HasForeignKey(w => w.TenantId).OnDelete(DeleteBehavior.Restrict);

        // --- 2. RELACIONES DE LA ORDEN DE TRABAJO ---
        modelBuilder.Entity<WorkOrder>().HasOne(w => w.Customer).WithMany(c => c.WorkOrders).HasForeignKey(w => w.CustomerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<WorkOrder>().HasOne(w => w.AssignedTechnician).WithMany().HasForeignKey(w => w.AssignedTechnicianId).OnDelete(DeleteBehavior.Restrict);

        // --- 3. TABLAS HIJAS (Cascade Delete) ---
        // Ítems consumidos
        modelBuilder.Entity<WorkOrderProduct>().HasOne(wi => wi.WorkOrder).WithMany(w => w.Products).HasForeignKey(wi => wi.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkOrderProduct>().HasOne(wi => wi.Product).WithMany(p => p.WorkOrderProducts).HasForeignKey(wi => wi.ProductId).OnDelete(DeleteBehavior.Restrict);

        // Evidencias fotográficas/firmas
        modelBuilder.Entity<WorkOrderAttachment>().HasOne(wa => wa.WorkOrder).WithMany(w => w.Attachments).HasForeignKey(wa => wa.WorkOrderId).OnDelete(DeleteBehavior.Cascade);

        // Historial de estados
        modelBuilder.Entity<WorkOrderHistory>().HasOne(wh => wh.WorkOrder).WithMany(w => w.HistoryLogs).HasForeignKey(wh => wh.WorkOrderId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkOrderHistory>().HasOne(wh => wh.ChangedByUser).WithMany().HasForeignKey(wh => wh.ChangedByUserId).OnDelete(DeleteBehavior.Restrict);

        // --- 4. MAPEO DE TIPOS Y CONCURRENCIA ---
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<WorkOrderProduct>().Property(wi => wi.Quantity).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<WorkOrderProduct>().Property(wi => wi.UnitPriceAtTimeOfUse).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<WorkOrder>().Property(w => w.Status).HasConversion<string>();
        modelBuilder.Entity<WorkOrderAttachment>().Property(wa => wa.Type).HasConversion<string>();
        modelBuilder.Entity<WorkOrder>().Property(w => w.RowVersion).IsRowVersion();

        // --- 5. AISLAMIENTO MULTI-TENANT (El Muro de Seguridad) ---
        modelBuilder.Entity<User>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<WorkOrder>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<WorkOrderProduct>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<WorkOrderAttachment>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<WorkOrderHistory>().HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        // restricciones unicas
        modelBuilder.Entity<Customer>()
            .HasIndex(c => new { c.TenantId, c.Rut })
            .IsUnique();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Revisamos si la entidad implementa IMustHaveTenant
            if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(IMustHaveTenant.TenantId)),
                    Expression.Property(Expression.Constant(_currentTenantService), nameof(ICurrentTenantService.TenantId))
                );
                var filter = Expression.Lambda(body, parameter);

                // Aplicamos el filtro global dinámicamente
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}