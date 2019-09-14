using Cafsa.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafsa.Web.Data
{
    public class DataContext : DbContext
    {
        //conectarnos a la base de datos
        //creamos la conexion a la base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        //creo las tablas que van a la base de datos
        //Estas seran propiedades tipo Dbset
        //Lo prulalizo de como quiero que se llmame la coleccion
        //va a cojer el modelo y lo va a convertir en una tabla 
        public DbSet<Referee> Referees { get; set; }


    }
}
