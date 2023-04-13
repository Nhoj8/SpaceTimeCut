//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Xna.Framework;
//using System.Collections;
//using System.Xml.Serialization;
//using Microsoft.Xna.Framework.Design;
//using System.IO;

//namespace SpaceTimeCut
//{
//    public struct SaveGameData
//    {
//        public string PlayerName;
//        public Vector2 AvatarPosition;
//        public int Level;
//        public int Score;
//    }
//    public class Storage
//    {
//        static string gamePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeEngine");
//        string saveFile = Path.Combine(gamePath, "Saves", "savefile.txt");
//        //StorageDevice device;
//        public void SaveWorld(string WorldName)
//        {
//            // Creates save file and game path
//            //saveFile = Path.Combine(gamePath, "Saves", "savefile.txt");
//            if (!File.Exists(saveFile))
//            {
//                if (!Directory.Exists(saveFile))
//                    //if (!Directory.Exists(Path.Combine(gamePath, "Saves")))
//                    //    if (!Directory.Exists(gamePath))
//                    Directory.CreateDirectory(Path.Combine(gamePath, "Saves"));
//                File.Create(saveFile);
//            }
//            //string worldstring = "";

//            using (var stream = File.Open(saveFile, FileMode.Open))
//            {
//                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
//                //using (FileStream fs = File.OpenWrite(saveFile))
//                {
//                    writer.Write(WorldName);
//                    writer.Write((short)Game1.width);
//                    writer.Write((short)Game1.height);
//                    for (int x = 0; x < Game1.width; x++)
//                        for (int y = 0; y < Game1.height; y++)
//                            //worldstring += Game1.world[x, y].ToString();
//                            writer.Write(Game1.world[x, y]);
//                    //System.Text.Unicode encoding = new System.Text.Unicode();
//                    //byte[] bytes = encoding.GetBytes(inputString);

//                    //Byte[] info =
//                    //    new UTF8Encoding(true).GetBytes((WorldName.Length + 1) + WorldName + worldstring);

//                    //// Add some information to the file.
//                    //fs.Write(info, 0, info.Length);
//                }
//                //var db = new LiteDB.LiteDatabase(saveFile);
//            }
//        }
//        public string FindWorlds()
//        {
//            string WorldName;
//            if (File.Exists(saveFile))
//            {
//                //using (FileStream fs = File.OpenRead(saveFile))
//                //{
//                //    Byte[] info = new Byte[4];
//                //    fs.Read(info, 0, 1);
//                //    int NameLenght = BitConverter.ToInt32(info, 0);
//                //        //Convert.ToInt32(info);
//                //    Byte[] Name = new Byte[NameLenght];
//                //    fs.Read(Name, 2, NameLenght);
//                //    WorldName = Name.ToString();
//                //}

//                using (var stream = File.Open(saveFile, FileMode.Open))
//                {
//                    using (var reader = new BinaryReader(stream, Encoding.UTF8))
//                    {
//                        WorldName = reader.ReadString();
//                    }
//                }
//                return WorldName;
//            }
//            else
//                return "";
//        }
//        public (string, Int16, Int16)ReadWorld()
//        {
//            string WorldName = "";
//            Int16 width = 0, height = 0;
//            if (File.Exists(saveFile))
//            {

//                using (var stream = File.Open(saveFile, FileMode.Open))
//                {
//                    using (var reader = new BinaryReader(stream, Encoding.UTF8))
//                    {

//                        WorldName = reader.ReadString();
//                        width = reader.ReadInt16();
//                        height = reader.ReadInt16();
//                        Game1.world = new Int16[width, height];
//                        for (int x = 0; x < width; x++)
//                            for (int y = 0; y < height; y++)
//                                Game1.world[x, y] = reader.ReadInt16();
//                    }
//                }

//            }
//            return (WorldName, width, height);
//        }
//        public void writeByteArray()
//        {
//            //first byte chooses a format
//           // BitConverter.ToDouble()
//        }

//    }
//}
