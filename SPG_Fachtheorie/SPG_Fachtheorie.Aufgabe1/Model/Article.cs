namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Article : EntityBase
    {

        public double Price { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }


    }
}
