using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Zenject;

public class ShopController : MonoBehaviour, IEnviromentInteraction
{
    private VisualElement _root;
    private Action OnConfirmAction;
    private ShopItemUI _dataItem;
    public Action OnExitShop;
    [Inject] private PlayerSpawnController _spawnController;
    [Inject] private GameplayController _gameplayController;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _cam;
    private ItemAttributetype[] _shopAttributes = new ItemAttributetype[] { ItemAttributetype.DESCRIPTION_SHOP,
                                                                            ItemAttributetype.DAMAGE,
                                                                            ItemAttributetype.FIRE,
                                                                            ItemAttributetype.WATER,
                                                                            ItemAttributetype.WIND,
                                                                            ItemAttributetype.GROUND,
                                                                            ItemAttributetype.ICE,
                                                                            ItemAttributetype.THUNDER
    };
    private LinkedListNode<ShopItemUI> _selectecItem;
    private LinkedList<ShopItemUI> _listItems;
    [SerializeField] private ItemShop[] _items;
    private GameInputs _inputs;

    public string ActionName => "Interagir";

    public float Delay => 0;

    public bool Repeat => false;

    private void Awake()
    {
        _inputs = new GameInputs(); 
        _inputs.UI.Left_Action.started += LeftSelect;
        _inputs.UI.Right_Action.started += RightSelect;
        _inputs.UI.Horizontal.started += ShopMoveHorizontal;
        _inputs.UI.Vertical.started += ShopMoveVertical;
        _inputs.UI.Cancel.started += ExitShop;
        _inputs.UI.Confirm.started += ConfirmAction;

        _root = GetComponent<UIDocument>().rootVisualElement;
    } 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterMediator mediator))
        {
            _gameplayController.ShowActions(this, async () =>
            {
                await Execute(null);
            });
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _gameplayController.HiddenActions();
        _gameplayController.CleanAction();
    }
    private void OnDestroy()
    {

        _inputs.UI.Left_Action.started -= LeftSelect;
        _inputs.UI.Right_Action.started -= RightSelect;
        _inputs.UI.Cancel.started -= ExitShop;
        _inputs.UI.Confirm.started -= ConfirmAction;
        _inputs.UI.Disable();
        _inputs.Dispose();
    }

    private void ShopMoveHorizontal(InputAction.CallbackContext obj)
    {
        var direction = obj.ReadValue<float>();
        _selectecItem.Value.Deselect();
        if (direction > 0)
        {
            if(_selectecItem.Next != null)
            {
                _selectecItem = _selectecItem.Next;
            }
            else
            {
                _selectecItem = _listItems.First;
            }
        }
        else
        {
            if (_selectecItem.Previous != null)
            {
                _selectecItem = _selectecItem.Previous;
            }
            else
            {
                _selectecItem = _listItems.Last;
            }
        }
        _selectecItem.Value.Select();
    }
    private void ShopMoveVertical(InputAction.CallbackContext obj)
    {
        var direction = obj.ReadValue<float>();
        _selectecItem.Value.Deselect();
        if(direction > 0)
        {
            for(int index = 0;index < 4; index++)
            {
                if(_selectecItem.Previous != null)
                {
                    _selectecItem = _selectecItem.Previous;
                }
                else
                {
                    _selectecItem = _listItems.Last;
                }
            }
        }
        else
        {
            for (int index = 0; index < 4; index++)
            {
                if (_selectecItem.Next != null)
                {
                    _selectecItem = _selectecItem.Next;
                }
                else
                {
                    _selectecItem = _listItems.First;
                }
            }
        }
        _selectecItem.Value.Select();
    }
    private void Selling()
    {
        if(_gameplayController.Inventory.ListInventory.Count == 0)
        {
            return;
        }
        _root.Q<VisualElement>("GridBuy").style.display = DisplayStyle.None;
        _root.Q<VisualElement>("GridSell").style.display = DisplayStyle.Flex;
        SetupSellingInventory().Forget();
        OnConfirmAction += Sell;
        OnConfirmAction -= Buy;
    } 
    private void Buying()
    {
        _root.Q<VisualElement>("GridBuy").style.display = DisplayStyle.Flex;
        _root.Q<VisualElement>("GridSell").style.display = DisplayStyle.None;
        SetupBuyingInventory().Forget();
        OnConfirmAction += Buy;
        OnConfirmAction -= Sell;
    }
    private void ConfirmAction(InputAction.CallbackContext obj)
    {
        OnConfirmAction?.Invoke();
    }
    private void RightSelect(InputAction.CallbackContext obj)
    {
        Selling();
    }

    private void ExitShop(InputAction.CallbackContext obj)
    {
        OnExitShop?.Invoke();
    }

    private void LeftSelect(InputAction.CallbackContext obj)
    {
        Buying();
    }

    private async UniTaskVoid SetupBuyingInventory()
    {
        var grid = _root.Q<VisualElement>("GridBuy");
        var amount = grid.childCount;
        for(int index = 0; index < amount; index++)
        {
            grid.RemoveAt(0);
        }
        HiddenData();
        var taskList = new List<UniTask>();

        _listItems = new LinkedList<ShopItemUI>();
        foreach (var data in _items)
        {
            if (data.Amount == 0)
                continue;
            var itemUI = new ShopItemUI() { };
            itemUI.OnSelect += ShowData;
            itemUI.OnDeSelect += HiddenData;
            grid.Add(itemUI);
            var task = itemUI.SetupIcon(data.Item, data.Amount);
            taskList.Add(task);
        }
        await UniTask.WhenAll(taskList.ToArray());
        _listItems = new LinkedList<ShopItemUI>(grid.Query<ShopItemUI>().ToList());
        _selectecItem = _listItems.First;
        _selectecItem.Value.Select(); 
    }
    private async UniTaskVoid SetupSellingInventory()
    {
        var grid = _root.Q<VisualElement>("GridSell");
        var amount = grid.childCount;
        for (int index = 0; index < amount; index++)
        {
            grid.RemoveAt(0);
        }
        HiddenData();
        var taskList = new List<UniTask>(); 

        foreach (var data in _gameplayController.Inventory.ListInventory)
        {
            if (data.Amount == 0)
                continue;
            var itemUI = new ShopItemUI() { };
            itemUI.OnSelect += ShowData;
            itemUI.OnDeSelect += HiddenData;
            grid.Add(itemUI);
            var task = itemUI.SetupIcon(data.Item, data.Amount);
            taskList.Add(task);
        }
        await UniTask.WhenAll(taskList.ToArray());
        _listItems = new LinkedList<ShopItemUI>(grid.Query<ShopItemUI>().ToList());
        _selectecItem = _listItems.First;
        _selectecItem.Value.Select();
    }
    private async void ShowData(ShopItemUI data)
    {
        _dataItem = data;
        var card = _root.Q<VisualElement>("Card");
        card.Q<Label>("Name").text = data.Data.Name;
        card.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(await Addressables.LoadAssetAsync<Sprite>(data.Data.Icon));
        foreach(var attribute in _shopAttributes)
        {
            var selectedAttribute = data.Data.Attributes.FirstOrDefault(x => x.Attribute == attribute);
            if(selectedAttribute != null)
            {
                var label = new Label()
                {
                    text = AttributeUtility.GetAttributeText(selectedAttribute),
                    enableRichText = true
                };
                label.style.marginBottom = 10; 
                label.style.width = Length.Percent(90);
                label.style.whiteSpace = WhiteSpace.Normal;
                card.Q<VisualElement>("Container").Add(label); 
            }
        }
        card.style.display = DisplayStyle.Flex;
    }
    private void HiddenData()
    {
        var card = _root.Q<VisualElement>("Card");
        var cardContainer = card.Q<VisualElement>("Container");
        int amount = cardContainer.childCount;
        for(int index = 0;index< amount; index++)
        {
            cardContainer.RemoveAt(0);
        }
        card.style.display = DisplayStyle.None;
    }
    private void Buy()
    {
        var price = Convert.ToInt32(_dataItem.Data.Attributes.First(x => x.Attribute == ItemAttributetype.PRICE_BUY).Value);
        if (_gameplayController.CanBuy(price))
        {
            _gameplayController.ConsumeMoney(price);
            var item = _items.First(x => _dataItem.Data.Name == x.Item.Name);
            _gameplayController.CollectItem(item.Item,1);
            item.Amount--;
            SetupBuyingInventory().Forget();
        }
        _root.Q<VisualElement>("Money").Q<Label>().text = _gameplayController.Money.ToString();
    }

    private void Sell()
    {
        var price = Convert.ToInt32(_dataItem.Data.Attributes.First(x => x.Attribute == ItemAttributetype.PRICE_SELL).Value);;
        _gameplayController.ConsumeMoney(-price);
        _gameplayController.UseItem(_dataItem.Data,1);
        _root.Q<VisualElement>("Money").Q<Label>().text = _gameplayController.Money.ToString();
    }
    public async UniTask Execute(params object[] items)
    {
        _inputs.UI.Enable();
        _gameplayController.ToggleInputs(false);
        SetupBuyingInventory().Forget();
        Buying();
        _root.Q<VisualElement>("Root").style.display = DisplayStyle.Flex;
        _root.Q<VisualElement>("Money").Q<Label>().text = _gameplayController.Money.ToString();
        _cam.Priority = 15;
        _spawnController.Player.ToggleMove(false);
        OnExitShop += ()=> 
        {
            _cam.Priority = 10;
            _root.Q<VisualElement>("Root").style.display = DisplayStyle.None;
            OnExitShop = null;
            _spawnController.Player.ToggleMove(true);
            _gameplayController.ToggleInputs(true);
        };
        await UniTask.CompletedTask;
    }
}

public class AttributeUtility
{
    public static string GetAttributeText(ItemAttribute attribute)
    {
        switch (attribute.Attribute)
        {
            case ItemAttributetype.DESCRIPTION_SHOP: return $"Description: {attribute.Value}";
            case ItemAttributetype.DAMAGE: return $"Damage: {attribute.Value}";
            case ItemAttributetype.FIRE: return $"<color=#E30935>Fire</color>: {attribute.Value}";
            case ItemAttributetype.WATER: return $"<color=#0A0AC2>Water</color>: {attribute.Value}";
            case ItemAttributetype.THUNDER: return $"<color=#FFDC40>Thunder</color>: {attribute.Value}";
            case ItemAttributetype.GROUND: return $"<color=#4D1F11>Ground</color>: {attribute.Value}";
            case ItemAttributetype.ICE: return $"<color=#5CC3FF>Ice</color>: {attribute.Value}";
            case ItemAttributetype.WIND: return $"<color=#BCD8E8>Wind</color>: {attribute.Value}"; 
            default: return "";
        }
    }
}
