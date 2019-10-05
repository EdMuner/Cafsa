using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        public async Task SeedAsync()
        {
            // Esta instrucción valida que exista DB y si no hay la  crea.
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();

            var manager = await CheckUserAsync("71219843", "Edison", "Munera", "edisonmunera72@gmail.com", "314 836 28 49", "Calle 57 # 36-58", "Primera", "Manager");
            var client = await CheckUserAsync("43155023", "Aida", "Buitrago", "aidabuitrago@hotmail.com", "314 718 79 53", "Calle 57 # 36-49", "Segunda", "Referee");
            var referee = await CheckUserAsync("15502341", "Alejandro", "Zapata", "alejozapata@gmail.com", "333 222 114 11", "Guarne parque", "Oficial", "client");

            await CheckServiceTypesAsync();
            await CheckManagerAsync(manager);
            await CheckRefereeAsync(referee);
            await CheckClientsAsync(client);
            await CheckServicesAsync();
            await CheckContractsAsync();
        }

        private async Task CheckRefereeAsync(User user)
        {
            if (!_context.Referees.Any())
            {
                _context.Referees.Add(new Referee { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckContractsAsync()
        {
            var referee = _context.Referees.FirstOrDefault();
            var client = _context.Clients.FirstOrDefault();
            var service = _context.Services.FirstOrDefault();
            if (!_context.Contracts.Any())
            {
                _context.Contracts.Add(new Contract
                {
                    StartDate = DateTime.Today,
                    Client = client,
                    Referee = referee,
                    Address = "Franzea",
                    Price = 22000M,
                    Service = service,
                    Remarks = "Torneo Empresarial Fin de semana - Llegar 15 minutos antes." +
                   "Debes llevar las planillas e indicadores." +
                   "Juagador sin carnet no puede jugar."
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            String category,
            string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Category = category
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckServicesAsync()
        {
            var Referee = _context.Referees.FirstOrDefault();
            var serviceType = _context.ServiceTypes.FirstOrDefault();
            if (!_context.Services.Any())
            {
                addService("Poblado", "Calle 43 #23 32", 22000M, Referee, serviceType, "Torneo pirata Fin de semana - Llegar 15 minutos antes.");               
                addService("Poblado", "Calle 43 #23 32", 22000M, Referee, serviceType, "Torneo legal Fin de semana - Llegar 15 minutos antes.");
                addService("Poblado", "Calle 43 #23 32", 22000M, Referee, serviceType, "Torneo alcaldia Fin de semana - Llegar 15 minutos antes.");
                await _context.SaveChangesAsync();

            }
        }

        private void addService(

            string neighborhood,
            string address,
            decimal price,          
            Referee referee,
            ServiceType serviceType,
            string remarks)
        {
            _context.Services.Add(new Service
            {
                StartDate = DateTime.Today,
                Neighborhood = neighborhood,
                Address = address,
                Price = price,
                Referee = referee,
                ServiceType = serviceType,
                Remarks = remarks
            });
        }

        private async Task CheckClientsAsync(User user)
        {
            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client { User = user });
                await _context.SaveChangesAsync();
            }
        }


        private async Task CheckServiceTypesAsync()
        {
            if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.Add(new ServiceType { Name = "One Referee" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Two Referee" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Anotador" });
                _context.ServiceTypes.Add(new ServiceType { Name = "TimeKeeper" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Instructor" });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Referee");
            await _userHelper.CheckRoleAsync("Client");
        }


    }
}
