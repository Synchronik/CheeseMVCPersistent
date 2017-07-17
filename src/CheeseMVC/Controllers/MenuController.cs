using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {

        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.menlist = context.Menus.ToList();

            return View();
        }


        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu { Name = addMenuViewModel.Name };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);


            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenu = new ViewMenuViewModel { Menu = menu, Items = items };
            return View(viewMenu);
        }
        
        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);
            List<Cheese> chezlist = context.Cheeses.ToList();

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, chezlist);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();

                if (!existingItems.Any())
                {
                    CheeseMenu cheeseMenu = new CheeseMenu
                        {
                            CheeseID = addMenuItemViewModel.cheeseID,
                            MenuID = addMenuItemViewModel.menuID
                        };
                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();

                }

            }

            return View(addMenuItemViewModel);

        }

    }
}
