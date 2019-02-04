using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IncarnationEngine
{
    public class INEFormation
    {
        string Name;
        List<INECharacterLayout> CharacterLayouts;
    }

    public class INECharacterLayout
    {
        INECharacter Character;
        INELayout Layout;
        Vector2 Posistion;
    }
}
