using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dormitory.Data;

namespace Dormitory.WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly DormitoryRepository _repo = new DormitoryRepository();

        private List<StudentReportItem> _students;
        private StudentReportItem _selStudent;
        private Faculty _selFaculty;
        private Curator _selCurator;

        public MainWindow()
        {
            InitializeComponent();
            RefreshAll();
        }

        private void RefreshAll()
        {
            try
            {
                _students = _repo.GetStudentReport();
                DgStudents.ItemsSource = _students;

                var facs = _repo.GetAllFaculties();
                DgFaculties.ItemsSource = facs;
                CmbStudFaculty.ItemsSource = facs;

                var curs = _repo.GetAllCurators();
                DgCurators.ItemsSource = curs;
                CmbStudCurator.ItemsSource = curs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления данных: " + ex.Message);
            }
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_students == null) return;
            if (CmbSort.SelectedIndex == 1) DgStudents.ItemsSource = _students.OrderBy(x => x.FullName).ToList();
            else if (CmbSort.SelectedIndex == 2) DgStudents.ItemsSource = _students.OrderBy(x => x.RoomNumber).ToList();
            else DgStudents.ItemsSource = _students;
        }

        private void BtnRefreshStud_Click(object sender, RoutedEventArgs e) => RefreshAll();

        private void DgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgStudents.SelectedItem is StudentReportItem s)
            {
                _selStudent = s;
                TxtStudName.Text = s.FullName;
                TxtStudRoom.Text = s.RoomNumber.ToString();
                TxtStudGroup.Text = s.GroupNum;
                CmbStudFaculty.SelectedValue = _repo.GetAllFaculties().FirstOrDefault(f => f.FacultyName == s.FacultyName)?.FacultyID;
                CmbStudCurator.SelectedValue = _repo.GetAllCurators().FirstOrDefault(c => c.FullName == s.CuratorName)?.CuratorID;
            }
        }

        private void BtnAddStud_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbStudFaculty.SelectedValue == null || CmbStudCurator.SelectedValue == null) return;
                if (!int.TryParse(TxtStudRoom.Text, out int room)) return;

                _repo.AddStudent(TxtStudName.Text, (int)CmbStudFaculty.SelectedValue, room, TxtStudGroup.Text, (int)CmbStudCurator.SelectedValue);
                RefreshAll();
                MessageBox.Show("Студент добавлен");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEditStud_Click(object sender, RoutedEventArgs e)
        {
            if (_selStudent == null) return;
            try
            {
                if (!int.TryParse(TxtStudRoom.Text, out int room)) return;
                _repo.UpdateStudent(_selStudent.StudentCardID, TxtStudName.Text, (int)CmbStudFaculty.SelectedValue, room, TxtStudGroup.Text, (int)CmbStudCurator.SelectedValue);
                RefreshAll();
                MessageBox.Show("Студент обновлен");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnDelStud_Click(object sender, RoutedEventArgs e)
        {
            if (_selStudent == null) return;
            if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _repo.DeleteStudent(_selStudent.StudentCardID);
                RefreshAll();
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            int? r = int.TryParse(TxtFilterRoom.Text, out int v) ? v : (int?)null;
            _students = _repo.FilterStudents(r, TxtFilterCur.Text, TxtFilterFac.Text);
            DgStudents.ItemsSource = _students;
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WordReportGenerator.CreateFullStudentReport(_students, "Полный отчет");
                WordReportGenerator.CreateGroupedFacultyReport(_repo.GetFacultySummary());
                MessageBox.Show("Отчеты созданы");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DgFaculties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgFaculties.SelectedItem is Faculty f)
            {
                _selFaculty = f;
                TxtFacName.Text = f.FacultyName;
                TxtFacDean.Text = f.DeanName;
                TxtFacPhone.Text = f.DeanPhone;
            }
        }
        private void BtnAddFac_Click(object sender, RoutedEventArgs e) { _repo.AddFaculty(TxtFacName.Text, TxtFacDean.Text, TxtFacPhone.Text); RefreshAll(); }
        private void BtnEditFac_Click(object sender, RoutedEventArgs e) { if (_selFaculty != null) { _repo.UpdateFaculty(_selFaculty.FacultyID, TxtFacName.Text, TxtFacDean.Text, TxtFacPhone.Text); RefreshAll(); } }
        private void BtnDelFac_Click(object sender, RoutedEventArgs e) { if (_selFaculty != null) { _repo.DeleteFaculty(_selFaculty.FacultyID); RefreshAll(); } }


        private void DgCurators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgCurators.SelectedItem is Curator c)
            {
                _selCurator = c;
                TxtCurName.Text = c.FullName;
                TxtCurAddr.Text = c.Address;
                TxtCurPhone.Text = c.Phone;
                TxtCurPos.Text = c.Position;
            }
        }
        private void BtnAddCur_Click(object sender, RoutedEventArgs e) { _repo.AddCurator(TxtCurName.Text, TxtCurAddr.Text, TxtCurPhone.Text, TxtCurPos.Text); RefreshAll(); }
        private void BtnEditCur_Click(object sender, RoutedEventArgs e) { if (_selCurator != null) { _repo.UpdateCurator(_selCurator.CuratorID, TxtCurName.Text, TxtCurAddr.Text, TxtCurPhone.Text, TxtCurPos.Text); RefreshAll(); } }
        private void BtnDelCur_Click(object sender, RoutedEventArgs e) { if (_selCurator != null) { _repo.DeleteCurator(_selCurator.CuratorID); RefreshAll(); } }

        private void CmbQueryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TxtQueryParam == null) return;
            TxtQueryParam.Text = "";
            DgQueryResults.ItemsSource = null;
        }

        private void BtnExecQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbQueryType.SelectedItem is ComboBoxItem item)
                {
                    string tag = item.Tag.ToString();
                    string param = TxtQueryParam.Text;

                    DataTable dt = _repo.ExecuteQuery(tag, param);

                    DgQueryResults.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения запроса: " + ex.Message);
            }
        }
    }
}