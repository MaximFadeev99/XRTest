namespace XRTest.Signals
{
    public class MagazineInsertedSignal
    {
        public readonly int MaxAmmoCount;
        public readonly bool IsEquippedOnRight;

        public MagazineInsertedSignal(int maxAmmoCount, bool isEquippedOnRight)
        {
            MaxAmmoCount = maxAmmoCount;
            IsEquippedOnRight = isEquippedOnRight;
        }
    }
}
