using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public static ListaPizze pizze = null;
        public IActionResult Index()
        {
            if (pizze == null)
            {
                pizze = new ListaPizze();
                
                Pizza pizzaOne = new Pizza(0, "Pizza Marinara", "Ingredienti: Pomodoro, Origano, Aglio, Basilico", 4.50, "img/Marinara.jpg");
                Pizza pizzaTwo = new Pizza(1,"Pizza Margherita", "Ingredienti: Pomodoro, Mozzarella,Basilico", 5.00, "img/Margherita.jpg");
                Pizza pizzaThree = new Pizza(2, "Pizza Cotto&Funghi", "Ingredienti: Pomodoro, Mozzarella, Prosc. Cotto, Funghi Porcini", 6.00, "img/Cotto&Funghi.jpg");
                Pizza pizzaFour = new Pizza(3, "Pizza Quattro Formaggi", "Ingredienti: Pomodoro, Mozzarella, Fontina, Gorgonzola, Parmigiano Reggiano", 6.50, "img/QuattroFormaggi.jpg");
                Pizza pizzaFive = new Pizza(4, "Pizza Salame", "Ingredienti: Pomodoro, Mozzarella, Salame Milano", 5.50, "img/Salame.jpg");

                pizze.lsPizze.Add(pizzaOne);
                pizze.lsPizze.Add(pizzaTwo);
                pizze.lsPizze.Add(pizzaThree);
                pizze.lsPizze.Add(pizzaFour);
                pizze.lsPizze.Add(pizzaFive);
            }

            return View(pizze);
        }

        public IActionResult ShowPizza(int id)
        {
            return View("ShowPizza", pizze.lsPizze[id]);
        }

        public IActionResult FormCreazionePizza()
        {
            Pizza newPizza = new Pizza()
            {
                Id = 0,
                Nome = "",
                Descrizione = "",
                sFoto = "",
                Prezzo = 0
            };
            return View(newPizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreaCardPizza(Pizza DatiPizza)
        {
            if (!ModelState.IsValid)
                return View("FormCreazionePizza", DatiPizza);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(DatiPizza.Foto.FileName);
            string fileName = DatiPizza.Nome + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                DatiPizza.Foto.CopyTo(stream);
            }

            Pizza nuovaPizza = new Pizza()
            {
                Id = DatiPizza.Id,
                Nome = DatiPizza.Nome,
                Descrizione = DatiPizza.Descrizione,
                sFoto = "/img/" + fileName,
                Prezzo = DatiPizza.Prezzo
            };
            pizze.lsPizze.Add(nuovaPizza);
            
            return View(nuovaPizza);
        }

        public IActionResult Edit(int id)
        {
            return View("EditPizza", pizze.lsPizze[id]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificaPizza(Pizza DatiPizza)
        {
            if (!ModelState.IsValid)
                return View("FormCreazionePizza", DatiPizza);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\File");
           
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(DatiPizza.Foto.FileName);
            string fileName = DatiPizza.Nome + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                DatiPizza.Foto.CopyTo(stream);
            }


            Pizza modificaPizza = pizze.lsPizze.Find(x => x.Id == DatiPizza.Id);

            modificaPizza.Nome = DatiPizza.Nome;
            modificaPizza.Descrizione = DatiPizza.Descrizione;
            if (modificaPizza.Foto != DatiPizza.Foto)
            {
                modificaPizza.Foto = DatiPizza.Foto;
                modificaPizza.sFoto = "/img/" + fileName;
            }
            else
            {
                modificaPizza.Foto = DatiPizza.Foto;
                modificaPizza.sFoto = DatiPizza.sFoto;
            }

            modificaPizza.Prezzo = DatiPizza.Prezzo;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Pizza rimuoviPizza = pizze.lsPizze.Find(x => x.Id == id);
            pizze.lsPizze.Remove(rimuoviPizza);
            return RedirectToAction("Index");
        }
    }
}
