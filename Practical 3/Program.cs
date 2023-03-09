using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Practical_3
{
    internal class Program
    {
        delegate Func<string> Operation(string s);

        static void Main(string[] args)
        {
            string filePath = "transactions.csv";
            string dateFormat = "dd.MM.yyyy";
            int recordsPerFile = 10;

            Func<string, DateTime> getDate = (s) => DateTime.ParseExact(s.Split(',')[0], dateFormat, null);
            Func<string, double> getAmount = (s) => double.Parse(s.Split(',')[1]);

            Action<DateTime, double> printTotal = (date, total) =>
            {
                Console.WriteLine($"{date.ToString(dateFormat)}: {total}");
            };

            Operation GetNewFilePath = (string s) =>
            {
                int i = 0;
                string Inner()
                {
                    i++;
                    return $"{s}_{i}.csv";
                }

                return Inner;
            };

            var getNewFilePath = GetNewFilePath(filePath.Split('.', 'c', 's', 'v')[0]) ;

            var transactions = File.ReadAllLines(filePath)
                .Select(line => new { Date = getDate(line), Amount = getAmount(line) }).ToList();

            for (int i = 0; i < transactions.Count; i += recordsPerFile)
            {
                string newFilePath = getNewFilePath();
                using (var writer = new StreamWriter(newFilePath))
                {
                    for (int j = i; j < i + recordsPerFile && j < transactions.Count; j++)
                    {
                        writer.WriteLine($"{transactions[j].Date.ToString(dateFormat)},{transactions[j].Amount}");
                    }
                }
            }

            var totalTrans = transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, TotalAmount = g.Sum(t => t.Amount) })
                .OrderBy(t => t.Date)
                .ToList();

            foreach (var transaction in totalTrans)
            {
                printTotal(transaction.Date, transaction.TotalAmount);
            }
        }
    }
}