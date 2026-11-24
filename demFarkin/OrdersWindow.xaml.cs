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
    public partial class OrdersWindow : Window
    {
        CatalogEntities db = new CatalogEntities();

        public OrdersWindow()
        {
            InitializeComponent();

            if (Session.Role == "Администратор")
            {
                ordersDg.IsReadOnly = false;
                addBtn.Visibility = Visibility.Visible;
                editBtn.Visibility = Visibility.Visible;
                deleteBtn.Visibility = Visibility.Visible;
            }
            else
            {
                ordersDg.IsReadOnly = true;
            }

            RefreshData();
        }

        private void RefreshData()
        {
            ordersDg.ItemsSource = null;
            ordersDg.ItemsSource = db.Orders.ToList();
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
                ordersDg.ItemsSource = null;
                ordersDg.ItemsSource = db.Orders.ToList();
                return;
            }

            ordersDg.ItemsSource = null;
            ordersDg.ItemsSource = db.Orders.Where(o => o.OrderStatus == filter).ToList();
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
                ordersDg.ItemsSource = null;
                ordersDg.ItemsSource = db.Orders.ToList();
                return;
            }

            ordersDg.ItemsSource = null;

            if (sort == "Цена доставки воз")
            {
                ordersDg.ItemsSource = db.Orders.OrderBy(o => o.DeliveryDate).ToList();
            }
            else if (sort == "Цена доставки уб")
            {
                ordersDg.ItemsSource = db.Orders.OrderByDescending(o => o.DeliveryDate).ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthWindow();
            window.Show();
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = searchTb.Text.Trim().ToLower();

            ordersDg.ItemsSource = null;
            ordersDg.ItemsSource = db.Orders.Where(o => o.ClientFullName.ToLower().Contains(text)).ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Session.Role == "Администратор")
            {
                var window = new AdminWindow();
                window.Show();
            }
            else
            {
                var window = new ManagerWindow();
                window.Show();
            }

            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderEditWindow(null, db);
            window.ShowDialog();
            RefreshData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var order = ordersDg.SelectedItem as Orders;

            if (order == null)
            {
                MessageBox.Show("Выберите заказ для редактирования");
                return;
            }

            var window = new OrderEditWindow(order, db);
            window.ShowDialog();
            RefreshData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var order = ordersDg.SelectedItem as Orders;

            if (order == null)
            {
                MessageBox.Show("Выберите заказ для удаления");
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить заказ?", "Подтверждение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
                RefreshData();
            }
        }
    }
}