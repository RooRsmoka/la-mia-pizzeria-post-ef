using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public static PizzaContext db = new PizzaContext();
        public static ListaPizze pizze = null;
        public IActionResult Index()
        {
            /*
            if (pizze == null)
            {
                pizze = new ListaPizze();
                
                Pizza pizzaOne = new Pizza("Pizza Marinara", "Ingredienti: Pomodoro, Origano, Aglio, Basilico", 4.50, "img/Marinara.jpg");
                Pizza pizzaTwo = new Pizza("Pizza Margherita", "Ingredienti: Pomodoro, Mozzarella,Basilico", 5.00, "img/Margherita.jpg");
                Pizza pizzaThree = new Pizza("Pizza Cotto&Funghi", "Ingredienti: Pomodoro, Mozzarella, Prosc. Cotto, Funghi Porcini", 6.00, "img/Cotto&Funghi.jpg");
                Pizza pizzaFour = new Pizza("Pizza Quattro Formaggi", "Ingredienti: Pomodoro, Mozzarella, Fontina, Gorgonzola, Parmigiano Reggiano", 6.50, "img/QuattroFormaggi.jpg");
                Pizza pizzaFive = new Pizza("Pizza Salame", "Ingredienti: Pomodoro, Mozzarella, Salame Milano", 5.50, "img/Salame.jpg");

                pizze.lsPizze.Add(pizzaOne);
                pizze.lsPizze.Add(pizzaTwo);
                pizze.lsPizze.Add(pizzaThree);
                pizze.lsPizze.Add(pizzaFour);
                pizze.lsPizze.Add(pizzaFive);

                db.Add(pizzaOne);
                db.Add(pizzaTwo);
                db.Add(pizzaThree);
                db.Add(pizzaFour);
                db.Add(pizzaFive);
                db.SaveChanges();
            }
            */

            return View(db);
        }

        public IActionResult ShowPizza(int id)
        {
            var pizzaId = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
            return View("ShowPizza", pizzaId);
        }

        public IActionResult FormCreazionePizza()
        {
            Pizza newPizza = new Pizza()
            {
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

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");

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

            db.Add(nuovaPizza);
            db.SaveChanges();
            return View(nuovaPizza);
        }

        public IActionResult Edit(int id)
        {
            var pizzaId = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
            return View("EditPizza", pizzaId);
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
            
            var modificaPizza = db.Pizze
                .Where(pizza => pizza.Id == DatiPizza.Id).FirstOrDefault();

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

            db.Pizze.UpdateRange(modificaPizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var pizzaId = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
            db.Pizze.Remove(pizzaId);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
