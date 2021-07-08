using Alg.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alg.Comparable
{
    public class OrgaoComparer : IComparer<Orgao>
    {
        public int Compare(Orgao x, Orgao y)
        {
            return x.codigo.CompareTo(y.codigo);
        }
    }
}
