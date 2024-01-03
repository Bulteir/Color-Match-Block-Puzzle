using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string Item_Id;
    public GameObject StoreController;
    public Button PurchaseButton;
    public TMP_Text PriceText;
    public TMP_Text ContentText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        StoreController.GetComponent<StoreController>().Purchase(Item_Id, PurchaseButton);
        Debug.Log("Satýn alma iþlemi baþladý");
    }

    public void SetItemMetaData(string Title, string Description, string Price)
    {
        //ideal olan IAP servisten gelen title ve description'ý göstermek ancak bunun localizationu nasýl idere edilir bilmediðimiz için kullanmýyoruz.
        PriceText.text = Price;

        if (Item_Id.Contains("coma_no_ads"))
        {
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_NoAds");
        }
        else if (Item_Id.Contains("coma_bomb_joker1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_BombJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_bomb_joker2"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_BombJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_bomb_joker3"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_BombJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_changer_joker1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_ChangerJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_changer_joker2"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_ChangerJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_changer_joker3"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_ChangerJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_max_combo_joker1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_MaxComboJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_max_combo_joker2"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_MaxComboJoker", new object[] { arguments });
        }
        else if (Item_Id.Contains("coma_max_combo_joker3"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "IAP_MaxComboJoker", new object[] { arguments });
        }
    }
}
