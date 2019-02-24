using System.Collections;
using System.Collections.Generic;

namespace IncarnationEngine
{
    public class INEReferenceData
    {
        public Dictionary<int, INERace> Races { get; private set; }
        public List<INEArchetype> Archetypes { get; private set; }

        public void LoadBaseData()
        {
            LoadRaces();
            LoadArchetypes();
        }

        private void LoadRaces()
        {
            //Add some code that will query my db for this list
            Races = new Dictionary<int, INERace>();

            Races.Add( 0, new INERace( "Human", true, new List<float>() {
                1f,
                1f,
                1f,
                1f,
                1f,
                1.35f } ) );
            Races.Add( 1, new INERace( "Ursaun", true, new List<float>() {
                1.7f,
                .95f,
                1.05f,
                1.75f,
                1.3f,
                .9f } ) );
            Races.Add( 2, new INERace( "Florir", true, new List<float>() {
                .7f,
                1.25f,
                1.15f,
                .8f,
                1.15f,
                .8f } ) );
            Races.Add( 3, new INERace( "Coratid", true, new List<float>() {
                1.45f,
                .85f,
                .95f,
                1.6f,
                .95f,
                1.2f } ) );
            Races.Add( 4, new INERace( "Rhokalan", true, new List<float>() {
                1.6f,
                .75f,
                .85f,
                2f,
                .85f,
                .85f } ) );
            Races.Add( 5, new INERace( "Ankekis", true, new List<float>() {
                1.35f,
                1.15f,
                1.4f,
                1.3f,
                1.35f,
                1f } ) );
            Races.Add( 6, new INERace( "Dtimiak", true, new List<float>() {
                1.15f,
                1.05f,
                1.2f,
                1.05f,
                1.1f,
                1.1f } ) );
        }

        private void LoadArchetypes()
        {
            Archetypes = new List<INEArchetype>() {
                new INEArchetype( 
                    "Magus",
                    new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
                    new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }
                )
            };
        }
    }
}
