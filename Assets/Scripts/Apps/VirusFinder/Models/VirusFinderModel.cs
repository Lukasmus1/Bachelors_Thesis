using System.Collections.Generic;
using Apps.VirusFinder.Controllers;
using Desktop.Commons;
using Desktop.Models;
using Desktop.Views;
using TMPro;
using UnityEngine;

namespace Apps.VirusFinder.Models
{
    public class VirusFinderModel
    {
        //List of virus GameObjects
        public List<GameObject> Viruses { get; private set; } = new();
        
        //Prefab for the virus icon
        private readonly GameObject _iconPrefab = Resources.Load<GameObject>("Prefabs/VirusIcon");

        //List of possible virus names
        private readonly string[] listOfNames =
        {
            "ą̵̵̵̛̖͔̻͇͚̟̬̯͓̙̞̤̻̏ͦ̀́̔͐̉̀̇̍ͨ̏͌̾̓ͩ̚̚̕͜͠",
            "d̸̷̡̲͍̝̼̫͉̻͍͚̗͉̆̐̆ͧ̆́͛̆ͮͯ͑̈́̕̕͡", 
            "s̨̛̘̲̝̟̉̓̆̈ͮͬ͗̋̓́̕ͅ_̸̶̵̣͚̫͚̫͎̲̺̱ͥ͊ͩ̍͋̿͆̈̀͌̐͗͜",
            "f̞̰̼͕̭̭ͣ͑ͮ̚",
            "p͍̰͛ͅ_̰̙̳̮͍̰͈͖̝͕̹̜͙̣̓ͭ̀̌̀ͧ́̒͂̊͋ͯ̕͢͝",
            "e̴̦̳̠͖͖͙̤̰̠̖̳͂͐̊͊̄ͧͩ̾͒̃͝͞",
            "q̮_͍̈́ͬ̑̋_̶̶̷͓͔̥̖̱̲̠͚͕̜̈́͛ͮͤ͆̔͒ͪͪͫ͜ͅ",
            "c̸̺͍̗̫͈̈ͤ̊ͭͬͧͭ̉ͯ͘͜͢_̸̨̤̥͙͙͉̮͔̼͎̼̜̠̺̬͛̎͑̓̆͊͑ͮ̕͠͞",
            "y̷̶̷̸̧̺̳̒́̄ͤ͢͝ͅ"
        };
        
        /// <inheritdoc cref="VirusFinderController.CreateRandomViruses"/>
        public void CreateRandomViruses(int maxCount)
        {
            for (int i = 0; i < Random.Range(0, maxCount + 1); i++)
            {
                GameObject newVirus = Object.Instantiate(_iconPrefab);
                
                string name = listOfNames[Random.Range(0, listOfNames.Length)];
                Vector2 pos = DesktopMvc.Instance.DesktopGeneratorController.GenerateRandomIconPosition(_iconPrefab.GetComponent<RectTransform>().sizeDelta);

                newVirus.GetComponent<IconScript>().SetProperties(pos, name);
                
                Viruses.Add(newVirus);
            }
        }
        
        /// <inheritdoc cref="VirusFinderController.FindViruses"/>
        public void EnableViruses(Transform virusParent)
        {
            foreach (GameObject virus in Viruses)
            {
                virus.transform.SetParent(virusParent);
                virus.SetActive(true);
            }
        }
    }
}