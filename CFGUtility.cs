using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaevaZad1
{
    class CFGUtility
    {
        /// <summary>
        /// Удаление бесплодных символов
        /// </summary>
        /// <param name="cfg">Начальная грамматика</param>
        /// <param name="newCfg">Грамматика без бесп-ых символов</param>
        public static void RemoveNegativSymbols(CFG cfg, ref CFG newCfg)
        {
            // Инициализация списка бесплодных символов
            List<string> negativSymbols = new List<string>();

            // Строка полезных терминалов и не терминалов
            string positivSymbolsStr = string.Join("", cfg.Terminals.ToArray());

            // Формируем список полезных не терминалов
            CreatePositivSymbolsList(cfg, positivSymbolsStr);
            // Определение бесполезных символов
            foreach (var curentRules in cfg.ProductionRules) // идем по правилам
            {
                if (!curentRules.leftHandSide.FlagPositiv)
                {
                    negativSymbols.Add(curentRules.leftHandSide.Symbol);

                }
            }

            //Проверка не пуст ли язык
            if (negativSymbols.Contains(cfg.StartSymbol))
            {
                Console.WriteLine("ERROR: Язык пуст");
                return;
            }

            newCfg = NewCfgWithoutNegativSymbols(cfg, negativSymbols);
        }

        /// <summary>
        /// Удаляем недостижимые символы
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="newCf"></param>
        public static void RemoveUnreachableSymbols(CFG cfg, ref CFG newCfg)
        {    
            // инициализация HashSet достижимых символов
            var reachableSymbols = new HashSet<string>(); // HashSet<T>  коллекцией, которая позволяет хранить набор уникальных элементов типа T 
            var newReachableSymbols = new HashSet<string>();

            // Инициализируем множество достижимых символов стартовым символом
            reachableSymbols.Add(cfg.StartSymbol);
            newReachableSymbols.Add(cfg.StartSymbol);

            // Формируем список достижимых символов
            CreateUnreachableSymbols(cfg, reachableSymbols, newReachableSymbols);

            // новая грамматика
            // Оставляем в грамматике только достижимые символы
            newCfg.StartSymbol = cfg.StartSymbol;
            newCfg.Terminals = cfg.Terminals;
            // Удаляем недостижимые символы из списка нетерминалов
            newCfg.NonTerminals = cfg.NonTerminals.Intersect(reachableSymbols).ToList();
            // Удаляем недостижимые правила
            newCfg.ProductionRules = cfg.ProductionRules.Where(rule => reachableSymbols.Contains(rule.leftHandSide.Symbol)).ToList();
        }

        /// <summary>
        /// Формируем список полезных(не бесплодные) не терминалов
        /// как только найден новый позитивный символ 
        /// ищем полное вхождение правой части в поситив лист
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="cfg"></param>
        /// <param name="positivSymbols"></param>
        /// <param name="flag"></param>
        private static void CreatePositivSymbolsList (CFG cfg, string positivSymbolsStr)
        {
            foreach (var curentRules in cfg.ProductionRules) // идем по правилам
            {
                // Если символ в левой части уже полезный, то переходим к следующему правилу
                if(curentRules.leftHandSide.FlagPositiv)
                {
                    break;
                }

                foreach (var rightSymbol in curentRules.RightHandSide) //ищем левую сторону
                {
                    bool flagFindNewPositivSimvol = true;

                    //правило полностью входит в поситив лист 
                    foreach (var item in rightSymbol)
                    {
                        if (!positivSymbolsStr.Contains(item))
                        {
                            flagFindNewPositivSimvol = false;
                            break;
                        }
                    }

                    // Если найден новый позитивный символ 
                    // добавляем его в поситив список
                    if(flagFindNewPositivSimvol)
                    {
                        //найден новый полезный символ
                        positivSymbolsStr += curentRules.leftHandSide.Symbol;
                        curentRules.leftHandSide.FlagPositiv = true;
                        CreatePositivSymbolsList(cfg, positivSymbolsStr);
                    }
                }
                   
            }
        }

        /// <summary>
        /// Вывод грамматики
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="str"></param>
        public static void  PrintCfg(CFG cfg, string str)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine(str);
            Console.WriteLine("----------------------");
            Console.WriteLine("Нетерминалы: " + string.Join(", ", cfg.NonTerminals));
            Console.WriteLine("Терминалы: " + string.Join(", ", cfg.Terminals));
            Console.WriteLine("Продукционные правила:");
            foreach (var rule in cfg.ProductionRules)
            {
                Console.WriteLine(rule.leftHandSide.Symbol + " -> " + string.Join(" ", rule.RightHandSide));
            }
            Console.WriteLine("Стартовый символ: " + cfg.StartSymbol);
            Console.WriteLine();
        }

        /// <summary>
        /// Формируем новую грамматику без бесполезных символов
        /// на основе списка полезных символов
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="negativSymbols"></param>
        /// <returns></returns>
        public static CFG NewCfgWithoutNegativSymbols(CFG cfg, List<string> negativSymbols)
        {
            // RightHandSide после удаления 
            List<string> newRightHandSide = new List<string>(); 

            // Формируем новую грамматику
            CFG newCfg = new CFG();
            newCfg.Terminals = cfg.Terminals;
            // удаляем бесполезн символы
            newCfg.NonTerminals = cfg.NonTerminals.Except(negativSymbols).ToList();
            newCfg.StartSymbol = cfg.StartSymbol;

            // Формируем в новой грамматике правила
            for (int i = 0; i < cfg.ProductionRules.Count; i++)
            {
                if (cfg.ProductionRules[i].leftHandSide.FlagPositiv) // если терминал полезный
                {
                    // в новой грамматике записываем левую сторону
                    foreach (var curentRules in cfg.ProductionRules[i].RightHandSide) // проход по правой стороне в старых правилах 
                    {
                        bool flag = true;
                        foreach (var item in negativSymbols)
                        {
                            if (curentRules.Contains(item))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            // добавляем только те правила, которые не содержат бесполезных символов
                            newRightHandSide.Add(curentRules);

                        }
                    }

                    if (newRightHandSide.Count != 0)
                    {
                        // формируем правила которые не содержат бесполезных символов
                        newCfg.ProductionRules.Add(new ProductionRule(cfg.ProductionRules[i].leftHandSide.Symbol, newRightHandSide));
                    }
                    newRightHandSide = new List<string>();
                }
            }
            return newCfg;
        }
       
       /// <summary>
       /// Формируем список достижимых символов
       /// </summary>
       /// <param name="cfg"></param>
       /// <param name="reachableSymbols"></param>
       /// <param name="newReachableSymbols"></param>
        public static void CreateUnreachableSymbols(CFG cfg, HashSet<string> reachableSymbols, HashSet<string> newReachableSymbols)
        {
            foreach (var productionRule in cfg.ProductionRules)
            {
                //Если стартовый символ в левой стороне, записываем его правую
                if(productionRule.leftHandSide.Symbol == cfg.StartSymbol)
                {
                    // добавляем правую часть посимвольно
                    List<string> charRightRuleList = new List<string>();
                    foreach (var rightRule in productionRule.RightHandSide)
                    {
                        foreach (var charRightRule in rightRule)
                        {
                            charRightRuleList.Add(charRightRule.ToString());
                        }
                    }
                    newReachableSymbols.UnionWith(charRightRuleList); // UnionWith для добавления List<string> в HashSet<string>
                }
                else
                {
                    // Если символ достижим из reachableSymbols, то добавляем его правую сторону
                    if (reachableSymbols.All(symbol => productionRule.leftHandSide.Symbol.Contains(symbol)))
                    {
                        // добавляем левую часть
                        newReachableSymbols.Add(productionRule.leftHandSide.Symbol);
                        // добавляем правую часть посимвольно
                        List<string> charRightRuleList = new List<string>();
                        foreach (var rightRule in productionRule.RightHandSide)
                        {
                            foreach(var charRightRule in rightRule)
                            {
                                charRightRuleList.Add(charRightRule.ToString()); 
                            }
                        }
                        newReachableSymbols.UnionWith(charRightRuleList); // UnionWith для добавления List<string> в HashSet<string>

                    }
                }
                
                if(newReachableSymbols.Count > reachableSymbols.Count)
                {
                    reachableSymbols.UnionWith(newReachableSymbols);
                    CreateUnreachableSymbols(cfg, reachableSymbols, newReachableSymbols);
                }
            }
        }
    }
}
