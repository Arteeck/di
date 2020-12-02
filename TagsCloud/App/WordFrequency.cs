﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TagsCloud.App
{
    public class WordFrequency
    {
        private readonly WordChecker wordChecker;

        public WordFrequency(WordChecker wordChecker)
        {
            this.wordChecker = wordChecker;
        }

        public Dictionary<string, double> GetFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var wordFrequencies = new Dictionary<string, double>();
            var words = lines
                .Select(x => x.ToLower())
                .Where(x => wordChecker.IsWordNotBoring(x));
            foreach (var word in words)
            {
                if (!wordFrequencies.ContainsKey(word))
                    wordFrequencies[word] = 1;
                wordFrequencies[word]++;
            }

            return wordFrequencies
                .ToDictionary(x=>x.Key,
                    x=>Math.Round(x.Value / lines.Length, 2));
        }
    }
}