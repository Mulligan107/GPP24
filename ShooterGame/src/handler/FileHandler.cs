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
            string[] files = Directory.GetFiles("imgs/", ".",SearchOption.AllDirectories); // Alle Files im Ordner Imgs werden gesucht

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
                Console.WriteLine(file);
            }
        }
        
        public List<LTexture> getFighter()
        {
            List<String> list = new List<String>();
            list.Add("Fighter\\Fighterrand");
            list.Add("Fighter\\Ray_spawn");
            list.Add("Fighter\\Fighter_death");
            list.Add("Fighter\\Fighter_shield");
            list.Add("Fighter\\Fighter_bullet");
            return getTextureList(list);
        }

        public List<LTexture> getScout()
        {
            List<String> list = new List<String>();
            list.Add("Scout\\Scout");
            list.Add("Scout\\Scout_death");
            return getTextureList(list);
        }

        public List<LTexture> getDread()
        {
            List<String> list = new List<String>();
            list.Add("Dread\\Dread");
            list.Add("Dread\\Dread_shield");
            list.Add("Dread\\Dread_death");
            list.Add("Dread\\Dread_bullet");
            list.Add("Dread\\Ray");
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
