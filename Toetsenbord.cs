using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    // Class Toetsenbord represents the keybaord used to provide keyboard input
    class Toetsenbord
    {

        Typist typist;

        public Toetsenbord()
        {
            typist = new Typist();
        }

        // Method representing a push on enter
        public void Enter()
        {
            SendKeys.SendWait("{ENTER}");
        }

        // Method represent pusing of combination ctrl + a
        public void CtrlA()
        {
            SendKeys.SendWait("^a");
        }

        // Method represents pushing of combination ctrl + s
        public void CtrlS()
        {
            SendKeys.SendWait("^s");
        }

        // Method represents pushing of combination ctrl + l
        public void CtrlL()
        {
            SendKeys.SendWait("^l");
        }

        // Method represents pushing of combination ctrl + d
        public void CtrlD()
        {
            SendKeys.SendWait("^d");
        }

        // Method represents pushing of combination alt + f4
        public void AltF4()
        {
            SendKeys.SendWait("%{f4}");
        }

        // Method represents pushing delete
        public void Delete()
        {
            SendKeys.SendWait("{DELETE}");
        }

        // Method represents pushing arrow right key
        public void Right()
        {
            SendKeys.SendWait("{RIGHT}");
        }

        // Method represents pushing arrow left key
        public void Left()
        {
            SendKeys.SendWait("{LEFT}");
        }

        // Method represents pushing arrow down key
        public void Down()
        {
            SendKeys.SendWait("{DOWN}");
        }

        // Method represents pushing arrow up key
        public void Up()
        {
            SendKeys.SendWait("{UP}");
        }

        // Method respresents pushing F2
        public void F2()
        {
            SendKeys.SendWait("{F2}");
        }

        // Method represents pushing F5
        public void F5()
        {
            SendKeys.SendWait("{F5}");
        }

        // Method for getting the amount of time paused between keystrokes and words
        public double getPauseTime(double burstlength, char character)
        {
            if (char.IsWhiteSpace(character) && burstlength == 0)
            {
                return typist.getNextWordBurstPause();
            }
            if (char.IsWhiteSpace(character))
            {
                return typist.getNextWordPause();
            }

            return typist.getNextKeystrokePause();
        }


        // Method respresent typing of characters in string
        // param s; String to type
        public void Typ(String s)
        {
            double burstLenght = 0;

            foreach (char c in s)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                burstLenght = burstLenght == 0 ? typist.getNextWordBurstLength() : burstLenght;
                if (c.Equals('%'))
                {
                    SendKeys.SendWait("{%}");
                }
                else

                if (c.Equals('('))
                {
                    SendKeys.SendWait("{(}");

                }
                else

                if (c.Equals(')'))
                {
                    SendKeys.SendWait("{)}");

                }
                else

                 if (c.Equals('+'))
                {
                    SendKeys.SendWait("{+}");


                }
                else

                if (c.Equals('"'))
                {
                    SendKeys.SendWait(Char.ToString(c));
                    SendKeys.SendWait(" ");


                }
                else

                if (c.Equals('{'))
                {
                    SendKeys.SendWait("{{}");


                }
                else
                if (c.Equals('}'))
                {
                    SendKeys.SendWait("{}}");

                }
                else
                {
                    SendKeys.SendWait(Char.ToString(c));

                }

                stopwatch.Stop();
                double correctie = stopwatch.Elapsed.TotalMilliseconds;

                if (Char.IsWhiteSpace(c))
                {
                    burstLenght--;
                }

                double pausetime = getPauseTime(burstLenght, c) - correctie;
                if (pausetime > 0)
                {
                    Thread.Sleep(Convert.ToInt32(pausetime));
                }
                else
                {
                    Thread.Sleep(20);
                }

            }



        }
    }
}