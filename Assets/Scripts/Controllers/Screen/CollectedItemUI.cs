using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectedItemUI : VisualElement
{
    private CancellationTokenSource _aliveCancelToken;
    public VisualElement Icon { get; }
    public Label Name { get; }
    public Label Amount { get; }
    public new class UxmlFactory : UxmlFactory<CollectedItemUI, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits{}
    public CollectedItemUI()
    {
        style.flexDirection = FlexDirection.RowReverse;
        style.color = Color.white;
        style.fontSize = 15;
        style.flexGrow = 0;
        style.width = 400;
        style.height = 50;
        style.alignItems = Align.Center;
        style.marginRight = 40;
        style.backgroundColor = new Color(0,0,0,1);

        Amount = new Label() {text="xO"};
        Amount.style.marginRight = 10;
        Add(Amount); 

        Name = new Label() {text= "Sem nome"};
        Name.style.width = Length.Percent(77);
        Name.style.paddingLeft = 10;
        Add(Name);

        Icon = new VisualElement();
        Icon.style.width = Length.Percent(10);
        Icon.style.height = Length.Percent(90);
        Icon.style.backgroundColor = Color.white;
        Add(Icon);
        _aliveCancelToken = new CancellationTokenSource();
    }
    ~CollectedItemUI()
    {
        _aliveCancelToken.Cancel();
    }
    public void Init(Sprite icon, string name, int amount)
    {
        Icon.style.backgroundImage = new StyleBackground(icon);
        Name.text = name;
        Amount.text = $"x{amount:D2}";
    } 
    public async UniTaskVoid Move()
    { 
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2)); 
            parent.Remove(this);
        }
        catch(Exception ex)
        {

        }
    }
}

public class EnviromentActionUI : VisualElement
{ 
    public new class UxmlFactory : UxmlFactory<EnviromentActionUI, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public EnviromentActionUI()
    { 
        style.flexDirection = FlexDirection.RowReverse;
        style.color = Color.white;
        style.fontSize = 15;
        style.flexGrow = 0;
        style.width = 600;
        style.justifyContent = Justify.SpaceAround;
        style.fontSize = 20;
        style.unityFontStyleAndWeight = FontStyle.Bold;
        style.height = 50;
        style.alignItems = Align.Center; 
        style.backgroundColor = new Color(0, 0, 0, 1);
    }
    public void Init(IEnviromentInteraction action)
    {
        var Container = new VisualElement();
        Container.style.flexDirection = FlexDirection.Row;
        Container.Add(new Label() { text = $"E - {action.ActionName}" }); 
        Add(Container);
    }
}
