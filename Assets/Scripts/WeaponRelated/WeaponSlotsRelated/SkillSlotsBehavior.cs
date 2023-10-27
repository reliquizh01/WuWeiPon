using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotsBehavior : AnimationMonoBehavior
{
    public List<SpriteRenderer> fillerSlotIconList;
    public SpriteRenderer originalCurrentSkillSlot;
    public SpriteRenderer fakeCurrentSkillSlot;

    bool isSlotSpinning = false;
    public void SlotClicked()
    {
        if(!isSlotSpinning)
        {
            isSlotSpinning =true;
            Play("SpinSlots", () => { isSlotSpinning = false; });
        }
    }

    public void OnMouseDown()
    {
        SlotClicked();
    }
}
