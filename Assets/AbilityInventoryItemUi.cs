using FT.Inventory;
using FT.TD;
using UnityEngine;

public class AbilityInventoryItemUi : InventoryItemUi
{
    private CharacterState _state;
    
    private void Awake() => 
        _state = GameObject.FindWithTag("Player").GetComponent<Character>().State;

    private void OnDestroy() => 
        _state.AddSpell.Set((Id, false));

    public override void InitializeItem(int id)
    {
        base.InitializeItem(id);
        _state.AddSpell.Set((Id, true));
    }

    protected override void DeinitializeItem()
    {
        _state.AddSpell.Set((Id, false));
        base.DeinitializeItem();
    }
}