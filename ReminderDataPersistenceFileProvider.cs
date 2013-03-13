using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hedgehog.UpdateReminder
{
    public class ReminderDataPersistenceFileProvider<T> where T : class, new()
    {
        public string FilePath { get; set; }

        public T LoadData()
        {
            T data = new T();
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(FilePath))
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(FilePath, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(T));
                        data = xml.Deserialize(stream) as T;
                        stream.Close();
                    }
                }
            }

            return data;
        }

        public void SaveData(T data)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var fileStream = store.CreateFile(FilePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (var writer = new StreamWriter(fileStream))
                {
                    ser.Serialize(writer, data);
                }
            }
        }

        public void EnsureFileExistence(T data)
        {
            var fileExist = false;
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                fileExist = storage.FileExists(FilePath);
            }

            if (!fileExist)
            {
                this.SaveData(data);
            }
        }
    }
}
