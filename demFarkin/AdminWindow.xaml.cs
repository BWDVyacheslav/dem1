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
    public partial class AdminWindow : Window
    {
        CatalogEntities db = new CatalogEntities();

        public class ProductRow
        {
            public Products Product { get; set; }
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

        public AdminWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private List<ProductRow> BuildRows(IEnumerable<Products> source)
        {
            var categories = db.Categories.ToList();

            return source.Select(p => new ProductRow
            {
                Product = p,
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

        private void RefreshData()
        {
            productsDg.ItemsSource = null;
            productsDg.ItemsSource = BuildRows(db.Products.ToList());
        }

        private void filterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = filterCb.SelectedItem as ComboBoxItem;

            if (value == null)
            {
                return;
            }

            var filter = value.Content.ToString();

            if (filter == "Все")
            {
                productsDg.ItemsSource = null;
                productsDg.ItemsSource = BuildRows(db.Products.ToList());
                return;
            }

            Categories category = db.Categories.FirstOrDefault(c => c.Name == filter);

            if (category == null)
            {
                return;
            }

            productsDg.ItemsSource = null;
            productsDg.ItemsSource = BuildRows(db.Products.Where(p => p.CategoryId == category.Id).ToList());
        }

        private void sortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = sortCb.SelectedItem as ComboBoxItem;

            if (value == null)
            {
                return;
            }

            var sort = value.Content.ToString();

            if (sort == "Все")
            {
                productsDg.ItemsSource = null;
                productsDg.ItemsSource = BuildRows(db.Products.ToList());
                return;
            }

            productsDg.ItemsSource = null;

            if (sort == "Цена воз")
            {
                productsDg.ItemsSource = BuildRows(db.Products.OrderBy(p => p.Price).ToList());
            }
            else if (sort == "Цена уб")
            {
                productsDg.ItemsSource = BuildRows(db.Products.OrderByDescending(p => p.Price).ToList());
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = searchTb.Text.Trim().ToLower();

            productsDg.ItemsSource = null;
            productsDg.ItemsSource = BuildRows(db.Products.Where(p => p.Name.ToLower().Contains(text)).ToList());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthWindow();
            window.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new OrdersWindow();
            window.Show();
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductEditWindow(null, db);
            window.ShowDialog();
            RefreshData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var row = productsDg.SelectedItem as ProductRow;

            if (row == null)
            {
                MessageBox.Show("Выберите товар для редактирования");
                return;
            }

            var window = new ProductEditWindow(row.Product, db);
            window.ShowDialog();
            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var row = productsDg.SelectedItem as ProductRow;

            if (row == null)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить товар?", "Подтверждение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                db.Products.Remove(row.Product);
                db.SaveChanges();
                RefreshData();
            }
        }
    }
}