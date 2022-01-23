namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class InvoiceItem : EntityBase
    {
        public Invoice Invoice { get; set; }

        public int Amount { get; set; }

        public Article Article { get; set; }

        public Guid ArticleId { get; set; }

        public double ArticlePrice { get; set; }
    }
}
