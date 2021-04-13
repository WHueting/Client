using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client
{
    //Class representing a commit
    class Commit
    {
        public string Hash { get; set; }
        public List<String> Message;
        public List<Diff> DiffList;

        //Constructor for commit
        //param name is the hash of the commit
        public Commit(string name)
        {
            Hash = name;
            Message = new List<String>();
            DiffList = new List<Diff>();

        }

        //Method for processing the contents of the commit to objects
        public void ProcesCommitContent(string[] content)
        {
            
            List<Diff> Diffs = new List<Diff>();
            Diff huidigeDiff = null;

            string[] lines = content;

            //header is true when no diff has past, indicating the lines is the commit metadata
            bool header = true;
            int count = 0;
            foreach (var line in lines)
            {
                if (count < 4)
                {
                    //Do nothing
                }
                else
                // the lines having these conditions contain the commit message 
                if (count >= 4 && header && !line.StartsWith(@"diff"))
                {
                    string trim = line.Trim();
                    if (!String.IsNullOrEmpty(trim))
                    {
                        Message.Add(trim);
                    }
                }
                else
                //Creates new diff object
                if (line.StartsWith(@"diff"))
                {
                    header = false;
                    if (huidigeDiff != null)
                    {
                        // Not the first diff indicating previous diff has ended, saving and adding the diff to the list
                        huidigeDiff.Save();
                        Diffs.Add(huidigeDiff);
                    }

                    huidigeDiff = new Diff();

                    if (line.EndsWith(@".java"))
                    {
                        //Java class found, indicating diff has to be executed on UI
                        huidigeDiff.setExecute(true);
                        //line is processed as metadata of Java class
                        huidigeDiff.SetJavaMeta(line);


                    }
                    else
                    {
                        //Found file is no Java class
                        huidigeDiff.setExecute(false);
                        string[] woorden = line.Split(' ');
                        string locatie = "+++ " + woorden[^1];
                        huidigeDiff.setNewFile(locatie);
                        huidigeDiff.setAction("copyFile");
                    }
                }
                else
                if (line.StartsWith(@"new file mode"))
                {
                    huidigeDiff.setAction("createFile");
                }
                else
                if (line.StartsWith(@"deleted file mode"))
                {
                    huidigeDiff.setAction("deleteFile");
                }
                else
                if (line.StartsWith(@"similarity index 100%"))
                {
                    huidigeDiff.setAction("similar");
                }
                else
                if (line.StartsWith(@"rename from"))
                {
                    huidigeDiff.setRenameFrom(line);
                    huidigeDiff.setAction("renameFile");
                }
                else
                if (line.StartsWith(@"rename to"))
                {
                    huidigeDiff.setRenameTo(line);
                    huidigeDiff.setAction("renameFile");
                }
                else
                if (line.StartsWith(@"index"))
                {
                    //Do nothing
                }
                else
                if (line.StartsWith(@"---"))
                {
                    huidigeDiff.setOldFile(line);
                }
                else
                if (line.StartsWith(@"+++"))
                {
                    huidigeDiff.setNewFile(line);
                }
                else
                if (line.StartsWith(@"@@"))
                {
                    
                    if ((huidigeDiff.regelsOud == (0, 0)) && (huidigeDiff.regelsNieuw == (0, 0)))
                    {   
                        //First encounter of "@@"
                        huidigeDiff.setRegelDelta(line);

                    }
                    else
                    {
                        //Not first encouter "@@" indicating multiple changes in the same file
                        huidigeDiff.Save();
                        huidigeDiff.setRegelDelta(line);

                    }

                }
                else
                if ((line.StartsWith(@"-") || line.StartsWith(@"+") || line.StartsWith(@" ")) && !header)
                {
                    //lines represent changes of lines in file
                    string pattern = @"^\+\s*package\s.*";
                    if (Regex.IsMatch(line, pattern))
                    {
                        //Package name can be pulled from some Java files.
                        huidigeDiff.setPackage(line);
                    }
                    huidigeDiff.addLine(line);
                }
                count++;
            }
            huidigeDiff.Save();
            Diffs.Add(huidigeDiff);
            DiffList = Diffs;
        }

    }


}