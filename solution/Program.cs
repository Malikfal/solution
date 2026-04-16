using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solution
{
    /// <summary>
    /// Программа для рассчёта сложных процентов по вкладу
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine(CalculateInterest(1000, 3, 10));

        }

        /// <summary>
        /// Рассчитывает рост вклада по годам с учетом сложных процентов
        /// </summary>
        /// <param name="initial_deposit">Начальная сумма вклада</param>
        /// <param name="years">Количество лет</param>
        /// <param name="interest_rate">Годовая процентная ставка</param>
        /// <returns>Строка с суммой вклада по каждому году</returns>
        public static string CalculateInterest(double initial_deposit, int years, double interest_rate)
        {
            StringBuilder result = new StringBuilder();
            double currentAmount = initial_deposit;

            for (int i = 1; i <= years; i++)
            {
                currentAmount *= (1 + interest_rate / 100);
                result.AppendLine($"Год {i}: {currentAmount:F2} руб.");
            }

            return result.ToString();
        }
    }
}
