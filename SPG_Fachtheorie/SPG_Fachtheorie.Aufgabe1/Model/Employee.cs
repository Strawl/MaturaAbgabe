namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Employee : EntityBase
    {
        public string Name { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
