using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Desktop.Models;
using UnityEngine;

namespace Saving.Models
{
    public class SaveLogic
    {
        private readonly string _path = Application.persistentDataPath + "/savefile.sav";
        
        private readonly SaveModel _model = new();
        
        public SaveLogic()
        {
            _model.desktop = DesktopModel.Instance;
        }
        
        public void SaveGame()
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(_path, FileMode.Create);
            formatter.Serialize(stream, _model);
            stream.Close();
        }

        public bool LoadGame()
        {
            if (!File.Exists(_path))
            {
                return false;
            }
            
            BinaryFormatter formatter = new();
            FileStream stream = new(_path, FileMode.Open);
            var data = formatter.Deserialize(stream) as SaveModel;
            stream.Close();

            if (data == null)
            {
                throw new Exception("Failed to load save data.");
            }
            
            _model.LoadDataFromModel(data);
            return true;
        }
    }
}