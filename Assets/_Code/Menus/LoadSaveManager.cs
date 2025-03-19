using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System;

public class LoadSaveManager : MonoBehaviour
{

    // Save game data
    [XmlRoot("GameData")]
    public class GameStateData
    {
        public struct DataTransform
        {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float scaleX;
            public float scaleY;
            public float scaleZ;
        }

        // Data for enemy
        public class DataEnemy
        {
            //Enemy Transform Data
            public DataTransform posRotScale;

            //Enemy ID
            public int enemyID;

            //Health
            public int health;
        }

        // Data for player
        public class DataPlayer
        {
            //Transform Data
            public DataTransform posRotScale;

            //Collected cash
            public float collectedCash;

            //Has Collected Gun 01?
            public bool collectedWeapon;

            //Health
            public int health;
        }

        // Instance variables
        public List<DataEnemy> enemies = new List<DataEnemy>();
        public DataPlayer player = new DataPlayer();
    }

    // Game data to save/load
    public GameStateData gameState = new GameStateData();


    public void Save(string fileName = "GameData.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, gameState);
            string gameDataXml = stringWriter.ToString();

            using (Aes aes = Aes.Create())
            {
                byte[] key = new byte[16]
                {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                };
                aes.Key = key;
                aes.GenerateIV();

                byte[] iv = aes.IV;

                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    // By default, the StreamWriter uses UTF-8 encoding.
                    // To change the text encoding, pass the desired encoding as the second parameter.
                    // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                    fileStream.Write(iv, 0, iv.Length);

                    // Encrypt and write the game data XML
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new StreamWriter(cryptoStream))
                        {
                            encryptWriter.Write(gameDataXml);
                        }
                    }
                }
            }

            Debug.Log("The file was encrypted and saved.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"The encryption failed. {ex}");
        }
    }


    public void Load(string fileName = "GameData.xml")
    {
        try
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] iv = new byte[16];
                fileStream.Read(iv, 0, iv.Length);

                using (Aes aes = Aes.Create())
                {
                    byte[] key = new byte[16]
                    {
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };
                    aes.Key = key;
                    aes.IV = iv; // Set the IV used during encryption

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new StreamReader(cryptoStream))
                        {
                            string decryptedXml = decryptReader.ReadToEnd();

                            XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
                            StringReader stringReader = new StringReader(decryptedXml);
                            gameState = (GameStateData)serializer.Deserialize(stringReader);
                        }
                    }
                }
            }

            Debug.Log("Game data loaded and decrypted.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Decryption or loading failed. {ex}");
        }
    }
}