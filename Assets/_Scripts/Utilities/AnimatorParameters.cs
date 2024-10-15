using UnityEngine;

namespace XRTest.Utilities
{
    public static class AnimatorParameters
    {
        public static readonly int Flex = Animator.StringToHash(nameof(Flex));
        public static readonly int Pinch = Animator.StringToHash(nameof(Pinch));
        public static readonly int IndexSlide = Animator.StringToHash(nameof(IndexSlide));
        public static readonly int Shoot = Animator.StringToHash(nameof(Shoot));
        public static readonly int IsShooting = Animator.StringToHash(nameof(IsShooting));
    }
}
