using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace demFarkin
{
    public partial class ClientWindow : Window
    {
        CatalogEntities db = new CatalogEntities();

        public class ProductRow
        {
            public string Article { get; set; }
            public string Name { get; set; }
            public string EdinicaIzmereniya { get; set; }
            public double? Price { get; set; }
            public string Postavshik { get; set; }
            public string Proizvoditel { get; set; }
            public string CategoryName { get; set; }
            public double? Discount { get; set; }
            public int? Quantity { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
        }

        public ClientWindow()
        {
            InitializeComponent();
            var categories = db.Categories.ToList();

            productsDg.ItemsSource = db.Products.ToList().Select(p => new ProductRow
            {
                Article = p.Article,
                Name = p.Name,
                EdinicaIzmereniya = p.EdinicaIzmereniya,
                Price = p.Price,
                Postavshik = p.Postavshik,
                Proizvoditel = p.Proizvoditel,
                CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name,
                Discount = p.Discount,
                Quantity = p.Quantity,
                Description = p.Description,
                Image = p.Image
            }).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthWindow();
            window.Show();
            Close();
        }
    }
}