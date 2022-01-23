using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{

    public enum Salutation { Mr, Mrs}
    public class Customer : EntityBase
    {

        public string Name { get; set; }

        public string Adress { get; set; }

        public List<Invoice> Invoices { get; set; }

        public Salutation Salutation { get; set; }


    }
}
