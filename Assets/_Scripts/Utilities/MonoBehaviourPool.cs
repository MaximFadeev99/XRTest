using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XRTest.Utilities
{
    //Warning!!! If T requires Zenject to inject dependencies and uses ZenAutoInjecter, then initialCount 
    //parameter in the constructor of MonoBehaviourPool must be 0. Otherwise, both Awake method of this script and
    //the method with [Inject] attribute will not be called for instances of T. 
    public class MonoBehaviourPool<T> where T : MonoBehaviour, IMonoBehaviourPoolElement
    {
        private readonly List<T> _allElements = new();
        private readonly T _prefab;
        private readonly Transform _parentTransform;
        private int _elementCount = 0;

        public IReadOnlyList<T> AllElements => _allElements;

        public MonoBehaviourPool(T prefab, Transform parentTransform, int initialCount = 0)
        {
            _prefab = prefab;
            _parentTransform = parentTransform;

            if (initialCount != 0)
                PopulateOnInitialization(initialCount);
        }

        public T GetIdleElement()
        {
            T idleElement = _allElements
                .FirstOrDefault(property => property.GameObject.activeSelf == false);

            if (idleElement == null)
                idleElement = CreateNewElement();

            return idleElement;
        }

        private void PopulateOnInitialization(int initialCount) 
        {
            for (int i = 0; i < initialCount; i++)
                CreateNewElement();
        }

        private T CreateNewElement() 
        {
            T newElement = GameObject.Instantiate
                    (_prefab, _parentTransform.position, Quaternion.identity, _parentTransform);

            if (newElement.GameObject == null)
                newElement.Awake();
            newElement.GameObject.name = $"{_prefab.name} {_elementCount++}";
            newElement.GameObject.SetActive(false);
            _allElements.Add(newElement);

            return newElement;
        }
    }

    public interface IMonoBehaviourPoolElement 
    {
        public GameObject GameObject { get; }

        public void Awake();
    }
}