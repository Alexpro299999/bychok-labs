using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dormitory.Data.EF;
using Dormitory.Data.EF.Models;
using Dormitory.Data.EF.Repositories;

namespace Dormitory.WpfApp.EF
{
    public partial class MainWindow : Window
    {
        private readonly DormitoryContext _ctx = new DormitoryContext();
        private readonly StudentRepository _studRepo;
        private readonly FacultyRepository _facRepo;
        private readonly CuratorRepository _curRepo;

        private List<StudentReportItem> _students;
        private object _selStudent;
        private Faculty _selFaculty;
        private Curator _selCurator;

        public MainWindow()
        {
            InitializeComponent();
            DbInitializer.Initialize(_ctx);
            _studRepo = new StudentRepository(_ctx);
            _facRepo = new FacultyRepository(_ctx);
            _curRepo = new CuratorRepository(_ctx);
            RefreshAll();
        }

        private void RefreshAll()
        {
            try
            {
                _students = _studRepo.GetReport();
                DgStudents.ItemsSource = _students;

                var facs = _facRepo.GetAll();
                DgFaculties.ItemsSource = facs;
                CmbStudFaculty.ItemsSource = facs;
                CmbStudFaculty.DisplayMemberPath = "FacultyName";
                CmbStudFaculty.SelectedValuePath = "FacultyID";

                var curs = _curRepo.GetAll();
                DgCurators.ItemsSource = curs;
                CmbStudCurator.ItemsSource = curs;
                CmbStudCurator.DisplayMemberPath = "FullName";
                CmbStudCurator.SelectedValuePath = "CuratorID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRefreshStud_Click(object sender, RoutedEventArgs e) => RefreshAll();

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_students == null) return;
            if (CmbSort.SelectedIndex == 1) DgStudents.ItemsSource = _students.OrderBy(x => x.FullName).ToList();
            else if (CmbSort.SelectedIndex == 2) DgStudents.ItemsSource = _students.OrderBy(x => x.RoomNumber).ToList();
            else DgStudents.ItemsSource = _students;
        }

        private void CmbQueryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TxtQueryParam != null) TxtQueryParam.Text = "";
            if (DgQueryResults != null) DgQueryResults.ItemsSource = null;
        }

        private void BtnExecQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbQueryType.SelectedItem is ComboBoxItem item)
                {
                    string tag = item.Tag.ToString();
                    string param = TxtQueryParam.Text;

                    switch (tag)
                    {
                        case "StudentsInRoom": // Запрос 1: Студенты в комнате
                            if (int.TryParse(param, out int room))
                            {
                                DgQueryResults.ItemsSource = _studRepo.Filter(room, null, null);
                            }
                            else
                            {
                                MessageBox.Show("Введите номер комнаты (число)");
                            }
                            break;

                        case "StudentsByCurator": // Запрос 2: Студенты куратора
                            DgQueryResults.ItemsSource = _studRepo.Filter(null, param, null);
                            break;

                        case "RoomStats": // Запрос 3: Статистика по комнатам
                            DgQueryResults.ItemsSource = (System.Collections.IEnumerable)_studRepo.GetRoomStats();
                            break;

                        case "CuratorsByFaculty": // Запрос 4: Кураторы факультета
                            DgQueryResults.ItemsSource = _curRepo.GetByFaculty(param);
                            break;

                        case "FacultySummary": // Запрос 5: Итоговый отчет
                            DgQueryResults.ItemsSource = _facRepo.GetSummary();
                            break;

                        default:
                            MessageBox.Show("Неизвестный тип запроса");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выполнения запроса: " + ex.Message);
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WordReportGenerator.CreateFullStudentReport(_students, "Полный отчет (EF)");
                WordReportGenerator.CreateGroupedFacultyReport(_facRepo.GetSummary());
                MessageBox.Show("Отчеты созданы");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnAddStud_Click(object sender, RoutedEventArgs e)
        {
            if (CmbStudFaculty.SelectedValue == null || CmbStudCurator.SelectedValue == null) return;
            if (!int.TryParse(TxtStudRoom.Text, out int room)) return;

            var s = new Student
            {
                FullName = TxtStudName.Text,
                FacultyID = (int)CmbStudFaculty.SelectedValue,
                CuratorID = (int)CmbStudCurator.SelectedValue,
                RoomNumber = room,
                GroupNum = TxtStudGroup.Text
            };
            _studRepo.Add(s);
            RefreshAll();
        }

        private void BtnEditStud_Click(object sender, RoutedEventArgs e)
        {
            if (_selStudent is StudentReportItem dto)
            {
                if (!int.TryParse(TxtStudRoom.Text, out int room)) return;
                var s = _ctx.Students.Find(dto.StudentCardID);
                if (s != null)
                {
                    s.FullName = TxtStudName.Text;
                    s.FacultyID = (int)CmbStudFaculty.SelectedValue;
                    s.CuratorID = (int)CmbStudCurator.SelectedValue;
                    s.RoomNumber = room;
                    s.GroupNum = TxtStudGroup.Text;
                    _studRepo.Update(s);
                    RefreshAll();
                }
            }
        }

        private void BtnDelStud_Click(object sender, RoutedEventArgs e)
        {
            if (_selStudent is StudentReportItem dto)
            {
                if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _studRepo.Delete(dto.StudentCardID);
                    RefreshAll();
                }
            }
        }

        private void DgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgStudents.SelectedItem is StudentReportItem s)
            {
                _selStudent = s;
                TxtStudName.Text = s.FullName;
                TxtStudRoom.Text = s.RoomNumber.ToString();
                TxtStudGroup.Text = s.GroupNum;

                var f = _facRepo.GetAll().FirstOrDefault(x => x.FacultyName == s.FacultyName);
                if (f != null) CmbStudFaculty.SelectedValue = f.FacultyID;

                var c = _curRepo.GetAll().FirstOrDefault(x => x.FullName == s.CuratorName);
                if (c != null) CmbStudCurator.SelectedValue = c.CuratorID;
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            int? r = int.TryParse(TxtFilterRoom.Text, out int v) ? v : null;
            _students = _studRepo.Filter(r, TxtFilterCur.Text, TxtFilterFac.Text);
            DgStudents.ItemsSource = _students;
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

        private void BtnAddFac_Click(object sender, RoutedEventArgs e)
        {
            _facRepo.Add(new Faculty { FacultyName = TxtFacName.Text, DeanName = TxtFacDean.Text, DeanPhone = TxtFacPhone.Text });
            RefreshAll();
        }

        private void BtnEditFac_Click(object sender, RoutedEventArgs e)
        {
            if (_selFaculty != null)
            {
                _selFaculty.FacultyName = TxtFacName.Text;
                _selFaculty.DeanName = TxtFacDean.Text;
                _selFaculty.DeanPhone = TxtFacPhone.Text;
                _facRepo.Update(_selFaculty);
                RefreshAll();
            }
        }

        private void BtnDelFac_Click(object sender, RoutedEventArgs e)
        {
            if (_selFaculty != null) { _facRepo.Delete(_selFaculty.FacultyID); RefreshAll(); }
        }

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

        private void BtnAddCur_Click(object sender, RoutedEventArgs e)
        {
            _curRepo.Add(new Curator { FullName = TxtCurName.Text, Address = TxtCurAddr.Text, Phone = TxtCurPhone.Text, Position = TxtCurPos.Text });
            RefreshAll();
        }

        private void BtnEditCur_Click(object sender, RoutedEventArgs e)
        {
            if (_selCurator != null)
            {
                _selCurator.FullName = TxtCurName.Text;
                _selCurator.Address = TxtCurAddr.Text;
                _selCurator.Phone = TxtCurPhone.Text;
                _selCurator.Position = TxtCurPos.Text;
                _curRepo.Update(_selCurator);
                RefreshAll();
            }
        }

        private void BtnDelCur_Click(object sender, RoutedEventArgs e)
        {
            if (_selCurator != null) { _curRepo.Delete(_selCurator.CuratorID); RefreshAll(); }
        }
    }
}