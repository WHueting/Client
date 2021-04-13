namespace Client
{
    //Class Statistics represents normal distribution for creating values for pauses
    class Statistics
    {
        private double mean;
        private double standardDeviation;

        // Constructor for Statistics
        // param mn, mean of normal distribution
        // param staDev, standard devation of normal distribution
        public Statistics(double mn, double staDev)
        {
            mean = mn;
            standardDeviation = staDev;
        }

        // Method getting a random value from normal distribution

        public double getNextGaussian()
        {
            MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(mean, standardDeviation);
            double sample = normalDist.Sample();
            return sample;

        }
    }
}
