using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaevaZad1
{
    /// <summary>
    /// Context-Free Grammar
    /// контекстно-свободная грамматика
    /// </summary>
    class CFG
    {
        public List<string> NonTerminals { get; set; }
        public List<string> Terminals { get; set; }
        public List<ProductionRule> ProductionRules { get; set; }
        public string StartSymbol { get; set; }

        public CFG()
        {
            NonTerminals = new List<string>();
            Terminals = new List<string>();
            ProductionRules = new List<ProductionRule>();
        }
    }
}
