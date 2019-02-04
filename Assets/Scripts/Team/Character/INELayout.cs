using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INELayout
    {
        List<INEWornArmor> WornArmor;
        List<INEWeaponSet> WeaponSets;
        List<INEConsumable> Potions;
        List<INESpell> Spells;
    }


    public class INEWornArmor
    {

    }

    public class INEAction
    {

    }

    public class INESpell
    {

    }

    public class INEWeaponSet
    {
        List<INEAction> Actions;
        List<INEWeildable> Weildables;
    }
}
