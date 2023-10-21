using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactable
{
    public class InteractableItem : MonoBehaviour, IPointerClickHandler
    {
        public Dictionary<InteractionEnum, List<Action>> interactionAction = new Dictionary<InteractionEnum, List<Action>>();

        public void Awake()
        {
            interactionAction.Add(InteractionEnum.OnClick, new List<Action>());
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (interactionAction[InteractionEnum.OnClick].Count > 0)
            {
                foreach (Action action in interactionAction[InteractionEnum.OnClick])
                {
                    action.Invoke();
                }

                interactionAction[InteractionEnum.OnClick].Clear();
            }
        }
    }
}