using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaevaZad1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем контекстно-свободную грамматику
            //var cfg = new CFG
            //{
            //    NonTerminals = new List<string> { "S", "A", "B", "C" },
            //    Terminals = new List<string> { "a", "b" },
            //    ProductionRules = new List<ProductionRule>
            //{
            //    new ProductionRule("S", new List<string> { "AB","CA" }),
            //    new ProductionRule("A", new List<string> { "a" }),
            //    new ProductionRule("B", new List<string> { "BC","AB" }),
            //    new ProductionRule("C", new List<string> { "aB","b" })
            //},
            //    StartSymbol = "S"
            //};
            //// Создаем контекстно-свободную грамматику
            //var cfg = new CFG
            //{
            //    NonTerminals = new List<string> { "S", "A", "B", "C" },
            //    Terminals = new List<string> { "a", "b", "c" },
            //    ProductionRules = new List<ProductionRule>
            //{
            //    new ProductionRule("S", new List<string> { "AB","A" }),
            //    new ProductionRule("A", new List<string> { "a" }),
            //    new ProductionRule("B", new List<string> { "BS","Ac" }),
            //    new ProductionRule("C", new List<string> { "aB","b" })
            //},
            //    StartSymbol = "S"
            //};
            // Создаем контекстно-свободную грамматику
            var cfg = new CFG
            {
                NonTerminals = new List<string> { "S", "B", "A" },
                Terminals = new List<string> { "a", "c" },
                ProductionRules = new List<ProductionRule>
            {
                new ProductionRule("S", new List<string> { "B", "a" }),
                new ProductionRule("A", new List<string> { "a"}),
                new ProductionRule("B", new List<string> { "BA", "Bc"})
            },
                StartSymbol = "S"
            };
            //// Выводим грамматику
            CFGUtility.PrintCfg(cfg, "Начальная грамматика");

            // Удаляем бесплодные символы
            CFG newCfgWithoutNegativSymbols = new CFG();
            CFGUtility.RemoveNegativSymbols(cfg, ref newCfgWithoutNegativSymbols);
            // Выводим обновленную грамматику
            CFGUtility.PrintCfg(newCfgWithoutNegativSymbols, "Грамматика после удаления бесплодных символов");

            // Удаляем недостижимые символы
            CFG newCfgWithoutUnreachableSymbols = new CFG();
            CFGUtility.RemoveUnreachableSymbols(newCfgWithoutNegativSymbols, ref newCfgWithoutUnreachableSymbols);
            // Выводим обновленную грамматику
            CFGUtility.PrintCfg(newCfgWithoutUnreachableSymbols, "Грамматика после удаления недостижимых символов");

        }
    }
}




