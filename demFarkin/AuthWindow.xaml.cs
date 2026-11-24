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
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        CatalogEntities db = new CatalogEntities();
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var login = loginTb.Text.Trim();
            var password = pb.Password;

            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            Session.Role = user.Roles.Name;

            if (user.Roles.Name == "Авторизованный клиент")
            {
                var window = new ClientWindow();
                window.Show();
                Close();
            }
            else if (user.Roles.Name == "Менеджер")
            {
                var window = new ManagerWindow();
                window.Show();
                Close();
            }
            else if (user.Roles.Name == "Администратор")
            {
                var window = new AdminWindow();
                window.Show();
                Close();
            }
        }
    }
}
