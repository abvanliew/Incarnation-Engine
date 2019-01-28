using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IncarnationEngine
{
    public class APITestUI : MonoBehaviour
    {
        public Dropdown EnvSelect;
        public InputField Path;
        public Button Post;
        public Button Get;
        public InputField BodyJson;
        public InputField Results;

        public void ClickGet()
        {
            INEGet();
        }

        public void ClickPost()
        {
            INEPost();
        }

        private async void INEGet()
        {
            string results = await INE.GetDataAsJson( Path.text );

            Results.text = results;
        }

        private async void INEPost()
        {
            string results = await INE.PostDataAsJson( Path.text, BodyJson.text );

            Results.text = results;
        }
    }
}
