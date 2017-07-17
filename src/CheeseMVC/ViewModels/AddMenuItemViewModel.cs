using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int cheeseID { get; set; }
        public int menuID { get; set; }

        public Menu Menu { get; set; }
        public List<SelectListItem> Cheeses { get; set; }

        public AddMenuItemViewModel() { }

        public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> chez)
        {
            this.Menu = menu;

            this.Cheeses = new List<SelectListItem>();

            foreach (Cheese cheez in chez)
            {

                this.Cheeses.Add(new SelectListItem
                {
                    Value = cheez.ID.ToString(),
                    Text = cheez.Name
                });

            }
        }
    }

    


}
