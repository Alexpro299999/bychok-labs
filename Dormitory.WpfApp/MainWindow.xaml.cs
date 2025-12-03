using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dormitory.Data;

namespace Dormitory.WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly DormitoryRepository _repository;
        private List<StudentReportItem> _currentList;
        private StudentReportItem _selectedStudent;

        public MainWindow()
        {
            InitializeComponent();
            _repository = new DormitoryRepository();
            LoadDictionaries();
            LoadData();
        }

        private void LoadDictionaries()
        {
            try
            {
                CmbFaculty.ItemsSource = _repository.GetAllFaculties();
                CmbCurator.ItemsSource = _repository.GetAllCurators();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                _currentList = _repository.GetStudentReport();
                ApplySort();
                DgStudents.ItemsSource = _currentList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ClearInputs();
        }

        private void DgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgStudents.SelectedItem is StudentReportItem item)
            {
                _selectedStudent = item;
                TxtFullName.Text = item.FullName;
                TxtRoom.Text = item.RoomNumber.ToString();
                TxtGroup.Text = item.GroupNum;

                foreach (Faculty f in CmbFaculty.Items)
                {
                    if (f.FacultyName == item.FacultyName)
                    {
                        CmbFaculty.SelectedItem = f;
                        break;
                    }
                }

                foreach (Curator c in CmbCurator.Items)
                {
                    if (c.FullName == item.CuratorName)
                    {
                        CmbCurator.SelectedItem = c;
                        break;
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbFaculty.SelectedValue == null || CmbCurator.SelectedValue == null)
                {
                    MessageBox.Show("Выберите факультет и куратора");
                    return;
                }

                if (!int.TryParse(TxtRoom.Text, out int room))
                {
                    MessageBox.Show("Некорректный номер комнаты");
                    return;
                }

                _repository.AddStudent(
                    TxtFullName.Text,
                    (int)CmbFaculty.SelectedValue,
                    room,
                    TxtGroup.Text,
                    (int)CmbCurator.SelectedValue
                );

                LoadData();
                ClearInputs();
                MessageBox.Show("Добавлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null) return;

            try
            {
                if (!int.TryParse(TxtRoom.Text, out int room))
                {
                    MessageBox.Show("Некорректный номер комнаты");
                    return;
                }

                _repository.UpdateStudent(_selectedStudent.StudentCardID, room, TxtGroup.Text);

                LoadData();
                ClearInputs();
                MessageBox.Show("Обновлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null)
            {
                MessageBox.Show("Выберите студента");
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.DeleteStudent(_selectedStudent.StudentCardID);
                    LoadData();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? room = null;
                if (int.TryParse(TxtFilterRoom.Text, out int r)) room = r;

                _currentList = _repository.FilterStudents(room, TxtFilterCurator.Text, TxtFilterFaculty.Text);
                ApplySort();
                DgStudents.ItemsSource = null;
                DgStudents.ItemsSource = _currentList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentList != null)
            {
                ApplySort();
                DgStudents.ItemsSource = null;
                DgStudents.ItemsSource = _currentList;
            }
        }

        private void ApplySort()
        {
            if (_currentList == null) return;

            if (CmbSort.SelectedIndex == 1)
                _currentList = _currentList.OrderBy(x => x.FullName).ToList();
            else if (CmbSort.SelectedIndex == 2)
                _currentList = _currentList.OrderBy(x => x.RoomNumber).ToList();
            else if (CmbSort.SelectedIndex == 3)
                _currentList = _currentList.OrderBy(x => x.FacultyName).ToList();
        }

        private void ClearInputs()
        {
            TxtFullName.Text = "";
            TxtRoom.Text = "";
            TxtGroup.Text = "";
            CmbFaculty.SelectedIndex = -1;
            CmbCurator.SelectedIndex = -1;
            _selectedStudent = null;
            DgStudents.SelectedItem = null;
        }

        private void BtnReportFull_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WordReportGenerator.CreateFullStudentReport(_currentList, "Отчет WPF");
                MessageBox.Show("Отчет создан");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnReportGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = _repository.GetFacultySummary();
                WordReportGenerator.CreateGroupedFacultyReport(data);
                MessageBox.Show("Отчет создан");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}