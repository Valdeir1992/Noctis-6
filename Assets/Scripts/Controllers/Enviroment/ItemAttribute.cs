[System.Serializable]
public class ItemAttribute
{
    public ItemAttributetype Attribute;
    public string Value;
}
public enum ItemAttributetype
{
    PRICE_BUY,
    PRICE_SELL,
    DESCRIPTION_SHOP,
    DESCRIPTION_INVENTORY,
    COLLECT,
    DAMAGE, 
    AMOUNT,
    RARITY,
    FIRE,
    WATER,
    WIND,
    GROUND,
    ICE,
    THUNDER,
} 
