using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRTest.Input;
using XRTest.Weapons;
using Zenject;

namespace XRTest.MainPlayer
{
    public class Player : MonoBehaviour
    {
        [Header("Hand Animations")]
        [SerializeField] private Animator _leftHandAnimator;
        [SerializeField] private Animator _rightHandAnimator;
        [SerializeField] private float _lerpSpeed = 0.05f;

        [Header("Interaction Relayer")]
        [SerializeField] private XRRayInteractor _leftInteractor;
        [SerializeField] private XRRayInteractor _rightInteractor;

        private InputLogger _inputLogger;
        private SignalBus _signalBus;
        private HandController _handController;
        private InteractionRelayer _interactionRelayer;
        private CombatAspect _combatAspect;

        [Inject]
        private void Construct(SignalBus signalBus, InputLogger inputLogger)
        {
            _signalBus = signalBus;
            _inputLogger = inputLogger;
        }

        private void Awake()
        {
            _combatAspect = new(_signalBus);
            _handController = new(_rightHandAnimator, _leftHandAnimator, _lerpSpeed);
            _interactionRelayer = new(_leftInteractor, _rightInteractor);
        }

        private void OnEnable()
        {
            _inputLogger.LeftGripInteracted += _handController.OnLeftGripInteracted;
            _inputLogger.RightGripInteracted += _handController.OnRightGripInteracted;
            _inputLogger.LeftGripInteracted += _interactionRelayer.OnLeftGripInteracted;
            _inputLogger.RightGripInteracted += _interactionRelayer.OnRightGripInteracted;
            _inputLogger.LeftTriggerInteracted += OnLeftTriggerInteracted;
            _inputLogger.RightTriggerInteracted += OnRightTriggerInteracted;

            _interactionRelayer.ObjectGrabbedWithLeft += OnObjectGrabbedWithLeft;
            _interactionRelayer.ObjectGrabbedWithRight += OnObjectGrabbedWithRight;
            _interactionRelayer.ObjectDroppedWithLeft += OnObjectDroppedWithLeft;
            _interactionRelayer.ObjectDroppedWithRight += OnObjectDroppedWithRight;
        }

        private void OnDisable()
        {
            _inputLogger.LeftGripInteracted -= _handController.OnLeftGripInteracted;
            _inputLogger.RightGripInteracted -= _handController.OnRightGripInteracted;
            _inputLogger.LeftGripInteracted -= _interactionRelayer.OnLeftGripInteracted;
            _inputLogger.RightGripInteracted -= _interactionRelayer.OnRightGripInteracted;
            _inputLogger.LeftTriggerInteracted -= OnLeftTriggerInteracted;
            _inputLogger.RightTriggerInteracted -= OnRightTriggerInteracted;

            _interactionRelayer.ObjectGrabbedWithLeft -= OnObjectGrabbedWithLeft;
            _interactionRelayer.ObjectGrabbedWithRight -= OnObjectGrabbedWithRight;
            _interactionRelayer.ObjectDroppedWithLeft -= OnObjectDroppedWithLeft;
            _interactionRelayer.ObjectDroppedWithRight -= OnObjectDroppedWithRight;
        }

        private void OnObjectGrabbedWithLeft(GameObject grabbedObject) 
        {
            if (grabbedObject.TryGetComponent(out Weapon grabbedWeapon)) 
                _combatAspect.EquipWeaponOnLeft(grabbedWeapon);

            _handController.ToggleLeftHandVisibility(false);
        }

        private void OnObjectGrabbedWithRight(GameObject grabbedObject)
        {
            if (grabbedObject.TryGetComponent(out Weapon grabbedWeapon))
                _combatAspect.EquipWeaponOnRight(grabbedWeapon);

            _handController.ToggleRightHandVisibility(false);
        }

        private void OnObjectDroppedWithLeft(GameObject droppedObject) 
        {
            if (droppedObject.TryGetComponent(out Weapon _))
                _combatAspect.UnequipWeaponOnLeft();

            _handController.ToggleLeftHandVisibility(true);
        }

        private void OnObjectDroppedWithRight(GameObject droppedObject)
        {
            if (droppedObject.TryGetComponent(out Weapon _))
                _combatAspect.UnequipWeaponOnRight();

            _handController.ToggleRightHandVisibility(true);
        }

        private void OnLeftTriggerInteracted(bool isPressed) 
        {
            _handController.OnLeftTriggerInteracted(isPressed);

            if (isPressed)
                _combatAspect.TryUseLeftWeapon();
            else
                _combatAspect.StopUsingLeftWeapon();

        }

        private void OnRightTriggerInteracted(bool isPressed)
        {
            _handController.OnRightTriggerInteracted(isPressed);

            if (isPressed)
                _combatAspect.TryUseRightWeapon();
            else 
                _combatAspect.StopUsingRightWeapon();
        }
    }
}