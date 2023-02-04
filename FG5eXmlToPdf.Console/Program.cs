using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FG5eXmlToPDF;
using FG5eXmlToPDF.Models;
using static System.Console;

namespace FG5eXmlToPdf.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args is null || args?.Length == 0)
            {
                WriteLine("Requires arg with path to fantasy grounds character xml");
            }
            else
            {
                foreach (string arg in args)
                {
                    if (File.Exists(arg) && arg.EndsWith(".xml"))
                    {
                        try
                        {
                            string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                            List<ICharacter> characters = FG5eXml.LoadCharacters(arg);
                            if (characters.Count() == 0)
                                WriteLine($"No characters found!");

                            foreach (ICharacter character in characters)
                            {
                                string? charName = character.Properities.FirstOrDefault((x) => x.Name == "Name")?.Value;
                                string? level = character.Properities.FirstOrDefault((x) => x.Name == "LevelTotal")?.Value;
                                string outFile = $@"{currentDirectory}{Path.DirectorySeparatorChar}{charName} ({level}).pdf";
                                if (charName is null)
                                    throw new Exception($"Character name was not found in {arg}");
                                if (level is null)
                                    WriteLine($"Character level was not found in {arg}");

                                if (!File.Exists(outFile))
                                {
                                    FileStream fs = File.Create(outFile);
                                    fs.Close();
                                }
                                FG5ePdf.Write(character, outFile);
                                WriteLine($"Wrote: {outFile}");
                            }
                        }
                        catch (Exception e)
                        {
                            WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        WriteLine("Can't find the file");
                    }
                }
            }
        }
    }
}
