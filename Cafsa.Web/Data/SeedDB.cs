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

            var manager = await CheckUserAsync("71219843", "Edison", "Munera", "edisonmunera72@gmail.com", "314 836 28 49", "Calle 57 # 36-58", "Manager");
            var client = await CheckUserAsync("43155023", "Comfenalco", "Guayabal", "comfenalco@gmail.com", "314 718 79 53", "Calle 57 # 36-49", "Client");
            var referee = await CheckUserAsync("15502341", "Alejandro", "Zapata", "alejozapata@gmail.com", "333 222 114 11", "Parque Boston Calle 36", "Referee");

            await CheckActivityTypesAsync();
            await CheckManagerAsync(manager);
            await CheckRefereeAsync(referee);
            await CheckClientsAsync(client);
            await CheckActivitiesAsync();
            await CheckServiceAsync();
        }

    
        private async Task CheckServiceAsync()
        {

            var referee = _context.Referees.FirstOrDefault();
            var client = _context.Clients.FirstOrDefault();
            var activity = _context.Activities.FirstOrDefault();

            if (!_context.Services.Any())
            {
                _context.Services.Add(new Service
                {
                    StartDate = DateTime.Today,
                    IsActive = true,
                    Client = client,  
                    Referee = referee,
                    Price = 22000M, 
                    Activity = activity,
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

        private async Task CheckRefereeAsync(User user)
        {
            if (!_context.Referees.Any())
            {
                _context.Referees.Add(new Referee { User = user });
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
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Referee");
            await _userHelper.CheckRoleAsync("Client");
        }

        private async Task CheckClientsAsync(User user)
        {
            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client { User = user });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckActivitiesAsync()
        {
            var referee = _context.Referees.FirstOrDefault();
            var activityType = _context.ActivityTypes.FirstOrDefault();         
            if (!_context.Activities.Any())
            {
                addActivity("Calle 43 #25 22","Poblado", referee, activityType, 22000M,"Torneo Empresarial");
                addActivity("Calle 23 #27 82","Boston", referee, activityType, 12000M, "Torneo Fin de Semana");
                await _context.SaveChangesAsync();

            }
        }

        private void addActivity(
            string address,        
            string neighborhood,
            Referee referee,
            ActivityType activityType,
            decimal price,                                    
            string remarks)
        {
            _context.Activities.Add(new Activity
            {
                Address = address,
                Neighborhood = neighborhood,
                Referee = referee,
                ActivityType = activityType,
                Price = price,           
                Remarks = remarks
            });
        }

        private async Task CheckActivityTypesAsync()
        {
            if (!_context.ActivityTypes.Any())
            {
                _context.ActivityTypes.Add(new ActivityType { Name = "One Referee" });
                _context.ActivityTypes.Add(new ActivityType { Name = "Two Referee" });
                _context.ActivityTypes.Add(new ActivityType { Name = "Anotador" });
                _context.ActivityTypes.Add(new ActivityType { Name = "TimeKeeper" });
                _context.ActivityTypes.Add(new ActivityType { Name = "Instructor" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
