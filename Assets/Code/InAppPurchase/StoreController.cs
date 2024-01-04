using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Unity.Services.Core.Environments;
using System.Linq;
using System.IO;

public class StoreController : MonoBehaviour, IDetailedStoreListener
{
    public GameObject PurchasingMask;
    public GameObject generalControllers;
    public List<GameObject> StoreItems;
    private IStoreController storeController;
    private IExtensionProvider storeExtensions;
    private Button purchaseButton;

    public AudioSource successfulPurchaseSound;
    public TMP_Text testConsoleOutput;

    void Start()
    {
        #region oyun açýlýrken çalýþan bir start methodu içinde çalýþtýrýlacak kod.
        //oyun açýlýrken store initial yapacaðýz. Çünkü restore buton için loadcatalog'un çalýþmýþ olmasý ve OnInitialized fonksiyonuna girmiþ olmasý gerekiyor
        //storePanel.GetComponent<StoreController>().UnityServicesInitial();
        #endregion
    }

    //rubik oyun mantýðý gereði initialize baþka yerde baþlatýlýyor. Bu yüzden servis baþlatma fonskiyonunu kullanmýyoruz.
    public async void UnityServicesInitial()
    {

        if (UnityServices.State != ServicesInitializationState.Initialized || UnityServices.State != ServicesInitializationState.Initializing)
        {
            await UnityServices.InitializeAsync();
        }

        LoadCatalog();
    }

    void LoadCatalog()
    {
        try
        {
            ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((Resources.LoadAsync<TextAsset>("IAPProductCatalog").asset as TextAsset).text);

            //StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.DeveloperUser;
            //StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                builder.AddProduct(item.id, item.type);
            }

            UnityPurchasing.Initialize(this, builder);
        }
        catch (Exception e)
        {

            Debug.Log("Hata: StoreController LoadCatalog hata oluþtu. " + e.Message);
        }

    }

    //IAP baþarýlý þekilde baþlatýldýðýnda girer
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        try
        {
            this.storeController = controller;
            this.storeExtensions = extensions;
            foreach (GameObject item in StoreItems)
            {
                //ProductCatalogItem itemData = catalog.allProducts.Where(i => i.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();

                Product product_ = storeController.products.all.Where(i => i.definition.id == item.GetComponent<StoreItem>().Item_Id).FirstOrDefault();
                item.GetComponent<StoreItem>().SetItemMetaData(product_.metadata.localizedTitle, product_.metadata.localizedDescription, product_.metadata.localizedPriceString + " " + product_.metadata.isoCurrencyCode);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Hata: StoreController OnInitialized hata oluþtu. " + e.Message);

        }
    }

    //IAP baþlatýlmasýnda hata ile karþýlaþýlýrsa
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP baþlatma hatasý. OnInitializeFailed: " + error);
        throw new System.NotImplementedException();
    }

    //IAP baþlatýlmasýnda hata ile karþýlaþýlýrsa
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}. \r\nError message: {message}");
    }

    //Satýn almada hata meydana gelirse
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //satýn alma baþarýlý olursa
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        PurchaseCompleted();
        PurchasingMask.SetActive(false);

        //do something, like give the player their currency, unlock the item 
        //update some metrics or analytics, etc...

        ApplyPurchasedItemEffect(purchaseEvent.purchasedProduct.definition.id, purchaseEvent.purchasedProduct.metadata.localizedDescription);

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productId, Button button)
    {
        purchaseButton = button;
        if (purchaseButton.IsActive())
        {
            purchaseButton.enabled = false;
        }
        PurchasingMask.SetActive(true);
        Product product = storeController.products.all.Where(i => i.definition.id == productId).FirstOrDefault();
        storeController.InitiatePurchase(product);
    }

    private void PurchaseCompleted()
    {
        try
        {
            if (purchaseButton.IsActive())
            {
                purchaseButton.enabled = true;
            }
        }
        catch (Exception e)
        {

            Debug.Log("PurchaseCompleted hata. PurchaseCompleted: " + e.Message);
        }

    }

    public void RestorePurchase()
    {
        if (storeController != null)
        {
            Product product_noAds = storeController.products.all.Where(i => i.definition.id == "coma_no_ads").FirstOrDefault();
            if (product_noAds != null)
            {
                if (product_noAds.hasReceipt)
                {
                    PlayerPrefs.SetString("NoAdsActive", "1");
                    PlayerPrefs.Save();

                    Debug.Log("Daha önce satýn aldýðýnýz reklam yok ürünü geri getirildi.");
                }
                else
                {
                    Debug.Log("Reklam yok ürünü satýn alýnmamýþ");
                }
            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

        Debug.Log("Satýn almada hata meydana geldi. message:" + failureDescription.message + " reason:" + failureDescription.reason + " ýd:" + failureDescription.productId);

        if (product.hasReceipt)
        {
            Debug.Log("Daha önce satýn alýnmýþ. id:" + product.definition.id);
            ApplyPurchasedItemEffect(product.definition.id, product.metadata.localizedDescription);
        }

        PurchaseCompleted();
        PurchasingMask.SetActive(false);
    }

    //maðazadan bir ürün satýn alýndýðýnda oyuna nasýl etki edecekse ayarlamalar yapýlýr.
    void ApplyPurchasedItemEffect(string itemId, string description)
    {
        if (itemId == "coma_no_ads")
        {
            PlayerPrefs.SetString("NoAdsActive", "1");
            PlayerPrefs.Save();
        }
        else if (itemId.Contains("coma_bomb_joker"))
        {
            string jokerQuantity = PlayerPrefs.GetString("BombJoker");
            if (jokerQuantity == "")
            {
                jokerQuantity = description;
                PlayerPrefs.SetString("BombJoker", jokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(jokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("BombJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId.Contains("coma_changer_joker"))
        {
            string jokerQuantity = PlayerPrefs.GetString("ChangerJoker");
            if (jokerQuantity == "")
            {
                jokerQuantity = description;
                PlayerPrefs.SetString("ChangerJoker", jokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(jokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("ChangerJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }
        else if (itemId.Contains("coma_max_combo_joker"))
        {
            string jokerQuantity = PlayerPrefs.GetString("MaxComboJoker");
            if (jokerQuantity == "")
            {
                jokerQuantity = description;
                PlayerPrefs.SetString("MaxComboJoker", jokerQuantity);
                PlayerPrefs.Save();
            }
            else
            {
                int amount = Int32.Parse(jokerQuantity);
                amount += Int32.Parse(description);
                PlayerPrefs.SetString("MaxComboJoker", amount.ToString());
                PlayerPrefs.Save();
            }
        }

        successfulPurchaseSound.Play();
    }
}
