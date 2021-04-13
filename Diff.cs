using System;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    //Class Diff represents a diff chunk
    class Diff
    {
        public List<(int, int)> RegelsNieuwList;
        public List<List<string>> chunks;
        public List<String> chunk;
        public string renameTo;
        public string renameFrom;
        public string bestandsnaam;
        public string sourcemap;
        public string projectnaam;
        public string subdirectory;
        public string action;
        public string oldfile;
        public string newfile;
        public string package;
        public (int, int) regelsOud;
        public (int, int) regelsNieuw;
        public Boolean execute;

        //Constructor for diff
        public Diff()
        {
            chunks = new List<List<String>>();
            chunk = new List<String>();
            RegelsNieuwList = new List<(int, int)>();
            renameFrom = "";
            renameTo = "";
            action = "";
            oldfile = "";
            newfile = "";
            package = "";
            execute = false;
            regelsOud = (0, 0);
            regelsOud = (0, 0);
        }

        //Methof for setting the metadata of file in diff e.g. sourcemap, projectnaam, bestandsnaam, subdirectory
        //param line; string containing metadata of Java file
        public void SetJavaMeta(string line) 
        {
            string[] onderdelen = line.Split(" ");
            string bestandspad = onderdelen[2];

            List<string> bestandspadonderdelen = bestandspad.Split('/').ToList();
            bestandspadonderdelen.RemoveAt(0);
            string bestand = bestandspadonderdelen[^1].Replace(' ', '_');
            string project;
            string directory = "";
            int srcIndex = -1;
            int javaIndex = 0;

            if (bestandspadonderdelen.Exists(x => x.Equals("src")))
            {
                srcIndex = bestandspadonderdelen.LastIndexOf("src");
                if (srcIndex == 0)
                {
                    project = "defaultProject";
                }
                else
                {
                    project = bestandspadonderdelen[srcIndex - 1];
                    if (srcIndex > 1)
                    {
                        for (int i = 0; i <= srcIndex - 2; i++)
                        {
                            directory = "/" + bestandspadonderdelen[i];
                        }
                    }
                }
            }
            else
            {
                project = "defaultProject";
            }

            string srcmap = "src";
            if (bestandspadonderdelen.Exists(x => x.Equals("java")))
            {
                javaIndex = bestandspadonderdelen.LastIndexOf("java");
                if (srcIndex < javaIndex)
                {
                    for (int i = srcIndex + 1; i <= javaIndex; i++)
                    {

                        srcmap = srcmap + "/" + bestandspadonderdelen[i];

                    }
                }
            }

            this.sourcemap = srcmap;
            this.projectnaam = project;
            this.bestandsnaam = bestand;
            this.subdirectory = directory;
        }

        // Method for setting the execute attribute
        // param exec
        public void setExecute(Boolean exec) 
        {
            this.execute = exec;
        }

        // Method for setting the renameTo attribute
        // param line; string containing renameTo value 
        public void setRenameTo(string line) 
        {
            string[] rename_to = line.Split(' ');
            string name = String.Join(' ', rename_to, 2, rename_to.Length - 2);
            this.renameTo = name;
        }

        // Method for setting the renameFrom attribute
        // param line; string containing renmaeFrom value
        public void setRenameFrom(string line)
        {
            string[] rename_from = line.Split(' ');
            string name = String.Join(' ', rename_from, 2, rename_from.Length - 2);
            this.renameFrom = name;
        }

        // Method for setting the action attribute
        // param line; string containing action value
        public void setAction(string action)
        {
            this.action = action;
        }

        // Method for setting the path of the old file
        // param line; string containing path of old file
        public void setOldFile(string line)
        {
            string locatie = line.Remove(0, 6);
            this.oldfile = locatie;
        }

        // Method for setting the path of the new file
        // param line; string containing path of new file
        public void setNewFile(string line)
        {
            string locatie = line.Remove(0, 6);
            this.newfile = locatie;
        }

        // Method for setting the package of Java file
        // param line; string containg package
        public void setPackage(string line)
        {
            string[] packagepad = line.Split(' ');
            this.package = packagepad[packagepad.Length - 1].Trim(new Char[] { ';' });
        }

        // Method for setting the line numbers of changed lines in files
        // string line; string containing a possibly changed line
        public void setRegelDelta(string line)
        {
            int beginOud;
            int aantalOud;
            int beginNieuw;
            int aantalNieuw;

            line = line.Trim(new Char[] { ' ', '@' });

            string[] regels = line.Split(' ');

            foreach (var regel in regels)
            {
                if (regel.StartsWith(@"-"))
                {
                    string[] getallen = regel.Substring(1).Split(',');
                    if (getallen.Length == 2)
                    {
                        beginOud = Int16.Parse(getallen[0]);
                        aantalOud = Int16.Parse(getallen[1]);
                        regelsOud = (beginOud, aantalOud);
                    }
                    else
                    {
                        beginOud = Int16.Parse(getallen[0]);
                        aantalOud = Int16.Parse(getallen[0]);
                        regelsOud = (beginOud, aantalOud);
                    }
                }

                if (regel.StartsWith(@"+"))
                {
                    string[] getallen = regel.Substring(1).Split(',');

                    if (getallen.Length == 2)
                    {
                        beginNieuw = Int16.Parse(getallen[0]);
                        aantalNieuw = Int16.Parse(getallen[1]);
                        regelsNieuw = (beginNieuw, aantalNieuw);
                    }
                    else
                    {
                        beginNieuw = Int16.Parse(getallen[0]);
                        aantalNieuw = Int16.Parse(getallen[0]);
                        regelsNieuw = (beginNieuw, aantalNieuw);
                    }


                    RegelsNieuwList.Add((beginNieuw, aantalNieuw));

                }
            }

        }

        // Method for adding a chunk to the list of chunks
        // param chunks; list of string containing possible changed lines
        public void addLines(List<String> chunk)
        {
            chunks.Add(chunk);
        }

        // Method for adding a line to a chunk
        // param line; string containing possibly changed line
        public void addLine(string line)
        {
            chunk.Add(line);
        }

        // Method for adding the chunk to the list of chunks and instatiating a new chunk 
        public void Save()
        {
            List<String> tempStrings = chunk;
            chunks.Add(tempStrings);
            chunk = new List<string>();
        }


    }

}