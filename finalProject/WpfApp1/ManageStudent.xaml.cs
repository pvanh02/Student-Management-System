using Finally.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Xps;

namespace Finally
{
    /// <summary>
    /// Interaction logic for ManageStudent.xaml
    /// </summary>
    public partial class ManageStudent : Window
    {
        FinallyContext final = new FinallyContext();

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

        public int Id { get; set; }

        private int ID;
        public ManageStudent(int Id)
        {
            InitializeComponent();
            load(Id);
            ID = Id;
        }

        public void load(int Id)
        {
            var combo = final.Students.Select(x => x.Address).Distinct().ToList();
            combo.Insert(0, "All");

            AddressComboBox.ItemsSource = combo;
            StudentDataGrid.ItemsSource = final.Students.Include(t => t.Class).Where(t => t.ClassId == Id).ToList();
        }

        private bool isAscending = false;

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {

            if (isAscending)
            {

                load(ID);

                SortButton.Content = "SortDesc";
            }
            else
            {
                StudentDataGrid.ItemsSource = final.Students
                                          .Where(x => x.ClassId == ID)
                                          .OrderByDescending(x => x.Id)
                                          .ToList();

                SortButton.Content = "SortAsc";
            }

            isAscending = !isAscending;
        }

        private void StudentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selected)
            {
                DateTime.TryParse(selected.DateOfBirth.ToString(), out DateTime date);
                txtID.Text = selected.Id.ToString();
                txtName.Text = selected.FullName;
                dpDOB.SelectedDate = date;
                rbMale.IsChecked = true;
                rbFemale.IsChecked = selected.Gender == 0;
                txtPhone.Text = selected.PhoneNumber;
                txtAddress.Text = selected.Address;
                txtClassName.Text = selected.Class.Name;
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
            txtClassName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageClass manageClass = new ManageClass();
            manageClass.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDataGrid.ItemsSource = final.Students.Where(t => t.FullName.Contains(txtSearch.Text) && t.ClassId == ID).ToList();
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
                    StudentDataGrid.ItemsSource = final.Students.Where(x => x.Address == selectedAddress && x.ClassId == ID).ToList();
                }

            }

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            ManageGrades grades = new ManageGrades(ID);
            grades.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student();
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
            if (string.IsNullOrEmpty(txtClassName.Text))
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
            var check1 = final.Students.Select(t => t.Username).ToList();
            foreach (var item in check1)
            {
                if (item.Equals(txtUsername.Text))
                {
                    MessageBox.Show("Username is exist!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            var check = final.Students.Select(t => t.Email).ToList();
            foreach (var item in check)
            {
                if (item.Equals(txtEmail.Text))
                {
                    MessageBox.Show("Email is exist!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            student.FullName = txtName.Text;
            DateOnly.TryParse(dpDOB.Text, out DateOnly date);
            student.DateOfBirth = date;
            student.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : 2);
            student.PhoneNumber = txtPhone.Text;
            student.Address = txtAddress.Text;
            student.ClassId = ID;
            student.Username = txtUsername.Text;
            student.Password = txtPassword.Text;
            student.Email = txtEmail.Text;
            student.RoleId = 2;
            final.Students.Add(student);
            final.SaveChanges();

            Student student1 = final.Students.FirstOrDefault(t => t.Username == txtUsername.Text);
            MessageBox.Show(student1.Id.ToString());
            if( ID == 1)
            {   
                Grade grade = new Grade();
                DateOnly.TryParse("2024-07-20", out DateOnly date1);
                for (int i = 1; i <= 10; i++)
                {
                    if (i != 5)
                    {
                        grade.StudentId = student1.Id;
                        grade.ClassId = ID;
                        grade.TeacherId = i;
                        grade.Grade1 = 0;
                        grade.DayOfGrade = date1;
                        final.Grades.Add(grade);
                        final.SaveChanges() ;
                        grade = new Grade();
                    }
                }
            }
            else if( ID == 2 ) {
                Grade grade = new Grade();
                DateOnly.TryParse("2024-07-20", out DateOnly date1);
                for (int i = 1; i <= 10; i++)
                {
                    if (i != 10)
                    {
                        grade.StudentId = int.Parse(txtID.Text);
                        grade.ClassId = ID;
                        grade.TeacherId = i;
                        grade.Grade1 = 0;
                        grade.DayOfGrade = date1;
                        final.Grades.Add(grade);
                        final.SaveChanges();
                        grade = new Grade();
                    }
                }
            }

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
                MessageBox.Show("PhoneNumber is invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Address is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtClassName.Text))
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
            Student student = final.Students.FirstOrDefault(t => t.Id == int.Parse(txtID.Text));
            student.FullName = txtName.Text;
            DateOnly.TryParse(dpDOB.Text, out DateOnly date);
            student.DateOfBirth = date;
            student.Gender = rbMale.IsChecked == true ? 1 : (rbFemale.IsChecked == true ? 0 : 2);
            student.PhoneNumber = txtPhone.Text;
            student.Address = txtAddress.Text;
            student.ClassId = ID;
            student.Username = txtUsername.Text;
            student.Password = txtPassword.Text;
            student.Email = txtEmail.Text;
            student.RoleId = 2;
            final.Students.Update(student);
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
            Student student = final.Students.Include(t => t.Grades).FirstOrDefault(x => x.Id == int.Parse(txtID.Text));
            if (student != null)
            {
                if (student.Grades.Count > 0)
                {
                    final.Grades.RemoveRange(student.Grades);
                }
                final.Students.Remove(student);
                var check = final.Grades.Where(t => t.StudentId == int.Parse(txtID.Text)).ToList();
                final.Grades.RemoveRange(check);
                
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

        private void ViewSchedules_Click(object sender, RoutedEventArgs e)
        {
            ManageSchedules manageSchedules = new ManageSchedules(ID);
            manageSchedules.Show();
            this.Close();
        }
    }
}
