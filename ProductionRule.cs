using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaevaZad1
{
    /// <summary>
    /// Продукционные правила
    /// </summary>
    class ProductionRule
    {
        public LeftHandSide leftHandSide { get; set; }
        public List<string> RightHandSide { get; set; }

        public ProductionRule(string lhs, List<string> rhs)
        {
            leftHandSide = new LeftHandSide(lhs);
            RightHandSide = rhs;
        }
    }

    class LeftHandSide
    {
        public string Symbol { get; set; }
        public bool FlagPositiv { get; set; }

        public LeftHandSide(string lhs)
        {
            Symbol = lhs;
            FlagPositiv = false;
        }
    }
}
