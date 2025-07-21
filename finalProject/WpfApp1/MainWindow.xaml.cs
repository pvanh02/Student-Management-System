using Finally.Models;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Finally
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        FinallyContext final = new FinallyContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            var check = final.Students.FirstOrDefault(p => (p.Username == username || p.Email == username) && p.Password == password);

            var check1 = final.Teachers.FirstOrDefault(p => (p.Username == username || p.Email == username) && p.Password == password);

            var check2 = final.Admins.FirstOrDefault(p => (p.Username == username || p.Email == username) && p.Password == password);



            if (check1 != null)
            {
                MessageBox.Show("Login successful!");
                GetAccountID.ID = check1.Id;
                GetAccountID.Role = 1;
                ManageDepartment department = new ManageDepartment();
                department.Show();
                this.Close();
            }
            else if (check2 != null)
            {
                MessageBox.Show("Login successful!");
                GetAccountID.ID = check2.Id;
                GetAccountID.Role = 3;
                ManageDepartment department = new ManageDepartment();
                department.Show();
                this.Close();

            }
            else if (check != null)
            {
                MessageBox.Show("Login successful!");
                GetAccountID.ID = check.Id;
                GetAccountID.Role = 2;
                ViewGrades viewGrades = new ViewGrades();
                viewGrades.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}