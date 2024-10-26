using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventoryController 
{
    private List<ItemInventory> _listInventory;

    public List<ItemInventory> ListInventory { get => _listInventory;}

    public void StartInventory()
    {
        _listInventory = new List<ItemInventory>();
    }

    public void AddItem(ItemInventory item)
    {
        var selectedItem = _listInventory.FirstOrDefault(x => x == item);
        if(selectedItem == null)
        {
            _listInventory.Add(item);
        }
        else
        {
            selectedItem.Amount += item.Amount;
        }
    }
    public void RemoveItem(ItemInventory item)
    {
        var selectedItem = _listInventory.FirstOrDefault(x => x == item);
        if (selectedItem != null)
        {
            selectedItem.Amount -= item.Amount;
            if(selectedItem.Amount <= 0)
            {
                _listInventory.Remove(selectedItem);
            }
        }
    }
}
