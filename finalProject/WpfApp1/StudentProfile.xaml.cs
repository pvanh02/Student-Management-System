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
    /// Interaction logic for StudentProfile.xaml
    /// </summary>
    public partial class StudentProfile : Window
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

        public StudentProfile()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
            Student student = final.Students.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (student != null)
            {
                DateTime.TryParse(student.DateOfBirth.ToString(), out DateTime date);
                txtFullName.Text = student.FullName;
                dpDateOfBirth.SelectedDate = date;
                rbMale.IsChecked = true;
                rbFemale.IsChecked = student.Gender == 0;
                txtPhoneNumber.Text = student.PhoneNumber;
                txtAddress.Text = student.Address;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ViewGrades viewGrades = new ViewGrades();
            viewGrades.Show();
            this.Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Student student = final.Students.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (student != null)
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
                student.FullName = txtFullName.Text;
                student.DateOfBirth = date;
                student.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : -1);
                student.PhoneNumber = txtPhoneNumber.Text;
                student.Address = txtAddress.Text;
                final.Students.Update(student);
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
