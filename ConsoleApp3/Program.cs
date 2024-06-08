using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace calculator
{
    internal class Program
    {
        private static double GetNumberFromUser(string message)
        {
            double number;
            while (true)
            {
                Console.WriteLine(message);
                if (double.TryParse(Console.ReadLine(), out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Ошибка!!! Введите корректное число.");
                }
            }
        }

        private static double PerformOperation(double a, double b, char oper)
        {
            double result = 0;

            switch (oper)
            {
                case '+':
                    result = a + b;
                    Console.WriteLine("Cумма " + a + " и " + b + " равна " + result + ".");
                    break;
                case '-':
                    result = a - b;
                    Console.WriteLine("Разность " + a + " и " + b + " равна " + result + ".");
                    break;
                case '*':
                    result = a * b;
                    Console.WriteLine("Умножение " + a + " на " + b + " равно " + result + ".");
                    break;
                case '/':
                    if (b == 0)
                    {
                        Console.WriteLine("Деление на 0 невозможно!");
                    }
                    else
                    {
                        result = a / b;
                        Console.WriteLine("Деление " + a + " на " + b + " равно " + result + ".");
                    }
                    break;
                case '^':
                    result = Math.Pow(a, b);
                    Console.WriteLine(a + " в степени " + b + " равно " + result);
                    break;
                case 's':
                    result = Math.Sin(a);
                    Console.WriteLine("Sin числа " + a + " равна " + result + ".");
                    break;
                case 'c':
                    result = Math.Cos(a);
                    Console.WriteLine("Cos числа " + a + " равна " + result + ".");
                    break;
                case 't':
                    result = Math.Tan(a);
                    Console.WriteLine("Tan числа " + a + " равна " + result + ".");
                    break;
                case 'k':
                    result = 1 / Math.Tan(a);
                    Console.WriteLine("Ctg числа " + a + " равна " + result + ".");
                    break;
                default:
                    Console.WriteLine("Неизвестный оператор.");
                    break;
            }

            return result;
        }

        public static void Main(string[] args)
        {
            // Читает файл с конфигурацией
            string json = File.ReadAllText("config.json");
            // Десериализовать JSON в объект конфигурации
            Config config = JsonConvert.DeserializeObject<Config>(json);

            // Меню выбора калькулятора
            Console.WriteLine("**Выберите калькулятор:**");
            Console.WriteLine("1. Базовый");
            Console.WriteLine("2. Тригонометрический");

            // Выбор пользователя
            int calculatorChoice;
            while (true)
            {
                Console.WriteLine("Введите номер калькулятора:");
                if (int.TryParse(Console.ReadLine(), out calculatorChoice))
                {
                    if (calculatorChoice >= 1 && calculatorChoice <= 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер калькулятора");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка!!! Введите корректное число.");
                }
            }

            // Запуск калькулятора который выбрал пользователь
            if (calculatorChoice == 1)
            {
                RunCalculator(config.BasicOperations);
            }
            else if (calculatorChoice == 2)
            {
                RunCalculator(config.TrigonometricOperations);
            }
        }


        private static void RunCalculator(List<Operation> operations)
        {
            while (true)
            {
                // Выводит меню с операциями для меню
                StringBuilder menu = new StringBuilder();
                menu.AppendLine("**Выберите действие**");
                int operationIndex = 1;
                foreach (var operation in operations)
                {
                    menu.AppendLine($"{operationIndex++}. {operation.Description}");
                }
                Console.WriteLine(menu);

                // получить выбор операции пользователя
                int operationChoice;
                while (true)
                {
                    Console.WriteLine("Введите номер операции:");
                    if (int.TryParse(Console.ReadLine(), out operationChoice))
                    {
                        if (operationChoice >= 1 && operationChoice <= operations.Count)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер операции");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка!!! Введите корректное число.");
                    }
                }

                // Ввод чисел пользователя
                double a, b;
                if (operations[operationChoice - 1].Symbol != 's' &&
                    operations[operationChoice - 1].Symbol != 'c' &&
                    operations[operationChoice - 1].Symbol != 't' &&
                    operations[operationChoice - 1].Symbol != 'k')
                {
                    a = GetNumberFromUser("Введите число 'a':");
                    b = GetNumberFromUser("Введите число 'b':");
                }
                else
                {
                    a = GetNumberFromUser("Введите число 'a':");
                    b = 0;
                }

                // Выполнение операции
                double result = PerformOperation(a, b, operations[operationChoice - 1].Symbol);

                // Вывод результатов
                Console.WriteLine($"Результат: {result}");

                // Спросить пользователя, хочет ли он продолжить
                Console.WriteLine("Продолжим? (y - ДА)");
                Console.WriteLine("НЕТ - любая клавиша");
                if (Console.ReadLine() != "y")
                    break;
            }
        }
    }


    public class Operation
    {
        public char Symbol { get; set; }
        public string Description { get; set; }

        public Operation(char symbol, string description)
        {
            Symbol = symbol;
            Description = description;
        }
    }

    
    public class Config
    {
        public List<Operation> BasicOperations { get; set; }
        public List<Operation> TrigonometricOperations { get; set; }
    }
}
