using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cafsa.Web.Data.Entities;

namespace Cafsa.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckServiceTypesAsync();
            await CheckRefereesAsync();
            await CheckClientsAsync();
            await CheckServicesAsync();
        }

       

        private async Task CheckServicesAsync()
        {
            var serviceType = _context.ServiceTypes.FirstOrDefault();
            addService(serviceType, 22000M);
            addService(serviceType, 18000M);
            addService(serviceType, 15000M);
            await _context.SaveChangesAsync();

        }

        private void addService(
            ServiceType serviceType,
            decimal price)
        {
            _context.Services.Add(new Service
            {
                ServiceType = serviceType,
                Price = price
            });
        }

        private async Task CheckClientsAsync()
        {
            if (!_context.Clients.Any())
            {
                AddClient("100412543", "Daniel", "Restrepo", "Avenida Guayabal", "314 456 282 49", "Comfenalco");
                AddClient("100412534", "Rata", "Lopez", "Estacion Metro Estadio", "314 999 282 49", "Liga Antioqueña de Futsal");
                AddClient("5404125432", "Jorge", "Franco", "Francisco Antonio Zea", "314 456 555 49", "Franzea");
                AddClient("5499125432", "Rodolfo", "Zapata", "Parque La Milagrosa", "314 444 282 49", "Torneo La Mila");
                await _context.SaveChangesAsync();
            }
        }

        private void AddClient(
            string document,
            string firstName,
            string lastName,
            string Address,
            string phone,
            string clientName)
        {
            _context.Clients.Add(new Entities.Client
            {
                Document = document,
                FirstName = firstName,
                LastName = lastName,
                Address = Address,
                Phone = phone,
                ClientName = clientName            
            });
        }

        private async Task CheckRefereesAsync()
        {
            if (!_context.Referees.Any())
            {
                AddReferee("71219843", "Edison", "Munera", "Calle 57 # 36-58 Boston", "314 836 28 49", "First Category");
                AddReferee("43155023", "Aida", "Buitrago", "Calle 57 # 36-60 Boston", "314 718 79 53", "Second Category");
                AddReferee("1001004384", "Alejandro", "Gomez", "Carrera 33 # 111-553 Sto Mgo", "314 213 56 47", "Third Category");
                AddReferee("1002003415", "Daniela", "Suarez", "Calle 57  Calle 25 Buenos Aires", "312 856 87 99", "Candidate");
                await _context.SaveChangesAsync();
            }
        }

        private void AddReferee(
            string document,
            string firstName,
            string lastName,
            string Address,
            string phone,
            string category)
        {
            _context.Referees.Add(new Entities.Referee
            {
                Document = document,
                FirstName = firstName,
                LastName = lastName,
                Address = Address,
                Phone = phone,
                Category = category
            });
        }

        private async Task CheckServiceTypesAsync()
        {
           if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.Add(new Entities.ServiceType { Name = "Referee_1" });
                _context.ServiceTypes.Add(new Entities.ServiceType { Name = "Referee_2" });
                _context.ServiceTypes.Add(new Entities.ServiceType { Name = "Anotador" });
                _context.ServiceTypes.Add(new Entities.ServiceType { Name = "Instructor" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
