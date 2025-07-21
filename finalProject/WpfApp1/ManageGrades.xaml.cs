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
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finally
{
    /// <summary>
    /// Interaction logic for ManageGrades.xaml
    /// </summary>
    public partial class ManageGrades : Window
    {
        FinallyContext final = new FinallyContext();

        private int ID;
        public ManageGrades(int Id)
        {
            InitializeComponent();
            load(Id);
            ID = Id;
        }

        public void load(int Id)
        {
            var combo = final.Teachers.Select(t => t.FullName).ToList();
            combo.Insert(0, "All");
            TeacherComboBox.ItemsSource = combo;

            GradesDataGrid.ItemsSource = final.Grades.Include(t => t.Teacher).Where(x => x.ClassId == Id && x.TeacherId == GetAccountID.ID).ToList();
        }

        private bool isAscending = false;

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {

            if (isAscending)
            {


                GradesDataGrid.ItemsSource = final.Grades
                                          .Where(x => x.ClassId == ID)
                                          .OrderBy(x => x.Grade1)
                                          .ToList();

                SortButton.Content = "SortGradeDesc";
            }
            else
            {
                GradesDataGrid.ItemsSource = final.Grades
                                          .Where(x => x.ClassId == ID)
                                          .OrderByDescending(x => x.Grade1)
                                          .ToList();

                SortButton.Content = "SortGradeAsc";
            }

            isAscending = !isAscending;
        }

        private void GradesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GradesDataGrid.SelectedItem is Grade selected)
            {
                DateTime.TryParse(selected.DayOfGrade.ToString(), out DateTime date);
                txtGradeID.Text = selected.Id.ToString();
                txtStudentID.Text = selected.StudentId.ToString();
                txtClassID.Text = selected.ClassId.ToString();
                txtTeacherID.Text = selected.TeacherId.ToString();
                txtGrade.Text = selected.Grade1.ToString();
                dpDate.SelectedDate = date;
            }
        }

        private void clear()
        {
            txtGradeID.Text = "";
            txtStudentID.Text = "";
            txtClassID.Text = "";
            txtTeacherID.Text = "";
            txtGrade.Text = "";
            dpDate.SelectedDate = null;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageStudent manageStudent = new ManageStudent(ID);
            manageStudent.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            GradesDataGrid.ItemsSource = final.Grades.Where(t => t.StudentId == int.Parse(txtSearch.Text) && t.ClassId == ID).ToList();
        }

        private void TeacherComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTeacher = TeacherComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedTeacher))
            {
                if (selectedTeacher.Equals("All"))
                {
                    load(ID);
                }
                else
                {
                    Teacher teacher = final.Teachers.Where(t => t.FullName == selectedTeacher).FirstOrDefault();
                    GradesDataGrid.ItemsSource = final.Grades.Where(x => x.TeacherId == teacher.Id && x.ClassId == ID).ToList();
                }

            }

        }

        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Grade grade = new Grade();
        //    if (string.IsNullOrEmpty(txtGradeID.Text))
        //    {
        //        MessageBox.Show("GradeId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtClassID.Text))
        //    {
        //        MessageBox.Show("ClassId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtStudentID.Text))
        //    {
        //        MessageBox.Show("StudnetId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(txtTeacherID.Text))
        //    {
        //        MessageBox.Show("TeacherId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (!float.TryParse(txtGrade.Text, out float a) || a < 0 || a > 10)
        //    {
        //        MessageBox.Show("Grade must be is number and between 0-10", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (dpDate.SelectedDate == null)
        //    {
        //        MessageBox.Show("Date is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    var check = final.Grades.Where(x => x.ClassId == ID && x.TeacherId == GetAccountID.ID).ToList();
        //    foreach (var x in check)
        //    {
        //        if(x.StudentId == int.Parse(txtStudentID.Text))
        //        {
        //            MessageBox.Show("You can add the same student in the class!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
        //    }
        //    grade.StudentId = int.Parse(txtStudentID.Text);
        //    grade.ClassId = int.Parse(txtClassID.Text);
        //    grade.TeacherId = int.Parse(txtTeacherID.Text);
        //    grade.Grade1 = float.Parse(txtGrade.Text);
        //    DateOnly.TryParse(dpDate.Text, out DateOnly date);
        //    grade.DayOfGrade = date;
        //    final.Grades.Add(grade);
        //    final.SaveChanges();
        //    MessageBox.Show("Add successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    load(ID);
        //    clear();

        //}

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtGradeID.Text))
            {
                MessageBox.Show("GradeId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtClassID.Text))
            {
                MessageBox.Show("ClassId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtStudentID.Text))
            {
                MessageBox.Show("StudnetId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtTeacherID.Text))
            {
                MessageBox.Show("TeacherId is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!float.TryParse(txtGrade.Text, out float a) || a < 0 || a > 10)
            {
                MessageBox.Show("Grade must be is number and between 0-10", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Date is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Grade grade = final.Grades.FirstOrDefault(x => x.Id == int.Parse(txtGradeID.Text));
            grade.StudentId = int.Parse(txtStudentID.Text);
            grade.ClassId = int.Parse(txtClassID.Text);
            grade.TeacherId = int.Parse(txtTeacherID.Text);
            grade.Grade1 = float.Parse(txtGrade.Text);
            DateOnly.TryParse(dpDate.Text, out DateOnly date);
            grade.DayOfGrade = date;
            final.Grades.Update(grade);
            if (final.SaveChanges() > 0)
            {
                MessageBox.Show("Update successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                load(ID);
                clear();
            }
            else
            {
                MessageBox.Show("Grades not found!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtGradeID.Text))
            {
                MessageBox.Show("Id Invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Grade grade = final.Grades.FirstOrDefault(x => x.Id == int.Parse(txtGradeID.Text));
            if (grade != null)
            {

                final.Grades.Remove(grade);
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
    }
}
