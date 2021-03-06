﻿using collNotes.Domain.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace collNotes.Services.MenuPage
{
    public class MenuPageService
    {
        public MenuPageService() { }

        private static List<HomeMenuItem> MenuItems { get; set; }

        public List<HomeMenuItem> GetMenuItems()
        {
            if (MenuItems is null)
            {
                // initialize list
                MenuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.About, Title="About" },
                    new HomeMenuItem {Id = MenuItemType.Trips, Title="Trips" },
                    new HomeMenuItem {Id = MenuItemType.Sites, Title="Sites" },
                    new HomeMenuItem {Id = MenuItemType.Specimen, Title="Specimen" },
                    new HomeMenuItem {Id = MenuItemType.Settings, Title="Settings" },
                    new HomeMenuItem {Id = MenuItemType.ExportImport, Title="Export/Import Data" }
                };
            }

            return MenuItems;
        }

        public async Task<bool> UpdateMenuPageAsync(MenuItemType menuItemType)
        {
            bool result = false;

            if (!(MenuItems is null))
            {
                foreach (var item in MenuItems)
                {
                    if (item.Id.Equals(menuItemType))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return await Task.FromResult(result);
        }
    }
}