using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INETeam
    {
        public string FullName { get; private set; }
        public int Index { get; private set; }
        readonly float Gold;
        readonly float Leadership;
        //float Upkeep;
        List<INECharacter> Characters;
        List<INEFormation> Formations;
        INEInventory Inventory;
        List<INESpell> SpellBook;

        public int CharacterCount { get { return Characters.Count; } }

        public INETeam()
        {
            FullName = "";
            Index = -1;
            Gold = 0;
            //Leadership = 0;
            //Upkeep = 0;
            Characters = new List<INECharacter>();
            Formations = new List<INEFormation>();
            Inventory = new INEInventory();
            SpellBook = new List<INESpell>();
        }

        public INETeam( INETeamResponse newTeam )
        {
            FullName = newTeam.TeamName;
            Index = newTeam.Team;
            Leadership = newTeam.Leadership;
            Gold = newTeam.Wealth;
            //Upkeep = 0;
            Characters = new List<INECharacter>();
            Formations = new List<INEFormation>();
            Inventory = new INEInventory();
            SpellBook = new List<INESpell>();
        }
    }
}
