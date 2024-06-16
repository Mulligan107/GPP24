using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame
{
    class FileHandler
    {
        private bool success = true;

        Dictionary<string,LTexture> textureMap = new Dictionary<string,LTexture>(); // Dictionary == HashMap, zum Mapping der Texturen

        public FileHandler()
        {
            loadFiles();
        }

        public void loadFiles() // NUR PNG!
        {
            string[] files = Directory.GetFiles("imgs/"); // Alle Files im Ordner Imgs werden gesucht

            foreach (string file in files) //Jede File wird an eine Texture gebunden
            {
                LTexture texture = new LTexture();
                if (!texture.loadFromFile(file)) // Abfrage ob die File geladen werden konnte
                {
                    Console.WriteLine("Failed to load!");
                    success = false;
                    break;
                }
                textureMap[file] = texture;
               // Console.WriteLine(file);
            }
        }
        public List<LTexture> getFighter()
        {
            List<String> list = new List<String>();
            list.Add("Fighterrand");
            list.Add("Ray_spawn");
            list.Add("Fighter_death");
            list.Add("Fighter_shield");
            list.Add("Fighter_bullet");
            return getTextureList(list);
        }
        public LTexture getTexture(string title)
        {
           // Console.WriteLine(textureMap.Count);
            return textureMap["imgs/" + title + ".png"];
        }

        public List<LTexture> getTextureList(List<String> texureNames)
        {
            List<LTexture> textureList = new List<LTexture>();
            foreach (String texureName in texureNames)
            {
                textureList.Add(getTexture(texureName));
            }
            return textureList;
        }

        public bool getStatus()
        {
            return success;
        }

    }

    


}
