using System;

namespace Client
{
    //Class Typist represents a typist profile
    class Typist
    {
        private Statistics keystrokePause = new Statistics(70, 50);
        private Statistics wordBurstPause = new Statistics(5000, 800);
        private Statistics wordBurst = new Statistics(4, 2);
        private Statistics wordPause = new Statistics(700, 330);
        public Typist()
        {

        }

        //Method for getting next keystroke pause
        public double getNextKeystrokePause()
        {

            return getNextFromStatistics(keystrokePause);
        }

        //Method for getting next word burst length
        public double getNextWordBurstLength()
        {

            return getNextFromStatistics(wordBurst);
        }

        //Method for getting next word burst pause
        public double getNextWordBurstPause()
        {

            return getNextFromStatistics(wordBurstPause);
        }

        //Method for getting next word pause
        public double getNextWordPause()
        {

            return getNextFromStatistics(wordPause);
        }

        //Method for getting next Gaussian value
        private double getNextFromStatistics(Statistics statistics)
        {
            double next = Math.Round(statistics.getNextGaussian());
            return next < 0 ? 0 : next;
        }
    }
}