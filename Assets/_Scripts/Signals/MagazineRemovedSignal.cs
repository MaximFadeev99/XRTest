namespace XRTest.Signals
{
    public class MagazineRemovedSignal
    {
        public readonly bool IsRemovedOnRight;

        public MagazineRemovedSignal(bool isRemovedOnRight)
        {
            IsRemovedOnRight = isRemovedOnRight;
        }
    }
}
