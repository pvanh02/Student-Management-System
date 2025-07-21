using Finally.Models;
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
    /// Interaction logic for ViewTeacherSchedules.xaml
    /// </summary>
    public partial class ViewTeacherSchedules : Window
    {
        FinallyContext final = new FinallyContext();

        private int DepartmentID;
        public ViewTeacherSchedules(int DepartmentId)
        {
            InitializeComponent();
            load();
            DepartmentID = DepartmentId;
        }

        public void load()
        {
            TimeComboBox.ItemsSource = GetTime();
            var check = final.Schedules.Where(t => t.TeacherId == GetAccountID.ID && t.TimeOfWeek.Value.Day >= 1 && t.TimeOfWeek.Value.Day <= 6 && t.TimeOfWeek.Value.Month == 7).ToList();
            ScheduleDataGrid.ItemsSource = check.Select(t => new
            {
                t.DayOfWeeks,
                t.Slot,
                ClassName = GetClassName(t.ClassId),
                TeacherName = GetTeacherName(t.TeacherId),
                Subject = GetSubject(t.TeacherId),
                t.TimeOfWeek,
            }).ToList();
        }

        private List<string> GetTime()
        {
            var list = new List<string>();
            var check = final.Schedules.ToList();
            string test = "";
            foreach (var t in check)
            {
                if (t.DayOfWeeks.Equals("Monday") && t.Slot == 1)
                {
                    test += t.TimeOfWeek + " To ";
                }
                if (t.DayOfWeeks.Equals("Saturday") && t.Slot == 1)
                {
                    test += t.TimeOfWeek;
                    list.Add(test);
                    test = "";
                }
            }
            return list;
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
                if (x.Id == id)
                {
                    return x.Name;
                }
            }
            return "";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageTeacher teacher = new ManageTeacher(DepartmentID);
            teacher.Show();
            this.Close();
        }

        private void TimeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTime = TimeComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedTime))
            {
                string[] check = selectedTime.Split(' ');
                DateOnly.TryParse(check[0], out DateOnly date1);
                DateOnly.TryParse(check[2], out DateOnly date2);

                Teacher teacher = final.Teachers.FirstOrDefault(t => t.Id == GetAccountID.ID);

                var check1 = final.Schedules.Where(t => t.TeacherId == teacher.Id && t.TimeOfWeek.Value.Day >= date1.Day && t.TimeOfWeek.Value.Day <= date2.Day && t.TimeOfWeek.Value.Month == date1.Month).ToList();
                ScheduleDataGrid.ItemsSource = check1.Select(t => new
                {
                    t.DayOfWeeks,
                    t.Slot,
                    ClassName = GetClassName(t.ClassId),
                    TeacherName = GetTeacherName(t.TeacherId),
                    Subject = GetSubject(t.TeacherId),
                    t.TimeOfWeek,
                }).ToList();
            }

        }
    }
}
