using Cafsa.Common.Models;
using Cafsa.Prism.Helpers;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    public class CafsaMasterDetailPageViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService;

        public CafsaMasterDetailPageViewModel(
            INavigationService navigationService
            ) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_home",
                    PageName = "ActivitiesPage",
                    Title = Languages.Activities
                },

                new Menu
                {
                    Icon = "ic_list_alt",
                    PageName = "ServicesPage",
                    Title = Languages.Services
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ModifyUserPage",
                    Title = Languages.ModifyUser
                },

                new Menu
                {
                    Icon = "ic_map",
                    PageName = "MapPage",
                    Title = Languages.Map
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = Languages.Logout
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
