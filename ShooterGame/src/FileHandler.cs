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
                Console.WriteLine(file);
                textureMap[file] = texture;
               // Console.WriteLine(file);
            }
        }

        public LTexture getTexture(string title)
        {
           // Console.WriteLine(textureMap.Count);
            return textureMap["imgs/" + title + ".png"];
        }

        public bool getStatus()
        {
            return success;
        }

    }

    


}
