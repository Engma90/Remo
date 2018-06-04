using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Remo.Connections
{
    class MyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        MyDictionary():base(){ }
        MyDictionary(int capacity):base(capacity){ }
        //public override void Add(TKey k, TValue v)
        //{

        //}

    }
}
