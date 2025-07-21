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
    /// Interaction logic for TeacherProfile.xaml
    /// </summary>
    public partial class TeacherProfile : Window
    {
        FinallyContext final =  new FinallyContext();

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
        private int ID { get; set; }

        public TeacherProfile(int iD)
        {
            InitializeComponent();
            ID = iD;
            load();
        }

        public void load()
        {
            Teacher teacher = final.Teachers.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (teacher != null)
            {
                DateTime.TryParse(teacher.DateOfBirth.ToString(), out DateTime date);
                txtFullName.Text = teacher.FullName;
                dpDateOfBirth.SelectedDate = date;
                rbMale.IsChecked = true;
                rbFemale.IsChecked = teacher.Gender == 0;
                txtPhoneNumber.Text = teacher.PhoneNumber;
                txtAddress.Text = teacher.Address;
                txtSubject.Text = teacher.Subject;
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
            Teacher teacher = final.Teachers.FirstOrDefault(x => x.Id == GetAccountID.ID);
            if (teacher != null)
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
                    MessageBox.Show("Adress is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrEmpty(txtPhoneNumber.Text) || !IsValidPhoneNumber(txtPhoneNumber.Text))
                {
                    MessageBox.Show("PhoneNumber is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrEmpty(txtSubject.Text))
                {
                    MessageBox.Show("Subject is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DateOnly.TryParse(dpDateOfBirth.Text, out DateOnly date);
                teacher.FullName = txtFullName.Text;
                teacher.DateOfBirth = date;
                teacher.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : -1);
                teacher.PhoneNumber = txtPhoneNumber.Text;
                teacher.Address = txtAddress.Text;
                teacher.Subject = txtSubject.Text;
                final.Teachers.Update(teacher);
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
