using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEInventory
    {
        List<INEArmor> Armors;
        List<INEWeildable> Weildables;
        List<INEConsumable> Consumables;
        List<INETradeGood> TradeGoods;
    }

    public class INEItem
    {
        public string FullName { get; private set; }
        public int Index { get; private set; }
        float Weight;
        float Cost;
        float Era;
        float Upkeep;
    }

    public class INEArmor : INEItem
    {

    }

    public class INEWeildable : INEItem
    {

    }

    public class INEConsumable : INEItem
    {

    }

    public class INETradeGood : INEItem
    {
        
    }
}
