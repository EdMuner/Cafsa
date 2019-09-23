using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Cafsa.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        //conectarnos a la base de datos
        //creamos la conexion a la base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        //creo las tablas que van a la base de datos
        //Estas seran propiedades tipo Dbset
        //Lo pluralizo de como quiero que se llame la coleccion
        //va a cojer el modelo y lo va a convertir en una tabla 
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<RefereeImage> RefereeImages { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceImage> ServiceImages { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }




    }
}
