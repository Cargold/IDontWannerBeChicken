using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class GetTheMoney_Script : MonoBehaviour, IStoreListener
{
    public static GetTheMoney_Script Instance;

    private int adCount;
    public int adCount_Show;

    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;
    public const string productId_0 = "package_0";
    public const string productId_1 = "package_1";
    public const string productId_2 = "package_2";
    public const string productId_3 = "package_3";

    public const string mineral_0 = "mineral_0";
    public const string mineral_2 = "mineral_2";
    public const string mineral_3 = "mineral_3";
    public const string mineral_4 = "mineral_4";

    public void Init_Func()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) return;

        Instance = this;

        adCount = 0;
        
        AppLovin.InitializeSdk();
        AppLovin.LoadRewardedInterstitial();
        AppLovin.SetUnityAdListener(this.gameObject.name);

        InitializePurchasing();
    }
    #region AppLovin
    public void ShowAD_Func()
    {
        adCount++;

        if(adCount_Show <= adCount)
        {
            adCount = 0;

            AppLovin.PreloadInterstitial();

            if (AppLovin.HasPreloadedInterstitial())
            {
                AppLovin.ShowInterstitial();
            }
            else
            {
                // 전면 광고 송출 실패
            }
        }
    }
    public void ShowReward_Func()
    {
        if (AppLovin.IsIncentInterstitialReady())
        {
            AppLovin.ShowRewardedInterstitial();
        }
    }
    #endregion
    #region InApp
    private bool IsInitialized()
    {
        return (storeController != null && extensionProvider != null);
    }
    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(productId_0, ProductType.Consumable, new IDs
        {
            { productId_0, AppleAppStore.Name },
            { productId_0, GooglePlay.Name },
        });

        builder.AddProduct(productId_1, ProductType.Consumable, new IDs
        {
            { productId_1, AppleAppStore.Name },
            { productId_1, GooglePlay.Name }, }
        );

        builder.AddProduct(productId_2, ProductType.Consumable, new IDs
        {
            { productId_2, AppleAppStore.Name },
            { productId_2, GooglePlay.Name },
        });

        builder.AddProduct(productId_3, ProductType.Consumable, new IDs
        {
            { productId_3, AppleAppStore.Name },
            { productId_3, GooglePlay.Name },
        });

        builder.AddProduct(mineral_0, ProductType.Consumable, new IDs
        {
            { mineral_0, AppleAppStore.Name },
            { mineral_0, GooglePlay.Name },
        });

        builder.AddProduct(mineral_2, ProductType.Consumable, new IDs
        {
            { mineral_2, AppleAppStore.Name },
            { mineral_2, GooglePlay.Name },
        });

        builder.AddProduct(mineral_3, ProductType.Consumable, new IDs
        {
            { mineral_3, AppleAppStore.Name },
            { mineral_3, GooglePlay.Name },
        });

        builder.AddProduct(mineral_4, ProductType.Consumable, new IDs
        {
            { mineral_4, AppleAppStore.Name },
            { mineral_4, GooglePlay.Name },
        });

        UnityPurchasing.Initialize(this, builder);
    }
    public void BuyProductID_Func(int _buyID)
    {
        switch (_buyID)
        {
            case 0:
                BuyProductID(productId_0);
                break;
            case 1:
                BuyProductID(productId_1);
                break;
            case 2:
                BuyProductID(productId_2);
                break;
            case 3:
                BuyProductID(productId_3);
                break;
            case 16:
                BuyProductID(mineral_0);
                break;
            case 17:
                BuyProductID(mineral_2);
                break;
            case 18:
                BuyProductID(mineral_3);
                break;
            case 19:
                BuyProductID(mineral_4);
                break;
        }
    }
    public void BuyProductID(string productId)
    {
        try
        {
            if (IsInitialized())
            {
                Product p = storeController.products.WithID(productId);

                if (p != null && p.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", p.definition.id));
                    storeController.InitiatePurchase(p);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }
    public void RestorePurchase()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions
                (
                    (result) => { Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
                );
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
    public void OnInitialized(IStoreController sc, IExtensionProvider ep)
    {
        Debug.Log("OnInitialized : PASS");

        storeController = sc;
        extensionProvider = ep;
    }
    public void OnInitializeFailed(InitializationFailureReason reason)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + reason);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        switch (args.purchasedProduct.definition.id)
        {
            case productId_0:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(16);
                break;

            case productId_1:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(17);
                break;

            case productId_2:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(18);
                break;

            case productId_3:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(19);
                break;

            case mineral_0:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(0);
                break;

            case mineral_2:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(1);
                break;

            case mineral_3:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(2);
                break;

            case mineral_4:
                Lobby_Manager.Instance.storeRoomClass.BuyStoreGoods_Func(3);
                break;
        }

        return PurchaseProcessingResult.Complete;
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion
}
