using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INETeam
    {
        public string FullName { get; private set; }
        public int Index { get; private set; }
        float Gold;
        float Leadership;
        float Upkeep;
        List<INECharacter> Characters;
        List<INEFormation> Formations;
        INEInventory Inventory;
        List<INESpell> SpellBook;
    }
}
