using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Destiny.Web.Models
{
    public class SixtyFourModel
    {
        public IList<string> Pools = null;

        public SixtyFourModel()
        {
            Pools = new List<string>();
            for (int i = 1; i<=8;i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    Pools.Add(i + "-" + j);
                }
            }
        }
    }
}