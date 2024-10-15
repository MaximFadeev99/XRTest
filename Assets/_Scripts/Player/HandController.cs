using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using XRTest.Utilities;

namespace XRTest.MainPlayer
{
    internal class HandController 
    {
        private readonly Animator _rightHandAnimator;
        private readonly GameObject _rightHand;
        private readonly Animator _leftHandAnimator;
        private readonly GameObject _leftHand;
        private readonly float _lerpSpeed;

        private CancellationTokenSource _leftCTS = new();
        private CancellationTokenSource _rightCTS = new();

        internal HandController(Animator rightHandAnimator, Animator leftHandAnimator, float lerpSpeed)
        {
            _rightHandAnimator = rightHandAnimator;
            _leftHandAnimator = leftHandAnimator;
            _lerpSpeed = lerpSpeed;
            _rightHand = _rightHandAnimator.gameObject;
            _leftHand = _leftHandAnimator.gameObject;
        }

        internal void OnLeftGripInteracted(bool isPressed) 
        {
            CancelPreviousAnimation(_leftCTS);

            if (isPressed)
            {
                _ = MakeHandPose(_leftHandAnimator, AnimatorParameters.Flex)
                    .AttachExternalCancellation(_leftCTS.Token);
            }
            else 
            {
                _ = UnmakeHandPose(_leftHandAnimator, AnimatorParameters.Flex)
                    .AttachExternalCancellation(_leftCTS.Token);
            }
        }

        internal void OnRightGripInteracted(bool isPressed)
        {
            CancelPreviousAnimation(_rightCTS);

            if (isPressed)
            {
                _ = MakeHandPose(_rightHandAnimator, AnimatorParameters.Flex)
                    .AttachExternalCancellation(_rightCTS.Token);
            }
            else
            {
                _ = UnmakeHandPose(_rightHandAnimator, AnimatorParameters.Flex)
                    .AttachExternalCancellation(_rightCTS.Token);
            }
        }

        internal void OnLeftTriggerInteracted(bool isPressed)
        {
            CancelPreviousAnimation(_leftCTS);

            if (isPressed)
            {
                _ = MakeHandPose(_leftHandAnimator, AnimatorParameters.Pinch)
                    .AttachExternalCancellation(_leftCTS.Token);
            }
            else
            {
                _ = UnmakeHandPose(_leftHandAnimator, AnimatorParameters.Pinch)
                    .AttachExternalCancellation(_leftCTS.Token);
            }
        }

        internal void OnRightTriggerInteracted(bool isPressed)
        {
            CancelPreviousAnimation(_rightCTS);

            if (isPressed)
            {
                _ = MakeHandPose(_rightHandAnimator, AnimatorParameters.Pinch)
                    .AttachExternalCancellation(_rightCTS.Token);
            }
            else
            {
                _ = UnmakeHandPose(_rightHandAnimator, AnimatorParameters.Pinch)
                    .AttachExternalCancellation(_rightCTS.Token);
            }
        }

        internal void ToggleRightHandVisibility(bool isVisible) 
        {
            _rightHand.SetActive(isVisible);
        }

        internal void ToggleLeftHandVisibility(bool isVisible)
        {
            _leftHand.SetActive(isVisible);
        }


        private async UniTask MakeHandPose(Animator targetAnimator, int targetParameter) 
        {
            float startValue = targetAnimator.GetFloat(targetParameter);
            float currentValue = startValue;

            while (currentValue < 1f) 
            {
                currentValue += _lerpSpeed;
                targetAnimator.SetFloat(targetParameter, currentValue);

                await UniTask.Yield();
            }

            targetAnimator.SetFloat(targetParameter, 1f);
        }

        private async UniTask UnmakeHandPose(Animator targetAnimator, int targetParameter) 
        {
            float startValue = targetAnimator.GetFloat(targetParameter);
            float currentValue = startValue;

            while (currentValue > 0f)
            {
                currentValue -= _lerpSpeed;
                targetAnimator.SetFloat(targetParameter, currentValue);

                await UniTask.Yield();
            }

            targetAnimator.SetFloat(targetParameter, 0f);
        }

        private void CancelPreviousAnimation(CancellationTokenSource targetCTS) 
        {
            if (targetCTS.Token.CanBeCanceled)
                targetCTS.Cancel();

            targetCTS = new();
        }
    }
}