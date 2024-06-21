using UnityEngine;

namespace AnimationContent
{
    [RequireComponent(typeof(Animator))]
    public class AbstractAnimation : MonoBehaviour
    {
        protected Animator Animator { get; private set; }

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}