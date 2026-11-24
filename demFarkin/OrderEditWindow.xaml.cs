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
    public partial class OrderEditWindow : Window
    {
        CatalogEntities db;
        private Orders _order;
        private bool _isNew;

        public OrderEditWindow(Orders order, CatalogEntities context)
        {
            InitializeComponent();
            db = context;

            if (order == null)
            {
                _isNew = true;
                _order = new Orders();
                statusCb.SelectedIndex = 0;

                int maxNumber = 0;
                if (db.Orders.Any())
                {
                    maxNumber = db.Orders.Max(o => o.OrderNumber);
                }

                _order.OrderNumber = maxNumber + 1;
                orderNumberTb.Text = _order.OrderNumber.ToString();
            }
            else
            {
                _isNew = false;
                _order = order;
                orderNumberTb.Text = order.OrderNumber.ToString();
                productsTb.Text = order.Products;
                orderDateDp.SelectedDate = DateTime.Parse(order.OrderDate);
                deliveryDateDp.SelectedDate = order.DeliveryDate;
                streetPvzTb.Text = order.StreetPVZ.ToString();
                clientFullNameTb.Text = order.ClientFullName;
                codeTb.Text = order.Code.ToString();

                foreach (ComboBoxItem item in statusCb.Items)
                {
                    if (item.Content.ToString() == order.OrderStatus)
                    {
                        statusCb.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(orderNumberTb.Text))
                errors.Add("Номер заказа: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(productsTb.Text))
                errors.Add("Товары: поле не может быть пустым");

            if (orderDateDp.SelectedDate == null)
                errors.Add("Дата заказа: выберите дату");

            if (deliveryDateDp.SelectedDate == null)
                errors.Add("Дата доставки: выберите дату");

            if (string.IsNullOrWhiteSpace(streetPvzTb.Text))
                errors.Add("Пункт выдачи: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(clientFullNameTb.Text))
                errors.Add("ФИО клиента: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(codeTb.Text))
                errors.Add("Код получения: поле не может быть пустым");

            if (statusCb.SelectedItem == null)
                errors.Add("Статус заказа: выберите статус");

            if (errors.Count > 0)
            {
                MessageBox.Show("Исправьте следующие ошибки:\n\n" + string.Join("\n", errors), "Ошибки валидации");
                return;
            }

            try
            {
                _order.OrderNumber = int.Parse(orderNumberTb.Text.Trim());
                _order.Products = productsTb.Text.Trim();
                _order.OrderDate = orderDateDp.SelectedDate.Value.ToString();
                _order.DeliveryDate = deliveryDateDp.SelectedDate.Value;
                _order.StreetPVZ = byte.Parse(streetPvzTb.Text.Trim());
                _order.ClientFullName = clientFullNameTb.Text.Trim();
                _order.Code = int.Parse(codeTb.Text.Trim());
                _order.OrderStatus = ((ComboBoxItem)statusCb.SelectedItem).Content.ToString();

                if (_isNew)
                {
                    db.Orders.Add(_order);
                }

                db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении в базу данных:\n" + ex.Message, "Ошибка");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}