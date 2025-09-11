using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 1. Четвертый из 4+ собственных классов
    // Используется для демонстрации агрегации в классе Shop
    public class Employee
    {
        public string Name { get; set; }

        public Employee(string name)
        {
            Name = name;
        }

        public void Work()
        {
            Console.WriteLine($"Сотрудник {Name} работает.");
        }
    }
}
