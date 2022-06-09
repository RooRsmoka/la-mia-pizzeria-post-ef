using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("Pizza")]
    public class NuovaPizza
    {
        [Key]
        [Required(ErrorMessage = "Il campo Id è obbligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Il Nome inserito non è valido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio")]
        [StringLength(200, MinimumLength = 15, ErrorMessage = "La descrizione inserita non è valida")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio")]
        [Range(1, 25, ErrorMessage = "Il prezzo inserito non è valido")]
        public double Prezzo { get; set; }

        public string? sFoto { get; set; }

        [NotMapped()]
        public IFormFile Foto { get; set; }
        
        //public byte[]? Data { get; set; }

        public NuovaPizza()
        {

        }

        /*
        public NuovaPizza(string Nome, string Descrizione, double Prezzo, string Foto)
        {
            this.Nome = Nome;
            this.Descrizione = Descrizione;
            this.Prezzo = Prezzo;
            this.sFoto = Foto;
        }
        */
    }
}
