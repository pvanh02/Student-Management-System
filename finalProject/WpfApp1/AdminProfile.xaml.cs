using Finally.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Finally
{
    /// <summary>
    /// Interaction logic for AdminProfile.xaml
    /// </summary>
    public partial class AdminProfile : Window
    {
        FinallyContext final = new FinallyContext();

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^0[0-9]{9,10}$";

            Regex regex = new Regex(pattern);

            if (regex.IsMatch(phoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int ID;

        public AdminProfile(int Id)
        {
            InitializeComponent();
            ID = Id;
            load();
        }

        public void load()
        {
            Admin admin = final.Admins.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (admin != null)
            {
                DateTime.TryParse(admin.DateOfBirth.ToString(), out DateTime date);
                txtFullName.Text = admin.FullName;
                dpDateOfBirth.SelectedDate = date;
                rbMale.IsChecked = true;
                rbFemale.IsChecked = admin.Gender == 0;
                txtPhoneNumber.Text = admin.PhoneNumber;
                txtAddress.Text = admin.Address;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ManageTeacher teacher = new ManageTeacher(ID);
            teacher.Show();
            this.Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = final.Admins.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (admin != null)
            {
                if (string.IsNullOrEmpty(txtFullName.Text))
                {
                    MessageBox.Show("Name is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (dpDateOfBirth.SelectedDate == null)
                {
                    MessageBox.Show("Date is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (rbFemale.IsChecked == false && rbMale.IsChecked == false)
                {
                    MessageBox.Show("Gender is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Address is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrEmpty(txtPhoneNumber.Text) || !IsValidPhoneNumber(txtPhoneNumber.Text))
                {
                    MessageBox.Show("PhoneNumber is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DateOnly.TryParse(dpDateOfBirth.Text, out DateOnly date);
                admin.FullName = txtFullName.Text;
                admin.DateOfBirth = date;
                admin.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : -1);
                admin.PhoneNumber = txtPhoneNumber.Text;
                admin.Address = txtAddress.Text;
                final.Admins.Update(admin);
                if (final.SaveChanges() > 0)
                {
                    MessageBox.Show("Update successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    load();
                }
                else
                {
                    MessageBox.Show("Product not found!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
