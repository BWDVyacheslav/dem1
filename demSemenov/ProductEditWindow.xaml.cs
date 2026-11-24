using demSemenov;
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

namespace demSemenov
{
    public partial class ProductEditWindow : Window
    {
        CatalogEntities db;
        private Products _product;
        private bool _isNew;
        private object value;
        private CatalogEntities db1;

        public ProductEditWindow(Products product, CatalogEntities context)
        {
            InitializeComponent();
            db = context;
            categoryCb.ItemsSource = db.Categories.ToList();

            if (product == null)
            {
                _isNew = true;
                _product = new Products();
            }
            else
            {
                _isNew = false;
                _product = product;
                articleTb.Text = product.Article;
                nameTb.Text = product.Name;
                edinicaTb.Text = product.EdinicaIzmereniya;
                priceTb.Text = product.Price.ToString();
                postavshikTb.Text = product.Postavshik;
                proizvoditelTb.Text = product.Proizvoditel;
                categoryCb.SelectedValue = product.CategoryId;
                discountTb.Text = product.Discount.ToString();
                quantityTb.Text = product.Quantity.ToString();
                descriptionTb.Text = product.Description;
            }
        }

        public ProductEditWindow(object value, CatalogEntities db1)
        {
            this.value = value;
            this.db1 = db1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(articleTb.Text))
                errors.Add("Артикул: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(nameTb.Text))
                errors.Add("Имя: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(edinicaTb.Text))
                errors.Add("Единица измерения: поле не может быть пустым");

            double price = 0;
            if (string.IsNullOrWhiteSpace(priceTb.Text))
                errors.Add("Цена: поле не может быть пустым");
            else if (!double.TryParse(priceTb.Text, out price))
                errors.Add("Цена: неверный формат числа (ожидается число, например 1000 или 1000,50)");
            else if (price < 0)
                errors.Add("Цена: значение не может быть отрицательным");

            if (string.IsNullOrWhiteSpace(postavshikTb.Text))
                errors.Add("Поставщик: поле не может быть пустым");

            if (string.IsNullOrWhiteSpace(proizvoditelTb.Text))
                errors.Add("Производитель: поле не может быть пустым");

            if (categoryCb.SelectedValue == null)
                errors.Add("Категория: выберите категорию из списка");

            double discount = 0;
            if (string.IsNullOrWhiteSpace(discountTb.Text))
                errors.Add("Скидка: поле не может быть пустым");
            else if (!double.TryParse(discountTb.Text, out discount))
                errors.Add("Скидка: неверный формат числа (ожидается число, например 5 или 5,5)");
            else if (discount < 0 || discount > 100)
                errors.Add("Скидка: значение должно быть от 0 до 100");

            int quantity = 0;
            if (string.IsNullOrWhiteSpace(quantityTb.Text))
                errors.Add("Количество: поле не может быть пустым");
            else if (!int.TryParse(quantityTb.Text, out quantity))
                errors.Add("Количество: неверный формат числа (ожидается целое число, например 10)");
            else if (quantity < 0)
                errors.Add("Количество: значение не может быть отрицательным");

            if (string.IsNullOrWhiteSpace(descriptionTb.Text))
                errors.Add("Описание: поле не может быть пустым");

            if (errors.Count > 0)
            {
                MessageBox.Show("Исправьте следующие ошибки:\n\n" + string.Join("\n", errors), "Ошибки валидации");
                return;
            }

            try
            {
                _product.Article = articleTb.Text.Trim();
                _product.Name = nameTb.Text.Trim();
                _product.EdinicaIzmereniya = edinicaTb.Text.Trim();
                _product.Price = price;
                _product.Postavshik = postavshikTb.Text.Trim();
                _product.Proizvoditel = proizvoditelTb.Text.Trim();
                _product.CategoryId = (int)categoryCb.SelectedValue;
                _product.Discount = discount;
                _product.Quantity = quantity;
                _product.Description = descriptionTb.Text.Trim();

                if (_isNew)
                {
                    db.Products.Add(_product);
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