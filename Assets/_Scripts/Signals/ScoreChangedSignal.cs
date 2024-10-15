namespace XRTest.Signals
{
    public class ScoreChangedSignal
    {
        public readonly int ScoreDelta;

        public ScoreChangedSignal(int scoreDelta) 
        {
            ScoreDelta = scoreDelta;
        }
    }
}