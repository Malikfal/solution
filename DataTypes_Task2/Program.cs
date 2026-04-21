using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes_Task2
{
    /// <summary>
    /// Программа формирующая ромб заднного размера
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string diamant = s_CreateDiamant(7);
            Console.WriteLine(diamant);
        }

        /// <summary>
        /// Формирует строковое представление ромба заданного размера
        /// </summary>
        /// <param name="size">Размер ромба</param>
        /// <returns>Строка с ромбом из символов 'x'</returns>
        public static string s_CreateDiamant(int size)
        {

            var result = new StringBuilder();
            int mid = size / 2;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Math.Abs(i - mid) + Math.Abs(j - mid) == mid) result.Append("x");
                    else result.Append(" ");
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    }
}
