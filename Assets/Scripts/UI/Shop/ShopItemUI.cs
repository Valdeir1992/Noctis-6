using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

public class ShopItemUI : VisualElement
{
    private int _amount;
    public Action<ShopItemUI> OnSelect;
    public Action OnDeSelect;
    private UIItemDataSO _data;

    public UIItemDataSO Data { get => _data;}
    public int Amount { get => _amount;}

    public new class UxmlFactory : UxmlFactory<ShopItemUI, UxmlTraits> { }

    public ShopItemUI()
    {
        style.width = 130;
        style.height = 100;
        style.backgroundColor = new Color(0, 0, 0, 1);
        style.marginTop = 15;
        style.marginRight = 15;
        style.alignItems = Align.Center;
        style.justifyContent = Justify.Center;

        var backgroundSlot = new VisualElement();
        backgroundSlot.style.width = Length.Percent(95);
        backgroundSlot.style.height = Length.Percent(95);
        backgroundSlot.name = "slot";
        backgroundSlot.style. alignItems = Align.Center;
        backgroundSlot.style.justifyContent = Justify.Center; 
        Add(backgroundSlot);

        var icon = new VisualElement();
        icon.style.width = 100;
        icon.style.height = 100;
        icon.name = "icon"; 
        icon.style.flexDirection = FlexDirection.ColumnReverse;
        backgroundSlot.Add(icon);

        var container = new VisualElement();
        container.style.position = Position.Absolute;
        container.style.bottom = 5;
        container.style.color = new Color(1, 1, 1, 0.6f);
        container.Add(new Label() { text = "1", name = "amount" });
        container.Add(new Label() { text = $"Price: {1}", name = "price" });
        container.style.width = Length.Percent(90);
        container.style.height = 30;
        container.style.flexDirection = FlexDirection.Row;
        container.style.backgroundColor = new Color(0, 0, 0, 1);
        container.style.justifyContent = Justify.Center;
        container.style.alignContent = Align.Center;
        Add(container);
    }
    public async UniTask<ShopItemUI> SetupIcon(UIItemDataSO data, int amount, bool buy = true)
    {
        _data = data;
        _amount = amount;
        string rarity = _data.Attributes.First(x => x.Attribute == ItemAttributetype.RARITY).Value;
        await UniTask.WhenAll(LoadIcon(data.Icon), SetRarity(Convert.ToInt32(rarity)));
        this.Q<Label>("amount").text = amount.ToString();
        string price = (buy) ? data.Attributes.First(x => x.Attribute == ItemAttributetype.PRICE_BUY).Value : data.Attributes.First(x => x.Attribute == ItemAttributetype.PRICE_SELL).Value;
        this.Q<Label>("price").text = $"Price: {price}";
        return this;
    }
    public void Select()
    {
        style.backgroundColor = new Color(0.5f, 0.5f, 0, 1);
        OnSelect?.Invoke(this);
    }
    public void Deselect()
    {
        style.backgroundColor = new Color(0, 0, 0, 1);
        OnDeSelect?.Invoke();
    } 
    private async UniTask LoadIcon(AssetReferenceSprite icon)
    {
        try
        {
            var sprite = await Addressables.LoadAssetAsync<Sprite>(icon);
            this.Q<VisualElement>("icon").style.backgroundImage = new StyleBackground(sprite);
        }
        catch
        {

        }
        finally
        {
            await UniTask.CompletedTask;
        }
    }
    private async UniTask SetRarity(int rarity)
    {
        try
        {
            var sprite = await Addressables.LoadAssetAsync<Sprite>($"SlotShop");
            var slot = this.Q<VisualElement>("slot");
            slot.style.backgroundImage = new StyleBackground(sprite);
            switch (rarity)
            {
                case 1:slot.style.unityBackgroundImageTintColor = new Color(1, 0, 0, 1);break;
                case 2: slot.style.unityBackgroundImageTintColor = new Color(0, 1, 0, 1); break;
                case 3: slot.style.unityBackgroundImageTintColor = new Color(1, 1, 0, 1); break; 
            }
        }
        catch
        {

        }
        finally
        {
             await UniTask.CompletedTask;
        }
    }
}
