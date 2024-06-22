using UnityEngine;

namespace ExtensionContent
{
    public class ExtensionMapMovement : MonoBehaviour
    {
        private float _stepDown = -1.5f;
        private float _stepLeft = 1.5f;
        private int _factor = 2;
        private int _defaultValue = 1;
        private int _index;

        public void ChangePosition(int index, Transform objectTransform)
        {
            if (index % _factor == 0)
                objectTransform.localPosition += new Vector3(0, 0, _stepLeft);
            else
                objectTransform.localPosition += new Vector3(_stepDown, 0, 0);
        }

        public void ResetPosition(Transform objectTransform)
        {
            objectTransform.localPosition = new Vector3(0, 0, 0);
        }

        public void SetPosition(int amount, Transform objectTransform)
        {
            _index = _defaultValue;

            for (int i = 0; i < amount; i++)
            {
                ChangePosition(_index, objectTransform);
                _index++;
            }
        }
    }
}