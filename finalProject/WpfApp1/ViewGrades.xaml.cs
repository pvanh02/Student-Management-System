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
using static Azure.Core.HttpHeader;

namespace Finally
{
    /// <summary>
    /// Interaction logic for ViewGrades.xaml
    /// </summary>
    public partial class ViewGrades : Window
    {
        FinallyContext final = new FinallyContext();

        private int classId;
        public ViewGrades()
        {
            InitializeComponent();
            load();
        }

        public void load()
        {
            var combo = final.Teachers.Select(t => t.Subject).ToList();
            combo.Insert(0, "All");
            SubjectComboBox.ItemsSource = combo;
            Student student = final.Students.FirstOrDefault(t => t.Id == GetAccountID.ID);
            var check = final.Grades.Where(x => x.StudentId == student.Id).ToList();
            GradesDataGrid.ItemsSource = check.Select(t => new
            {
                t.Id,
                StudentName = student.FullName,
                ClassName = GetClassName(t.ClassId),
                TeacherName = GetTeacherName(t.TeacherId),
                t.Grade1,
                Subject = GetSubject(t.TeacherId),
                t.DayOfGrade
            }).ToList();
        }

        private string GetSubject(int? id)
        {
            var check = final.Teachers.ToList();
            foreach (var t in check)
            {
                if (t.Id == id)
                {
                    return t.Subject;
                }
            }
            return "";
        }

        private string GetTeacherName(int? id)
        {
            var check = final.Teachers.ToList();
            foreach (var t in check)
            {
                if (t.Id == id)
                {
                    return t.FullName;
                }
            }
            return "";
        }

        private string GetClassName(int? id)
        {
            var check = final.Classes.ToList();
            foreach (var x in check)
            {
                if(x.Id == id)
                {
                    classId =x.Id;
                    return x.Name;
                }    
            }
            return "";
        }

        private bool isAscending = false;

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = final.Students.FirstOrDefault(t => t.Id == GetAccountID.ID);
            if (isAscending)
            {
                var check = final.Grades.Where(x => x.StudentId == student.Id).OrderBy(x => x.Grade1).ToList();
                GradesDataGrid.ItemsSource = check.Select(t => new
                {
                    t.Id,
                    StudentName = student.FullName,
                    ClassName = GetClassName(t.ClassId),
                    TeacherName = GetTeacherName(t.TeacherId),
                    Subject = GetSubject(t.TeacherId),
                    t.Grade1,
                    t.DayOfGrade
                }).ToList();

                SortButton.Content = "SortGradeDesc";
            }
            else
            {
                var check = final.Grades.Where(x => x.StudentId == student.Id).OrderByDescending(x => x.Grade1).ToList();
                GradesDataGrid.ItemsSource = check.Select(t => new
                {
                    t.Id,
                    StudentName = student.FullName,
                    ClassName = GetClassName(t.ClassId),
                    TeacherName = GetTeacherName(t.TeacherId),
                    Subject = GetSubject(t.TeacherId),
                    t.Grade1,
                    t.DayOfGrade
                }).ToList();

                SortButton.Content = "SortGradeAsc";
            }

            isAscending = !isAscending;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SubjectComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedSubject = SubjectComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedSubject))
            {
                if (selectedSubject.Equals("All"))
                {
                    load();
                }
                else
                {
                    Student student = final.Students.FirstOrDefault(t => t.Id == GetAccountID.ID);
                    Teacher teacher = final.Teachers.Where(t => t.Subject == selectedSubject).FirstOrDefault();
                    if (teacher != null)
                    {
                        var check = final.Grades.Where(t => t.TeacherId == teacher.Id && t.StudentId == student.Id).ToList();
                        GradesDataGrid.ItemsSource = check.Select(t => new
                        {
                            t.Id,
                            StudentName = student.FullName,
                            ClassName = GetClassName(t.ClassId),
                            TeacherName = GetTeacherName(t.TeacherId),
                            Subject = GetSubject(t.TeacherId),
                            t.Grade1,
                            t.DayOfGrade
                        }).ToList();
                    }
                }

            }

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            StudentProfile studentProfile = new StudentProfile();
            studentProfile.Show();
            this.Close();

        }

        private void ViewButton1_Click(object sender, RoutedEventArgs e)
        {
            ViewSchedules viewSchedules = new ViewSchedules(classId);
            viewSchedules.Show();
            this.Close();
        }
    }

   
}

