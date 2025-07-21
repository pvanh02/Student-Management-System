using Finally.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

namespace Finally
{
    

    /// <summary>
    /// Interaction logic for ManageDepartment.xaml
    /// </summary>
    public partial class ManageDepartment : Window
    {
        FinallyContext final = new FinallyContext();

        public ManageDepartment()
        {
            InitializeComponent();
            loadDepartment();
        }

        public void loadDepartment()
        {
            //if (GetAccountID.Role == 3)
            //{
            //    ButtonForAdmin.Visibility = Visibility.Visible;
            //    TxtForAdmin.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ButtonForAdmin.Visibility = Visibility.Collapsed;
            //    TxtForAdmin.Visibility = Visibility.Collapsed;
            //}

            var load = final.Departments.ToList();

            ListDepartment.ItemsSource = load.Select(d => new
            {
                d.Id,
                d.Name,
                Subject = d.Id == 1 ? "Biology,Chemistry,Mathematics,Physics" : (d.Id == 2 ? "English,Civic education,Literature,Geography,History" : "None")
            }).ToList();
        }
        //private void ListDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedDepartment = ListDepartment.SelectedItem as dynamic;
        //    if (selectedDepartment != null)
        //    {
        //        txtId.Text = selectedDepartment.Id;
        //        txtName.Text = selectedDepartment.Name;
        //        txtSubject.Text = selectedDepartment.Subject;
        //    }
        //}

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.CommandParameter is int Id)
            {
                ManageTeacher teacher = new ManageTeacher(Id);
                Department department = final.Departments.FirstOrDefault(d => d.Id == Id);
                teacher.Title = "Teacher of Department: " + department.Name;
                teacher.Show();
                this.Close();
            }
        }

        //private void clear()
        //{
        //    txtId.Text = "";
        //    txtName.Text = "";
        //    txtSubject.Text = "";
        //}
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            ManageClass manageClass = new ManageClass();
            manageClass.Show();
            this.Close();
        }

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Department department = new Department();
        //    if (string.IsNullOrEmpty(txtId.Text))
        //    {
        //        MessageBox.Show("Id is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtName.Text))
        //    {
        //        MessageBox.Show("Name is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtSubject.Text))
        //    {
        //        MessageBox.Show("Subject is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    department.Name = txtName.Text;
        //    final.Departments.Add(department);
        //    final.SaveChanges();
        //    MessageBox.Show("Add successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    loadDepartment();
        //    clear();
        //}

        //private void EditButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtId.Text))
        //    {
        //        MessageBox.Show("TxtId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtName.Text))
        //    {
        //        MessageBox.Show("Name is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtSubject.Text))
        //    {
        //        MessageBox.Show("Subject is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    Department department = final.Departments.FirstOrDefault(t => t.Id == int.Parse(txtId.Text));
        //    department.Name = txtName.Text;
        //    final.Departments.Update(department);
        //    if (final.SaveChanges() > 0)
        //    {
        //        MessageBox.Show("Update successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //        loadDepartment();
        //        clear();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Product not found!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void DeleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtId.Text))
        //    {
        //        MessageBox.Show("TxtId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    try
        //    {
        //        Department department = final.Departments.FirstOrDefault(t => t.Id == int.Parse(txtId.Text));
        //        var check = final.Teachers.Select(t => t.DepartmentId == int.Parse(txtId.Text)).ToList();
        //        if (department != null)
        //        {
        //            if (check == null)
        //            {
        //                MessageBox.Show("You can delete Department have many teacher in there!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }

        //            final.Departments.Remove(department);
        //            if (final.SaveChanges() > 0)
        //            {
        //                MessageBox.Show("Delete successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //                loadDepartment();
        //                clear();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Delete failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Invalid ID!", "Allert", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void ClearButton_Click(object sender, RoutedEventArgs e)
        //{
        //    clear();
        //}
    }
}
