using System;
using System.Windows.Automation;
using Microsoft.Test.Input;
using System.Threading;

namespace Client
{
    //Class UIElement represent a AutomationElement
    class UIElement
    {
        public AutomationElement element;

        // Constructor
        public UIElement()
        { }

        // Constructor, searching tree from RootElement with descendants as scope
        // param name; string containing name of AutomationElement to look for
        public UIElement(string name)
        {
            try
            {
                int count = 0;

                do
                {
                    element = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name));
                    Thread.Sleep(100);
                    count++;
                }
                while (element == null && count < 300);

                if (element == null)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException) 
            {
                Console.WriteLine("Element " + name + " niet gevonden.");
            }
        }

        // Constructor, searching tree from given element as root with descendants as scope
        // param name; string containing name of AutomationElement to look for
        // param root; UIElement representing the root of the search tree
        public UIElement(string name, UIElement root)
        {
            try
            {
                int count = 0;

                do
                {
                    element = root.element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name)); ;
                    Thread.Sleep(100);
                    count++;
                }
                while (element == null && count < 300);

                if (element == null)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Element " + name + " niet gevonden.");
            }
            
        }

        // Constructor, searching tree from given element as root with descendants as scope and controltype as extra property
        // param name; string containing name of AutomationElement to look for
        // param root; UIElement representing the root of the search tree
        // param controltype; ControlType of AutomationElement to look for
        public UIElement(string name, UIElement root, object controltype)
        {
            var controlTypeProperty = new PropertyCondition(AutomationElement.ControlTypeProperty, controltype);
            var nameProperty = new PropertyCondition(AutomationElement.NameProperty, name);
            var andProperty = new AndCondition(controlTypeProperty, nameProperty);

            try
            {
                int count = 0;

                do
                {
                    element = root.element.FindFirst(TreeScope.Descendants, andProperty);
                    Thread.Sleep(100);
                    count++;
                }
                while (element == null && count < 300);

                if (element == null)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Element " + name + " niet gevonden.");
            }
            
        }

        // Constructor, searching tree from given element as root with descendants as scope and controltype as extra property
        // param name1; string containing name of AutomationElement to look for
        // param name2: string containing name of AutomationElement to look for
        // param root; UIElement representing the root of the search tree
        // param controltype; ControlType of AutomationElement to look for
        public UIElement(string name1, string name2, UIElement root, object controltype)
        {
            var controlTypeProperty = new PropertyCondition(AutomationElement.ControlTypeProperty, controltype);
            var nameProperty1 = new PropertyCondition(AutomationElement.NameProperty, name1);
            var nameProperty2 = new PropertyCondition(AutomationElement.NameProperty, name2);
            var orProperty = new OrCondition(nameProperty1, nameProperty2);
            var andProperty = new AndCondition(controlTypeProperty, orProperty);


            try
            {
                int count = 0;

                do
                {
                    element = root.element.FindFirst(TreeScope.Descendants, andProperty); 
                    Thread.Sleep(100);
                    count++;
                }
                while (element == null && count < 300);

                if (element == null)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Element " + name1 + " of " + name2 + " niet gevonden.");
            }

           
        }

        // Constructor
        // param ae; already found AutomationElement
        public UIElement(AutomationElement ae)
        {
            element = ae;
        }

        // Constructor, searching tree from RootElement as root with descendants as scope and controltype as extra property
        // param name; string containing name of AutomationElement to look for
        // param controltype; ControlType of AutomationElement to look for
        public UIElement(string name, object controltype)
        {
            var controlTypeProperty = new PropertyCondition(AutomationElement.ControlTypeProperty, controltype);
            var nameProperty = new PropertyCondition(AutomationElement.NameProperty, name);
            var andProperty = new AndCondition(controlTypeProperty, nameProperty);

            try
            {
                int count = 0;

                do
                {
                    element = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, andProperty);
                    Thread.Sleep(100);
                    count++;
                }
                while (element == null && count < 300);

                if (element == null)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Element " + name + " niet gevonden.");
            }
            
        }

        // Method that return the ToggleState of AutomationElements as a boolean
        // return togglestate; boolean indicating if AutomationElement is toggled
        public Boolean GetToggleState() 
        {
            Boolean togglestate = false;

            TogglePattern togglePattern = element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            if (togglePattern.Current.ToggleState.Equals(ToggleState.On))
            {
                togglestate = true;
            }
            
            return togglestate;
        }

        // Method to invoke an AutomationElement
        public void Invoke()
        {
            InvokePattern invoke = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invoke.Invoke();
        }

        // Method to expand an AutomationElement
        public void Expand()
        {
            ExpandCollapsePattern expand = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expand.Expand();
        }

        //Method to collapse an AutomationElement
        public void Collapse()
        {
            ExpandCollapsePattern collapse = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            collapse.Collapse();
        }

        // Method to perform a single left mouseclick on an AutomationElement
        public void LeftClick()
        {
            System.Windows.Point wpoint = element.GetClickablePoint();
            System.Drawing.Point dpoint = new System.Drawing.Point(Convert.ToInt32(wpoint.X), Convert.ToInt32(wpoint.Y));
            Mouse.MoveTo(dpoint);
            Thread.Sleep(500);
            Mouse.Click(MouseButton.Left);
            Thread.Sleep(500);
        }

        // Method to perform a single right mouseclick on an AutomationElement
        public void RightClick()
        {
            System.Windows.Point wpoint = element.GetClickablePoint();
            System.Drawing.Point dpoint = new System.Drawing.Point(Convert.ToInt32(wpoint.X), Convert.ToInt32(wpoint.Y));
            Mouse.MoveTo(dpoint);
            Thread.Sleep(500);
            Mouse.Click(MouseButton.Right);
            Thread.Sleep(500);
        }

        //Method to perform a double left mouseclick on an AutomationElement.

        public void DoubleLeftClick()
        {
            System.Windows.Point wpoint = element.GetClickablePoint();
            System.Drawing.Point dpoint = new System.Drawing.Point(Convert.ToInt32(wpoint.X), Convert.ToInt32(wpoint.Y));
            Mouse.MoveTo(dpoint);
            Thread.Sleep(500);
            Mouse.DoubleClick(MouseButton.Left);
            Thread.Sleep(500);
        }
    }
}
