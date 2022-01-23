using Xunit;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;

namespace SPG_Fachtheorie.Aufgabe1.Test;

/// <summary>
/// Unittests für den DBContext.
/// Die Datenbank wird im Ordner SPG_Fachtheorie\SPG_Fachtheorie.Aufgabe1.Test\bin\Debug\net6.0\Invoice.db
/// erzeugt und kann mit SQLite Management Studio oder DBeaver betrachtet werden
/// </summary>
public class InvoiceContextTests
{
    /// <summary>
    /// Prüft, ob die Datenbank mit dem Model im InvoiceContext angelegt werden kann.
    /// </summary>
    [Fact]
    public void CreateDatabaseTest()
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=Invoice.db")
            .Options;

        using var db = new InvoiceContext(options);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();



        Company comp = new Company()
        {
            Name = "BOC Group",
            Address = "Mozartgasse",
            Email = "lalalal@blabal.com",
            PhoneNumber = "0660121342"
        };
        db.Companys.Add(comp);

        Employee employee = new Employee()
        {
            Name = "Lolliipopopo"
        };
        db.Employees.Add(employee);

        Customer cust = new Customer()
        {
            Name = "MAx Musterschüler",
            Salutation = Salutation.Mr,
            Adress = "Wolf of Wallstreet"
        };
        db.Customers.Add(cust);

        Article article = new Article()
        {
            Name = "Pinata Horse",
            Price = 5005.500
        };
        Article article1 = new Article()
        {
            Name = "Rolls Roys",
            Price = 999.999999
        };
        Article article2 = new Article()
        {
            Name = "Pinata Horse",
            Price = 5005.500
        };
        db.Articles.Add(article);
        db.Articles.Add(article1);
        db.Articles.Add(article2);

        Invoice invoice = new Invoice()
        {
            Employee = employee,
            Customer = cust,
            Date = System.DateTime.Now,
            Discount = 35,
            Number = 1

        };
        db.Invoices.Add(invoice);

        InvoiceItem item1 = new InvoiceItem()
        {
            Article = article,
            Invoice = invoice,
            Amount = 3,
            ArticlePrice = article.Price
        };
        InvoiceItem item2 = new InvoiceItem()
        {
            Article = article1,
            Invoice = invoice,
            Amount = 2,
            ArticlePrice = article1.Price
        };
        InvoiceItem item3 = new InvoiceItem()
        {
            Article = article2,
            Invoice = invoice,
            Amount = 5,
            ArticlePrice = article2.Price
        };
        db.InvoicesItems.AddRange(item1, item2, item3);

        db.SaveChanges();
    }
}