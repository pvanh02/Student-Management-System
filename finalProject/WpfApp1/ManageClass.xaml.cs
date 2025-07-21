using Finally.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ManageClass.xaml
    /// </summary>
    public partial class ManageClass : Window
    {
        FinallyContext final = new FinallyContext();

        public ManageClass()
        {
            InitializeComponent();
            loadClass();
        }


        public void loadClass()
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

            var load = final.Classes.ToList();

            ListClass.ItemsSource = load.Select(d => new
            {
                d.Id,
                d.Name,
                Content = d.Id == 1 ? "Specialized math class" : (d.Id == 2 ? "Specialized physics class" : "None")
            }).ToList();
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.CommandParameter is int Id)
            {
                ManageStudent student = new ManageStudent(Id);
                Class theClass = final.Classes.FirstOrDefault(d => d.Id == Id);
                student.Title = "Student Of Class: " + theClass.Name;
                student.Show();
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageDepartment manageDepartment = new ManageDepartment();
            manageDepartment.Show();
            this.Close();
        }

        //private void clear()
        //{
        //    txtId.Text = "";
        //    txtName.Text = "";
        //    txtContent.Text = "";
        //}
        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Class class1 = new Class();
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
        //    if (string.IsNullOrEmpty(txtContent.Text))
        //    {
        //        MessageBox.Show("Content is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    class1.Name = txtName.Text;
        //    final.Classes.Add(class1);
        //    final.SaveChanges();
        //    MessageBox.Show("Add successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    loadClass();
        //    clear();
        //}

        //private void EditButton_Click(object sender, RoutedEventArgs e)
        //{
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
        //    if (string.IsNullOrEmpty(txtContent.Text))
        //    {
        //        MessageBox.Show("Content is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    Class class1 = final.Classes.FirstOrDefault(c => c.Id == int.Parse(txtId.Text));
        //    class1.Name = txtName.Text;
        //    final.Classes.Update(class1);
        //    if (final.SaveChanges() > 0)
        //    {
        //        MessageBox.Show("Update successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //        loadClass();
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
        //    Class class1 = final.Classes.FirstOrDefault(c => c.Id == int.Parse(txtId.Text));
        //    var check = final.Students.Select(t => t.ClassId == int.Parse(txtId.Text)).ToList();
        //    if (class1 != null)
        //    {
        //        if (check == null)
        //        {
        //            MessageBox.Show("You can delete Class have many Student in there!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        final.Classes.Remove(class1);
        //        if (final.SaveChanges() > 0)
        //        {
        //            MessageBox.Show("Delete successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //            loadClass();
        //            clear();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Delete failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        //private void ClearButton_Click(object sender, RoutedEventArgs e)
        //{
        //    clear();
        //}
    }
}
