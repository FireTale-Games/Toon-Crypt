using FT.Data;
using FT.Inventory;
using FT.TD;
using UnityEngine;

public class AbilityInventoryItemUi : InventoryItemUi
{
    private CharacterState _state;
    
    private void Awake() => 
        _state = GameObject.FindWithTag("Player").GetComponent<Character>().State;

    private void OnDestroy() => 
        _state.AddSpell.Set(new AbilityStruct(Id, false));

    public override void InitializeItem(int id)
    {
        if (id == -1)
            DeinitializeItem();
            
        Item item = ItemDatabase.Get(id);
        if (id == -1)
            return;

        Id = item.Id;
        _itemImage.sprite = item.Sprite;
        _itemImage.color = new Color(1, 1, 1, 1);
        _state.AddSpell.Set(new AbilityStruct(Id, true));
    }
}