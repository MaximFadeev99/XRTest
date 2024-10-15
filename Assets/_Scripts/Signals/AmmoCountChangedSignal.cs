namespace XRTest.Signals
{
    public class AmmoCountChangedSignal
    {
        public readonly int CurrentCount;
        public readonly bool HasChangedOnRight;

        public AmmoCountChangedSignal(int currentCount, bool hasChangedOnRight)
        {
            CurrentCount = currentCount;
            HasChangedOnRight = hasChangedOnRight;
        }
    }
}