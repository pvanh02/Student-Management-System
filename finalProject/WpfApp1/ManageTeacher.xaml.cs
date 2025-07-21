using Finally.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finally
{
    /// <summary>
    /// Interaction logic for ManageTeacher.xaml
    /// </summary>
    public partial class ManageTeacher : Window
    {
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            Regex regex = new Regex(pattern);

            if (regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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


        FinallyContext final = new FinallyContext();

        public int Id { get; set; }

        private int ID;
        public ManageTeacher(int Id)
        {
            InitializeComponent();
            load(Id);
            ID = Id;
        }

        public void load(int Id)
        {
            if (GetAccountID.Role == 3)
            {
                ViewForAdmin.Visibility = Visibility.Visible;
                ViewForTeacher.Visibility = Visibility.Collapsed;
                ViewForTeacher1.Visibility = Visibility.Collapsed;
                ButtonForAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                ViewForAdmin.Visibility = Visibility.Collapsed;
                ViewForTeacher1.Visibility = Visibility.Visible;
                ViewForTeacher.Visibility = Visibility.Visible;
                ButtonForAdmin.Visibility = Visibility.Collapsed;
            }

            var combo = final.Teachers.Select(x => x.Address).Distinct().ToList();
            combo.Insert(0, "All");

            AddressComboBox.ItemsSource = combo;

            TeacherDataGrid.ItemsSource = final.Teachers.Include(t => t.Department).Where(t => t.DepartmentId == Id).ToList();
        }

        //private bool isAscending = false;

        //private void SortButton_Click(object sender, RoutedEventArgs e)
        //{

        //    if (isAscending)
        //    {

        //        load(ID);

        //        SortButton.Content = "SortDesc";
        //    }
        //    else
        //    {
        //        TeacherDataGrid.ItemsSource = final.Teachers
        //                                  .Where(x => x.DepartmentId == ID)
        //                                  .OrderByDescending(x => x.Id)
        //                                  .ToList();

        //        SortButton.Content = "SortAsc";
        //    }

        //    isAscending = !isAscending;
        //}

        private void TeacherDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeacherDataGrid.SelectedItem is Teacher selected)
            {
                DateTime.TryParse(selected.DateOfBirth.ToString(), out DateTime date);
                txtID.Text = selected.Id.ToString();
                txtName.Text = selected.FullName;
                dpDOB.SelectedDate = date;
                rbMale.IsChecked = true;
                rbFemale.IsChecked = selected.Gender == 0;
                txtPhone.Text = selected.PhoneNumber;
                txtAddress.Text = selected.Address;
                txtSubject.Text = selected.Subject;
                txtDepartmentName.Text = selected.Department.Name;
                txtUsername.Text = selected.Username;
                txtPassword.Text = selected.Password;
                txtEmail.Text = selected.Email;
            }
        }

        private void clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            dpDOB.SelectedDate = null;
            rbMale.IsChecked = false;
            rbFemale.IsChecked = false;
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtSubject.Text = "";
            txtDepartmentName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageDepartment manageDepartment = new ManageDepartment();
            manageDepartment.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherDataGrid.ItemsSource = final.Teachers.Where(t => t.FullName.Contains(txtSearch.Text) && t.DepartmentId == ID).ToList();
        }

        private void AddressComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedAddress = AddressComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedAddress))
            {
                if (selectedAddress.Equals("All"))
                {
                    load(ID);
                }
                else
                {
                    TeacherDataGrid.ItemsSource = final.Teachers.Where(x => x.Address == selectedAddress && x.DepartmentId == ID).ToList();
                }

            }

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherProfile teacher = new TeacherProfile(ID);
            teacher.Show();
            this.Close();
        }

       
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Teacher teacher = new Teacher();
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Id is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpDOB.SelectedDate == null)
            {
                MessageBox.Show("Date is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (rbFemale.IsChecked == false && rbMale.IsChecked == false)
            {
                MessageBox.Show("Gender is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtPhone.Text) || !IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("PhoneNumber is Invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Address is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                MessageBox.Show("Subject is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDepartmentName.Text))
            {
                MessageBox.Show("ClassName is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Username is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email must be follow format abc@example.com", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var check1 = final.Teachers.Select(t => t.Username).ToList();
            foreach (var item in check1)
            {
                if (item.Equals(txtUsername.Text))
                {
                    MessageBox.Show("Username is exist!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            var check = final.Teachers.Select(t => t.Email).ToList();
            foreach (var item in check)
            {
                if (item.Equals(txtEmail.Text))
                {
                    MessageBox.Show("Email is exist!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            teacher.FullName = txtName.Text;
            DateOnly.TryParse(dpDOB.Text, out DateOnly date);
            teacher.DateOfBirth = date;
            teacher.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : 2);
            teacher.PhoneNumber = txtPhone.Text;
            teacher.Address = txtAddress.Text;
            teacher.Subject = txtSubject.Text;
            teacher.DepartmentId = ID;
            teacher.Username = txtUsername.Text;
            teacher.Password = txtPassword.Text;
            teacher.Email = txtEmail.Text;
            teacher.RoleId = 1;
            final.Teachers.Add(teacher);
            final.SaveChanges();
            MessageBox.Show("Add successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            load(ID);
            clear();

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Id is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpDOB.SelectedDate == null)
            {
                MessageBox.Show("Date is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (rbFemale.IsChecked == false && rbMale.IsChecked == false)
            {
                MessageBox.Show("Gender is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtPhone.Text) || !IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("PhoneNumber is Invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Address is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                MessageBox.Show("Subject is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDepartmentName.Text))
            {
                MessageBox.Show("ClassName is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Username is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email must be follow format abc@example.com", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Teacher teacher = final.Teachers.FirstOrDefault(t => t.Id == int.Parse(txtID.Text));
            teacher.FullName = txtName.Text;
            DateOnly.TryParse(dpDOB.Text, out DateOnly date);
            teacher.DateOfBirth = date;
            teacher.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : 2);
            teacher.PhoneNumber = txtPhone.Text;
            teacher.Address = txtAddress.Text;
            teacher.Subject = txtSubject.Text;
            teacher.DepartmentId = ID;
            teacher.Username = txtUsername.Text;
            teacher.Password = txtPassword.Text;
            teacher.Email = txtEmail.Text;
            teacher.RoleId = 2;
            final.Teachers.Update(teacher);
            if (final.SaveChanges() > 0)
            {
                MessageBox.Show("Update successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                load(ID);
                clear();
            }
            else
            {
                MessageBox.Show("Product not found!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Id Invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Teacher teacher = final.Teachers.Include(T => T.Grades).FirstOrDefault(x => x.Id == int.Parse(txtID.Text));
            if (teacher != null)
            {
                if (teacher.Grades.Count > 0)
                {
                    final.Grades.RemoveRange(teacher.Grades);
                }
                final.Teachers.Remove(teacher);
                if (final.SaveChanges() > 0)
                {
                    MessageBox.Show("Delete successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    load(ID);
                    clear();
                }
                else
                {
                    MessageBox.Show("Delete failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void ViewButton1_Click(object sender, RoutedEventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile(ID);
            adminProfile.Show();
            this.Close();
        }

        private void ViewSchedules_Click(object sender, RoutedEventArgs e)
        {
            ViewTeacherSchedules viewTeacherSchedules = new ViewTeacherSchedules(ID);
            viewTeacherSchedules.Show();
            this.Close();
        }
    }
}
