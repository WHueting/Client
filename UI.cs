using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Client
{
    // Class UI represents the controller that interacts with the UI of the OS
    class UI
    {
        private string workspaceLocation;
        private string repoLocation;
        private int waittime;

        private Toetsenbord toetsenbord;
        private UIElement eclipseWindow;


        public UI()
        {

        }

        // Method to initialise Eclipse
        public void Init()
        {
            workspaceLocation = ConfigurationManager.AppSettings.Get("workspacelocation");
            repoLocation = ConfigurationManager.AppSettings.Get("repofolder");
            waittime = Int32.Parse(ConfigurationManager.AppSettings.Get("waittime"));

            toetsenbord = new Toetsenbord();
            eclipseWindow = StartEclipse();
            CreateWorkspace("User1");
            CreateRepo("User1");

        }

        // Method to run eclipse
        public UIElement StartEclipse()
        {
            UIElement eclipseSDK;
            UIElement file;
            UIElement eclipse;

            string name = ConfigurationManager.AppSettings.Get("eclipse");

            eclipse = new UIElement(name, ControlType.ListItem);
            eclipse.DoubleLeftClick();

            Thread.Sleep(30000);

            Console.WriteLine("Zoeken naar Eclipse");
            eclipseSDK = new UIElement("Eclipse SDK", ControlType.Window);
            Thread.Sleep(waittime);

            Console.WriteLine("Zoeken naar File");
            file = new UIElement("File", eclipseSDK, ControlType.MenuItem);
            Thread.Sleep(waittime);

            return eclipseSDK;
        }

        // Method to create a new Eclipse workspace
        // param name; name of a user becoming the name of the new workspace
        public void CreateWorkspace(string name)
        {

            UIElement File = new UIElement("File", eclipseWindow, ControlType.MenuItem);
            File.LeftClick();
            Thread.Sleep(waittime);

            UIElement SwitchWorkplace = new UIElement("Switch Workspace", eclipseWindow, ControlType.MenuItem);
            SwitchWorkplace.LeftClick();
            Thread.Sleep(waittime);

            UIElement Other = new UIElement("Other...", eclipseWindow, ControlType.MenuItem);
            Other.Invoke();
            Thread.Sleep(waittime);

            toetsenbord.Typ(workspaceLocation + "\\" + name);
            Thread.Sleep(waittime);
            toetsenbord.Enter();

            int count = 0;
            do
            {
                Thread.Sleep(100);
                count++;
            }
            while (eclipseWindow.element != null && count < 100);


            Console.WriteLine("Zoeken naar Eclipse");
            UIElement eclipseSDK = new UIElement("Eclipse SDK", ControlType.Window);
            Thread.Sleep(waittime);

            Console.WriteLine("Zoeken naar File");
            UIElement file = new UIElement("File", eclipseSDK, ControlType.MenuItem);
            Thread.Sleep(waittime);


            UIElement maximaliseren = new UIElement("Maximaliseren", eclipseSDK, ControlType.Button);
            Thread.Sleep(waittime);
            maximaliseren.LeftClick();

            UIElement welcome = new UIElement("Welcome", eclipseSDK, ControlType.TabItem);
            Thread.Sleep(waittime);

            welcome.RightClick();
            Thread.Sleep(waittime);

            UIElement close = new UIElement("Close", ControlType.MenuItem);
            Thread.Sleep(waittime);

            close.LeftClick();
            Thread.Sleep(waittime);

            UIElement Window = new UIElement("Window", eclipseSDK, ControlType.MenuItem);
            Window.LeftClick();
            Thread.Sleep(waittime);

            UIElement Preferences = new UIElement("Preferences", eclipseSDK, ControlType.MenuItem);
            Preferences.LeftClick();
            Thread.Sleep(waittime);

            UIElement PreferencesWindow = new UIElement("Preferences", eclipseSDK, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement PreferencesJava = new UIElement("Java", PreferencesWindow, ControlType.TreeItem);
            Thread.Sleep(waittime);

            PreferencesJava.DoubleLeftClick();
            Thread.Sleep(waittime);

            UIElement Editor = new UIElement("Editor", PreferencesJava, ControlType.TreeItem);
            Thread.Sleep(waittime);

            Editor.DoubleLeftClick();
            Thread.Sleep(waittime);

            UIElement ContentAssist = new UIElement("Content Assist", Editor, ControlType.TreeItem);
            Thread.Sleep(waittime);

            ContentAssist.LeftClick();
            Thread.Sleep(waittime);

            UIElement AutoActivation = new UIElement("Auto Activation", PreferencesWindow, ControlType.Group);
            Thread.Sleep(waittime);

            UIElement EnableAutoActivation = new UIElement("Enable auto activation", AutoActivation, ControlType.CheckBox);
            Thread.Sleep(waittime);

            if (EnableAutoActivation.GetToggleState())
            {
                EnableAutoActivation.LeftClick();
            }

            UIElement Typing = new UIElement("Typing", Editor, ControlType.TreeItem);
            Thread.Sleep(waittime);

            Typing.LeftClick();
            Thread.Sleep(waittime);

            UIElement AutoClose = new UIElement("Automatically close", PreferencesWindow, ControlType.Group);
            Thread.Sleep(waittime);

            foreach (AutomationElement checkbox in AutoClose.element.FindAll(TreeScope.Children, Condition.TrueCondition))
            {
                UIElement uiCheckBox = new UIElement(checkbox);

                if (uiCheckBox.GetToggleState() && uiCheckBox.element.Current.IsEnabled)
                {
                    uiCheckBox.LeftClick();
                }
                Thread.Sleep(waittime);
            }

            UIElement AutoInsert = new UIElement("Automatically insert at correct position", PreferencesWindow, ControlType.Group);
            Thread.Sleep(waittime);

            foreach (AutomationElement checkbox in AutoInsert.element.FindAll(TreeScope.Children, Condition.TrueCondition))
            {
                UIElement uiCheckBox = new UIElement(checkbox);

                if (uiCheckBox.GetToggleState())
                {
                    uiCheckBox.LeftClick();
                }
                Thread.Sleep(waittime);
            }

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            eclipseWindow = eclipseSDK;
        }

        // Method to create a git repository
        // param user; string containing the name of a user becoming the name of the repository
        public void CreateRepo(string user)
        {
            string pad = repoLocation + "/" + user;

            UIElement perspective = new UIElement("Open Perspective", eclipseWindow, ControlType.Button);
            Thread.Sleep(waittime);

            perspective.LeftClick();
            Thread.Sleep(waittime);

            UIElement perspectiveWindow = new UIElement("Open Perspective", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement git = new UIElement("Git", perspectiveWindow, ControlType.ListItem);
            Thread.Sleep(waittime);

            git.Invoke();
            Thread.Sleep(waittime);

            UIElement gitRepositories = new UIElement("Git Repositories", eclipseWindow, ControlType.Tab);
            Thread.Sleep(waittime);

            UIElement createRepo = new UIElement("Create a new Git Repository and add it to this view", gitRepositories, ControlType.Button);
            Thread.Sleep(waittime);

            createRepo.LeftClick();
            Thread.Sleep(waittime);

            UIElement createRepoWindow = new UIElement("Create a Git Repository", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement repositoryDirectory = new UIElement("Repository directory:", createRepoWindow, ControlType.Edit);
            Thread.Sleep(waittime);

            repositoryDirectory.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Typ(pad);
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            UIElement javaPerspective = new UIElement("Java", eclipseWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            javaPerspective.LeftClick();
            Thread.Sleep(waittime);
        }

        // Method to commit a created or changed project
        //param message; list of string containing the commit message
        //param user; string containing user who performs the commit
        public void CommitToRepo(List<String> message, string user)
        {
            UIElement perspective = new UIElement("Open Perspective", eclipseWindow, ControlType.Button);
            Thread.Sleep(waittime);

            perspective.LeftClick();
            Thread.Sleep(waittime);

            UIElement perspectiveWindow = new UIElement("Open Perspective", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement git = new UIElement("Git", perspectiveWindow, ControlType.ListItem);
            Thread.Sleep(waittime);
            git.Invoke();
            Thread.Sleep(waittime);

            UIElement gitRepositories = new UIElement("Git Repositories", eclipseWindow, ControlType.Tab);
            Thread.Sleep(waittime);

            UIElement repository = new UIElement();
            AutomationElementCollection AEC = gitRepositories.element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem));
            foreach (AutomationElement el in AEC)
            {
                if (el.Current.Name.Contains(user + "\\"))
                {
                    repository = new UIElement(el);
                    break;
                }
            }
            Thread.Sleep(waittime);

            repository.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.F5();
            Thread.Sleep(waittime);

            repository.RightClick();
            Thread.Sleep(waittime);

            UIElement Commit = new UIElement("Commit...", ControlType.MenuItem);
            Thread.Sleep(waittime);

            Commit.LeftClick();
            Thread.Sleep(waittime);

            UIElement gitStaging = new UIElement("Git Staging", eclipseWindow, ControlType.Tab);
            Thread.Sleep(waittime);

            UIElement refresh = new UIElement("Refresh", gitStaging, ControlType.Button);
            Thread.Sleep(waittime);

            refresh.LeftClick();
            Thread.Sleep(waittime);

            UIElement add = new UIElement("Add all files including not selected ones to the index", gitStaging, ControlType.Button);
            Thread.Sleep(waittime);

            add.LeftClick();
            Thread.Sleep(waittime);

            UIElement commitMessage = new UIElement("", gitStaging, ControlType.Edit);
            Thread.Sleep(waittime);

            commitMessage.LeftClick();
            Thread.Sleep(waittime);

            foreach (var messageline in message)
            {
                toetsenbord.Typ(messageline);
                Thread.Sleep(200);
                toetsenbord.Enter();
            }

            Thread.Sleep(waittime);

            UIElement author = new UIElement("Author:", gitStaging, ControlType.Edit);
            Thread.Sleep(waittime);

            author.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Typ(user + " <" + user + "@users.nl>");
            Thread.Sleep(waittime);

            UIElement committer = new UIElement("Committer:", gitStaging, ControlType.Edit);
            Thread.Sleep(waittime);

            committer.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Typ(user + " <" + user + "@users.nl>");
            Thread.Sleep(waittime);

            UIElement commitButton = new UIElement("Commit", gitStaging, ControlType.Button);
            Thread.Sleep(waittime);

            commitButton.LeftClick();
            Thread.Sleep(waittime);

            UIElement javaPerspective = new UIElement("Java", eclipseWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            javaPerspective.Invoke();
            Thread.Sleep(waittime);
        }


        // Method to clone a repository
        // param ouduser; string containing the repository to be cloned
        // param newuser; string containing the repository to clone into
        public void CloneRepo(string ouduser, string newuser)
        {

            string oudpad = repoLocation + "/" + ouduser;
            string newpad = repoLocation + "/" + newuser;

            UIElement perspective = new UIElement("Open Perspective", eclipseWindow, ControlType.Button);
            Thread.Sleep(waittime);
            perspective.LeftClick();
            Thread.Sleep(waittime);

            UIElement perspectiveWindow = new UIElement("Open Perspective", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement git = new UIElement("Git", perspectiveWindow, ControlType.ListItem);
            Thread.Sleep(waittime);

            git.Invoke();
            Thread.Sleep(waittime);

            UIElement gitRepositories = new UIElement("Git Repositories", eclipseWindow, ControlType.Tab);
            Thread.Sleep(waittime);

            UIElement cloneRepo = new UIElement("Clone a Git Repository and add the clone to this view", gitRepositories, ControlType.Button);
            Thread.Sleep(waittime);

            cloneRepo.LeftClick();
            Thread.Sleep(waittime);

            UIElement cloneWindow = new UIElement("Clone Git Repository", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            toetsenbord.Typ("file://" + oudpad);
            Thread.Sleep(waittime);

            UIElement next = new UIElement("&Next  ", cloneWindow, ControlType.Button);
            Thread.Sleep(waittime);

            next.LeftClick();
            Thread.Sleep(waittime);

            next.LeftClick();
            Thread.Sleep(waittime);

            UIElement destination = new UIElement("Destination", cloneWindow, ControlType.Group);
            Thread.Sleep(waittime);

            UIElement destinationedit = new UIElement("", destination, ControlType.Edit);
            Thread.Sleep(waittime);

            destinationedit.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Typ(newpad);
            Thread.Sleep(waittime);

            UIElement import = new UIElement("Import all existing Eclipse projects after clone finishes", cloneWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            if (!import.GetToggleState())
            {
                import.LeftClick();
            }
            Thread.Sleep(waittime);

            UIElement finish = new UIElement("Finish", cloneWindow, ControlType.Button);
            Thread.Sleep(waittime);

            finish.LeftClick();
            Thread.Sleep(waittime);

            UIElement javaPerspective = new UIElement("Java", eclipseWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            javaPerspective.LeftClick();
            Thread.Sleep(waittime);
        }

        
        // Method to change to an existing Eclipse workspace
        // param name; string containing name of workspace
        public void ChangeWorkspace(string name)
        {

            UIElement File = new UIElement("File", eclipseWindow, ControlType.MenuItem);
            File.LeftClick();
            Thread.Sleep(waittime);

            UIElement SwitchWorkplace = new UIElement("Switch Workspace", eclipseWindow, ControlType.MenuItem);
            SwitchWorkplace.LeftClick();
            Thread.Sleep(waittime);

            UIElement Other = new UIElement("Other...", eclipseWindow, ControlType.MenuItem);
            Other.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Typ(workspaceLocation + "\\" + name);
            Thread.Sleep(waittime);
            toetsenbord.Enter();

            int count = 0;
            do
            {
                Thread.Sleep(100);
                count++;
            }
            while (eclipseWindow.element != null && count < 100);


            Console.WriteLine("Zoeken naar Eclipse");
            UIElement eclipseSDK = new UIElement("Eclipse SDK", ControlType.Window);
            Thread.Sleep(waittime);

            Console.WriteLine("Zoeken naar File");
            UIElement file = new UIElement("File", eclipseSDK, ControlType.MenuItem);
            Thread.Sleep(waittime);

            eclipseWindow = eclipseSDK;

        }

        // Method to create a new Java project
        // param name; string containing name of new project
        public void CreateProject(string name)
        {

            UIElement File = new UIElement("File", eclipseWindow, ControlType.MenuItem);
            Thread.Sleep(waittime);

            File.LeftClick();
            Thread.Sleep(waittime);

            UIElement NewProject = new UIElement("New	Alt+Shift+N", eclipseWindow, ControlType.MenuItem);
            NewProject.LeftClick();
            Thread.Sleep(waittime);

            UIElement NewJavaProject = new UIElement("Java Project", eclipseWindow, ControlType.MenuItem);
            NewJavaProject.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Typ(name);
            toetsenbord.Enter();

            UIElement DontCreate = new UIElement("Don't Create", eclipseWindow);
            Thread.Sleep(waittime);
            DontCreate.LeftClick();
            Thread.Sleep(waittime);
        }

        // Method to share a Java project with a Git repository
        // param projectnaam; string containing name of project to be shared
        // param subdirectory; string containing path of java project in repository
        // param user; string containing name of repository to share with
        public void ShareProject(string projectnaam, string subdirectory, string user)
        {

            UIElement packageExplorer = new UIElement("Package Explorer", eclipseWindow, ControlType.Pane);
            UIElement javaproject = new UIElement();
            foreach (AutomationElement project in packageExplorer.element.FindAll(TreeScope.Descendants, Condition.TrueCondition))
            {
                if (project.Current.Name.EndsWith("]"))
                {
                    if (project.Current.Name.StartsWith(projectnaam + " [") || project.Current.Name.StartsWith("> " + projectnaam + " ["))
                    {
                        javaproject = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
                else
                {

                    if (project.Current.Name.Equals(projectnaam) || project.Current.Name.Equals("> " + projectnaam))
                    {
                        javaproject = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
            }

            javaproject.RightClick();
            Thread.Sleep(waittime);

            UIElement context = new UIElement("Context", ControlType.Menu);
            Thread.Sleep(waittime);

            UIElement team = new UIElement("Team", context, ControlType.MenuItem);
            Thread.Sleep(waittime);

            team.LeftClick();
            Thread.Sleep(waittime);

            UIElement share = new UIElement("Share Project...", eclipseWindow, ControlType.MenuItem);
            Thread.Sleep(waittime);

            share.LeftClick();
            Thread.Sleep(waittime);

            UIElement shareWindow = new UIElement("Share Project", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement openen = new UIElement("Openen", shareWindow, ControlType.Button);
            Thread.Sleep(waittime);
            openen.LeftClick();
            Thread.Sleep(waittime);

            UIElement repositoy = new UIElement("Repository:", ControlType.List);
            Thread.Sleep(waittime);

            foreach (AutomationElement repo in repositoy.element.FindAll(TreeScope.Children, Condition.TrueCondition))
            {
                if (repo.Current.Name.StartsWith(user + " "))
                {
                    UIElement item = new UIElement(repo);
                    item.LeftClick();
                    Thread.Sleep(waittime);
                }
            }

            if (!String.IsNullOrEmpty(subdirectory))
            {
                Thread.Sleep(waittime);

                UIElement subdirectoryEdit = new UIElement("Path within repository:", shareWindow, ControlType.Edit);
                Thread.Sleep(waittime);

                subdirectoryEdit.LeftClick();
                Thread.Sleep(waittime);

                toetsenbord.Typ(subdirectory);

            }

            toetsenbord.Enter();
            Thread.Sleep(waittime);
        }


        // Method to change a Java class file
        // param diff; diff containing the information needed to change the file
        public void Change(Diff diff)
        {
            string bestandsnaam;
            string packagepad;

            UIElement packageExplorer = new UIElement("Package Explorer", eclipseWindow, ControlType.Pane);
            UIElement projectnaam = new UIElement();
            foreach (AutomationElement project in packageExplorer.element.FindAll(TreeScope.Descendants, Condition.TrueCondition))
            {
                if (project.Current.Name.EndsWith("]"))
                {
                    if (project.Current.Name.StartsWith(diff.projectnaam + " [") || project.Current.Name.StartsWith("> " + diff.projectnaam + " ["))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
                else
                {

                    if (project.Current.Name.Equals(diff.projectnaam) || project.Current.Name.Equals("> " + diff.projectnaam))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
            }

            projectnaam.DoubleLeftClick();
            Thread.Sleep(waittime);

            UIElement src = new UIElement(diff.sourcemap, "> " + diff.sourcemap, projectnaam, ControlType.TreeItem);
            Thread.Sleep(waittime);

            src.DoubleLeftClick();
            Thread.Sleep(waittime);

            List<String> pad = diff.newfile.Split('/').ToList();
            if (pad.Exists(x => x.Equals("Java") || x.Equals("java")))
            {
                pad.RemoveRange(0, pad.FindLastIndex(x => x.Equals("Java") || x.Equals("java")) + 1);
                bestandsnaam = pad.Last().Replace(' ', '_');
                pad.RemoveAt(pad.Count - 1);
                if (pad.Count >= 1)
                {
                    packagepad = String.Join('.', pad).Replace(' ', '_');
                }
                else
                {
                    packagepad = "(default package)";
                }
            }
            else
            {
                bestandsnaam = pad.Last();
                packagepad = "(default package)";
            }

            UIElement packageelement = new UIElement(packagepad, "> " + packagepad, src, ControlType.TreeItem);
            Thread.Sleep(waittime);
            packageelement.Expand();
            Thread.Sleep(waittime);

            UIElement bestand = new UIElement(bestandsnaam, "> " + bestandsnaam, packageelement, ControlType.TreeItem);
            Thread.Sleep(waittime);
            bestand.DoubleLeftClick();

            UIElement tekstvak = new UIElement(bestandsnaam, eclipseWindow, ControlType.TabItem);
            Thread.Sleep(waittime);

            for (int i = 0; i <= diff.chunks.Count - 1; i++)
            {

                toetsenbord.CtrlL();
                Thread.Sleep(100);
                toetsenbord.Typ(diff.RegelsNieuwList[i].Item1.ToString());
                toetsenbord.Enter();
                Thread.Sleep(1000);

                foreach (string line in diff.chunks[i])
                {
                    if (line.StartsWith("-"))
                    {
                        Thread.Sleep(200);
                        toetsenbord.CtrlD();
                        Thread.Sleep(200);
                    }
                    else
                    if (line.StartsWith("+"))
                    {
                        toetsenbord.Left();
                        Thread.Sleep(200);
                        toetsenbord.Enter();
                        Thread.Sleep(200);
                        toetsenbord.Typ(line.Substring(1).Trim());
                        Thread.Sleep(200);
                        toetsenbord.Right();
                        Thread.Sleep(200);
                    }
                    else
                    {
                        toetsenbord.Down();
                        Thread.Sleep(200);

                    }

                }
            }

            toetsenbord.CtrlS();
            Thread.Sleep(waittime);

            tekstvak.RightClick();
            Thread.Sleep(waittime);

            UIElement context = new UIElement("Context", ControlType.Menu);
            Thread.Sleep(waittime);

            UIElement close = new UIElement("Close", context, ControlType.MenuItem);
            Thread.Sleep(waittime);

            close.LeftClick();
            Thread.Sleep(waittime);

            src.Collapse();
            Thread.Sleep(waittime);

            projectnaam.Collapse();
            Thread.Sleep(waittime);

        }

        // Method to rename a Java class file
        // param diff; diff containing the information needed to rename the file
        public void Rename(Diff diff)
        {
            string oud = diff.renameFrom.Split('/')[diff.renameFrom.Split('/').Length - 1].Replace(' ', '_');
            string nieuw = diff.renameTo.Split('/')[diff.renameTo.Split('/').Length - 1].Replace(' ', '_');
            string packagepad;

            UIElement packageExplorer = new UIElement("Package Explorer", eclipseWindow, ControlType.Pane);
            UIElement projectnaam = new UIElement();

            foreach (AutomationElement project in packageExplorer.element.FindAll(TreeScope.Descendants, Condition.TrueCondition))
            {
                if (project.Current.Name.EndsWith("]"))
                {
                    if (project.Current.Name.StartsWith(diff.projectnaam + " [") || project.Current.Name.StartsWith("> " + diff.projectnaam + " ["))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
                else
                {

                    if (project.Current.Name.Equals(diff.projectnaam) || project.Current.Name.Equals("> " + diff.projectnaam))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
            }

            projectnaam.Expand();
            Thread.Sleep(waittime);

            UIElement src = new UIElement(diff.sourcemap, "> " + diff.sourcemap, projectnaam, ControlType.TreeItem);
            Thread.Sleep(waittime);

            src.Expand();
            Thread.Sleep(waittime);

            List<String> pad = diff.renameFrom.Split('/').ToList();
            if (pad.Count > 1 && (pad.Exists(x => x.Equals("Java") || x.Equals("java"))))
            {
                pad.RemoveRange(0, pad.FindLastIndex(x => x.Equals("Java") || x.Equals("java")) + 1);

                pad.RemoveAt(pad.Count - 1);
                if (pad.Count >= 1)
                {
                    packagepad = String.Join('.', pad).Replace(' ', '_');
                }
                else
                {
                    packagepad = "(default package)";
                }
            }
            else
            {

                packagepad = "(default package)";
            }

            UIElement packageelement = new UIElement(packagepad, "> " + packagepad, src, ControlType.TreeItem);
            Thread.Sleep(waittime);


            packageelement.Expand();
            Thread.Sleep(waittime);

            UIElement bestand = new UIElement(oud, "> " + oud, packageelement, ControlType.TreeItem);
            Thread.Sleep(waittime);

            bestand.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.F2();
            Thread.Sleep(waittime);

            toetsenbord.Typ(nieuw.Substring(0, nieuw.Length - 5));
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            src.Collapse();
            Thread.Sleep(waittime);

            projectnaam.Collapse();
            Thread.Sleep(waittime);
        }

        // Method to delete a Java class file
        // param diff; diff containing the information needed to delete the file
        public void Delete(Diff diff)
        {
            string bestandsnaam;
            string packagepad;

            UIElement packageExplorer = new UIElement("Package Explorer", eclipseWindow, ControlType.Pane);
            UIElement projectnaam = new UIElement();
            foreach (AutomationElement project in packageExplorer.element.FindAll(TreeScope.Descendants, Condition.TrueCondition))
            {
                if (project.Current.Name.EndsWith("]"))
                {
                    if (project.Current.Name.StartsWith(diff.projectnaam + " [") || project.Current.Name.StartsWith("> " + diff.projectnaam + " ["))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
                else
                {

                    if (project.Current.Name.Equals(diff.projectnaam) || project.Current.Name.Equals("> " + diff.projectnaam))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
            }

            projectnaam.Expand();
            Thread.Sleep(waittime);


            UIElement src = new UIElement("src", "> src", eclipseWindow, ControlType.TreeItem);
            Thread.Sleep(waittime);

            src.Expand();
            Thread.Sleep(waittime);

            List<String> pad = diff.oldfile.Split('/').ToList();
            if (pad.Exists(x => x.Equals("Java") || x.Equals("java")))
            {
                pad.RemoveRange(0, pad.FindLastIndex(x => x.Equals("Java") || x.Equals("java")) + 1);
                bestandsnaam = pad.Last().Replace(' ', '_');
                pad.RemoveAt(pad.Count - 1);
                if (pad.Count >= 1)
                {
                    packagepad = String.Join('.', pad).Replace(' ', '_');
                }
                else
                {
                    packagepad = "(default package)";
                }
            }
            else
            {
                bestandsnaam = pad.Last();
                packagepad = "(default package)";
            }

            UIElement packageelement = new UIElement(packagepad, "> " + packagepad, src, ControlType.TreeItem);
            Thread.Sleep(waittime);
            packageelement.Expand();
            Thread.Sleep(waittime);


            UIElement bestand = new UIElement(bestandsnaam, "> " + bestandsnaam, packageelement, ControlType.TreeItem);
            Thread.Sleep(waittime);

            bestand.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);
        }

        // Method to create a Java class file
        // param diff; diff containing the information needed to create the file
        public void Create(Diff diff)
        {
            UIElement packageExplorer = new UIElement("Package Explorer", eclipseWindow, ControlType.Pane);
            UIElement projectnaam = new UIElement();
            foreach (AutomationElement project in packageExplorer.element.FindAll(TreeScope.Descendants, Condition.TrueCondition))
            {
                if (project.Current.Name.EndsWith("]"))
                {
                    if (project.Current.Name.StartsWith(diff.projectnaam + " [") || project.Current.Name.StartsWith("> " + diff.projectnaam + " ["))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
                else
                {

                    if (project.Current.Name.Equals(diff.projectnaam) || project.Current.Name.Equals("> " + diff.projectnaam))
                    {
                        projectnaam = new UIElement(project);
                        Thread.Sleep(waittime);
                    }
                }
            }

            projectnaam.DoubleLeftClick();
            Thread.Sleep(waittime);

            UIElement src = new UIElement();
            foreach (AutomationElement srcmap in projectnaam.element.FindAll(TreeScope.Children, Condition.TrueCondition))
            {
                if (srcmap.Current.Name.Equals(diff.sourcemap) || srcmap.Current.Name.Equals("> " + diff.sourcemap))
                {
                    src = new UIElement(srcmap);
                    Thread.Sleep(waittime);
                }
            }

            if (src.element == null)
            {
                NewSourceFolder(diff.sourcemap, projectnaam);
                Thread.Sleep(waittime);
                src = new UIElement(diff.sourcemap, "> " + diff.sourcemap, projectnaam, ControlType.TreeItem);
                Thread.Sleep(waittime);

            }

            src.LeftClick();
            Thread.Sleep(waittime);

            UIElement file = new UIElement("File", eclipseWindow, ControlType.MenuItem);
            file.LeftClick();
            Thread.Sleep(waittime);

            UIElement newProject = new UIElement("New	Alt+Shift+N", eclipseWindow, ControlType.MenuItem);
            newProject.LeftClick();
            Thread.Sleep(waittime);

            UIElement newJavaProject = new UIElement("Class", eclipseWindow);
            newJavaProject.LeftClick();
            Thread.Sleep(waittime);

            UIElement newClassWindow = new UIElement("New Java Class", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement packageEdit = new UIElement("Package:", newClassWindow, ControlType.Edit);
            packageEdit.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Typ(diff.package);
            Thread.Sleep(waittime);

            UIElement nameEdit = new UIElement("Name:", newClassWindow, ControlType.Edit);
            nameEdit.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Typ(diff.bestandsnaam.Remove(diff.bestandsnaam.Length - 5, 5));
            toetsenbord.Enter();

            UIElement textvak = new UIElement(diff.bestandsnaam, eclipseWindow, ControlType.TabItem);
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            foreach (string line in diff.chunks[0])
            {
                string regel = line.Substring(1).Trim();

                toetsenbord.Typ(regel);
                toetsenbord.Enter();
            }
            Thread.Sleep(waittime);

            toetsenbord.CtrlS();
            Thread.Sleep(waittime);

            textvak.RightClick();
            Thread.Sleep(waittime);

            UIElement context = new UIElement("Context", ControlType.Menu);
            Thread.Sleep(waittime);

            UIElement close = new UIElement("Close", context, ControlType.MenuItem);
            Thread.Sleep(waittime);

            close.LeftClick();
            Thread.Sleep(waittime);

            src.Collapse();
            Thread.Sleep(waittime);

            projectnaam.Collapse();
            Thread.Sleep(waittime);


        }

        // Method to create new source folder
        // param source; string containing name of new source
        // param project; UIElement of project to create the source folder in
        public void NewSourceFolder(string source, UIElement project)
        {
            project.LeftClick();
            Thread.Sleep(waittime);

            UIElement file = new UIElement("File", eclipseWindow);
            Thread.Sleep(waittime);

            file.Expand();
            Thread.Sleep(waittime);

            UIElement newProject = new UIElement("New	Alt+Shift+N", eclipseWindow, ControlType.MenuItem);
            Thread.Sleep(waittime);

            newProject.LeftClick();
            Thread.Sleep(waittime);

            UIElement newJavaProject = new UIElement("Source Folder", eclipseWindow);
            Thread.Sleep(waittime);

            newJavaProject.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            toetsenbord.Delete();
            toetsenbord.Typ(source);
            Thread.Sleep(waittime);

            UIElement sourceFolderWindow = new UIElement("New Source Folder", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement checkbox1 = new UIElement("Update exclusion filters in other source folders to solve nesting", sourceFolderWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            if (!checkbox1.GetToggleState())
            {
                checkbox1.LeftClick();
            }
            Thread.Sleep(waittime);

            UIElement checkbox2 = new UIElement("Ignore optional compile problems", sourceFolderWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            if (!checkbox2.GetToggleState())
            {
                checkbox2.LeftClick();
            }
            Thread.Sleep(waittime);

            toetsenbord.Enter();

            Thread.Sleep(waittime);
        }

        // Method to branch
        // param oldUser; string containing repo to branch of
        // param newUser; string containing name of new repo/branch
        public void Branch(string oldUser, string newUser)
        {

            CreateWorkspace(newUser);
            CloneRepo(oldUser, newUser);
        }

        // Method to merge two branches
        // param name; string containing the name of repositories to be merged
        // param user; string containing name of current user
        public void Merge(string name, string user)
        {
            string[] onderdelen = name.Split("_");
            string ouder1 = onderdelen[0];
            string ouder2 = onderdelen[1];
            string currentUser = user;

            UIElement perspective = new UIElement("Open Perspective", eclipseWindow, ControlType.Button);
            Thread.Sleep(waittime);

            perspective.LeftClick();
            Thread.Sleep(waittime);

            UIElement perspectiveWindow = new UIElement("Open Perspective", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement git = new UIElement("Git", perspectiveWindow, ControlType.ListItem);
            Thread.Sleep(waittime);

            git.Invoke();
            Thread.Sleep(waittime);

            UIElement gitRepositories = new UIElement("Git Repositories", eclipseWindow, ControlType.Tab);
            Thread.Sleep(waittime);

            UIElement repository = new UIElement();

            AutomationElementCollection AEC = gitRepositories.element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem));
            foreach (AutomationElement el in AEC)
            {
                if (el.Current.Name.Contains(currentUser + "\\"))
                {
                    repository = new UIElement(el);
                    break;
                }
            }

            repository.DoubleLeftClick();
            Thread.Sleep(waittime);

            UIElement remotes = new UIElement("Remotes", repository, ControlType.TreeItem);
            Thread.Sleep(waittime);

            string temergen = null;


            if (ouder1.Equals("Huidig") || ouder2.Equals("Huidig"))
            {
                if (ouder1.Equals("Huidig"))
                {
                    temergen = ouder2;
                }
                else
                {
                    temergen = ouder1;
                }
            }
            else
            {

                remotes.DoubleLeftClick();
                Thread.Sleep(waittime);

                UIElement origin = new UIElement("origin", remotes, ControlType.TreeItem);
                Thread.Sleep(waittime);

                origin.DoubleLeftClick();
                Thread.Sleep(waittime);

                AutomationElementCollection origins = gitRepositories.element.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem));
                foreach (AutomationElement el in origins)
                {
                    if (el.Current.Name.EndsWith(ouder1))
                    {
                        temergen = ouder2;
                    }
                    else

                    if (el.Current.Name.Contains(ouder2))
                    {
                        temergen = ouder1;
                    }
                    else

                        Console.WriteLine("Current element in origins not matched");

                }
            }

            Thread.Sleep(waittime);

            remotes.RightClick();
            Thread.Sleep(waittime);

            UIElement context = new UIElement("Context", ControlType.Menu);
            Thread.Sleep(waittime);

            UIElement createRemote = new UIElement("Create Remote...", context, ControlType.MenuItem);
            Thread.Sleep(waittime);

            createRemote.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Typ("origin" + temergen);
            Thread.Sleep(waittime);

            UIElement remoteWindow = new UIElement("New Remote", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement configureFetch = new UIElement("Configure fetch", remoteWindow, ControlType.RadioButton);
            Thread.Sleep(waittime);

            TogglePattern togglePattern = configureFetch.element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            if (!configureFetch.GetToggleState())
            {
                configureFetch.LeftClick();
            }
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            toetsenbord.Typ("file://" + ConfigurationManager.AppSettings.Get("repofolder") + "/" + temergen);
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            UIElement fetchWindow = new UIElement("Configure Fetch", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement saveAndFetch = new UIElement("Save and Fetch", fetchWindow, ControlType.Button);
            Thread.Sleep(waittime);

            saveAndFetch.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);

            repository.RightClick();
            Thread.Sleep(waittime);

            context = new UIElement("Context", ControlType.Menu);
            Thread.Sleep(waittime);

            UIElement merge = new UIElement("Merge...", context, ControlType.MenuItem);
            Thread.Sleep(waittime);

            merge.LeftClick();
            Thread.Sleep(waittime);

            UIElement identify = new UIElement("Identify Yourself", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement fullName = new UIElement("Full name:", identify, ControlType.Edit);
            Thread.Sleep(waittime);

            fullName.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Typ(currentUser);
            Thread.Sleep(waittime);

            UIElement email = new UIElement("E-mail:", identify, ControlType.Edit);
            Thread.Sleep(waittime);

            email.LeftClick();
            Thread.Sleep(waittime);

            toetsenbord.CtrlA();
            Thread.Sleep(waittime);

            toetsenbord.Delete();
            Thread.Sleep(waittime);

            toetsenbord.Typ(currentUser + "@users.nl");
            Thread.Sleep(waittime);

            UIElement dontShow = new UIElement("Don't show this dialog again", identify, ControlType.CheckBox);
            TogglePattern dontShowTogglePattern = dontShow.element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;

            if (!dontShow.GetToggleState())
            {
                dontShow.LeftClick();
            }
            Thread.Sleep(waittime);
            
            toetsenbord.Enter();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);
            
            UIElement mergeWindow = new UIElement("Merge \'master\'", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);
                                 
            UIElement remoteTracking = new UIElement("Remote Tracking", mergeWindow, ControlType.TreeItem);
            Thread.Sleep(waittime);

            remoteTracking.Expand();
            Thread.Sleep(waittime);

            UIElement remote = new UIElement();
            AutomationElementCollection remoterepos = remoteTracking.element.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem));
            foreach (AutomationElement el in remoterepos)
            {

                if (el.Current.Name.StartsWith("origin" + temergen + "/"))
                {
                    remote = new UIElement(el);
                    break;
                }
            }

            Thread.Sleep(waittime);
            remote.LeftClick();
            Thread.Sleep(waittime);

            UIElement mergecommit = new UIElement("If a fast-forward, create a merge commit", mergeWindow, ControlType.RadioButton);
            Thread.Sleep(waittime);

            TogglePattern mergetogglePattern = mergecommit.element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            if (!mergecommit.GetToggleState())
            {
                mergecommit.LeftClick();
            }
            Thread.Sleep(waittime);

            UIElement mergebutton = new UIElement("Merge", mergeWindow, ControlType.Button);
            Thread.Sleep(waittime);

            mergebutton.LeftClick();
            Thread.Sleep(waittime);

            UIElement mergeResultWindow = new UIElement("Merge Result", eclipseWindow, ControlType.Window);
            Thread.Sleep(waittime);

            UIElement ok = new UIElement("OK", mergeResultWindow, ControlType.Button);
            Thread.Sleep(waittime);

            ok.LeftClick();
            Thread.Sleep(waittime);

            UIElement javaPerspective = new UIElement("Java", eclipseWindow, ControlType.CheckBox);
            Thread.Sleep(waittime);

            javaPerspective.LeftClick();
            Thread.Sleep(waittime);
        }

        // Method to exit Eclipse
        public void Stop()
        {
            toetsenbord.AltF4();
            Thread.Sleep(waittime);

            toetsenbord.Enter();
            Thread.Sleep(waittime);
        }

    }
}
