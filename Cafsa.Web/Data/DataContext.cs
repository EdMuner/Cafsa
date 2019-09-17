﻿using Microsoft.EntityFrameworkCore;
using Cafsa.Web.Data.Entities;

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
        //Lo pluralizo de como quiero que se llame la coleccion
        //va a cojer el modelo y lo va a convertir en una tabla 
        public DbSet<Referee> Referees { get; set; }


    }
}
