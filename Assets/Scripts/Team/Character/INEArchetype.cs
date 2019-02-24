using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IncarnationEngine
{
    public class INEArchetype
    {
        public readonly string Name;
        public readonly List<int> AttributeIDs;
        public readonly List<int> SkillIDs;
        public readonly List<int> PerkIDs;

        public INEArchetype()
        {
            Name = "Default Archetype";
            AttributeIDs = new List<int>() { 0, 1, 2, 3, 4, 5 };
            SkillIDs = new List<int>() { 0, 1, 2, 3, 4, 5 };
            PerkIDs = new List<int>();
        }

        public INEArchetype( string name, List<int> attributeIDs, List<int> skillIDs )
        {
            Name = name;
            AttributeIDs = attributeIDs ?? new List<int>() { 0, 1, 2, 3, 4, 5 };
            SkillIDs = skillIDs ?? new List<int>() { 0, 1, 2, 3, 4, 5 };
            PerkIDs = new List<int>();
        }

    }
}
