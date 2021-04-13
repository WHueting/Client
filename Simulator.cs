using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Client
{
    //Class Simulator represent the simulator for the simulation of commits
    class Simulator
    {
        TCPClient tcpclient;
        UI ui;

        List<String> projecten;

        string currentUser;
        string locationRepo;

        //Constructor for simulator
        public Simulator()
        {

        }

        // Initialises the simulator by initialising the UI controller and TCPClient
        public void Init()
        {
            int port_no = Int32.Parse(ConfigurationManager.AppSettings.Get("port_no"));
            string server_ip = ConfigurationManager.AppSettings.Get("server_ip");

            SetCurrentUser("User1");
            locationRepo = ConfigurationManager.AppSettings.Get("repofolder");
            projecten = new List<String>();
            ui = new UI();
            ui.Init();
            tcpclient = new TCPClient(port_no, server_ip);
        }

        // Method for setting the currentUser to the given value
        public void SetCurrentUser(string user)
        {
            currentUser = user;
        }

        // Main method for executing simulation of commit
        public void Simulate()
        {
            // As long as simulate is true, continue simulation
            Boolean simulate = true;
            while (simulate == true)
            {
                // Get next command from host
                string command = tcpclient.SendReady();

                if (command.StartsWith("ChangeWorkspace_"))
                {
                    string user = command.Remove(0, 16);
                    ui.ChangeWorkspace(user);
                    SetCurrentUser(user);

                }

                if (command.StartsWith("Merge_"))
                {
                    string users = command.Remove(0, 6);
                    ui.Merge(users, currentUser);
                }

                if (command.StartsWith("Branch_"))
                {
                    string users = command.Remove(0, 7);
                    string oudUser = users.Split('_')[0];
                    string newUser = users.Split('_')[1];

                    ui.Branch(oudUser, newUser);
                    SetCurrentUser(newUser);
                }

                if (command.StartsWith("Commit_"))
                {
                    // Get the contents of commit for parsing
                    string commithash = command.Remove(0, 7);
                    string[] commitcontent = GetCommit();

                    Commit commit = new Commit(commithash);

                    // Parses contents into commit object
                    commit.ProcesCommitContent(commitcontent);

                    List<Diff> waitList = new List<Diff>();

                    //Processes all diffs as instructions for UI controller
                    foreach (Diff diff in commit.DiffList)
                    {
                        if (!String.IsNullOrEmpty(diff.projectnaam) && !projecten.Exists(x => x.Equals(diff.projectnaam)))
                        {
                            ui.CreateProject(diff.projectnaam);
                            ui.ShareProject(diff.projectnaam, diff.subdirectory, currentUser);
                            projecten.Add(diff.projectnaam);
                        }

                        if (diff.action.Equals("renameFile"))
                        {
                            ui.Rename(diff);
                        }

                        if (diff.action.Equals("copyFile"))
                        {
                            waitList.Add(diff);
                        }
                        else

                        if (diff.action.Equals("createFile"))
                        {
                            ui.Create(diff);
                        }
                        else

                        if (diff.action.Equals("deleteFile"))
                        {
                            ui.Delete(diff);
                        }
                        else

                        if (diff.action.Equals("") && !diff.oldfile.Equals(""))
                        {
                            ui.Change(diff);

                        }
                    }
                    //Gets all non Java files and places them in correct directory
                    foreach (Diff wait in waitList)
                    {
                        GetFile(wait.newfile);
                    }

                    ui.CommitToRepo(commit.Message, currentUser);


                }

                if (command.StartsWith("Stop_"))
                {
                    ui.Stop();
                    simulate = false;
                }
            }

            Console.WriteLine("Repository ready");
        }

        // Method for placing non-Java files in correct directory. 
        // Param newpath is path of the directory the file has to be placed.
        public void GetFile(string newpath)
        {

            using (Stream stream = tcpclient.getFile(newpath))
            {
                if (newpath.StartsWith("src"))
                {
                    newpath = "defaultProject/" + newpath;
                }

                string path = locationRepo + "/" + currentUser + "/" + newpath;

                DirectoryInfo di = new DirectoryInfo(locationRepo + "/" + currentUser + "/" + String.Join("/", newpath.Split('/').SkipLast(1).ToArray()));
                if (di.Exists)
                {
                    //Directory exist so file can be placed
                    using (Stream s = File.Create(path))
                    {
                        stream.CopyTo(s);
                    }
                }
                else
                {
                    //Directory does not exist, a directory is created before file can be placed
                    di.Create();

                    using (Stream s = File.Create(path))
                    {
                        stream.CopyTo(s);
                    }
                }
            }
        }

        //Method for getting the contents of the current commit
        //return string[]
        public string[] GetCommit()
        {
            List<String> commitlines = new List<String>();

            using (Stream stream = tcpclient.getCommit())
            {
                StreamReader sr = new StreamReader(stream);
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    commitlines.Add(line);
                }
            }

            return commitlines.ToArray();
        }

    }
}
