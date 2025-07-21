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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finally
{
    /// <summary>
    /// Interaction logic for ManageSchedules.xaml
    /// </summary>
    public partial class ManageSchedules : Window
    {
        FinallyContext final = new FinallyContext();

        private int ID;
        public ManageSchedules(int Id)
        {
            InitializeComponent();
            load(Id);
            ID = Id;
        }
        public void load(int Id)
        {
            if(GetAccountID.Role == 3)
            {
                ButtonForAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonForAdmin.Visibility = Visibility.Collapsed;
            }

            TimeComboBox.ItemsSource = GetTime();
            SchedulesDataGrid.ItemsSource = final.Schedules.Where(t => t.ClassId == Id && t.TimeOfWeek.Value.Day >= 1 && t.TimeOfWeek.Value.Day <= 6 && t.TimeOfWeek.Value.Month == 7).ToList();
        }
  

        private void GradesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SchedulesDataGrid.SelectedItem is Schedule selected)
            {
                DateTime.TryParse(selected.TimeOfWeek.ToString(), out DateTime date);
                txtID.Text = selected.Id.ToString();
                txtDayOfWeeks.Text = selected.DayOfWeeks.ToString();
                txtSlot.Text = selected.Slot.ToString();
                txtClassID.Text = selected.ClassId.ToString();
                txtTeacherID.Text = selected.TeacherId.ToString();
                dpTime.SelectedDate = date;
            }
        }

        private void clear()
        {
            txtID.Text = "";
            txtDayOfWeeks.Text = "";
            txtSlot.Text = "";
            txtClassID.Text = "";
            txtTeacherID.Text = "";
            dpTime.SelectedDate = null;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ManageStudent manageStudent = new ManageStudent(ID);
            manageStudent.Show();
            this.Close();
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

        private void TimeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTime = TimeComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedTime))
            {
                string[] check = selectedTime.Split(' ');
                DateOnly.TryParse(check[0], out DateOnly date1);
                DateOnly.TryParse(check[2], out DateOnly date2);


                SchedulesDataGrid.ItemsSource = final.Schedules.Where(t => t.ClassId == ID && t.TimeOfWeek.Value.Day >= date1.Day && t.TimeOfWeek.Value.Day <= date2.Day && t.TimeOfWeek.Value.Month == date1.Month).ToList();

            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Schedule schedules = new Schedule();
            int checkClassID = 0;
            int checkTeacherID = 0;
            var checkClass = final.Classes.Where(t => t.Id == ID).ToList();
            var checkTeacher = final.Schedules.Where(a => a.ClassId == ID).Select(t => t.TeacherId).ToList();
            if (string.IsNullOrEmpty(txtDayOfWeeks.Text))
            {
                MessageBox.Show("DayOfWeeks is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(txtSlot.Text, out int t) || t < 1 || t > 4 )
            {
                MessageBox.Show("Slot must be is number and between 1-4", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(dpTime.SelectedDate == null)
            {
                MessageBox.Show("The Time Of Week is null!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((!int.TryParse(txtClassID.Text, out int a)) || a < 0)
            {
                MessageBox.Show("ClassId must be is number and greater than 0!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtClassID.Text))
            {
                MessageBox.Show("ClassId must be is number and greater than 0!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //foreach (var item in checkClass)
            //{
            //    if(item.Id == int.Parse(txtClassID.Text)){
            //        checkClassID = 1;
            //    }
            //}
            //if (checkClassID == 0)
            //{
            //    MessageBox.Show("ClassId not exist in the list class!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            if ((!int.TryParse(txtClassID.Text, out int b)) || b < 0)
            {
                MessageBox.Show("TeacherId must be is number and greater than 0!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var item in checkTeacher)
            {
                if (item == int.Parse(txtTeacherID.Text))
                {
                    checkTeacherID = 1;
                }
            }
            if (checkTeacherID == 0)
            {
                MessageBox.Show("TeacherId not exist in the list class!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var check = final.Schedules.ToList();
            DateOnly.TryParse(dpTime.Text, out DateOnly date1);
            foreach (var item in check)
            {
                if (item.Slot == int.Parse(txtClassID.Text) && item.TimeOfWeek.Equals(date1)) {
                    MessageBox.Show("You can add the same slot in the same day!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            DateOnly.TryParse(dpTime.Text, out DateOnly date);
            schedules.DayOfWeeks = txtDayOfWeeks.Text;
            schedules.Slot = int.Parse(txtSlot.Text);
            schedules.TeacherId = int.Parse(txtTeacherID.Text);
            schedules.ClassId = int.Parse(txtClassID.Text);
            schedules.TimeOfWeek = date;
            final.Schedules.Add(schedules);
            final.SaveChanges();
            MessageBox.Show("Add successfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            load(ID);
            clear();

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int checkClassID = 0;
            int checkTeacherID = 0;
            var checkClass = final.Classes.Where(t => t.Id == ID).ToList();
            var checkTeacher = final.Schedules.Where(a => a.ClassId == ID).Select(t => t.TeacherId).ToList();
            if (string.IsNullOrEmpty(txtDayOfWeeks.Text))
            {
                MessageBox.Show("DayOfWeeks is empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpTime.SelectedDate == null)
            {
                MessageBox.Show("The Time Of Week is null!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((!int.TryParse(txtSlot.Text, out int t)) || t < 1 || t > 4)
            {
                MessageBox.Show("Slot must be is number and between 1-4", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtClassID.Text))
            {
                MessageBox.Show("ClassId must be is number and greater than 0!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //foreach (var item in checkClass)
            //{
            //    if (item.Id == int.Parse(txtClassID.Text))
            //    {
            //        checkClassID = 1;
            //    }
            //}
            //if (checkClassID == 0)
            //{
            //    MessageBox.Show("ClassId not exist in the list class!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            if ((!int.TryParse(txtClassID.Text, out int b)) || b < 0)
            {
                MessageBox.Show("TeacherId must be is number and greater than 0!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var item in checkTeacher)
            {
                if (item == int.Parse(txtTeacherID.Text))
                {
                    checkTeacherID = 1;
                }
            }
            if (checkTeacherID == 0)
            {
                MessageBox.Show("TeacherId not exist in the list class!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateOnly.TryParse(dpTime.Text, out DateOnly date);
            Schedule schedules = final.Schedules.FirstOrDefault(x => x.Id == int.Parse(txtID.Text));
            schedules.DayOfWeeks = txtDayOfWeeks.Text;
            schedules.Slot = int.Parse(txtSlot.Text);
            schedules.TeacherId = int.Parse(txtTeacherID.Text);
            schedules.ClassId = int.Parse(txtClassID.Text);
            schedules.TimeOfWeek = date;
            final.Schedules.Update(schedules);
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
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Id Invalid!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Schedule schedule = final.Schedules.FirstOrDefault(x => x.Id == int.Parse(txtID.Text));
            if (schedule != null)
            {

                final.Schedules.Remove(schedule);
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
