using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Practical_3_4
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.txt");

            Func<string, IEnumerable<string>> tokenize = (text) =>
            {
                char[] delimiters = { ' ', '\r', '\n', '\t', '.', ',', ';', ':', '?', '!', '-', '(', ')' };
                return text.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            };

            Func<IEnumerable<string>, IDictionary<string, int>> computeFrequency = (tokens) =>
            {
                IDictionary<string, int> frequency = new Dictionary<string, int>();
                foreach (string token in tokens)
                {
                    if (frequency.ContainsKey(token))
                    {
                        frequency[token]++;
                    }
                    else
                    {
                        frequency[token] = 1;
                    }
                }
                return frequency;
            };

            Action<IDictionary<string, int>> displayStatistics = (frequency) =>
            {
                foreach (var pair in frequency.OrderByDescending(p => p.Value))
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                }
            };

            IDictionary<string, int> totalFrequency = new Dictionary<string, int>();
            foreach (string fileName in fileNames)
            {
                string text = File.ReadAllText(fileName);
                IEnumerable<string> tokens = tokenize(text);
                IDictionary<string, int> frequency = computeFrequency(tokens);
                foreach (var pair in frequency)
                {
                    string word = pair.Key;
                    int count = pair.Value;
                    if (totalFrequency.ContainsKey(word))
                    {
                        totalFrequency[word] += count;
                    }
                    else
                    {
                        totalFrequency[word] = count;
                    }
                }
            }

            displayStatistics(totalFrequency);
        }
    }
}


