using System;
using UnityEngine;

namespace IncarnationEngine
{
    [Serializable] public class INEInterface
    {
        [SerializeField] public GameObject Header;
        [SerializeField] public GameObject LoginPanel;
        [SerializeField] public GameObject TestData;
        [SerializeField] public TeamListUI TeamList;
        [SerializeField] public CharacterBuildUI CharacterBuildPanel;
    }
}
