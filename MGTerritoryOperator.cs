using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using MGCTerritoryTypeDef;
using LitJson;


public partial class MGTerritoryOperator : MonoBehaviour {	
	private enum BuildingEnum : int
	{
		BUILDING_TOWNVIEW = 0,
		BUILDING_HOME, 
		BUILDING_HERO, 
		BUILDING_MINE, 
		BUILDING_FACTORY, 
		BUILDING_TECH, 
		BUILDING_ALLY,
        BUILDING_SHOP,
		BUILDING_POINT,
		//BUILDING_MEDICLE, 				
		BUILDING_MAX,			
	};

    public enum BottomFunctionEnum : int
    {
        //BOTTOM_BULLETIN,
        BOTTOM_MISSION = 0,
        BOTTOM_WAREHOUSE,
        BOTTOM_FRIEND,
        BOTTOM_MAIL,        

        //BOTTOM_CHAT_GROUP,
        //BOTTOM_CHAT_PUBLIC,
        //BOTTOM_CHAT_PRIVATE,
        //BOTTOM_CHAT_SYSTEM,
        BOTTOM_INVITE,	
        
        BOTTOM_MAX
    };

	public enum FunctionMenuTypeEnum
	{
		FUNCTION_MENU_MAIN,
		FUNCTION_MENU_BUILDING,
		FUNCTION_MENU_BOTTOM,
		FUNCTION_MENU_GOLDSHOP,
		FUNCTION_MENU_ROLLINGEGG,
		FUNCTION_MENU_PORTRAIT,
		FUNCTION_MENU_MAX
	}
	
	
    public bool standAloneUITest = false;
    public MGBarrier m_barrier;
	public MGBroadcast m_broadcast;
    public MGVillagerManager m_villagerMgr;


    public string tutorialFinishConditionTargetButtonName = "";
    public EZInputDelegate tutorialFinishConditionTargetButtonDelegate = null;
    private AutoSpriteControlBase tutorialFinishConditionTargetButtonControl = null;
    private EZInputDelegate currentTutorialFinishConditionTargetButtonDelegate = null;

    public string tutorialDisplayConditionTargetButtonName = "";
    public EZInputDelegate tutorialDisplayConditionTargetButtonDelegate = null;
    private AutoSpriteControlBase tutorialDisplayConditionTargetButtonControl = null;
    private EZInputDelegate currentTutorialDisplayConditionTargetButtonDelegate = null;
	//---------------------------
	//Cache all the necessary UI and main camera.
	//---------------------------
    public UIPanel systemButtonPanel;
    public UIPanel townCameraControlPanel;
    public UIPanel bottomRightPanel;        // panel that contains enter battle/market buttons

    // New UI control references
    public UIStateToggleBtn[] allMainMenuBtnArray;
    //public GameObject[] mainMenuButtonBound;

    public UIPanel leftMainMenuPanel;
    public UIPanel rightMainMenuPanel;
    public UIPanel bottomMainMenuPanel;	
	public UIPanel storeAndWarMenuPanel;

	public List<GameObject> objListToBeHiddenAfterUnfreezed = new List<GameObject>();

    public GameObject territorySubMenuPanel;
        	

	//Upper UI Set
    public UIPanel portraitPanel;
    public UIPanel moneyPanel;
    public UIPanel buffPanel;
    public UIPanel buffTimePanel;
	public SpriteText nameText;
	//public SpriteText levelText;
	public SpriteText goldText;
	public SpriteText crystalText;
	public SpriteText moneyText;
    public SpriteText apText;
    public UIPanel experiencePanel;
    public UIStateToggleBtn stBtn_Ones;
    public UIStateToggleBtn stBtn_Tens;
	//Building Hint Panel
	public UIPanel buildingHintPanel;
	public PackedSprite allyTextImg;
	public PackedSprite crystalTextImg;
	public PackedSprite factoryTextImg;
	public PackedSprite heroTextImg;
	public PackedSprite homeTextImg;
	//public PackedSprite medicalTextImg;
	public PackedSprite techTextImg;
    public PackedSprite marketTextImg;
	
	//AllyMenuPanel
	public UIPanel allyMenuPanel;

	//FactoryMenuPanel
	
    public UIPanel factoryEquipUpgradePanel;
    
	//HeroMenuPanel	
	
		
	//HomeMenuPanel
	
	//MedicalMenuPanel
	public UIPanel medicalMenuPanel;
	//MineMenuPanel

	//TechMenuPanel	
    public UIPanel techInfoPanel;

    public UIPanel getTaxPanel;

    //public UIStateToggleBtn functionPanelOpenCloseBtn;
    //public UIStateToggleBtn CDQueuePanelOpenCloseBtn;

    public GameObject buyCurrencyButtonPrefabRoot;

	public GameObject doubleMsgBoxIconRoot;
	
	// Right-side function menu
	public UIPanel functionMenuBGPanel;

   	public MGDialogue dialogue;
	public MGTutorialOrganizer organizer;

    //Friend On Line Notify Button
    public UIButton FriendOnLineNotifyBtn0;
    public UIButton FriendOnLineNotifyBtn1;
    public UIPanel FriendOnLineNotifyPanel;
    	
	//---------------------------
	//Public variable.
	//---------------------------

	
	//---------------------------
	//Private variable. 
	//---------------------------	 
	//Main Menu Panel
	public UIPanel menuBackBtnPanel;
    public UIButton menuBackButton;
	public UIButton buyCurrencyButton;
	public UIButton openMarketItemNewsPanelButton;
    GameObject commonMenuPanelSet;
    GameObject homeMenuPanelSet;
    UIPanel homeMenuPanel;
    GameObject heroMenuPanelSet;
    UIPanel heroMenuPanel;
    UIPanel trainMenuPanel;
    GameObject mineMenuPanelSet;
    UIPanel mineMenuPanel;
    GameObject factoryMenuPanelSet;
    UIPanel factoryMenuPanel;
    GameObject techMenuPanelSet;
    UIPanel techMenuPanel;
    UIPanel mailMenuPanel;
    UIPanel missionMenuPanel;
    //UIStateToggleBtn m_mainMissionToggle;
    //UIStateToggleBtn m_subMissionToggle;
    //UIStateToggleBtn m_dailyMissionToggle;
    UIPanel goldMarketMainMenuPanel;
    UIPanel goldMarketTabMenuPanel;
    public GameObject[] goldShopMenuButtons;
    UIPanel rollingEggMainMenuPanel;
    UIPanel warehouseMenuPanel;
//    UIPanel marketMenuPanel;
    UIPanel friendMenuPanel;
    UIPanel searchMenuPanel;
    UIPanel buyGoldPrimaryMenuPanel;
    UIPanel buyGoldSecondaryMenuPanel;
    PackedSprite goldEdge;
    PackedSprite goldCircle;
    MGTerritoryOperator_BuyCurrency MGTBuyCurrency;
	
	UIScrollList otherPlayerUnitScrollList;

	private BuildingEnum currentBuildingID = BuildingEnum.BUILDING_TOWNVIEW;	
//	private BuildingEnum firstSelectBuildingID = BuildingEnum.BUILDING_TOWNVIEW;
	private BuildingEnum targetBuildingID = 0;
	private bool isCameraMoving = false;
	//private float accumTime = 0.0f;
	//private Vector3 oriPos;
	//private Vector3 oriRot;	
	//Hero Info Panel
	//private float modelRotateSpeed = 50.0f;
	//Territory 	
	private int playerForce;	
	//GameObject heroGObj;
	//private AssetBundle bundle;

    //[Add By Neo]: Data provided by scene manager when Initial().
    private MGCClientSDK m_clientSDKRef;
    private MGCommonData m_commonDataRef;
    private MGWordsTable m_wordsTableRef;
    private MGTerritoryBuffManager m_territoryBuffMgr;
    public delegate void EnterBattleDelegate();
    private EnterBattleDelegate m_enterBattleDelegate;
    private MGGameGod.SwitchSceneDelegate m_switchSceneDelegate = null;
    private string m_targetScene = "";
    public delegate void ExecuteQuestDelegate(string levelID);
    private ExecuteQuestDelegate m_executeQuestDelegate;

    public delegate void HomeLevelUpDelegate(int homeNewlevel);

    private BuildingEnum singleClickBuilding = BuildingEnum.BUILDING_MAX;
	
	private Item[] m_MailAttachItemSelected;

    private MGSceneMasterTerritory.BackToMainMenuDelegate m_backToMainMenuDelegate;
	
	public delegate void DealBattleDelegate();
    private DealBattleDelegate _m_dealBattleDelegate;
	
	private int unReadFriendInviteCount = 0;

    //payment UI Object
    public GameObject BuyCurrencyPanelObj;
    //StoreKitMgrObj
    private GameObject StoreKitMgrObj;
    //IABAndroidMgrObj
    private GameObject IABAndroidMgrObj;
    //Login Reward UI GameObject
    private GameObject LoginRewardPanelObj;

    private MGTutorialOrganizer TutorialOrganizerComp;
	
	private const string m_territoryDownloadListName = "TerritoryList";
	//Home Hero Rune Panel
	public UIPanel HomeHeroRunePanel;
	
	// Purchase Resource -------------------------
	//Resource Purchase Button
	public UIButton BonusPurchaseBtn;
	public UIButton SilverCoinPurchaseBtn;
	//Resource Purchase Panel
	public UIPanel BuyResourecePanel;
	//How much Gold will pay when Buy Resource
	private int GoldCost = 0;
	//Which resource that player will buy 
	private ResourcePurchaseType ResourceType = ResourcePurchaseType.None;
	//--------------------------------------------
	
	public UIPanel EventMovePanel;
	// If EventProperty = 2, it present that Event will triggered at the percentage of 20%
	private int EventProperty = 2;
	public GameObject OtherPlayerEventPanel;
	//------------------------------------
	
    //this string is the public key of HOU project in Google Play 
    private const string publickey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkOfGlFcplEwdbzPC+aS16Bfo4bAqoPI2HvonZvuEHg7o/eDpfYWcQcsw9zggMnDlcFnEa2KRAtwgvO639X4Zqn0DpJZmfnqJNriI7ZmbvSKPqSO3P+esl4xad72lRXTO6TgCMOX9Phef/UBbyScYgoSLG26BgIegT0haJaz0I93fzSRd8CgGIgNtRRux6dJSkNq08lPfVDA1ETSq1t1TnhyT9KsPdCg5h9Go6BHyahmYJjPq61AzjsZ+msaG/9j+3mcGRZsBRMbaJJ1lD+xGIsbg9VB9zlkPEj8IeHuwhWHso5ECup1zjs2/3VD0xTsQDf2CpRX5gBOjM8DrUr729QIDAQAB";

#if UNITY_IPHONE
        //container of product infomation
        List<StoreKitProduct> ProductInfo = new List<StoreKitProduct>();
#endif
    //container of paymen UI
    List<MGTerritoryOperator_BuyCurrency.ProductInfo> ProductInfoList = new List<MGTerritoryOperator_BuyCurrency.ProductInfo>();

	public static MGTerritoryOperator Instance;
	
	private SpriteText mailCountText;
	private MGRealTimeAnimationPlayer m_speakerControl;
	
	private bool m_isResourcesDownloadComplete = false;
	
	void Awake () {
		useGUILayout = false;
	}
	
	void Start()
	{
        //	syncBox.Hide();	
    }
	
	private void OnApplicationPause(bool isPause){
		//MGAudioManager.StopMusic(false, 0.05f);	
	}

    private void InstantiateAllDynamicPanel()
    {
		InstantiateCDPanel();
		InstantiateEquipHeroInfoPanel();
		if(!MGGameConfig.GetValue<bool>( EObjectDefine.TERRITORY_UI))
		{
			InstantiateEquipUpgradePanel();
			InstantiateGetTaxPanel();
		}
		InstantiateHeroBookPanel();
		InstantiateHeroEquipItemInfoPanel();
		InstantiateHeroInfoPanel();
		InstantiateHeroTrainPanel();
		InstantiateHomeAssignList();
		InstantiateMissionAwardPanel();
		InstantiateMissionInfoPanel();       
		InstantiateRollEggDeck();
		InstantiateSubMenuPanels();
		InstantiateTechInfoPanel();
		InstantiateWarehouseEquipInfoPanel();       
    }
	
    // New UI control functions

    private bool isSubMenuPanelsInstantiated = false;	
    private void InstantiateSubMenuPanels()
    {        
        GameObject territorySubMenuPanelObj = Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, MGResourcePaths.m_UI + "Territory/TerritorySubMenuPanel")) as GameObject;
        MGUITextAdjuster.DoIt(territorySubMenuPanelObj);
        
        commonMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("CommonMenuPanelSet").gameObject;
        goldEdge = commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("GoldEdge").GetComponent<PackedSprite>();
        goldCircle = commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("BuildingInfoBG").FindChild("GoldCircle").GetComponent<PackedSprite>();
        //menuBackButton = commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("BackBtn").gameObject.GetComponent<UIButton>();
        homeMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("HomeMenuPanelSet").gameObject;
        homeMenuPanel = homeMenuPanelSet.transform.FindChild("HomeMainMenuPanel").gameObject.GetComponent<UIPanel>();
        heroMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("HeroMenuPanelSet").gameObject;
        heroMenuPanel = heroMenuPanelSet.transform.FindChild("HeroMainMenuPanel").gameObject.GetComponent<UIPanel>();
        mineMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("MineMenuPanelSet").gameObject;
        mineMenuPanel = mineMenuPanelSet.transform.FindChild("MineMainMenuPanel").gameObject.GetComponent<UIPanel>();
		//trainMenuPanel = heroMenuPanelSet.transform.FindChild("HeroTrainMenuPanel").gameObject.GetComponent<UIPanel>();
		//trainMenuPanel = heroMenuPanelSet.transform.FindChild("MineMainMenuPanel").gameObject.GetComponent<UIPanel>();
        //factoryMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("FactoryMenuPanelSet").gameObject;
        //factoryMenuPanel = factoryMenuPanelSet.transform.FindChild("FactoryMainMenuPanel").gameObject.GetComponent<UIPanel>();
        techMenuPanelSet = territorySubMenuPanelObj.transform.FindChild("TechMenuPanelSet").gameObject;
        techMenuPanel = techMenuPanelSet.transform.FindChild("TechMainMenuPanel").gameObject.GetComponent<UIPanel>();
        mailMenuPanel = territorySubMenuPanelObj.transform.FindChild("MailMenuPanelSet").FindChild("MailMainMenuPanel").gameObject.GetComponent<UIPanel>();
        missionMenuPanel = territorySubMenuPanelObj.transform.FindChild("MissionMenuPanelSet").FindChild("MissionMainMenuPanel").gameObject.GetComponent<UIPanel>();
        //m_mainMissionToggle = missionMenuPanel.transform.FindChild("1_MainMissionBtn").FindChild("1_MainMissionBtn").gameObject.GetComponent<UIStateToggleBtn>();
        //m_subMissionToggle = missionMenuPanel.transform.FindChild("2_SubMissionBtn").FindChild("2_SubMissionBtn").gameObject.GetComponent<UIStateToggleBtn>();
        //m_dailyMissionToggle = missionMenuPanel.transform.FindChild("3_DailyMissionBtn").FindChild("3_DailyMissionBtn").gameObject.GetComponent<UIStateToggleBtn>();
//        goldMarketMainMenuPanel = territorySubMenuPanelObj.transform.FindChild("GoldMarketMenuPanelSet").FindChild("GoldMarketMainMenuPanel").gameObject.GetComponent<UIPanel>();
        goldMarketTabMenuPanel = territorySubMenuPanelObj.transform.FindChild("GoldMarketMenuPanelSet").FindChild("GoldMarketTabMenuPanel").gameObject.GetComponent<UIPanel>();
        //buyGoldPrimaryMenuPanel = territorySubMenuPanelObj.transform.FindChild("BuyGoldMenuPanelSet").FindChild("BuyGoldPrimaryMenuPanel").gameObject.GetComponent<UIPanel>();
        //buyGoldSecondaryMenuPanel = territorySubMenuPanelObj.transform.FindChild("BuyGoldMenuPanelSet").FindChild("BuyGoldSecondaryMenuPanel").gameObject.GetComponent<UIPanel>();
        rollingEggMainMenuPanel = territorySubMenuPanelObj.transform.FindChild("RollingEggMenuPanelSet").FindChild("RollingEggMainMenuPanel").gameObject.GetComponent<UIPanel>();
		for (int i = 0; i < 4; i++)
		{
			goldShopMenuButtons[i] = goldMarketTabMenuPanel.transform.FindChild((i + 1).ToString() + "_Tab" + (i + 1).ToString() + "Btn").gameObject;			
		}
        warehouseMenuPanel = territorySubMenuPanelObj.transform.FindChild("WarehouseMenuPanelSet").FindChild("WarehouseMainMenuPanel").gameObject.GetComponent<UIPanel>();
//        marketMenuPanel = territorySubMenuPanelObj.transform.FindChild("MarketMenuPanelSet").FindChild("MarketMenuPanel").gameObject.GetComponent<UIPanel>();
        friendMenuPanel = territorySubMenuPanelObj.transform.FindChild("FriendMenuPanelSet").FindChild("FriendMainMenuPanel").gameObject.GetComponent<UIPanel>();
        searchMenuPanel = territorySubMenuPanelObj.transform.FindChild("FriendMenuPanelSet").FindChild("SearchMainMenuPanel").gameObject.GetComponent<UIPanel>();
		friendDetailPanel = territorySubMenuPanelObj.transform.FindChild("FriendMenuPanelSet").FindChild("FriendInfoMenuPanel").gameObject.GetComponent<UIPanel>();
		friendUIButtonPanel = friendSearchPanel.transform.parent.FindChild("FriendUIButtonSetPanel").GetComponent<UIPanel>();
        foreach (SpriteBase sprt in territorySubMenuPanelObj.GetComponentsInChildren<SpriteBase>())
        {
            sprt.RenderCamera = UIManager.instance.uiCameras[0].camera;
			
			Transform t = sprt.transform;
			bool isCommonMenuPanelSet = false;
			while( t.parent != null )
			{
				if ( string.Compare(t.parent.name, "CommonMenuPanelSet") == 0) {
					isCommonMenuPanelSet = true;
					break;
				}
				t = t.parent;
			}
			
            if (isCommonMenuPanelSet)
            {                
                //if (string.Compare(sprt.name, "BuyCurrencyBtn") == 0 /*|| string.Compare(sprt.name, "BackBtn") == 0*/)
                //    RegisterSpriteBaseToManager(sprt, transform.parent.FindChild("SpriteManager0").gameObject.GetComponent<SpriteManager>());                
                if (sprt.name.Contains("ButtonBG"))
				{
					//RegisterSpriteBaseToManager(sprt, instSprtMgr);
				}
                else if (sprt.name.Contains("BG"))
                    RegisterSpriteBaseToManager(sprt, transform.parent.FindChild("SpriteManager7").gameObject.GetComponent<SpriteManager>());                
            }
			else if (sprt.name == "TextImg" || sprt.name.Contains("_Update") || sprt.name == "2_EquipDowngradeBtn" || sprt.name == "1_EnchantEquipBtn"
				|| sprt.name == "BuildingLVBG")
                RegisterSpriteBaseToManager(sprt, transform.parent.FindChild("SpriteManagerText").gameObject.GetComponent<SpriteManager>());
            else if (sprt.name == "IconImg")
                RegisterSpriteBaseToManager(sprt, transform.parent.FindChild("SpriteManager0").gameObject.GetComponent<SpriteManager>());                       
            else if (!sprt.name.Contains("MsgCountBG") && !sprt.name.Contains("NewInfoBG"))
                RegisterSpriteBaseToManager(sprt, transform.parent.FindChild("SpriteManager8").gameObject.GetComponent<SpriteManager>());
        }

        transform.parent.FindChild("SpriteManager0").gameObject.GetComponent<SpriteManager>().SortDrawingOrder();
		instSprtMgr.SortDrawingOrder();
        transform.parent.FindChild("SpriteManager7").gameObject.GetComponent<SpriteManager>().SortDrawingOrder();
        transform.parent.FindChild("SpriteManager8").gameObject.GetComponent<SpriteManager>().SortDrawingOrder();
        
        foreach (UIButton btn in territorySubMenuPanelObj.GetComponentsInChildren<UIButton>())
            btn.scriptWithMethodToInvoke = this;
		foreach (UIStateToggleBtn btn in territorySubMenuPanelObj.GetComponentsInChildren<UIStateToggleBtn>())
		{
			btn.scriptWithMethodToInvoke = this;
			btn.AddInputDelegate(TerritorySubmenuToggleButtonDelegate);
		}
        
        territorySubMenuPanelObj.transform.parent = territorySubMenuPanel.transform.parent;
        territorySubMenuPanelObj.transform.localPosition = territorySubMenuPanel.transform.localPosition;
        DestroyImmediate(territorySubMenuPanel);
        territorySubMenuPanel = territorySubMenuPanelObj;

//        InstantiatePanelCommonHandler(territorySubMenuPanel);

        //territorySubMenuPanel.transform.FindChild("CommonMenuPanelSet").FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("BuildingInfoBG").gameObject.GetComponent<UIPanel>().BringIn();
       
        isSubMenuPanelsInstantiated = true;

		// Update all level display from building data cache
		if (buildingInfoCache != null)
			for (int i = 0; i < buildingInfoCache.Length; i++)			
				UpdateBuildingLVDisplay((byte)i, buildingInfoCache[i].buildingLevel);
		SetCurrencyPanel(MGCTerritoryTypeDef.EHudCurrencyType.HUD_CURRENCY_CRYSTAL);
    }

	PackedSprite _m_lastMouseoverSubmenuBG = null;
	private void TerritorySubmenuToggleButtonDelegate(ref POINTER_INFO ptr)
	{		
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE)
		{
			if(commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + ptr.targetObj.name[0]) == null ||
				(lastMenuBtnPressed != null && lastMenuBtnPressed.name == ptr.targetObj.name))
				return;
			if (_m_lastMouseoverSubmenuBG != null)
				_m_lastMouseoverSubmenuBG.SetColor(new Color(MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_R, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_G, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_B));
			_m_lastMouseoverSubmenuBG = commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + ptr.targetObj.name[0]).gameObject.GetComponentInChildren<PackedSprite>();
			_m_lastMouseoverSubmenuBG.SetColor(new Color(MGCTerritoryTypeDef.Constants.DEFAULT_LIST_ITEM_MOUSEOVER_R, MGCTerritoryTypeDef.Constants.DEFAULT_LIST_ITEM_MOUSEOVER_G, MGCTerritoryTypeDef.Constants.DEFAULT_LIST_ITEM_MOUSEOVER_B));
		}
		else if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE_OFF)
		{
			if (_m_lastMouseoverSubmenuBG != null)
				_m_lastMouseoverSubmenuBG.SetColor(new Color(MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_R, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_G, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_B));
		}
	}

    /*
     *     public string tutorialTargetButtonName = "";    
    public EZInputDelegate tutorialTargetButtonDelegate = null;
    private AutoSpriteControlBase tutorialTargetButtonControl = null;
     */

	string _m_lastTutorialFinishConditionTargetButtonName = "";
    public void SetTutorialFinishConditionTargetButtonDelegate(string name, EZInputDelegate del)
    {
		
        if (tutorialFinishConditionTargetButtonControl != null && currentTutorialFinishConditionTargetButtonDelegate != null)
			tutorialFinishConditionTargetButtonControl.RemoveLateInputDelegate(currentTutorialFinishConditionTargetButtonDelegate);
//            tutorialFinishConditionTargetButtonControl.RemoveInputDelegate(currentTutorialFinishConditionTargetButtonDelegate);
		
		GameObject target = null;
				
		foreach(AutoSpriteControlBase child in this.transform.parent.GetComponentsInChildren<AutoSpriteControlBase>())
		{
			if(child.name == name)
			{
				target = child.gameObject;
				break;
			}
		}
		
		if(name == "Machine0Button" && target == null)
		{
			target = goldMarketRollingEggListPanel.transform.FindChild("Machine0").FindChild("Description").FindChild("Button").gameObject;
		}
		else if(name == "Machine2Button" && target == null)
		{
			target = goldMarketRollingEggListPanel.transform.FindChild("Machine2").FindChild("Description").FindChild("Button").gameObject;
		}
		else if(name == "Option1ButtonEasy" && target == null)
		{
			target = m_stageQuickLink.transform.FindChild("DifficultyOptions1/ButtonEasy").gameObject;
		}
		else if(name == "HeroEquipItem00" && target == null)
		{
			if(heroEquipItemScrollList.Count > 0)
				target = heroEquipItemScrollList.GetItem(0).transform.FindChild("HeroEquipItemListItemButton").gameObject;
			else
			{
				m_commonDataRef.avatarEventArgs.Avatar.TutorialProgress += 2;
				TutorialOrganizerComp.SetupProgressEnv(m_commonDataRef.avatarEventArgs.Avatar.TutorialProgress);
				return;
			}
		}
		else if(name == "HeroStartClassChangeButton" && target == null)
		{
			target = heroClassChangePanel.transform.FindChild("HeroClassChangeButtonPanel/HeroStartClassChangeButton").gameObject;
		}
		else if(name == "HeroVictimFree1")
		{
			UIScrollList heroScrollList = heroClassChangePanel.transform.FindChild("VictimHerosPanel/VictimHeroScrollList").GetComponent<UIScrollList>();
			if(heroScrollList.Count <= 0)
			{
				m_commonDataRef.avatarEventArgs.Avatar.TutorialProgress += 5;	//redirect to back button
				TutorialOrganizerComp.SetupProgressEnv(m_commonDataRef.avatarEventArgs.Avatar.TutorialProgress);
				return;
			}
		}
		
        if (target != null && target.GetComponent<AutoSpriteControlBase>() != null)
        {
            tutorialFinishConditionTargetButtonControl = target.GetComponent<AutoSpriteControlBase>();
			if(name != "MissionInfoFunction1Button")
				foreach(AutoSpriteControlBase child in target.GetComponentsInChildren<AutoSpriteControlBase>())
					child.controlIsEnabled = true;
            tutorialFinishConditionTargetButtonControl.AddLateInputDelegate(del);
            currentTutorialFinishConditionTargetButtonDelegate = del;

            TutorialOrganizerComp.SetActiveCircle(target, true);

            tutorialFinishConditionTargetButtonName = "";
            tutorialFinishConditionTargetButtonDelegate = null;
        }
        else
        {
            tutorialFinishConditionTargetButtonName = name;
            tutorialFinishConditionTargetButtonDelegate = del;
        }

		if (target != null			
			&& _m_lastTutorialFinishConditionTargetButtonName == "BackBtn"
			&& (target.transform.parent.parent.name == "MainMenuPanel" || target.transform.parent.name == "BottomFunctionPanel" || target.transform.parent.name == "BattleMarketPanel")
			&& _m_currentMenuType != FunctionMenuTypeEnum.FUNCTION_MENU_MAIN)
		{
			menuBackButton.controlIsEnabled = true;
		}

		_m_lastTutorialFinishConditionTargetButtonName = name;
    }

    public void SetTutorialDisplayConditionTargetButtonDelegate(string name, EZInputDelegate del)
    {
		Debug.Log("SetTutorialDisplayConditionTargetButtonDelegate: " + name);

        if (tutorialDisplayConditionTargetButtonControl != null && currentTutorialDisplayConditionTargetButtonDelegate != null)
            tutorialDisplayConditionTargetButtonControl.RemoveInputDelegate(currentTutorialDisplayConditionTargetButtonDelegate);

		GameObject target = null;
		foreach(AutoSpriteControlBase child in this.transform.parent.GetComponentsInChildren<AutoSpriteControlBase>())
		{
			if(child.name == name)
			{
				child.controlIsEnabled = true;
				target = child.gameObject;
				break;
			}
		}
		
        if (target != null && target.GetComponent<AutoSpriteControlBase>() != null)
        {
            tutorialDisplayConditionTargetButtonControl = target.GetComponent<AutoSpriteControlBase>();
            tutorialDisplayConditionTargetButtonControl.AddInputDelegate(del);
            currentTutorialDisplayConditionTargetButtonDelegate = del;

            tutorialDisplayConditionTargetButtonName = "";
            tutorialDisplayConditionTargetButtonDelegate = null;
        }
        else
        {
            tutorialDisplayConditionTargetButtonName = name;
            tutorialDisplayConditionTargetButtonDelegate = del;
        }
    }

    private void InstantiatePanelCommonHandler(GameObject targetPanel)
    {
		AutoSpriteControlBase findControl = null;
        if (tutorialFinishConditionTargetButtonName.Length != 0)
		{
            foreach (AutoSpriteControlBase ctrl in targetPanel.GetComponentsInChildren<AutoSpriteControlBase>())				
                if (string.Compare(ctrl.name, tutorialFinishConditionTargetButtonName) == 0)
				{
					findControl = ctrl;						
                    break;
				}

			if(findControl != null)
			{
				if (tutorialFinishConditionTargetButtonControl != null && currentTutorialFinishConditionTargetButtonDelegate != null)
					tutorialFinishConditionTargetButtonControl.RemoveLateInputDelegate(currentTutorialFinishConditionTargetButtonDelegate);
				
				tutorialFinishConditionTargetButtonControl = findControl;
				
				tutorialFinishConditionTargetButtonControl.AddLateInputDelegate(tutorialFinishConditionTargetButtonDelegate);
                currentTutorialFinishConditionTargetButtonDelegate = tutorialFinishConditionTargetButtonDelegate;

                TutorialOrganizerComp.SetActiveCircle(tutorialFinishConditionTargetButtonControl.gameObject, true);
			}
        }

        if (tutorialDisplayConditionTargetButtonName.Length != 0)
        {
			findControl = null;
            foreach (AutoSpriteControlBase ctrl in targetPanel.GetComponentsInChildren<AutoSpriteControlBase>())
                if (string.Compare(ctrl.name, tutorialDisplayConditionTargetButtonName) == 0)
                {
					findControl = ctrl;					
                    break;
                }
			
			if(findControl != null)
			{
				 if (tutorialDisplayConditionTargetButtonControl != null && currentTutorialDisplayConditionTargetButtonDelegate != null)
                	tutorialDisplayConditionTargetButtonControl.RemoveInputDelegate(currentTutorialDisplayConditionTargetButtonDelegate);
				
				tutorialDisplayConditionTargetButtonControl = findControl;
                tutorialDisplayConditionTargetButtonControl.AddInputDelegate(tutorialDisplayConditionTargetButtonDelegate);
                currentTutorialDisplayConditionTargetButtonDelegate = tutorialDisplayConditionTargetButtonDelegate;

                TutorialOrganizerComp.SetActiveCircle(tutorialDisplayConditionTargetButtonControl.gameObject, true);
			}
        }
    }

	//private bool _m_isInMainMenu = false;

	private int _m_currentMenuSubType = 0;
	public FunctionMenuTypeEnum _m_currentMenuType = FunctionMenuTypeEnum.FUNCTION_MENU_MAIN;

    private void DismissMainMenuPanel()
    {
		//_m_isInMainMenu = false;

        //if (CDQueuePanelOpenCloseBtn.StateNum != 0)
        //{
            //CDQueuePanelOpenCloseBtn.ToggleState();
            //cdQueuePanelCloseBtnPressed();
            //functionPanelOpenCloseBtn.Hide(true);
        //}
		
		if(m_bIsBuffTimePanel)
		{
			OnBuffPanelEndLongPressed();
		}

        leftMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().Dismiss();
        rightMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().Dismiss();
        bottomMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().Dismiss();
		storeAndWarMenuPanel.Dismiss();

        bottomRightPanel.Dismiss();        
        //cdQueuePanel.transform.localPosition = new Vector3(cdQueuePanel.transform.localPosition.x - 600, cdQueuePanel.transform.localPosition.y, cdQueuePanel.transform.localPosition.z);

        moneyPanel.Dismiss();
        experiencePanel.Dismiss();
        buffPanel.Dismiss();
        portraitPanel.Dismiss();
        systemButtonPanel.Dismiss();
        //townCameraControlPanel.Dismiss();
        MGGossipMgr.Instance.CloseChatButton();
    }

    private void BringInMainMenuPanel()
    {
		//_m_isInMainMenu = true;
		_m_currentMenuType = FunctionMenuTypeEnum.FUNCTION_MENU_MAIN;		

        leftMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().BringIn();       
        rightMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().BringIn();        
        bottomMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().BringIn();
		storeAndWarMenuPanel.BringIn();
        
        bottomRightPanel.BringIn();        
        //cdQueuePanel.transform.localPosition = new Vector3(cdQueuePanel.transform.localPosition.x + 600, cdQueuePanel.transform.localPosition.y, cdQueuePanel.transform.localPosition.z);                

        moneyPanel.BringIn();
        experiencePanel.BringIn();
        buffPanel.BringIn();
        portraitPanel.BringIn();
        systemButtonPanel.BringIn();
        //townCameraControlPanel.BringIn();
        m_backToMainMenuDelegate();
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		InvokeRepeating("CheckUILock", 0, 1.0f);			
#endif
        MGGossipMgr.Instance.ShowChatButton();
    }

#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
	public void CheckUILock()
	{
		if (!UIManager.instance.blockInput)
		{
			CancelInvoke("CheckUILock");
			ReleaseHotKeyLock();

			if (TutorialOrganizerComp.m_isWaitingForResuming)			
				TutorialOrganizerComp.ResumeAdvanceProgress();
			
		}
	}
#endif

    public GameObject GetBottomFunctionBtnByEnum(BottomFunctionEnum btnType)
    {
        switch (btnType)
        {
            //case BottomFunctionEnum.BOTTOM_BULLETIN:
            //    return bottomMainMenuPanel.transform.FindChild("BulletinBtn").gameObject;                
            case BottomFunctionEnum.BOTTOM_FRIEND:
                return rightMainMenuPanel.transform.FindChild("MenuBG").FindChild("FriendBtn").gameObject;
            case BottomFunctionEnum.BOTTOM_MAIL:
                return rightMainMenuPanel.transform.FindChild("MenuBG").FindChild("MailBtn").gameObject;
            case BottomFunctionEnum.BOTTOM_MISSION:
				return rightMainMenuPanel.transform.FindChild("MenuBG").FindChild("MissionBtn").gameObject;
            case BottomFunctionEnum.BOTTOM_WAREHOUSE:
                return rightMainMenuPanel.transform.FindChild("MenuBG").FindChild("WarehouseBtn").gameObject;
            //case BottomFunctionEnum.BOTTOM_CHAT_GROUP:
            //    return bottomMainMenuPanel.transform.FindChild("ChatGroupBtn").gameObject;
            //case BottomFunctionEnum.BOTTOM_CHAT_PUBLIC:
            //    return bottomMainMenuPanel.transform.FindChild("ChatPublicBtn").gameObject;
            //case BottomFunctionEnum.BOTTOM_CHAT_PRIVATE:
            //    {
            //        if (!isSubMenuPanelsInstantiated)
            //            InstantiateSubMenuPanels();
            //        return friendMenuPanel.transform.FindChild("1_MessageBtn").gameObject;
            //    }
            //case BottomFunctionEnum.BOTTOM_CHAT_SYSTEM:
            //    return bottomMainMenuPanel.transform.FindChild("ChatSystemBtn").gameObject;
            case BottomFunctionEnum.BOTTOM_INVITE:
				return bottomMainMenuPanel.transform.FindChild("InviteBtn").gameObject;
        }

        return null;
    }
    
    public void SetBottomFunctionBtnMsgCount(int msgCount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        
        if (msgCount == 0)
        {
            targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text = "";
            targetBtn.transform.FindChild("MsgCountBG").GetComponent<PackedSprite>().renderer.enabled = false;
        }
        else
        {
            targetBtn.transform.FindChild("MsgCountBG").GetComponent<PackedSprite>().renderer.enabled = true;
            targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text = "" + msgCount;
        }
    }
	
	public void SetBottomFunctionBtnNewCount(int newCount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        
        if (newCount == 0)
        {
            targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text = "";
            targetBtn.transform.FindChild("NewInfoBG").GetComponent<PackedSprite>().renderer.enabled = false;
        }
        else
        {
            targetBtn.transform.FindChild("NewInfoBG").GetComponent<PackedSprite>().renderer.enabled = true;
            targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text = "" + newCount;
        }
    }

    public void IncrementBottomFunctionMgsCount(int amount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        int oldCount = 0;
        if( targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text.Length != 0)
            oldCount = int.Parse(targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text);
        SetBottomFunctionBtnMsgCount(oldCount + amount, btnType);
    }
	
	public void DecrementBottomFunctionMgsCount(int amount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        int oldCount = 0;
        if( targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text.Length != 0)
            oldCount = int.Parse(targetBtn.transform.FindChild("MsgCountBG").FindChild("MsgCountText").gameObject.GetComponent<SpriteText>().Text);
		if(oldCount >= amount)
			SetBottomFunctionBtnMsgCount(oldCount - amount, btnType);
    }
	
	public void IncrementBottomFunctionNewCount(int amount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        int oldCount = 0;
        if( targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text.Length != 0)
            oldCount = int.Parse(targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text);
        SetBottomFunctionBtnNewCount(oldCount + amount, btnType);
    }
	
	public void DecrementBottomFunctionNewCount(int amount, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);
        int oldCount = 0;
        if( targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text.Length != 0)
            oldCount = int.Parse(targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text);
		if(oldCount >= amount)
			SetBottomFunctionBtnNewCount(oldCount - amount, btnType);
    }

    public void ClearAllBottomFunctionBtnMsgCount()
    {        
        foreach (BottomFunctionEnum value in BottomFunctionEnum.GetValues(typeof(BottomFunctionEnum)))
            if (value != BottomFunctionEnum.BOTTOM_MAX)            
                SetBottomFunctionBtnMsgCount(0, value);            
    }
   
    public void SetBottomFunctionBtnInfo(string info, BottomFunctionEnum btnType)
    {
        GameObject targetBtn = GetBottomFunctionBtnByEnum(btnType);

        if (info.Length == 0)
        {
			targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text = "";
            targetBtn.transform.FindChild("NewInfoBG").GetComponent<PackedSprite>().renderer.enabled = false;
        }
        else
        {
            targetBtn.transform.FindChild("NewInfoBG").GetComponent<PackedSprite>().renderer.enabled = true;
            targetBtn.transform.FindChild("NewInfoBG").FindChild("NewInfoText").gameObject.GetComponent<SpriteText>().Text = info;
        }
    }

    public void ClearAllBottomFunctionBtnInfo()
    {
        foreach (BottomFunctionEnum value in BottomFunctionEnum.GetValues(typeof(BottomFunctionEnum)))
            if (value != BottomFunctionEnum.BOTTOM_MAX)
                SetBottomFunctionBtnInfo("", value);
    }

	public void AttachColliderToBottomFunctionBtn()
	{
		GetBottomFunctionBtnByEnum(BottomFunctionEnum.BOTTOM_FRIEND).transform.FindChild("BtnCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(BottomFunctionColliderDelegate);
		GetBottomFunctionBtnByEnum(BottomFunctionEnum.BOTTOM_MAIL).transform.FindChild("BtnCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(BottomFunctionColliderDelegate);
		GetBottomFunctionBtnByEnum(BottomFunctionEnum.BOTTOM_WAREHOUSE).transform.FindChild("BtnCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(BottomFunctionColliderDelegate);
		GetBottomFunctionBtnByEnum(BottomFunctionEnum.BOTTOM_MISSION).transform.FindChild("BtnCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(BottomFunctionColliderDelegate);
	}

	public void BottomFunctionColliderDelegate(ref POINTER_INFO ptr)
	{
		UIStateToggleBtn btn = ptr.targetObj.transform.parent.gameObject.GetComponent<UIStateToggleBtn>();
		if (btn == null || !btn.controlIsEnabled)
			return;
		
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.MOVE:
				btn.SetState(1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				btn.SetState(0);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
				btn.SetState(2);
				ptr.targetObj = btn;
				btn.OnInput(ptr);				
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				btn.SetState(0);
				break;
			case POINTER_INFO.INPUT_EVENT.PRESS:
				btn.SetState(2);				
				break;
			case POINTER_INFO.INPUT_EVENT.TAP:
				btn.SetState(2);
				ptr.targetObj = btn;
				btn.OnInput(ptr);
				break;
		}
	}

	private void MainMenuToggleBtnMouseOverDelegate(ref POINTER_INFO ptr)
	{
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP/* || ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE_OFF || ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE*/)
		{
			singleClickBuilding = (BuildingEnum)(int.Parse(ptr.targetObj.name.Substring(ptr.targetObj.name.IndexOf("_") + 1)));
			MainMenuBtnSingleClick(ptr.targetObj.gameObject);
		}
		else if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE)
		{
			if (ptr.targetObj.gameObject.GetComponent<UIStateToggleBtn>().StateName == "Normal")
			{
				MainMenuBtnSingleClick(ptr.targetObj.gameObject);
			}
		}
		else if (ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE_OFF)
		{
			singleClickBuilding = BuildingEnum.BUILDING_MAX;
			UnselectAllMainMenuButton();
		}
	}
	
	private void CloseAllOutUI() 
    {
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HeroExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("TechExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("MarketExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		rightMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
		m_openHomePanel = false;
		m_openHeroPanel = false;
		m_openTechPanel = false;
		m_openMarketPanel = false;
		for(int i=0;i<6;i++)
		{
			if(allMainMenuBtnArray[i].controlIsEnabled == true && !MGSceneMasterTerritory.Instance.m_tutorialOrgnizer.CheckIsDuringTutorial())
				allMainMenuBtnArray[i].SetToggleState(0);
		}
		MGGossipMgr.Instance.OnOpenOtherUI();
		
		SetGetVisitedRewardBtnVisible(true);
		SetRankRewardBtnVisible(true);
		
		DismissSystemPanel(systemButtonPanel.transform.FindChild("SystemBtn").gameObject);
	}
	
	private bool m_openHomePanel = false;
	private void HomeBtnSingleClick(GameObject caller) 
    {
		if(m_openHomePanel)
		{
			caller.transform.parent.FindChild("HomeExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			m_openHomePanel = false;
			SetGetVisitedRewardBtnVisible(true);
			SetRankRewardBtnVisible(true);
		}
		else
		{
			CloseAllOutUI();
			caller.transform.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			caller.transform.parent.FindChild("HomeExtend").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			caller.transform.parent.FindChild("HomeExtend").FindChild("Tax").FindChild("Text").GetComponent<SpriteText>().Text = 
				m_wordsTableRef.getUIText("0200022") + m_commonDataRef.playerDetailedInfo.availableNum + "/" + m_commonDataRef.playerDetailedInfo.availableNumMax;
			
       		MGCDelegateMgr.instance.Building_INFO -= getHomeUpdateData;
        	MGCDelegateMgr.instance.Building_INFO += getHomeUpdateData;
			SendBuildingInfoREQ((byte)MGCTypeDef.BUILDING_ID.MAIN_TOWN);
					
			SendChargeInfoOutUIREQ(MGCTypeDef.CHARGE_TYPE.CHARGE_FORCED_LEVY, MGCTypeDef.COOLDOWN_TYPE.COOLDOWN_LEVY, forceTaxData);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			
			m_openHomePanel = true;
			/*
			MGCDelegateMgr.instance.Building_INFO -= GetMineUpdateData;
        	MGCDelegateMgr.instance.Building_INFO += GetMineUpdateData;
        	SendBuildingInfoREQ((byte)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE);
			*/
			MGCDelegateMgr.instance.MineInfo -= GetCrystalMineInfo;
        	MGCDelegateMgr.instance.MineInfo += GetCrystalMineInfo;

        	if (bminfoeaCache == null)
			{
            	m_clientSDKRef.getBNOMineInfo();
			}
        	else
			{
            	GetCrystalMineInfo(this, bminfoeaCache);
			}
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			
			if(m_battleShieldOpen)
			{
				bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
				m_battleShieldOpen = false;
			}
			SetGetVisitedRewardBtnVisible(false);
			SetRankRewardBtnVisible(false);
			CloseOtherPlayerEvent();
		}
	}
	
	private void getHomeUpdateData(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.Building_INFO -= getHomeUpdateData;
        MGCBuilding_INFOEventArgs buildingInfo = (MGCBuilding_INFOEventArgs)bea;
        if (buildingInfo.buildingInfo.buildingType != (byte)MGCTypeDef.BUILDING_ID.MAIN_TOWN)
            return;
        buildingInfoCache[buildingInfo.buildingInfo.buildingType] = buildingInfo.buildingInfo;
		
    	MGTerritoryBuildingData buildingData = GetBuildingDataFromCache(buildingInfo.buildingInfo.buildingObjectId, buildingInfo.buildingInfo.buildingLevel);

		//if (buffedGetTaxAmount > 0 && buffedGetTaxAmount != buildingData.m_operationGain)
        if (buildingInfo.buildingInfo.buildingOperationBonus > 0 )
		{
            leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("TaxGet").GetComponent<SpriteText>().Text =
                "" + Color.green + Math.Floor((buildingData.m_operationGain * (1 + ((float)buildingInfo.buildingInfo.buildingOperationBonus / 100))));

            leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Bonus").FindChild("BonusText").GetComponent<SpriteText>().Text =
                "" + Color.green + buildingInfo.buildingInfo.buildingOperationBonus + "%";
		}
		else
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("TaxGet").GetComponent<SpriteText>().Text =
				"" + buildingData.m_operationGain;

            leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Bonus").FindChild("BonusText").GetComponent<SpriteText>().Text =
                buildingInfo.buildingInfo.buildingOperationBonus + "%";
		}
		if(m_commonDataRef.playerDetailedInfo.availableNum > 0)
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Tax").GetComponent<UIButton>().controlIsEnabled = true;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Tax").GetComponent<UIButton>().SetState(0);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").GetComponent<UIButton>().controlIsEnabled = true;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").GetComponent<UIButton>().SetState(0);
		}
		else
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Tax").GetComponent<UIButton>().controlIsEnabled = false;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Tax").GetComponent<UIButton>().SetState(3);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").GetComponent<UIButton>().controlIsEnabled = false;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").GetComponent<UIButton>().SetState(3);
		}
		
		MGCDelegateMgr.instance.Building_INFO -= GetMineUpdateData;
        MGCDelegateMgr.instance.Building_INFO += GetMineUpdateData;
        SendBuildingInfoREQ((byte)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE);
		
		MGSceneMasterTerritory.Instance.m_tutorialOrgnizer.LockDynamicUISet(leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Tax"));
	}
	
	private void forceTaxData(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.ChargeInfo -= forceTaxData;
		MGCGeneralChargeInfoEventArgs bgciea = (MGCGeneralChargeInfoEventArgs)bea;
		if(bgciea.ChargeType != MGCTypeDef.CHARGE_TYPE.CHARGE_FORCED_LEVY)
			return;
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").FindChild("Text").GetComponent<SpriteText>().Text = 
			""+bgciea.Money;
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("ForceTax").FindChild("Word").GetComponent<SpriteText>().Text = 
			m_wordsTableRef.getUIText("0202008").Replace("%N", System.Environment.NewLine);
		
		SendChargeInfoOutUIREQ(MGCTypeDef.CHARGE_TYPE.CHARGE_FORCED_MINING, MGCTypeDef.COOLDOWN_TYPE.COOLDOWN_MINING, forceMineData);
	}
	
	private bool m_openHeroPanel = false;
	private void HeroBtnSingleClick(GameObject caller) 
    {
		if(m_openHeroPanel)
		{
			caller.transform.parent.FindChild("HeroExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			m_openHeroPanel = false;
		}
		else
		{
			CloseAllOutUI();
			caller.transform.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			caller.transform.parent.FindChild("HeroExtend").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			MGCDelegateMgr.instance.Building_INFO -= getHeroHouseUpdateData;
			MGCDelegateMgr.instance.Building_INFO += getHeroHouseUpdateData;
			SendBuildingInfoREQ((byte)MGCTypeDef.BUILDING_ID.HERO_ROOM);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			m_openHeroPanel = true;
			
			if(m_battleShieldOpen)
			{
				bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
				m_battleShieldOpen = false;
			}
			CloseOtherPlayerEvent();
		}
	}
	
	private void getHeroHouseUpdateData(object sender, MGCBaseEventArgs bea)
	{
		MGCDelegateMgr.instance.Building_INFO -= getHeroHouseUpdateData;
		MGCBuilding_INFOEventArgs buildingInfo = (MGCBuilding_INFOEventArgs)bea;
		if (buildingInfo.buildingInfo.buildingType != (byte)MGCTypeDef.BUILDING_ID.HERO_ROOM)
			return;		
		buildingInfoCache[buildingInfo.buildingInfo.buildingType] = buildingInfo.buildingInfo;		
		MGTerritoryBuildingData buildingData = GetBuildingDataFromCache(buildingInfo.buildingInfo.buildingObjectId, buildingInfo.buildingInfo.buildingLevel);
		
		//string levelText = m_wordsTableRef.getUIText("0201004") + "Lv." + buildingInfo.buildingInfo.buildingLevel;
		//leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HeroExtend").FindChild("LevelText").GetComponent<SpriteText>().Text = levelText;
	}
	
	private void GetMineUpdateData(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.Building_INFO -= GetMineUpdateData;
        MGCBuilding_INFOEventArgs buildingInfo = (MGCBuilding_INFOEventArgs)bea;

        if (buildingInfo.buildingInfo.buildingType != (byte)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE)
            return;
        buildingInfoCache[buildingInfo.buildingInfo.buildingType] = buildingInfo.buildingInfo;
		
        MGTerritoryBuildingData buildingData = GetBuildingDataFromCache(buildingInfo.buildingInfo.buildingObjectId, buildingInfo.buildingInfo.buildingLevel);
		
		if(buildingInfoCache[(int)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE].buildingOperationBonus > 0)
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("GetEach").GetComponent<SpriteText>().Text 
			= Color.green + "" + Math.Ceiling(buildingData.m_operationGain * (1.0f + buildingInfoCache[(int)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE].buildingOperationBonus /100.0f));
		}
		else
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("GetEach").GetComponent<SpriteText>().Text 
				= "" + buildingData.m_operationGain;
		}
		
		string levelText = m_wordsTableRef.getUIText("0201005") + "Lv." + buildingInfo.buildingInfo.buildingLevel;
		//leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("LevelText").GetComponent<SpriteText>().Text = levelText;
		//leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Mine").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0202018") + m_wordsTableRef.GetIconText("SILVER") + buildingData.m_operationCost;
		if(buildingData.m_operationCost > 0)
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("PayEach").GetComponent<SpriteText>().Text 
				= "" + buildingData.m_operationCost;
		}
		
		if(currentMineAmount > 0)
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Mine").GetComponent<UIButton>().controlIsEnabled = true;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Mine").GetComponent<UIButton>().SetState(0);
		}
		else
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Mine").GetComponent<UIButton>().controlIsEnabled = false;
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("Mine").GetComponent<UIButton>().SetState(3);
		}
		
		if(buildingInfoCache[(int)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE].buildingOperationBonus > 0)
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("MineBonus").FindChild("MineBonus").GetComponent<SpriteText>().Text 
				= Color.green + "" + buildingInfoCache[(int)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE].buildingOperationBonus + "%";
		}
		else
		{
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("MineBonus").FindChild("MineBonus").GetComponent<SpriteText>().Text 
				= buildingInfoCache[(int)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE].buildingOperationBonus + "%";
		}
		
		//Refresh the Crystal max number
		m_clientSDKRef.getBNOMineInfo();
	}
	
	private void forceMineData(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.ChargeInfo -= forceMineData;
		MGCGeneralChargeInfoEventArgs bgciea = (MGCGeneralChargeInfoEventArgs)bea;
		if(bgciea.ChargeType != MGCTypeDef.CHARGE_TYPE.CHARGE_FORCED_MINING)
			return;
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("MineFromOut").FindChild("Cost").GetComponent<SpriteText>().Text = ""+bgciea.Money;
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("HomeExtend").FindChild("MineFromOut").FindChild("Text").GetComponent<SpriteText>().Text = 
			m_wordsTableRef.getUIText("0202081").Replace("%N", System.Environment.NewLine);
	}
	
	private bool m_openTechPanel = false;
	private void TechBtnSingleClick(GameObject caller) 
    {
		if(m_openTechPanel)
		{
			caller.transform.parent.FindChild("TechExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			m_openTechPanel = false;
		}
		else
		{
			CloseAllOutUI();
			caller.transform.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			caller.transform.parent.FindChild("TechExtend").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			MGCDelegateMgr.instance.Building_INFO -= GetTechUpdateData;
        	MGCDelegateMgr.instance.Building_INFO += GetTechUpdateData;
        	SendBuildingInfoREQ((byte)MGCTypeDef.BUILDING_ID.COLLEGE);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			m_openTechPanel = true;
			
			if(m_battleShieldOpen)
			{
				bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
				m_battleShieldOpen = false;
			}
			CloseOtherPlayerEvent();
		}
	}
	
	private void GetTechUpdateData(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.Building_INFO -= GetTechUpdateData;
        MGCBuilding_INFOEventArgs buildingInfo = (MGCBuilding_INFOEventArgs)bea;
        if (buildingInfo.buildingInfo.buildingType != (byte)MGCTypeDef.BUILDING_ID.COLLEGE)
            return;
        buildingInfoCache[buildingInfo.buildingInfo.buildingType] = buildingInfo.buildingInfo;
		
		string levelText = m_wordsTableRef.getUIText("0201007") + "Lv." + buildingInfo.buildingInfo.buildingLevel;
		leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("TechExtend").FindChild("LevelText").GetComponent<SpriteText>().Text = levelText;
	}
	
	private bool m_openMarketPanel = false;
	private void MarketBtnSingleClick(GameObject caller) 
    {
		if(m_openMarketPanel)
		{
			caller.transform.parent.FindChild("MarketExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			m_openMarketPanel = false;
		}
		else
		{
			CloseAllOutUI();
			caller.transform.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			caller.transform.parent.FindChild("MarketExtend").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			leftMainMenuPanel.transform.FindChild("MenuBG").FindChild("BackLight").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			m_openMarketPanel = true;
			
			if(m_battleShieldOpen)
			{
				bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
				m_battleShieldOpen = false;
			}
			CloseOtherPlayerEvent();
		}
	}
	
    private float timeToStartBringIn = 0.0f;
    private void MainMenuBtnSingleClick(GameObject caller)   // first click: display button name; second click: enter sub-function
    {        
        BuildingEnum clickedBtn = (BuildingEnum)(int.Parse(caller.name.Substring(caller.name.IndexOf("_") + 1)));
        UnselectAllMainMenuButton();
        if (singleClickBuilding == clickedBtn)  // go to sub-function
        {
			bool isPressSuccess = false;
            /*iTween.CameraFadeAdd();
            GameObject cameraFadeObject = GameObject.Find("iTween Camera Fade");
            // Set fade obj only to take effect on main camera
            if (cameraFadeObject)
                cameraFadeObject.layer = 1;
            iTween.CameraFadeTo(1, 3.0f);            */            
            switch (clickedBtn)
            {
                case BuildingEnum.BUILDING_HOME:
					isPressSuccess = homeBtnPressed();					
                    break;
                case BuildingEnum.BUILDING_HERO:
					isPressSuccess = heroBtnPressed();
                    break;
                case BuildingEnum.BUILDING_MINE:
					isPressSuccess = mineBtnPressed();
                    break;
                case BuildingEnum.BUILDING_FACTORY:
					isPressSuccess = factoryBtnPressed();
                    break;
                case BuildingEnum.BUILDING_TECH:
					isPressSuccess = techBtnPressed();
                    break;
                case BuildingEnum.BUILDING_ALLY:
                    // Not implemented yet
//					isPressSuccess = marketBtnPressed();
                    break;
                case BuildingEnum.BUILDING_SHOP:
//					isPressSuccess = marketBtnPressed();
                    break;
            }

			if (isPressSuccess)
			{
				singleClickBuilding = BuildingEnum.BUILDING_MAX;
				ShowMaskCube(true);
			}
        }
        else
        {
			
            singleClickBuilding = clickedBtn;            
            // display small text of building             
            switch (clickedBtn)
            {
                case BuildingEnum.BUILDING_HOME:
                    allMainMenuBtnArray[0].SetToggleState("Selected");                                                                              
                    timeToStartBringIn = Time.timeSinceLevelLoad;
                    allMainMenuBtnArray[0].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_HERO:                    
                    allMainMenuBtnArray[1].SetToggleState("Selected");                    
                    timeToStartBringIn = Time.timeSinceLevelLoad;
                    allMainMenuBtnArray[1].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_MINE:
                    // test villager
                    /*MGVillagerEvent vEvent = new MGVillagerEvent();
                    vEvent.m_methodToInvoke = "QuestionMarkPressed";
                    m_villagerMgr.GetVillager("Test").PopEvent(vEvent);*/

                    allMainMenuBtnArray[2].SetToggleState("Selected");
                    allMainMenuBtnArray[2].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_FACTORY:
                    allMainMenuBtnArray[3].SetToggleState("Selected");
                    allMainMenuBtnArray[3].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_TECH:
                    // test villager
                    /*m_villagerMgr.GetVillager("Test").SetDirection(EVilliagerDirection.E_VILLIAGER_DIRECTION_DR);
                    m_villagerMgr.GetVillager("Test2").SetDirection(EVilliagerDirection.E_VILLIAGER_DIRECTION_DL);
                    currentITweenpathName = "CameraAnimPath_" + playerForce + "_" + BuildingEnum.BUILDING_HOME.ToString();                        
                    m_villagerMgr.GetVillager("Test").transform.position = iTweenPath.GetPath(currentITweenpathName)[0];
                    currentITweenpathName = "CameraAnimPath_" + playerForce + "_" + BuildingEnum.BUILDING_HERO.ToString();
                    m_villagerMgr.GetVillager("Test2").transform.position = iTweenPath.GetPath(currentITweenpathName)[0];*/

                    allMainMenuBtnArray[4].SetToggleState("Selected");
                    allMainMenuBtnArray[4].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_ALLY:
                    //allMainMenuBtnArray[5].SetToggleState("Selected");
                    //allMainMenuBtnArray[5].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    allMainMenuBtnArray[5].SetToggleState("Selected");
                    allMainMenuBtnArray[5].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
                case BuildingEnum.BUILDING_SHOP:
                    allMainMenuBtnArray[5].SetToggleState("Selected");
                    allMainMenuBtnArray[5].transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().BringIn();
                    break;
            }

            // display button bound
            //caller.transform.parent.FindChild("ButtonBound").position = caller.transform.position;
        }
    }

    public void OnTestFastBringInToggle(GameObject caller)
    {
        if (caller.GetComponent<UIStateToggleBtn>().StateNum == 0)
        {
            //debug.log("UI panel fast bringin/dismiss OFF");
            UIManager.instance.isUsingFastTransitions = false;
        }
        else
        {
            //debug.log("UI panel fast bringin/dismiss ON");
            UIManager.instance.isUsingFastTransitions = true;
        }
    }

    private void OnHomeTextPicEndBringin(UIPanelBase panel, EZTransition transition)
    {
        ////debug.log("End TextPic bring in: " + Time.timeSinceLevelLoad);
        moneyText.Text = "" + timeToStartBringIn + "->" + Time.timeSinceLevelLoad;
    }

    private void UnselectAllMainMenuButton()
    {
        //UIStateToggleBtn[] allMainMenuBtnArray = mainMenuPanel.transform.FindChild("MenuBG").GetComponentsInChildren<UIStateToggleBtn>();
        		
        foreach (UIStateToggleBtn btn in allMainMenuBtnArray)
        {
			if(btn.controlIsEnabled)
            	btn.SetToggleState("Normal");
            btn.transform.FindChild("TextPic").gameObject.GetComponent<UIPanel>().Dismiss();            
        }

        //foreach (GameObject btnBound in mainMenuButtonBound)
            //btnBound.transform.position = new Vector3(0, 0, -200.0f);
    }
		
	private void medicleBtnPressed()
	{		
		//moveToTargetBuilding(BuildingEnum.BUILDING_MEDICLE);
	}
	
	private void allyBtnPressed()
	{		
		moveToTargetBuilding(BuildingEnum.BUILDING_ALLY);
	}

    private void OnLoadMail(object sender, MGCBaseEventArgs ea)
    {
        m_barrier.Hide();
        MGCLoadMailEventArgs mailArgs = (MGCLoadMailEventArgs)ea;

        Mail[] myMail = mailArgs.getMails();
        //debug.log("On mail");
        if (myMail != null && myMail.Length != 0)
        {
            //debug.log("mail count of this page = " + myMail.Length);
            //foreach (Mail mail in myMail)
            //{
                //debug.log("mail:" + mail.Content);
            //}
        }
        
    }
	
	private bool m_battleShieldOpen = false;
	private void battleShieldBtnPressed()
	{		
		m_battleShieldOpen = !m_battleShieldOpen;
		if(m_battleShieldOpen)
		{
			bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(1.0f, 1.0f, 1.0f);
			
			if(MGSceneMasterTerritory.Instance.IsPvpOutofTime)
			{
				bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").gameObject.SetActive(false);
				bottomRightPanel.transform.FindChild("BattleExtend").FindChild("PVPBtn").gameObject.SetActive(false);
			}
			else
			{
				bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").gameObject.SetActive(true);
				bottomRightPanel.transform.FindChild("BattleExtend").FindChild("PVPBtn").gameObject.SetActive(true);
				if(bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").GetComponent<UIButton>().controlIsEnabled == true)
				{
					//bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").GetComponent<Animation>().Play("PPVEBtnShake");
				}
			}
		}
		else
		{
			bottomRightPanel.transform.FindChild("BattleExtend").localScale = new Vector3(0.001f, 0.001f, 1.0f);
			//bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").GetComponent<Animation>().Stop("PPVEBtnShake");
		}
		if(MGGameConfig.GetValue<bool>( EObjectDefine.TERRITORY_UI))
		{
			CloseAllOutUI();
		}
	}
	
    private void battleBtnPressed()
	{
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
			return;
#endif
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
		m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
		
		if (heroListArgsCache == null)
			UpdateHeroInfo(DealEnterBattle);
		else
		{
			MGCommonData.hiredHeroList = new List<ClientHeroInfo>( heroListArgsCache._m_heroList.hiredHeroInfoArray);
			DealEnterBattle();
		}
	}
	
	private void PVP1V1BtnPressed()
	{
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
			return;
#endif
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
		
		m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
		Invoke("hidebarrier",30.0f);
		
		if (heroListArgsCache == null)
			UpdateHeroInfo(DealEnterPVP1V1);
		else
		{
			MGCommonData.hiredHeroList = new List<ClientHeroInfo>( heroListArgsCache._m_heroList.hiredHeroInfoArray);
			DealEnterPVP1V1();
		}
	}
	void hidebarrier()
	{
		m_barrier.Hide();
	}
	private void PVP2V2BtnPressed()
	{
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
			return;
#endif
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
		
		m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
		
		if (heroListArgsCache == null)
			UpdateHeroInfo(DealEnterPVP2V2);
		else
		{
			MGCommonData.hiredHeroList = new List<ClientHeroInfo>( heroListArgsCache._m_heroList.hiredHeroInfoArray);
			DealEnterPVP2V2();
		}
	}

    private void PPVEBtnPressed()
    {
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
		{
			Debug.LogWarning("GetHotKeyLock");
			return;
		}
#endif
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
		
		if (heroListArgsCache == null)
			UpdateHeroInfo(DealEnterPPVE);
		else
		{
			MGCommonData.hiredHeroList = new List<ClientHeroInfo>( heroListArgsCache._m_heroList.hiredHeroInfoArray);
			DealEnterPPVE();
		}
    }
	
	private void ArenaBtnPressed()
    {
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
		{
			Debug.LogWarning("GetHotKeyLock");
			return;
		}
#endif
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
		
		if (heroListArgsCache == null)
			UpdateHeroInfo(DealEnterArena);
		else
		{
			MGCommonData.hiredHeroList = new List<ClientHeroInfo>( heroListArgsCache._m_heroList.hiredHeroInfoArray);
			DealEnterArena();
		}
    }
	
	private void OnConfirmBtnPressed(ref POINTER_INFO ptr)
	{
		if(ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
		{
			RestoreMainCameraRaycast();
			ReleaseHotKeyLock();
		    popUpSingleButtonPanel.Dismiss();
			popUpMaskPanel.Dismiss();
		}
	}
	
	private void UpdateHeroInfo( DealBattleDelegate whichDelegate){
		_m_heroListReady = false;
		_m_dispatchHeroListReady = false;
		_m_dealBattleDelegate = whichDelegate;
		
		postHeroListHandler = GetHeroList;
		//SetBNOHeroListHandler(OnBNOHeroListWithPostHandler);
		SendHeroListInfoUpdateREQ();
		
		postDispatchHeroListHandler = GetDispatchHeroList;
		IsDispatchHeroInitialize();
	}
	
	
	bool _m_heroListReady= false;
	private void GetHeroList( object sender, MGCBaseEventArgs ea ){

		postHeroListHandler = null;
		_m_heroListReady = true;
		if( _m_dispatchHeroListReady )
			_m_dealBattleDelegate( );
	}
	bool _m_dispatchHeroListReady= false;
	private void GetDispatchHeroList( object sender, MGCBaseEventArgs ea ){
		postDispatchHeroListHandler = null;		

		_m_dispatchHeroListReady = true;
		if( _m_heroListReady )
			_m_dealBattleDelegate( );
	}
	
	private void DealEnterBattle( ){

#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		ReleaseHotKeyLock();
#endif
		m_barrier.Hide();
		m_VisitFrinedData.CheckALLHeorCDTime();
		if( MGSceneMasterTerritory.Instance.IsAnyHeroEnabledBattle() ) {
			if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 0;
            	TryToSwitchScene("LevelSelect");
			}
		}
		else {
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			pageStyle.message = m_wordsTableRef.getUIText( "0301109" );
			pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
			SetPopUpPage(pageStyle);
		}
	}
	
	private void DealEnterPVP1V1( ){

#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		ReleaseHotKeyLock();
#endif
		m_barrier.Hide();
		m_VisitFrinedData.CheckALLHeorCDTime();
		if( MGSceneMasterTerritory.Instance.IsAnyHeroEnabledBattle() ) {
			if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 1;
            	TryToSwitchScene("LevelSelect");
			}
		}
		else {
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			pageStyle.message = m_wordsTableRef.getUIText( "0301109" );
			pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
			SetPopUpPage(pageStyle);
		}
	}
	
	private void DealEnterPVP2V2( ){

#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		ReleaseHotKeyLock();
#endif
		m_barrier.Hide();
		m_VisitFrinedData.CheckALLHeorCDTime();
		if( MGSceneMasterTerritory.Instance.IsAnyHeroEnabledBattle() ) {
			if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 2;
            	TryToSwitchScene("LevelSelect");
			}
		}
		else {
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			pageStyle.message = m_wordsTableRef.getUIText( "0301109" );
			pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
			SetPopUpPage(pageStyle);
		}
	}
	
	private void DealEnterPPVE( ){
		#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		ReleaseHotKeyLock();
#endif
		m_barrier.Hide();
		m_VisitFrinedData.CheckALLHeorCDTime();
		if( MGSceneMasterTerritory.Instance.IsAnyHeroEnabledBattle() ) {
			if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 3;
            	TryToSwitchScene("LevelSelect");
			}
		}
		else {
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			pageStyle.message = m_wordsTableRef.getUIText( "0301109" );
			pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
			SetPopUpPage(pageStyle);
		}
	}
	
	private void DealEnterArena()
	{
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		ReleaseHotKeyLock();
#endif
		m_barrier.Hide();
		m_VisitFrinedData.CheckALLHeorCDTime();
		if(MGSceneMasterTerritory.Instance.IsAnyHeroEnabledBattle())
		{
			if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 4;
            	TryToSwitchScene("LevelSelect");
			}
		}
		else
		{
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			pageStyle.message = m_wordsTableRef.getUIText( "0301109" );
			pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
			SetPopUpPage(pageStyle);
		}
	}
	
	private void AskLogout() {
		functionErrorHandleMsgBoxStyle_Double = new PopUpPage_DoubleStyle();
		functionErrorHandleMsgBoxStyle_Double.popUpType = (byte)PopUpPage_DoubleStyle.PopUpType.DOUBLE_BUTTON;
		functionErrorHandleMsgBoxStyle_Double.message = m_wordsTableRef.getUIText("0600003");
		functionErrorHandleMsgBoxStyle_Double.functionButtonText = m_wordsTableRef.getUIText("0202003");
		functionErrorHandleMsgBoxStyle_Double.functionButtonDelegate = TryLogout;
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		functionErrorHandleMsgBoxStyle_Double.cancelButtonDelegate = BattleCancelBtnPressed;
#endif
		SetPopUpPage(functionErrorHandleMsgBoxStyle_Double);
	}
	
	private void TryLogout( ref POINTER_INFO ptr ){
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
		{
			if (m_switchSceneDelegate != null)
			{
				m_territoryBuffMgr.Reset();
				m_clientSDKRef.LogoutAccount();
				MGGameGod.Instance.m_AccountManager.ResetAccount();
				
				if(MGSaveManager.HasKey(MGExternalAccountManager.SaveTokenKey))
					MGSaveManager.DeleteKey(MGExternalAccountManager.SaveTokenKey);
				
				m_commonDataRef.newMailCount = 0;
				m_commonDataRef.QuestGuideProcess = 0;
				m_switchSceneDelegate("Login");
			}
		}
	}

	private void BattleCancelBtnPressed(ref POINTER_INFO ptr)
	{
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
		{
			popUpPanelOKPressed();
			ReleaseHotKeyLock();
		}
	}

    private void BattleConfirmBtnPressed(ref POINTER_INFO ptr)
    {
        if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
        {
            //MGAudioManager.PlaySound("SelectHero");
			MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
            //if (m_enterBattleDelegate != null)
            //    m_enterBattleDelegate();			
			popUpPanelOKPressed();
            if (m_switchSceneDelegate != null)
			{
                //TryToSwitchScene("Battle");
				m_commonDataRef.pvpType = 0;
                TryToSwitchScene("LevelSelect");
			}
        }
    }
	
	private void PVP1V1ConfirmBtnPressed(ref POINTER_INFO ptr)
    {
        if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
        {
			MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
            //if (m_enterBattleDelegate != null)
            //    m_enterBattleDelegate();			
			popUpPanelOKPressed();
            if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 1;
                TryToSwitchScene("LevelSelect");
			}
        }
    }
	
	private void PVP2V2ConfirmBtnPressed(ref POINTER_INFO ptr)
    {
        if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
        {
			MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_SELECTHERO);
            //if (m_enterBattleDelegate != null)
            //    m_enterBattleDelegate();			
			popUpPanelOKPressed();
            if (m_switchSceneDelegate != null)
			{
				m_commonDataRef.pvpType = 2;
                TryToSwitchScene("LevelSelect");
			}
        }
    }
		
	//---------------------------
	//UpperPanel Button
	//---------------------------
	public void BringinSystemPanel(GameObject caller)
	{
        caller.GetComponent<UIButton>().methodToInvoke = "DismissSystemPanel";
        //CDQueuePanelOpenCloseBtn.Hide(true);
        systemButtonPanel.transform.FindChild("SystemContentBG").gameObject.GetComponent<UIBistateInteractivePanel>().BringIn();

		if (MGAudioManager.GetBGMMute())
		{
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>();

			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
		}
		else
		{
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().SetToggleState(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>();

			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
		}

		if (MGAudioManager.GetSEMute())
		{
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().SetToggleState(1);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>();

			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
		}
		else
		{
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().SetToggleState(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>();

			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
		}
	}
    
    public void DismissSystemPanel(GameObject caller)
    {
        caller.GetComponent<UIButton>().methodToInvoke = "BringinSystemPanel";
        //CDQueuePanelOpenCloseBtn.Hide(false);
        systemButtonPanel.transform.FindChild("SystemContentBG").gameObject.GetComponent<UIBistateInteractivePanel>().Dismiss();
    }

    private void OnBGMBtnPressed(GameObject caller)
    {
		if (caller.GetComponent<UIStateToggleBtn>().StateName == "Normal")
		{
			MGAudioManager.SetBGMMute(false);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>();
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
		}
		else
		{
			MGAudioManager.SetBGMMute(true);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>();
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("BGMButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
		}
    }

    private void OnSEBtnPressed(GameObject caller)
    {
		if (caller.GetComponent<UIStateToggleBtn>().StateName == "Normal")
		{
			MGAudioManager.SetSEMute(false);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>();
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
		}
		else
		{
			MGAudioManager.SetSEMute(true);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").gameObject.GetComponent<UIStateToggleBtn>().layers[1]
				= systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>();
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_On").gameObject.GetComponent<PackedSprite>().SetDrawLayer(0);
			systemButtonPanel.transform.FindChild("SystemContentBG").FindChild("SEButton").FindChild("Over_Off").gameObject.GetComponent<PackedSprite>().SetDrawLayer(3);
		}
    }

	//---------------------------
	//AllyMenuPanel Button
	//---------------------------
    private void allyMenuBackBtnPressed()
	{
		if(isCameraMoving)
			return;		
		moveToTargetBuilding(BuildingEnum.BUILDING_TOWNVIEW);
		allyMenuPanel.Dismiss();
		//mainMenuPanel.BringIn();
        //HideRightMenuObj(false);
	}
		    	
	//---------------------------
	//MedicalMenuPanel Button
	//---------------------------
    private void medicalMenuBackBtnPressed()
	{
		if(isCameraMoving)
			return;		
		moveToTargetBuilding(BuildingEnum.BUILDING_TOWNVIEW);
		medicalMenuPanel.Dismiss();
		//mainMenuPanel.BringIn();
        //HideRightMenuObj(false);
        
	}

	private void enableBuildMenu(BuildingEnum buildingEnum)
	{
        // Common part
		if (buildingEnum != BuildingEnum.BUILDING_TOWNVIEW)
		{
			//mainCamera.gameObject.active = false;
			commonMenuPanelSet.transform.FindChild("MenuButtonPanel").gameObject.GetComponent<UIPanel>().BringIn();
			menuBackBtnPanel.BringIn();
			//territorySubMenuPanel.transform.FindChild("CommonMenuPanelSet").FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("BuildingInfoBG").gameObject.GetComponent<UIPanel>().Dismiss();
			//territorySubMenuPanel.transform.FindChild("CommonMenuPanelSet").FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("BuildingInfoBG").gameObject.GetComponent<UIPanel>().BringIn();
			InvokeRepeating("RotateBuildingInfoBG", 0, 0.05f);
		}
		
        // Building part
		switch(buildingEnum)
		{
		case BuildingEnum.BUILDING_HOME:
			//mainMenuPanel.Dismiss();
            SwitchMenu(homeMenuPanel);
            homeMenuPanel.transform.parent.FindChild("LVPanel").gameObject.GetComponent<UIPanel>().BringIn();
            //homeMenuPanel.transform.FindChild("HomeMenuMainPanel").GetComponent<UIPanel>().BringIn();

            /*if (!standAloneUITest && heroListArgsCache == null)
            {
                MGCDelegateMgr.instance.BNOHeroList -= OnBNOHeroList;
                MGCDelegateMgr.instance.BNOHeroList -= OnBNOHeroListAutoRefresh;
                MGCDelegateMgr.instance.BNOHeroList += OnBNOHeroList;

                m_barrier.Show();
                SendHeroListInfoREQ();
                //SendCityCDInfoREQ();
            }*/
			break;					
		case BuildingEnum.BUILDING_HERO:
			//mainMenuPanel.Dismiss();
			//heroMenuPanel.BringIn();
            //heroMenuPanel.transform.FindChild("HeroMenuMainPanel").GetComponent<UIPanel>().BringIn();
            SwitchMenu(heroMenuPanel);
            heroMenuPanel.transform.parent.FindChild("LVPanel").gameObject.GetComponent<UIPanel>().BringIn();
            // Send hero info req
            /*if (!standAloneUITest && heroListArgsCache == null)
            {
                m_barrier.Show();
                SendHeroListInfoREQ();
            }*/
			break;
		case BuildingEnum.BUILDING_MINE:
            SwitchMenu(mineMenuPanel);
            mineMenuPanel.transform.parent.FindChild("LVPanel").gameObject.GetComponent<UIPanel>().BringIn();
			break;
		case BuildingEnum.BUILDING_FACTORY:
			//mainMenuPanel.Dismiss();
			//factoryMenuPanel.BringIn();
            //factoryMenuPanel.transform.FindChild("FactoryMenuMainPanel").GetComponent<UIPanel>().BringIn();
            SwitchMenu(factoryMenuPanel);
            factoryMenuPanel.transform.parent.FindChild("LVPanel").gameObject.GetComponent<UIPanel>().BringIn();
			break;			
		case BuildingEnum.BUILDING_TECH:
			//mainMenuPanel.Dismiss();            
            SwitchMenu(techMenuPanel);
            techMenuPanel.transform.parent.FindChild("LVPanel").gameObject.GetComponent<UIPanel>().BringIn();
			break;
		/*case BuildingEnum.BUILDING_MEDICLE:
			//mainMenuPanel.Dismiss();
			medicalMenuPanel.BringIn();
			break;			*/
		case BuildingEnum.BUILDING_ALLY:
			//mainMenuPanel.Dismiss();
			allyMenuPanel.BringIn();
			break;				
		case BuildingEnum.BUILDING_TOWNVIEW:			
			break;				
		}			
	}
	
	private void hideBuildingHint()
	{
		allyTextImg.Hide(true);		
		crystalTextImg.Hide(true);	
		factoryTextImg.Hide(true);
		heroTextImg.Hide(true);
		homeTextImg.Hide(true);
		//medicalTextImg.Hide(true);
		techTextImg.Hide(true);
        marketTextImg.Hide(true);
	}	
	
	private void enableBuildingHint(BuildingEnum buildingEnum)		
	{		
		hideBuildingHint();		
		switch(buildingEnum)
		{
		case BuildingEnum.BUILDING_HOME:
			homeTextImg.Hide(false);			
			break;					
		case BuildingEnum.BUILDING_HERO:
			heroTextImg.Hide(false);			
			break;			
		case BuildingEnum.BUILDING_MINE:
			crystalTextImg.Hide(false);						
			break;
		case BuildingEnum.BUILDING_FACTORY:
			factoryTextImg.Hide(false);			
			break;			
		case BuildingEnum.BUILDING_TECH:
			techTextImg.Hide(false);			
			break;
		/*case BuildingEnum.BUILDING_MEDICLE:
			medicalTextImg.Hide(false);
			buildingHintPanel.BringIn();
			break;			*/
		case BuildingEnum.BUILDING_ALLY:
			allyTextImg.Hide(false);			
			break;				
        case BuildingEnum.BUILDING_SHOP:
            marketTextImg.Hide(false);
            break;

		case BuildingEnum.BUILDING_TOWNVIEW:						
			return;				
		}
        buildingHintPanel.BringIn();		
	}	
	//---------------------------
	//Monobehaviour
	//---------------------------
    
    //Comment by Neo: Remove MGMain dependency.
    //Replace with Initial().
    //protected void Awake () 
    //{
        
    //    //Side Panel
    //    sidePanelOpenBtn.Hide(true);		
    //    sidePanelCloseBtn.Hide(false);	
    //    //Set Rotate Hero Delegate
    //    modelTurnLeftBtn.AddInputDelegate(trunHeroModelLeftBtnPressed);
    //    modelTurnRightBtn.AddInputDelegate(trunHeroModelRightBtnPressed);
    //    heroAbilityPanel.BringIn();
    //    heroBioPanel.Dismiss();
    //    heroSkillPanel.Dismiss();
    //    heroEXInfoPanel.Dismiss();
    //    //Chat Panel
    //    chatCloseBtn.Hide(false);
    //    chatOpenBtn.Hide(true);
    //    // CD queue Panel
    //    cdQueueOpenBtn.Hide(false);
    //    cdQueueCloseBtn.Hide(true);

    //    foreach (UIButton3D btn in heroInfoSkillButton)
    //    {
    //        btn.SetInputDelegate(heroInfoSkillButtonPressed);            
    //    }
		
    //    foreach( UIStateToggleBtn btn in missionSortMethodOnButton)
    //    {
    //        btn.SetInputDelegate(sortMissionMethodOnBtnPressed);
    //    }
		
    //    foreach( UIStateToggleBtn btn in missionSortMethodOffButton)
    //    {
    //        btn.SetInputDelegate(sortMissionMethodOffBtnPressed);
    //    }

    //    //heroListPanel.AddInputDelegate(HeroInfoPageDrag);
    //    //heroListPanel.gameObject.transform.FindChild("HeroListCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(HeroInfoPageDrag);


    //    /*foreach (UIStateToggleBtn btn in popUpMultiChoiceButtonArray)
    //    {
    //        btn.SetInputDelegate(popUpMultiChoiceBtnPressed);
    //    }*/
		
    //    heroEquipHeroWeaponButton.AddInputDelegate(heroEquipHeroInfoItemPressed);
    //    heroEquipHeroArmorButton.AddInputDelegate(heroEquipHeroInfoItemPressed);
    //    heroEquipHeroAccessoryButton.AddInputDelegate(heroEquipHeroInfoItemPressed);
    //    heroEquipHeroGodButton.AddInputDelegate(heroEquipHeroInfoItemPressed);
        
    //    //Get avatar info from common date.
    //    if( !MGMain.instance )
    //    {
    //        //debug.log("No mg main instance created");
    //        playerForce = 1;
    //        nameText.text = "Moregeek";
    //        levelText.text = "10";
    //        crystalText.text = "123456/999999";
    //        goldText.text = "999999";
    //        moneyText.text = "123456/765432";	
    //    }else{ 
    //        playerForce = (int)MGMain.instance.commonData.avatarEventArgs.Avatar.FactionId;
    //        nameText.text = MGMain.instance.commonData.avatarEventArgs.Avatar.LordName;
    //        levelText.text = "" + MGMain.instance.commonData.avatarEventArgs.Avatar.Level;
    //        crystalText.text = "" + MGMain.instance.commonData.avatarEventArgs.Avatar.Crystal;
    //        goldText.text = "" + MGMain.instance.commonData.avatarEventArgs.Avatar.GameCoin;
    //        moneyText.text = "" + MGMain.instance.commonData.avatarEventArgs.Avatar.Ntd;
            
    //        // Setup EventhandlerMGCBNOCity_CooldownINFO
    //        RegisterAllHandler();
            
    //    }

    //    // comment to avoid req lobby server
    //    //SendCityInfoREQ();
    //    //SendGeneralInfoREQ();


    //    // to do: load different building models according to city info

    //    // Detect resolution and adjust item/hero per page on list UI
    //    AdjustListUIByResolution();
    //    //AdjustListUIByResolution();
       
    //    //Set hero information by different force.
    //    switch(playerForce)
    //    {
    //    case 1:// West
    //        heroNameText.text = heroName[1];
    //        heroBioText.text = heroBio[1];
    //        break;
    //    case 2:// East
    //        heroNameText.text = heroName[2];
    //        heroBioText.text = heroBio[2];
    //        break;
    //    }		
    //    //Set Default Camera
    //    //debug.log("Player force: " + playerForce);
    //    mainCamera.position = buildingReference[playerForce].cameraLocationReference[(int)MGTerritoryOperator.BuildingEnum.BUILDING_TOWNVIEW].camTargetPosition;							
    //    mainCamera.eulerAngles = buildingReference[playerForce].cameraLocationReference[(int)MGTerritoryOperator.BuildingEnum.BUILDING_TOWNVIEW].camTargetRotation;
    //    //Start coroutine to load territory model
    //    StartCoroutine(LoadTerritoryAndHero());
		
    //    // load hero list bar layout presets and store in table
    //    LoadHeroListBarLayoutPreset();
    //}

    private int CalculateChild(Transform t)
    {
        if (t.childCount == 0)
            return 1;
        else
        {
            int totalCount = 0;
            for (int i = 0; i < t.childCount; i++)
                totalCount += CalculateChild(t.GetChild(i));
            return totalCount + 1;
        }

    }


	bool _m_isShadowLightProduced = false;
	
    //public void Initial(MGCClientSDK clientSDK, MGCommonData commonData, MGWordsTable wordsTable, EnterBattleDelegate enterBattleDelegate, ExecuteQuestDelegate executeQuestDelegate) {
    public void Initial(MGCClientSDK clientSDK, MGCommonData commonData, MGWordsTable wordsTable, MGGameGod.SwitchSceneDelegate switchSceneDelegate, MGSceneMasterTerritory.BackToMainMenuDelegate backToMainMenuDelegate,
        MGTerritoryBuffManager territoryBuffMgr)
    {
        MGDebug.Log("Going to initial territory operator");
        //int firstLayerChildCount;
        //int totalObjCount = 0;
        // Test calculate child count of each UI set in DynamicUISet
        /*for (int i = 0; i < transform.parent.FindChild("DynamicUISet").childCount; i++)
        {
            firstLayerChildCount = CalculateChild(transform.parent.FindChild("DynamicUISet").GetChild(i));
            totalObjCount += firstLayerChildCount;
            //Debug.Log(transform.parent.FindChild("DynamicUISet").GetChild(i).name + " has "
                + firstLayerChildCount + " children");
        }
        //Debug.Log("Total obj count under DataListUISet= " + totalObjCount);*/


        //Store data references provide by scene manager.
        m_clientSDKRef = clientSDK;
        m_commonDataRef = commonData;
        m_wordsTableRef = wordsTable;
		m_barrier.SetWordsTable(wordsTable);
        m_switchSceneDelegate = switchSceneDelegate;
        m_backToMainMenuDelegate = backToMainMenuDelegate;

        m_territoryBuffMgr = territoryBuffMgr;
        m_territoryBuffMgr.buffEnd = OnBuffTimeUp;
        m_territoryBuffMgr.buffNearEnd = OnBuffNearEnd;

        //m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
        MGUITextAdjuster.DoIt(m_barrier.gameObject);
		MGUITextAdjuster.DoIt(m_broadcast.gameObject);
		m_commonDataRef.newMailCount = 0;
		
		otherPlayerUnitScrollList = OtherPlayerEventPanel.transform.FindChild("OtherPlayerScrollList").GetComponent<UIScrollList>();
        // CD queue Panel        

        // To do: add heroInfoTowerButton delegate if necessary

        /*foreach (UIStateToggleBtn btn in missionSortMethodOnButton) {
            btn.SetInputDelegate(sortMissionMethodOnBtnPressed);
        }

        foreach (UIStateToggleBtn btn in missionSortMethodOffButton) {
            btn.SetInputDelegate(sortMissionMethodOffBtnPressed);
        }*/

		//CDQueuePanelOpenCloseBtn.SetToggleState("Normal");

        homePlayerInfoPanel.transform.FindChild("HomePlayerInfoTextPanel").FindChild("Text").gameObject.GetComponent<UITextField>().SetCommitDelegate(OnPlayerInfoLostFocus);
        homePlayerInfoPanel.transform.FindChild("HomePlayerInfoTextPanel").FindChild("Text").gameObject.GetComponent<UITextField>().SetFocusDelegate(OnPlayerInfoTextRefocus);

//		for (int i = 0; i < marketItemNewsPanel.transform.FindChild("NewsPageButtons").gameObject.GetComponentsInChildren<UIButton>().Length; i++)		
//			marketItemNewsPanel.transform.FindChild("NewsPageButtons").gameObject.GetComponentsInChildren<UIButton>()[i].SetInputDelegate(OnNewsPageBtnPressed);					
		//marketItemNewsPanel.transform.FindChild("MarketListPageScrollList").gameObject.GetComponent<UIScrollList>().SetValueChangedDelegate(OnMarketADItemListChanged);
		
        mainCamera.transform.FindChild("CameraControlBoard").gameObject.GetComponent<UIButton>().SetInputDelegate(OnCameraControlBlockInput);

#if !UNITY_IPHONE && !UNITY_ANDROID
		if(!MGGameConfig.GetValue<bool>(EObjectDefine.TERRITORY_UI))
		{
			foreach (UIStateToggleBtn btn in allMainMenuBtnArray)
			{
				btn.AddInputDelegate(MainMenuToggleBtnMouseOverDelegate);
			}
		}
#endif

        //heroListPanel.AddInputDelegate(HeroInfoPageDrag);
        //heroListPanel.gameObject.transform.FindChild("HeroListCollider").gameObject.GetComponent<UIButton>().AddInputDelegate(HeroInfoPageDrag);

        /*foreach (UIStateToggleBtn btn in popUpMultiChoiceButtonArray)
        {
            btn.SetInputDelegate(popUpMultiChoiceBtnPressed);
        }*/

        //Find TutorialOrganizer GameObject's Component Once and get reference 
        TutorialOrganizerComp = GameObject.Find("TutorialOrganizer").GetComponent<MGTutorialOrganizer>();

        if (m_commonDataRef != null)
        {
            playerForce = (int)m_commonDataRef.avatarEventArgs.Avatar.FactionId;           
 
            // swap 1 and 2
            if (playerForce == 1)
                playerForce = 2;
            else if (playerForce == 2)
                playerForce = 1;

            // 1: East
            // 2: West
            // 3: Mideast

            nameText.Text = m_commonDataRef.avatarEventArgs.Avatar.Name;            
            //levelText.Text = "" + m_commonDataRef.avatarEventArgs.Avatar.Level;
            SetPlayerLvNumImg(m_commonDataRef.avatarEventArgs.Avatar.Level);
            crystalText.Text = "" + m_commonDataRef.avatarEventArgs.Avatar.Crystal;
            moneyText.Text = "" + m_commonDataRef.avatarEventArgs.Avatar.GameCoin;
            //moneyText.Text = "" + m_commonDataRef.avatarEventArgs.Avatar.Ntd;            
        }
        else
        {
            playerForce = 1;
            nameText.Text = "Moregeek";
            //levelText.Text = "10";
            crystalText.Text = "123456/999999";
            goldText.Text = "999999";
            moneyText.Text = "123456/765432";	
        }


        // Setup EventhandlerMGCBNOCity_CooldownINFO
        RegisterAllHandler();

        m_clientSDKRef.GetTime();
        // comment to avoid req lobby server
		SendGeneralInfoREQ();
        SendCityInfoREQ();
        
        Invoke("SendVillagerREQ", 5.0f);
        //SendCityCDInfoREQ();

        //Set Default Camera
        //debug.log("Player force: " + playerForce);
        mainCamera.position = buildingReference[playerForce].cameraLocationReference[(int)MGTerritoryOperator.BuildingEnum.BUILDING_TOWNVIEW].camTargetPosition;
        mainCamera.eulerAngles = buildingReference[playerForce].cameraLocationReference[(int)MGTerritoryOperator.BuildingEnum.BUILDING_TOWNVIEW].camTargetRotation;
        //m_mainAudioListener = mainCamera.gameObject.GetComponentInChildren<AudioListener>();
		m_mainAudioListener = MGGameGod.Instance.gameObject.GetComponentInChildren<AudioListener>();
        // load hero list bar layout presets and store in table
        LoadHeroListBarLayoutPreset();

        // New UI controls        
        ClearAllBottomFunctionBtnMsgCount();
        ClearAllBottomFunctionBtnInfo();
		AttachColliderToBottomFunctionBtn();
                        
        InitialPool();
		//Invoke("InitialPool", 5.0f);
        InitialShopTabs();
        // Set platform infomation
#if UNITY_IPHONE || UNITY_ANDROID
        SendSetOnlineStatusREQ(MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_MOBILE);
#else
        SendSetOnlineStatusREQ(MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_WEB);
#endif

        // Send invitaions request
        SendRelationRequestListREQ();
        SetCurrencyPanel(MGCTerritoryTypeDef.EHudCurrencyType.HUD_CURRENCY_CRYSTAL);

        // Test: Set buff (will be banned in the formal version) and then try to get buff
        //m_clientSDKRef.AddAvatarBuff("162000100001");        

        /*m_clientSDKRef.RemoveAvatarBuff("162000100001");
        m_clientSDKRef.RemoveAvatarBuff("162000100002");
        m_clientSDKRef.RemoveAvatarBuff("162000100003");
        m_clientSDKRef.RemoveAvatarBuff("162000100004");        
        m_clientSDKRef.RemoveAvatarBuff("162000200001");
        m_clientSDKRef.RemoveAvatarBuff("162000200002");
        m_clientSDKRef.RemoveAvatarBuff("162000200003");
        m_clientSDKRef.RemoveAvatarBuff("162000200004");
        m_clientSDKRef.RemoveAvatarBuff("162000300001");
        m_clientSDKRef.RemoveAvatarBuff("162000300002");
        m_clientSDKRef.RemoveAvatarBuff("162000300003");
        m_clientSDKRef.RemoveAvatarBuff("162000300004"); 
        m_clientSDKRef.RemoveAvatarBuff("162000100005");
        m_clientSDKRef.RemoveAvatarBuff("162000200005");
        m_clientSDKRef.RemoveAvatarBuff("162000300005");*/
        // for test: remove buff

        m_clientSDKRef.GetAvatarBuff( "" );    
		
		m_commonDataRef.updateTeamArgs = null;//initial team info
		IncrementBottomFunctionMgsCount(m_commonDataRef.inviteMsgCount, BottomFunctionEnum.BOTTOM_MAIL);   

		//m_clientSDKRef.SetOnlineStatus(0);
		//m_clientSDKRef.GetMTUMails();
		//Debug.Log("send GetMTUMails");
		
		//InvokeRepeating("SendGetMTU", 3.0f, 10.0f);
		
		Invoke("SendGetMTU", 3.0f);
		
		InvokeRepeating("UpdateBroadCastCycle", 1.0f, 60.0f);
		
		//ask visited reward size
		if(m_commonDataRef.IsCleanFriendVisitMeReward == false)
		{
			MGCDelegateMgr.instance.GetRewardSize_MyFriendVisitMeEvent -= OnGetRewardSize_MyFriendVisitMePacket;
			MGCDelegateMgr.instance.GetRewardSize_MyFriendVisitMeEvent += OnGetRewardSize_MyFriendVisitMePacket;
			m_clientSDKRef.SendGetRewardSize_MyFriendVisitMe_Package();
		}
		
        // Test icon test        
        m_villagerMgr.Initial(m_clientSDKRef, m_wordsTableRef, SetPopUpPage, popUpPanelOKPressed);
        LoadTownViewWithForce();

//#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!_m_isShadowLightProduced)
		{
			GameObject lightObj = (GameObject)GameObject.Instantiate(Resources.Load(MGResourcePaths.m_shadowLight));
			lightObj.transform.parent = transform;
			_m_isShadowLightProduced = true;
		}
//#endif
		
		MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCount;
		MGCDelegateMgr.instance.GetQuestStatusCount += SetQuestCount;
		m_clientSDKRef.GetQuestStatusCount();
		
		if (!isSubMenuPanelsInstantiated)
			InstantiateSubMenuPanels();

		if (!isDynamicPrefabsInstantiated)
			InstantiateDynamicPrefabs();
		
		// Disable market button for CCB
#if CLOSE_GOLDSHOP
		dynamicMarketBtn.GetComponent<UIButton>().controlIsEnabled = false;
#endif

#if UNITY_IPHONE //IPHONE Cash Flow Manager and Event Listener GameObject
		if(StoreKitMgrObj == null)
			StoreKitMgrObj = GameObject.Find("StoreKitManager(Clone)");
#endif

#if UNITY_ANDROID //ANDROID Cash Flow Manager and Event Listener GameObject
		if(IABAndroidMgrObj == null)
			IABAndroidMgrObj =  GameObject.Find("IABAndroidManager(Clone)");
#endif

        MGCDelegateMgr.instance.SyncProductCategory -= OnSyncProductCategory;
        MGCDelegateMgr.instance.SyncProductCategory += OnSyncProductCategory;

        MGGossipMgr.Instance.SetWordsTableRef(m_wordsTableRef);
        MGGossipMgr.Instance.SetMaskPanelRef(popUpMaskPanel);
        MGGossipMgr.Instance.SetCommDataRef(m_commonDataRef);
        MGGossipMgr.Instance.SetClientSDKRef(m_clientSDKRef);
        MGGossipMgr.Instance.InitChatUI(bottomMainMenuPanel.transform.FindChild("ChatTip").gameObject);
        MGGossipMgr.Instance.SetStartInvokeRepeatGamePlayMsg();
        MGGossipMgr.Instance.SetBarrierRef(m_barrier);
        MGGossipMgr.Instance.SetChatBtnRef(bottomMainMenuPanel.transform.FindChild("ChatBtn").gameObject);
		bottomMainMenuPanel.transform.FindChild("ChatBtn").GetComponent<UIButton>().scriptWithMethodToInvoke = MGGossipMgr.Instance.gossipui;
		
		mailCountText = itemListPanel.transform.FindChild("ItemListMailCount").GetComponent<SpriteText>();
		
		SendNotice();
		ResetBattleDifficulty();
		SetQuestListHandler(OnGetQuestListStatusCountCondition);
		MGCClientSDK.instance.GetQuestList();
		if(MGGameConfig.GetValue<bool>( EObjectDefine.TERRITORY_UI))
		{
			allMainMenuBtnArray[0].methodToInvoke = "HomeBtnSingleClick";
			allMainMenuBtnArray[1].methodToInvoke = "HeroBtnSingleClick";
			allMainMenuBtnArray[2].methodToInvoke = "MineBtnSingleClick";
			allMainMenuBtnArray[3].methodToInvoke = "FactoryBtnSingleClick";
			allMainMenuBtnArray[4].methodToInvoke = "TechBtnSingleClick";
			allMainMenuBtnArray[5].methodToInvoke = "MarketBtnSingleClick";
		}
		else
		{
			for(int i=0;i<6;i++)
			{
				allMainMenuBtnArray[i].methodToInvoke = "MainMenuBtnSingleClick";
			}
		}

        buffPanel.transform.FindChild("BG").GetComponent<UIButton>().AddInputDelegate(OnEnterbuffPanelDel);
        //buffTimePanel.transform.FindChild("CloseBuffTimeButton").GetComponent<UIButton>().AddInputDelegate(OnClosebuffPanelDel);
		
		InvokeMissionTip();
		
		m_clientSDKRef.GetPVPRankReward();
		m_clientSDKRef.GetArenaRankReward();
		
		//Logo Event (Only when the player log into the territory scene and play the logo event)
		if(!MGGameConfig.GetUserValue<bool>("bShowLogo"))
		{
			MGGameGod.EventUnit EU = new MGGameGod.EventUnit();
			EU.type = MGGameGod.Event_type.client;
			EU.Args = null;
			MGGameGod.Instance.EventQueue.Enqueue(EU);
			if(MGGameGod.Instance.EventQueue.Count != 0 && MGGameGod.Instance.IsEventUIShowing == false)
			{
				HandleQueueEvent((MGGameGod.EventUnit)MGGameGod.Instance.EventQueue.Dequeue());
			}
			MGGameConfig.SetUserValue("bShowLogo",true);
		}
		MGCDelegateMgr.instance.AttackEvent -= OnAttackEvent;
		MGCDelegateMgr.instance.AttackEvent += OnAttackEvent;
		m_clientSDKRef.AttackEvent();
		MGCDelegateMgr.instance.GetAttackedFriendList -= OnGetAttackedFriendList;
		MGCDelegateMgr.instance.GetAttackedFriendList += OnGetAttackedFriendList;
		m_clientSDKRef.GetAttackedFriendList(true);
    }
	
	public void OnAttackEvent(object sender, MGCBaseEventArgs bea)
    {
	}
	
	private List<string> m_otherPlayerEventLordNameList = new List<string>();
	private List<string> m_otherPlayerEventLordPortraitList = new List<string>();
	private List<int> m_otherPlayerEventLordFactionList = new List<int>();
	private List<string> m_otherPlayerEventLordStageIDList = new List<string>();
	private bool m_haveAttackedFriends = false;
	public void OnGetAttackedFriendList(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.GetAttackedFriendList -= OnGetAttackedFriendList;
		MGCGetAttackedFriendListEventArgs friendListEventArgs = (MGCGetAttackedFriendListEventArgs)bea;
		if(friendListEventArgs.JustCheck)
		{
			if(friendListEventArgs.HaveAttackedFriendsFlag)
			{
				m_haveAttackedFriends = true;
				MGGameGod.EventUnit EU = new MGGameGod.EventUnit();
				EU.type = MGGameGod.Event_type.server;
				EU.Args = null;
				MGGameGod.Instance.EventQueue.Enqueue(EU);
				if(MGGameGod.Instance.EventQueue.Count != 0 && MGGameGod.Instance.IsEventUIShowing == false)
				{
					HandleQueueEvent((MGGameGod.EventUnit)MGGameGod.Instance.EventQueue.Dequeue());
				}
				MGCDelegateMgr.instance.GetAttackedFriendList -= OnGetAttackedFriendList;
				MGCDelegateMgr.instance.GetAttackedFriendList += OnGetAttackedFriendList;
				m_clientSDKRef.GetAttackedFriendList(false);
			}
		}
		else
		{
			for(int i=0;i<friendListEventArgs.AttackedFriendList.Count;i++)
			{
				m_otherPlayerEventLordNameList.Add((string)friendListEventArgs.AttackedFriendList[i][MGCProperties.ATTR_NAME]);
				m_otherPlayerEventLordPortraitList.Add((string)friendListEventArgs.AttackedFriendList[i][MGCProperties.ATTR_PORTRAIT_OBJID]);
				m_otherPlayerEventLordFactionList.Add((int)friendListEventArgs.AttackedFriendList[i][MGCProperties.ATTR_FACTION_ID]);
				m_otherPlayerEventLordStageIDList.Add((string)friendListEventArgs.AttackedFriendList[i][MGCProperties.ATTR_STAGE_ID]);
			}
		}
	}

	public GameObject dynamicBuyGoldBtn;
//	public GameObject dynamicMarketBtn;
	public GameObject dynamicRollingEggShortcutBtn;

	bool isDynamicPrefabsInstantiated = false;
	void InstantiateDynamicPrefabs()
	{
//		GameObject btnObj = Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.LOCALIZED_EVENT_BUNDLE, "MarketBtn")) as GameObject;
//		btnObj.GetComponent<UIButton>().scriptWithMethodToInvoke = this;
//		SpriteBase[] sprtBase = btnObj.transform.GetComponentsInChildren<SpriteBase>();
//		foreach (SpriteBase sprt in sprtBase)			
//			sprt.RenderCamera = UIManager.instance.uiCameras[0].camera;
//		MGUITextAdjuster.DoIt(btnObj);		
//		btnObj.transform.parent = dynamicMarketBtn.transform.parent;
//		btnObj.transform.localPosition = dynamicMarketBtn.transform.localPosition;
//		btnObj.name = dynamicMarketBtn.name;
//		Destroy(dynamicMarketBtn);
//		dynamicMarketBtn = btnObj;

		GameObject btnObj = Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.LOCALIZED_EVENT_BUNDLE, "BuyGoldBtn")) as GameObject;
		btnObj.GetComponent<UIButton>().scriptWithMethodToInvoke = this;
		SpriteBase[] sprtBase = btnObj.transform.GetComponentsInChildren<SpriteBase>();
		foreach (SpriteBase sprt in sprtBase)
			sprt.RenderCamera = UIManager.instance.uiCameras[0].camera;
		MGUITextAdjuster.DoIt(btnObj);
		btnObj.transform.parent = dynamicBuyGoldBtn.transform.parent;
		btnObj.transform.localPosition = dynamicBuyGoldBtn.transform.localPosition;
		btnObj.name = dynamicBuyGoldBtn.name;
		Destroy(dynamicBuyGoldBtn);
		dynamicBuyGoldBtn = btnObj;

		btnObj = Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.LOCALIZED_EVENT_BUNDLE, "RollingEggShortcutBtn")) as GameObject;
		btnObj.GetComponent<UIButton>().scriptWithMethodToInvoke = this;
		sprtBase = btnObj.transform.GetComponentsInChildren<SpriteBase>();
		foreach (SpriteBase sprt in sprtBase)
			sprt.RenderCamera = UIManager.instance.uiCameras[0].camera;
		MGUITextAdjuster.DoIt(btnObj);
		btnObj.transform.parent = dynamicRollingEggShortcutBtn.transform.parent;
		btnObj.transform.localPosition = dynamicRollingEggShortcutBtn.transform.localPosition;
		btnObj.name = dynamicRollingEggShortcutBtn.name;
		Destroy(dynamicRollingEggShortcutBtn);
		dynamicRollingEggShortcutBtn = btnObj;

		//buyGoldBtnObj.name = "TestBuyGoldBtn";

		isDynamicPrefabsInstantiated = true;
	}

    void TutorialIsDone()
    {
        if(!MGGameConfig.GetUserValue<bool>("bSendLoginReward"))
        {
            MGGameConfig.SetUserValue("bSendLoginReward", true );
            MGCDebug.Log(" TutorialIsDone Send GetLoginReward");
            MGCClientSDK.instance.GetLoginReward();
        }
    }

	public void SendQuestCountReqAfterUnfreezed()
	{
		MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCount;
		MGCDelegateMgr.instance.GetQuestStatusCount += SetQuestCount;
		m_clientSDKRef.GetQuestStatusCount();
	}

	public void SetQuestCount(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCount;
		MGCGetQuestStatusCountEventArgs questCountArgs = (MGCGetQuestStatusCountEventArgs)bea;
		SetQuestCountValue(questCountArgs);
		
		InvokeMissionTip();
	}
	
	public void InvokeMissionTip()
	{
		CancelInvoke("PlayMsgCountBGAni");
		Invoke("PlayMsgCountBGAni",3.0f);
		CancelInvoke("PlayNewInfoBGAni");
		Invoke("PlayNewInfoBGAni",3.0f);
	}
	
	private void PlayMsgCountBGAni()
	{
		rightMainMenuPanel.transform.FindChild("MenuBG/MissionBtn/MsgCountBG").gameObject.GetComponent<Animation>().Play("WaitMissionTip");
		rightMainMenuPanel.transform.FindChild("MenuBG/MailBtn/MsgCountBG").gameObject.GetComponent<Animation>().Play("WaitMissionTip");
	}
	
	private void PlayNewInfoBGAni()
	{
		rightMainMenuPanel.transform.FindChild("MenuBG/MissionBtn/NewInfoBG").gameObject.GetComponent<Animation>().Play("CompleteAnimation");
		rightMainMenuPanel.transform.FindChild("MenuBG/MailBtn/NewInfoBG").gameObject.GetComponent<Animation>().Play("CompleteAnimation");
	}
	
	private void PlayEventAni()
	{
		MGGameGod.Instance.IsEventUIShowing = true;
		EventMovePanel.BringIn();
		EventMovePanel.transform.FindChild("EventFadePanel/EventTextPanel").GetComponent<UIPanel>().BringIn();
		EventMovePanel.transform.FindChild("EventFadePanel/EvenIocnPanel").GetComponent<UIPanel>().BringIn();
		EventMovePanel.transform.FindChild("EventFadePanel/EvenEffectPanel").GetComponent<UIPanel>().BringIn();
		Invoke("PlayPPVEBtnMoveAnimationBegin",2.6f);
		Invoke("PlayPPVEBtnMoveAnimationEnd",4.6f);
		//Invoke("PlayPPVEBtnMoveAnimationEnd",2.0f);
		EventMovePanel.transform.FindChild("EventFadePanel").GetComponent<UIPanel>().Dismiss();
		Invoke("DismissEventMovePanel",6.6f);
	}
	
	private void PlayPPVEBtnMoveAnimationBegin()
	{
		bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").localScale = Vector3.zero;
		bottomRightPanel.transform.FindChild("BattleExtend").localScale = Vector3.one;
	}
	private void PlayPPVEBtnMoveAnimationEnd()
	{
		bottomRightPanel.transform.FindChild("BattleExtend").FindChild("ArenaBtn").localScale = Vector3.one;
		Invoke("HideBattleExtend", 1.0f);
		if(!m_inInnerUILayer)
		{
			this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").localScale = Vector3.one;
		}
	}
	
	private bool m_showOtherPlayer = false;
	private void OpenOtherPlayerEvent(GameObject caller)
	{
		m_showOtherPlayer = !m_showOtherPlayer;
		if(m_showOtherPlayer)
		{
			CloseAllOutUI();
			OtherPlayerEventPanel.transform.localScale = Vector3.one;
			ShowOtherPlayerUnit();
		}
		else
			OtherPlayerEventPanel.transform.localScale = Vector3.zero;
	}
	
	private void PlayOtherPlayerEventAni()
	{
		if(!m_commonDataRef.HavePlayOtherPlayerEventAnimation)
		{
			m_commonDataRef.HavePlayOtherPlayerEventAnimation = true;
			MGGameGod.Instance.IsEventUIShowing = true;
			OtherPlayerEventPanel.transform.localScale = Vector3.one;
			OtherPlayerEventPanel.transform.FindChild("EvenEffectPanel").GetComponent<UIPanel>().BringIn();
			Invoke("CloseOtherPlayerEventPanel",4.6f);
		}
		else
		{
			StopOtherPlayerEventAni();
		}
	}
	
	private void ShowOtherPlayerUnit()
	{
		int totalUnitNumber = otherPlayerUnitScrollList.Count;
		
		if (totalUnitNumber + MGCTerritoryTypeDef.Constants.UNIT_PER_PAGE_IN_OTHER_PLAYER_LIST 
			- totalUnitNumber % MGCTerritoryTypeDef.Constants.UNIT_PER_PAGE_IN_OTHER_PLAYER_LIST			
			> eventOtherPlayerUnitPool.Length )
		{
			ExpandCommonItemListPoolToSize(ref eventOtherPlayerUnitPool, totalUnitNumber - eventOtherPlayerUnitPool.Length
				, "EventOtherPlayerUnit", eventOtherPlayerUnitPoolParent, null);
		}
		int itemAssigned = 0;
		otherPlayerUnitScrollList.ClearList(false);
		for(int i=0;i<m_otherPlayerEventLordNameList.Count;i++)
        {
			GameObject currentItem = GetOtherPlayerUnitFromPool();
			currentItem.name = "OtherPlayerUnit" + itemAssigned.ToString("00");
			currentItem.transform.FindChild("PlayerItem").FindChild("PlayerNameBG").FindChild("Text").GetComponent<SpriteText>().Text = m_otherPlayerEventLordNameList[i];
			switch (m_otherPlayerEventLordFactionList[i])
            {
				case 1:
					currentItem.transform.FindChild("PlayerItem").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000005");
                    break;
				case 2:
					currentItem.transform.FindChild("PlayerItem").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000006");
                    break;
				case 3:
					currentItem.transform.FindChild("PlayerItem").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000007");
                    break;
			}
			MGPlayerMarkPortraitData portraitData = null;
			GameObject pic;
	
			portraitData = MGDataManager.instance.GetPlayerPortraitData(m_otherPlayerEventLordPortraitList[i]);

			pic = (GameObject)MGDownloader.instance.getObject(BundleType.BATCHITEMS_BUNDLE, MGResourcePaths.m_GUIBatchPackedSprites + portraitData.m_graphSmall);
        
			currentItem.transform.FindChild("PlayerItem").FindChild("PlayerItemPic").GetComponent<PackedSprite>().Copy(pic.GetComponent<PackedSprite>());
			currentItem.transform.FindChild("PlayerItem").FindChild("BattleBtn").GetComponent<UIButton>().scriptWithMethodToInvoke = this;
			currentItem.transform.FindChild("PlayerItem").FindChild("BattleBtn").GetComponent<UIButton>().Data = m_otherPlayerEventLordStageIDList[i];
			
			if(m_otherPlayerEventLordNameList.Count > i+1)
			{
				i++;
				currentItem.transform.FindChild("PlayerItem2").FindChild("PlayerNameBG").FindChild("Text").GetComponent<SpriteText>().Text = m_otherPlayerEventLordNameList[i];
				
				switch (m_otherPlayerEventLordFactionList[i])
	            {
					case 1:
						currentItem.transform.FindChild("PlayerItem2").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000005");
	                    break;
					case 2:
						currentItem.transform.FindChild("PlayerItem2").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000006");
	                    break;
					case 3:
						currentItem.transform.FindChild("PlayerItem2").FindChild("PlayerCountryBG").FindChild("Text").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0000007");
	                    break;
				}
		
				portraitData = MGDataManager.instance.GetPlayerPortraitData(m_otherPlayerEventLordPortraitList[i]);
	
				pic = (GameObject)MGDownloader.instance.getObject(BundleType.BATCHITEMS_BUNDLE, MGResourcePaths.m_GUIBatchPackedSprites + portraitData.m_graphSmall);
	        
				currentItem.transform.FindChild("PlayerItem2").FindChild("PlayerItemPic").GetComponent<PackedSprite>().Copy(pic.GetComponent<PackedSprite>());
				currentItem.transform.FindChild("PlayerItem2").FindChild("BattleBtn").GetComponent<UIButton>().scriptWithMethodToInvoke = this;
				currentItem.transform.FindChild("PlayerItem2").FindChild("BattleBtn").GetComponent<UIButton>().Data = m_otherPlayerEventLordStageIDList[i];
				currentItem.transform.FindChild("PlayerItem2").localScale = Vector3.one;
				
			}
			else
				currentItem.transform.FindChild("PlayerItem2").localScale = Vector3.zero;
			otherPlayerUnitScrollList.AddItem(currentItem);
			++itemAssigned;
        }
	}
	
	private void OtherPlayerUnitPressed(GameObject caller)
	{
		LevelData level = MGLevelDataFetcher.FetchLevelData((string)caller.transform.GetComponent<UIButton>().Data, new MGXMLObject((TextAsset)MGDownloader.instance.getObject(BundleType.LOCALIZED_GAMEDATA_BUNDLE, "world")));
		if(level.m_costCrystal <= m_commonDataRef.playerDetailedInfo.crystal)
		{
			int lordNameIndex = int.Parse(caller.transform.parent.parent.name.Substring(caller.transform.parent.parent.name.Length-2, 1));
			if(lordNameIndex > 0)
				lordNameIndex = int.Parse(caller.transform.parent.parent.name.Substring(caller.transform.parent.parent.name.Length-2, 2));
			else
				lordNameIndex = int.Parse(caller.transform.parent.parent.name.Substring(caller.transform.parent.parent.name.Length-1, 1));
			m_commonDataRef.arenaOpponentName = m_otherPlayerEventLordNameList[lordNameIndex];
        	m_commonDataRef.levelData = level;
			
			m_commonDataRef.bLevelSelectCompleteAndOpenHeroSelectUI = true;
			TryToSwitchScene("LevelSelect");
		}
	}
	
	private void HideBattleExtend()
	{
		bottomRightPanel.transform.FindChild("BattleExtend").localScale = Vector3.zero;
	}
	
	private void DismissEventMovePanel()
	{
		EventMovePanel.Dismiss();
		StopEventAni();
	}
	
	
	private void StopEventAni()
	{
		//Check if the Event Queue
		MGGameGod.Instance.IsEventUIShowing = false;
		if(MGGameGod.Instance.EventQueue.Count != 0)
		{
			HandleQueueEvent((MGGameGod.EventUnit)MGGameGod.Instance.EventQueue.Dequeue());
		}
	}
	
	private void CloseOtherPlayerEventPanel()
	{
		OtherPlayerEventPanel.transform.FindChild("EvenEffectPanel").localScale = Vector3.zero;
		OtherPlayerEventPanel.transform.localScale = Vector3.zero;
		StopOtherPlayerEventAni();
	}
	
	private void StopOtherPlayerEventAni()
	{
		MGGameGod.Instance.IsEventUIShowing = false;
		if(MGGameGod.Instance.EventQueue.Count != 0)
		{
			HandleQueueEvent((MGGameGod.EventUnit)MGGameGod.Instance.EventQueue.Dequeue());
		}
		if(!m_inInnerUILayer)
		{
			this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").localScale = Vector3.one;
		}
		InvokeRepeating("HighLightEventBtn",0, 0.5f);
	}
	
	private void HighLightEventBtn()
	{
		this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").GetComponent<UIButton>().SetState(1);
		Invoke("UnhighLightEventBtn", 0.25f);
	}
	
	private void UnhighLightEventBtn()
	{
		this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").GetComponent<UIButton>().SetState(0);
	}
	
	private bool m_inInnerUILayer = false;
	private void HideOtherPlayerEventBtn(bool hide)
	{
		m_inInnerUILayer = hide;
		if(hide)
		{
			this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").localScale = Vector3.zero;
		}
		else
		{
			if(m_haveAttackedFriends)
			{
				this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").localScale = Vector3.one;
			}
		}
	}
	
	private void CloseOtherPlayerEvent()
	{
		m_showOtherPlayer = false;
		OtherPlayerEventPanel.transform.localScale = Vector3.zero;
	}
	
	public void SetQuestCountValue(MGCGetQuestStatusCountEventArgs questCountArgs)
    {
//		acceptiveMainMissionCount = questCountArgs.MainQuestActivatedCount;
		acceptiveMainMissionCount = questCountArgs.MainQuestActivatedCount + questCountArgs.SubQuestActivatedCount;
//		acceptiveSubMissionCount = questCountArgs.SubQuestActivatedCount;
		acceptiveDailyMissionCount = questCountArgs.DailyQuestActivatedCount;
		acceptiveActivityMissionCount = questCountArgs.ActiviyQuestActivatedCount;
//		reportableMainMissionCount = questCountArgs.MainQuestCompletedCountt;
		reportableMainMissionCount = questCountArgs.MainQuestCompletedCountt + questCountArgs.SubQuestCompletedCountt;
//		reportableSubMissionCount = questCountArgs.SubQuestCompletedCountt;
		reportableDailyMissionCount = questCountArgs.DailyQuestCompletedCountt;
		reportableActivityMissionCount = questCountArgs.ActiviyQuestCompletedCountt;

		SetBottomFunctionBtnMsgCount(acceptiveMainMissionCount  + acceptiveDailyMissionCount + acceptiveActivityMissionCount, BottomFunctionEnum.BOTTOM_MISSION);
		SetBottomFunctionBtnNewCount(reportableMainMissionCount + reportableDailyMissionCount + reportableActivityMissionCount, BottomFunctionEnum.BOTTOM_MISSION);
	}

    public void UnloadAllAssetBundle()
    {
        //if(bundle)
            //bundle.Unload(true);
    }
    
	
    protected void Update () {
        //Check Camera is moving or not.
        //float percentOfMoving;
        /*if(isCameraMoving){
            accumTime += Time.deltaTime;		
            if(accumTime >= cameraMovingTime){
                mainCamera.position = buildingReference[playerForce].cameraLocationReference[(int)targetBuildingID].camTargetPosition;					
                mainCamera.eulerAngles = buildingReference[playerForce].cameraLocationReference[(int)targetBuildingID].camTargetRotation;
                currentBuildingID = targetBuildingID;
                isCameraMoving = false;
                enableBuildMenu(targetBuildingID);
            }else{			
                //Camera movemeng will be fast at the start and slow at the end.
                percentOfMoving = accumTime / cameraMovingTime;
                percentOfMoving = Mathf.Sqrt(percentOfMoving);
                mainCamera.position = Vector3.Lerp(oriPos, buildingReference[playerForce].cameraLocationReference[(int)targetBuildingID].camTargetPosition, percentOfMoving);
                mainCamera.eulerAngles = Vector3.Lerp(oriRot, buildingReference[playerForce].cameraLocationReference[(int)targetBuildingID].camTargetRotation, percentOfMoving);				
            }
        }*/

		
        // Check long pressed
        if (skillBtnStartPressTime > 0 && (Time.time - skillBtnStartPressTime) > longPressTime)
        {
            ShowSkillDescriptionPanel();
        }

        // Check camera rotate around target building
        /*if (testLockCameraTarget)
        {
            mainCamera.LookAt(buildingReference[playerForce].cameraLocationReference[(int)targetBuildingID].camLookatPosition);               
        }*/
		
		DealTerritoryCamera();
    }
	
    void OnDestroy()
    {
		Instance = null;
        //UnregisterAllHandler();//It seems like that game may crash on switchscene, need to check
    }

    public void RegisterAllHandler()
    {
        //MGCDelegateMgr.instance.City_CooldownINFO += OnCityCDDataArrived;//
        //MGCDelegateMgr.instance.Building_CooldownINFO += OnBuildingCDDataArrived;

        MGCDelegateMgr.instance.City_INFO += OnCityInfoArrived;
        //MGCDelegateMgr.instance.Player_DetailedINFO += OnPlayerDataArrived;        
        //MGCDelegateMgr.instance.Building_INFO += OnBuildingInfo;
        MGCDelegateMgr.instance.DirectUpgrade += OnDirectUpdate;

        //MGCDelegateMgr.instance.Tax += OnGetMyCityTax;
        MGCDelegateMgr.instance.AssignDuty += OnAssignDuty;
        MGCDelegateMgr.instance.RemoveDuty += OnRemoveDuty;

        MGCDelegateMgr.instance.Mining += OnMineGetCrystal;

        MGCDelegateMgr.instance.HireHero += OnHireHero;
        MGCDelegateMgr.instance.FireHero += OnFireHero;
        MGCDelegateMgr.instance.TrainHero += OnTrainHero;
        MGCDelegateMgr.instance.BNOHeroEquip += OnBNOHeroEquip;

        //MGCDelegateMgr.instance.AnswerQuiz += OnAnswerQuiz;

        MGCDelegateMgr.instance.PlayerProperties += OnPlayerProperties;
        //MGCDelegateMgr.instance.PlayerPortraitList += OnPortraitList;
        //MGCDelegateMgr.instance.PlayerMarkList += OnMarkList;

        //MGCDelegateMgr.instance.SetupPlayerDescription += OnSetupPlayerDescription;
        //MGCDelegateMgr.instance.SetupPlayerPortrait += OnSetPlayerPortrait;
        //MGCDelegateMgr.instance.SetupPlayerMark += OnSetPlayerEmblem;

        MGCDelegateMgr.instance.ReceiveQuiz += OnReceiveQuiz;

        //MGCDelegateMgr.instance.Building_ClearCooldown += OnClearCooldown;

        MGCDelegateMgr.instance.SendMsg += OnRecvMsgAck;
        
        MGCDelegateMgr.instance.GetTechnologyList += OnGetTechnologyList;
        MGCDelegateMgr.instance.UpdateTechnologyLevel += OnUpdateTechnologyLevel;

        MGCDelegateMgr.instance.ShopInfo += OnShopInfo;
            
        MGCDelegateMgr.instance.GetRelationRequestList += OnGetAvatarRelationListCount;
        MGCDelegateMgr.instance.GetRelationRequestList += OnGetFriendList;
        MGCDelegateMgr.instance.AskCreateRelation += OnAskCreateRelation;
        MGCDelegateMgr.instance.BreakUpRelation += OnBreakUpRelation;

        MGCDelegateMgr.instance.GetMoneyBonus += OnGetMoneyBonus;

        MGCDelegateMgr.instance.GetTime += OnGetServerTime;

        MGCDelegateMgr.instance.GetAvatarBuff += OnGetTerritoryBuff;

        MGCDelegateMgr.instance.MallBuyItem += OnBuyMallItem;
        MGCDelegateMgr.instance.GetPersonalLimit += OnGetItemPersonalLimitTable;

		MGCDelegateMgr.instance.ExchangeSerialNumber += OnExchangeSerialNumber;
		
		MGCDelegateMgr.instance.InviteTeam += OnBattleInviteTerritoryDelegate;

        MGEventManager.instance.RegisterEvent(EEvent.GAME_PAUSE, OnGamePause);
        MGEventManager.instance.RegisterEvent(EEvent.GAME_RESUME, OnGameResume);
        m_territoryBuffMgr.RegisterResumeEvent();

        invokeRepeatingMethodNameList.Add("UpdateCDUIBySecond");
        invokeRepeatingMethodNameList.Add("UpdateCrystalMineAmount");
        invokeRepeatingMethodNameList.Add("UpdateVillagerEventTimer");
		invokeRepeatingMethodNameList.Add("UpdateRollingEggMachineRefreshDate");

        MGCDelegateMgr.instance.UpdateUI += OnServerUpdateInfo;

        MGCDelegateMgr.instance.HeroCollection += OnGetHeroCollection;

        MGCDelegateMgr.instance.VillagerList += OnVillagerList;
        MGCDelegateMgr.instance.VillagerEventList += OnVillagerEventList;
        MGCDelegateMgr.instance.VillagerEventAnswer += OnVillagerEventAnswer;

        MGCDelegateMgr.instance.RefreshEverydayQuest += OnRefreshEverydayQuest;
        MGCDelegateMgr.instance.AddDailyQuestReportCount += OnAddDailyQuestReportCount;
		MGCDelegateMgr.instance.CompleteQuest += OnCompleteQuest;
		MGCDelegateMgr.instance.AcceptQuest += OnAcceptQuest;
		MGCDelegateMgr.instance.GiveUpQuest += OnGiveUpQuest;
		MGCDelegateMgr.instance.ActivateQuest += OnActiveQuest;
		MGCDelegateMgr.instance.GetQuestDetail += OnQuestDetail;

		MGCDelegateMgr.instance.TowersInfo += OnHeroTowersInfo;

		MGCDelegateMgr.instance.HeroRefresh += OnHeroRefresh;
		MGCDelegateMgr.instance.PartialHero += OnHeroRefreshUpdateList;
		
		MGCDelegateMgr.instance.GetDrawEggCount += OnGetDrawEggCount;
		MGCDelegateMgr.instance.GetAdvertisementArticles += OnGetMallAdvertisementArticle;

		MGCDelegateMgr.instance.Backpack_ItemList += OnBackpack_ItemList;

		MGCDelegateMgr.instance.ExecuteQuest += OnExecuteQuest;		
		//SetBNOHeroListUpdateHandler(OnHeroRefreshUpdateList);		
		SetBackpackInfoHandler(OnBackpack_Info);
		RemoveAllBNOHeroListHandler();
		RemoveAllBNOHeroListUpdateHandler();
		RemoveAllQuestListHandler();
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		MGEventManager.instance.RegisterEvent(EEvent.INPUT_KEY_UP, DealHotkey_Territory);
#endif

        //register event of get product id from server
        MGCDelegateMgr.instance.SyncProductCategory -= OnSyncProductCategory;
        MGCDelegateMgr.instance.SyncProductCategory += OnSyncProductCategory;

        //register event of receive Login Reward Packet
        MGCDelegateMgr.instance.GetLoginRewardEvent -= OnLoginReward;
        MGCDelegateMgr.instance.GetLoginRewardEvent += OnLoginReward;

        TutorialOrganizerComp.TutorialIsDone -= new MGTutorialOrganizer.EventHandler(TutorialIsDone);
        TutorialOrganizerComp.TutorialIsDone += new MGTutorialOrganizer.EventHandler(TutorialIsDone);

        //MGCDelegateMgr.instance.SendMsg -= OnsendMessage;
        //MGCDelegateMgr.instance.SendMsg += OnsendMessage;

        //Cash Flow Purchase 
        MGCDelegateMgr.instance.CashFlow_Purchase -= OnCashFlow_Purchase;
        MGCDelegateMgr.instance.CashFlow_Purchase += OnCashFlow_Purchase;
    }
        	
    public void UnregisterAllHandler()
    {
        //MGCDelegateMgr.instance.City_CooldownINFO -= OnCityCDDataArrived;
		//MGCDelegateMgr.instance.Building_CooldownINFO -= OnBuildingCDDataArrived;
        MGCDelegateMgr.instance.City_INFO -= OnCityInfoArrived;
        //MGCDelegateMgr.instance.Player_DetailedINFO -= OnPlayerDataArrived;

        //MGCDelegateMgr.instance.Building_INFO -= OnBuildingInfo;
        MGCDelegateMgr.instance.DirectUpgrade -= OnDirectUpdate;
		MGCDelegateMgr.instance.HireHero -= OnHireHero;
		MGCDelegateMgr.instance.FireHero -= OnFireHero;
		//MGCDelegateMgr.instance.AnswerQuiz -= OnAnswerQuiz;

        //MGCDelegateMgr.instance.Tax -= OnGetMyCityTax;
        MGCDelegateMgr.instance.AssignDuty -= OnAssignDuty;
        MGCDelegateMgr.instance.RemoveDuty -= OnRemoveDuty;
        MGCDelegateMgr.instance.Mining -= OnMineGetCrystal;
        MGCDelegateMgr.instance.TrainHero -= OnTrainHero;
        MGCDelegateMgr.instance.BNOHeroEquip -= OnBNOHeroEquip;

        MGCDelegateMgr.instance.HeroList -= OnBNOHeroList;
        MGCDelegateMgr.instance.HeroList -= OnBNOHeroListAutoRefresh;
        MGCDelegateMgr.instance.HeroList -= OnBNOHeroListSwitchScene;
		MGCDelegateMgr.instance.HeroList -= OnBNOHeroListWithPostHandler;

        MGCDelegateMgr.instance.PlayerProperties -= OnPlayerProperties;
        //MGCDelegateMgr.instance.PlayerPortraitList -= OnPortraitList;
        //MGCDelegateMgr.instance.PlayerMarkList -= OnMarkList;

        //MGCDelegateMgr.instance.SetupPlayerDescription -= OnSetupPlayerDescription;
        //MGCDelegateMgr.instance.SetupPlayerPortrait -= OnSetPlayerPortrait;
        //MGCDelegateMgr.instance.SetupPlayerMark -= OnSetPlayerEmblem;

        MGCDelegateMgr.instance.ReceiveQuiz -= OnReceiveQuiz;
        //MGCDelegateMgr.instance.Building_ClearCooldown -= OnClearCooldown;

        MGCDelegateMgr.instance.SendMsg -= OnRecvMsgAck;
        
        MGCDelegateMgr.instance.GetTechnologyList -= OnGetTechnologyList;
        MGCDelegateMgr.instance.UpdateTechnologyLevel -= OnUpdateTechnologyLevel;

        MGCDelegateMgr.instance.ShopInfo -= OnShopInfo;

        MGCDelegateMgr.instance.GetRelationRequestList -= OnGetAvatarRelationListCount;
        MGCDelegateMgr.instance.GetRelationRequestList -= OnGetFriendList;
        MGCDelegateMgr.instance.AskCreateRelation -= OnAskCreateRelation;
        MGCDelegateMgr.instance.BreakUpRelation -= OnBreakUpRelation;

        MGCDelegateMgr.instance.GetMoneyBonus -= OnGetMoneyBonus;
        MGCDelegateMgr.instance.Backpack_Info -= OnBackpack_Info;
        MGCDelegateMgr.instance.Backpack_Info -= OnBackpack_Info_SwitchScene;

        MGCDelegateMgr.instance.GetObjectIdList -= OnGetObjectIdList;

        MGCDelegateMgr.instance.GetTime -= OnGetServerTime;

        MGCDelegateMgr.instance.GetAvatarBuff -= OnGetTerritoryBuff;

        MGCDelegateMgr.instance.MallBuyItem -= OnBuyMallItem;
        MGCDelegateMgr.instance.GetPersonalLimit -= OnGetItemPersonalLimitTable;
		MGCDelegateMgr.instance.ExchangeSerialNumber -= OnExchangeSerialNumber;
		
		MGCDelegateMgr.instance.InviteTeam -= OnBattleInviteTerritoryDelegate;        

        //Add by Neo
        MGEventManager.instance.UnregisterEvent(EEvent.GAME_PAUSE, OnGamePause);
        MGEventManager.instance.UnregisterEvent(EEvent.GAME_RESUME, OnGameResume);
        m_territoryBuffMgr.UnregisterResumeEvent();        
        MGCDelegateMgr.instance.HeroCollection -= OnGetHeroCollection;

        MGCDelegateMgr.instance.UpdateUI -= OnServerUpdateInfo;

        MGCDelegateMgr.instance.VillagerList -= OnVillagerList;
        MGCDelegateMgr.instance.VillagerEventList -= OnVillagerEventList;
		MGCDelegateMgr.instance.VillagerEventAnswer -= OnVillagerEventAnswer;

        MGCDelegateMgr.instance.RefreshEverydayQuest -= OnRefreshEverydayQuest;
        MGCDelegateMgr.instance.AddDailyQuestReportCount -= OnAddDailyQuestReportCount;
		MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCountMissionPage;
		MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCount;
		MGCDelegateMgr.instance.CompleteQuest -= OnCompleteQuest;
		MGCDelegateMgr.instance.AcceptQuest -= OnAcceptQuest;
		MGCDelegateMgr.instance.GiveUpQuest -= OnGiveUpQuest;
		MGCDelegateMgr.instance.ActivateQuest -= OnActiveQuest;
		MGCDelegateMgr.instance.GetQuestDetail -= OnQuestDetail;

		postHeroListHandler = null;		

		MGCDelegateMgr.instance.TowersInfo -= OnHeroTowersInfo;

		MGCDelegateMgr.instance.HeroRefresh -= OnHeroRefresh;
		MGCDelegateMgr.instance.PartialHero -= OnHeroRefreshUpdateList;

		MGCDelegateMgr.instance.ExecuteQuest -= OnExecuteQuest;		

		MGCDelegateMgr.instance.GetDrawEggCount -= OnGetDrawEggCount;
		MGCDelegateMgr.instance.GetAdvertisementArticles -= OnGetMallAdvertisementArticle;
		RemoveAllQuestListHandler();
		RemoveAllBNOHeroListHandler();
		RemoveAllBNOHeroListUpdateHandler();

        MGCDelegateMgr.instance.SyncProductCategory -= OnSyncProductCategory;
		MGCDelegateMgr.instance.GetRewardSize_MyFriendVisitMeEvent -= OnGetRewardSize_MyFriendVisitMePacket;
		RemoveAllBackpackInfoHandler();
		RemoveAllManupulateDelegate();

        MGCDelegateMgr.instance.CashFlow_Purchase -= OnCashFlow_Purchase;
		
		MGCDelegateMgr.instance.GetRecommendList -= OnGetRecommendList;
		MGCDelegateMgr.instance.AvatarStatList -= SetRelationListPage;
		MGCDelegateMgr.instance.GetFriendVisitedListEvent -= OnGetFriendVisitedListPacket;
		MGCDelegateMgr.instance.GetHeroVisitFriendCoolDownListEvent -= OnGetHeroVisitFriendCoolDownListPacket;
		MGCDelegateMgr.instance.AvatarStatistics -= OnAvatarStatistics;
		MGCDelegateMgr.instance.SearchAvatar -= OnSearchAvatar;
		MGCDelegateMgr.instance.GetReward_HeroVisitFriendByIDEvent -= OnGetReward_HeroVisitFriendByIDPacket;
		MGCDelegateMgr.instance.GetReward_MyFriendVisitMeEvent -= OnGetReward_MyFriendVisitMePacket;

#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		MGEventManager.instance.UnregisterEvent(EEvent.INPUT_KEY_UP, DealHotkey_Territory);
#endif
        MGCDelegateMgr.instance.GetLoginRewardEvent -= OnLoginReward;

        TutorialOrganizerComp.TutorialIsDone -= new MGTutorialOrganizer.EventHandler(TutorialIsDone);

		CancelInvoke("StopFlagAnimation");
		CancelInvoke("PlayFlagAnimation");
		CancelInvoke("SendVillagerREQ");

        //MGCDelegateMgr.instance.SendMsg -= OnsendMessage;
    }

    public void BringInFunctionPanel(GameObject caller)
    {
        //debug.log("Bring in function panel");
        UnselectAllMainMenuButton();
        bottomMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().BringIn();
		//storeAndWarMenuPanel.BringIn();
        //functionPanelOpenCloseBtn.methodToInvoke = "DismissFunctionPanel";
        //MGAudioManager.PlaySound("Accept");
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_ACCEPT);
    }

    public void DismissFunctionPanel(GameObject caller)
    {
        //debug.log("Dismiss function panel");
        UnselectAllMainMenuButton();
        bottomMainMenuPanel.transform.parent.gameObject.GetComponent<UIPanel>().Dismiss();
		//storeAndWarMenuPanel.Dismiss();
        //functionPanelOpenCloseBtn.methodToInvoke = "BringInFunctionPanel";
        //MGAudioManager.PlaySound("Cancel");
		MGAudioManager.PlaySoundByKey(MGCSoundType.SOUND_TYPE.SOUND_CANCEL);
    }

    private bool isBGPanelBringIn = false;

    public void BringInBGPanel()
    {
        if (isBGPanelBringIn)
            return;

        isBGPanelBringIn = true;
		functionMenuBGPanel.transform.FindChild("BGPic").gameObject.GetComponent<UIPanel>().BringIn();
		functionMenuBGPanel.transform.FindChild("BGPic").gameObject.GetComponent<UIPanel>().AddTempTransitionDelegate(OnBGPanelPicEndBringin);
        //functionMenuBGPanel.transform.FindChild("BGPic1").gameObject.GetComponent<UIPanel>().BringIn();
        //functionMenuBGPanel.transform.FindChild("BGPic2").gameObject.GetComponent<UIPanel>().BringIn();

        //functionMenuBGPanel.transform.FindChild("BGPic3").gameObject.GetComponent<UIPanel>().AddTempTransitionDelegate(OnBGPanelPic3EndBringin);
        //functionMenuBGPanel.transform.FindChild("BGPic3").gameObject.GetComponent<UIPanel>().BringIn();

        //functionPanelOpenCloseBtn.Hide(true);
        //CDQueuePanelOpenCloseBtn.Hide(true);
        //debug.log("BG panel bring in done");
    }

    public void SetTempEndBGPanelBringInDelegate(UIPanelBase.TransitionCompleteDelegate del)
    {
        functionMenuBGPanel.transform.FindChild("BGPic").gameObject.GetComponent<UIPanel>().AddTempTransitionDelegate(del);
    }

    public void OnBGPanelPicEndBringin(UIPanelBase panel, EZTransition transition)
    {
        functionMenuBGPanel.transform.FindChild("FunctionMenuBGMask").gameObject.GetComponent<UIPanel>().Dismiss();
		if (mainCamera.GetComponent<iTween>() == null)
			mainCamera.gameObject.active = false;
		else
			InvokeRepeating("CheckIfMainCameraMoveOver", 0, 0.5f);


        iTween.CameraFadeDestroy();
		functionMenuBGPanel.transform.FindChild("BGPic").localScale =new Vector3(2.0f,2.0f,2.0f);
    }

	public void CheckIfMainCameraMoveOver()
	{
		if (mainCamera.GetComponent<iTween>() == null)
		{
			mainCamera.gameObject.active = false;
			CancelInvoke("CheckIfMainCameraMoveOver");
		}
	}

    public void DismissBGPanel()
    {
        if (!isBGPanelBringIn)
            return;
        isBGPanelBringIn = false;

        mainCamera.gameObject.active = true;
		functionMenuBGPanel.transform.FindChild("BGPic").gameObject.GetComponent<UIPanel>().Dismiss();
        //functionMenuBGPanel.transform.FindChild("BGPic1").gameObject.GetComponent<UIPanel>().Dismiss();
        //functionMenuBGPanel.transform.FindChild("BGPic2").gameObject.GetComponent<UIPanel>().Dismiss();
        //functionMenuBGPanel.transform.FindChild("BGPic3").gameObject.GetComponent<UIPanel>().Dismiss();

        //functionPanelOpenCloseBtn.Hide(false);
        //CDQueuePanelOpenCloseBtn.Hide(false);
    }
    

        

    /// <summary>
    ///  functions of panels under hero menu
    /// </summary> 	

    //private string cachedGoalBtnName = "";

    private UIStateToggleBtn lastMenuBtnPressed = null;
    private UIPanel lastMenuPanel = null;

    private void TestHighLight(GameObject caller)
    {
        ////debug.log(caller.name + " is high-lighted!");
        HighLightButton(caller.GetComponent<UIStateToggleBtn>());

        TestMenuSwitch();
    }

    private void TestMenuSwitch()
    {
        homeMenuPanelSet.transform.FindChild("HomeMainMenuPanel").gameObject.GetComponent<UIPanel>().Dismiss();
        SwitchMenu( homeMenuPanelSet.transform.FindChild("HomeInfoMenuPanel").gameObject.GetComponent<UIPanel>() );
    }

    private void HighLightButton(UIStateToggleBtn btn)
    {
		_m_lastMouseoverSubmenuBG = null;

        if (lastMenuBtnPressed != null)
        {
            lastMenuBtnPressed.SetColor(Color.white);
            lastMenuBtnPressed.transform.parent.FindChild("Text").gameObject.GetComponent<SpriteText>().SetColor(Color.white);
            lastMenuBtnPressed.gameObject.transform.parent.GetComponent<UIPanel>().Dismiss();

            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + lastMenuBtnPressed.name[0]).gameObject.GetComponentInChildren<PackedSprite>()
                .SetColor(new Color(MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_R, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_G, MGCTerritoryTypeDef.Constants.DEFAULT_MENU_BUTTON_COLOR_B));
            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + lastMenuBtnPressed.name[0]).gameObject.GetComponent<UIPanel>().Dismiss();

            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonCollider" + lastMenuBtnPressed.name[0]).gameObject.GetComponent<UIPanel>().Dismiss();
        }
        lastMenuBtnPressed = btn;

        if (btn == null)
            return;

        btn.SetColor(Color.black);
        btn.gameObject.transform.parent.FindChild("Text").gameObject.GetComponent<SpriteText>().SetColor(Color.yellow);
        btn.gameObject.transform.parent.GetComponent<UIPanel>().BringIn();
        commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + btn.name[0]).gameObject.GetComponentInChildren<PackedSprite>().SetColor(Color.white);
        commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + btn.name[0]).gameObject.GetComponent<UIPanel>().BringIn();
        commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonCollider" + btn.name[0]).gameObject.GetComponent<UIPanel>().BringIn();
    }

    private void SwitchMenu(UIPanel menuToBringIn)
    {
		MGDebug.Log( "SwitchMenu" );
		if (lastMenuPanel == menuToBringIn)
			return;

		if (lastMenuPanel != null)		
			lastMenuPanel.Dismiss();
		
		GameObject lvPnl;
		if (menuToBringIn == null)
		{
			commonMenuPanelSet.transform.FindChild("MenuButtonPanel").gameObject.GetComponent<UIPanel>().Dismiss();
			menuBackBtnPanel.Dismiss();
			CancelInvoke("RotateBuildingInfoBG");

			for (int j = 1; j <= MGCTerritoryTypeDef.Constants.MAX_MENU_BUTTON_NUMBER; j++)
				commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + j).localScale = Vector3.one;

			if (lastMenuPanel != null && lastMenuPanel.transform.parent.FindChild("LVPanel") != null)
			{
				lvPnl = lastMenuPanel.transform.parent.FindChild("LVPanel").gameObject;

				if (lvPnl != null && lvPnl.GetComponent<UIPanel>() != null)
					lvPnl.GetComponent<UIPanel>().Dismiss();
				lastMenuPanel = null;
			}
			return;
		}
		
        lastMenuPanel = menuToBringIn;
		lvPnl = lastMenuPanel.transform.parent.FindChild("LVPanel").gameObject;
		if (lvPnl != null && lvPnl.GetComponent<UIPanel>() != null)
			lvPnl.GetComponent<UIPanel>().BringIn();

        int i;
        for ( i = 1; i <= menuToBringIn.transform.GetChildCount(); i++)
        {
            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + i).localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }
        for(;i<= MGCTerritoryTypeDef.Constants.MAX_MENU_BUTTON_NUMBER; i++)
            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").FindChild("MenuPanel").FindChild("ButtonBG" + i).localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

        menuToBringIn.BringIn();
    }

    private void UpdatehouseLVInMenu(MGCCity_INFOEventArgs cityInfo)
    {
        if (cityInfo == null || cityInfo.cityInfo.buildingsInfo == null)
            return;
 
        downloaderRequestList.Clear();
        string request;                
		
        foreach (MGCStructDef.BuildingInfo bInfo in cityInfo.cityInfo.buildingsInfo)
        {
            buildingInfoCache[bInfo.buildingType] = bInfo;

            UpdateBuildingLVDisplay(bInfo.buildingType, bInfo.buildingLevel);
            request = "";
            
            switch (bInfo.buildingType)
            {
                case (byte)MGCTypeDef.BUILDING_ID.HERO_ROOM:
                    request = LoadSelectedBuildingModel(BuildingEnum.BUILDING_HERO, (bInfo.buildingLevel-1 - (bInfo.buildingLevel-1) % 20) / 20 + 1);
                    break;

                case (byte)MGCTypeDef.BUILDING_ID.MAIN_TOWN:
                    request = LoadSelectedBuildingModel(BuildingEnum.BUILDING_HOME, (bInfo.buildingLevel - 1 - (bInfo.buildingLevel - 1) % 20) / 20 + 1);
                    break;

                case (byte)MGCTypeDef.BUILDING_ID.CRYSTAL_MINE:
                    request = LoadSelectedBuildingModel(BuildingEnum.BUILDING_MINE, (bInfo.buildingLevel - 1 - (bInfo.buildingLevel - 1) % 20) / 20 + 1);
                    break;

                case (byte)MGCTypeDef.BUILDING_ID.FORGE:
                    request = LoadSelectedBuildingModel(BuildingEnum.BUILDING_FACTORY, (bInfo.buildingLevel - 1 - (bInfo.buildingLevel - 1) % 20) / 20 + 1);
                    break;

                case (byte)MGCTypeDef.BUILDING_ID.COLLEGE:
                    request = LoadSelectedBuildingModel(BuildingEnum.BUILDING_TECH, (bInfo.buildingLevel - 1 - (bInfo.buildingLevel - 1) % 20) / 20 + 1);
                    break;
            }

            if(request.Length != 0)
			{			
                downloaderRequestList.Add(requestBuildingName);
				MGDownloader.instance.AddAssetNameToUnloadList(requestBuildingName);
			}
        }

        // Load ally and town view directly, since no server info now     
        // Only do this while the city info is not for updating only a single building
        if (cityInfo.cityInfo.buildingsInfo.Length > 1)
        {
            downloaderRequestList.Add(LoadSelectedBuildingModel(BuildingEnum.BUILDING_TOWNVIEW, 1));
            downloaderRequestList.Add(LoadSelectedBuildingModel(BuildingEnum.BUILDING_POINT, 2));
			downloaderRequestList.Add(LoadSelectedBuildingModel(BuildingEnum.BUILDING_ALLY, 3));
			MGDownloader.instance.AddAssetNameToUnloadList(LoadSelectedBuildingModel(BuildingEnum.BUILDING_TOWNVIEW, 1));
			MGDownloader.instance.AddAssetNameToUnloadList(LoadSelectedBuildingModel(BuildingEnum.BUILDING_POINT, 2));
			MGDownloader.instance.AddAssetNameToUnloadList(LoadSelectedBuildingModel(BuildingEnum.BUILDING_ALLY, 3));
        }
        MGDebug.Log("Send downloader request: count = " + downloaderRequestList.Count);
        m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000030"));
		MGDebug.Log("Send downloader request: count = " + downloaderRequestList.Count);
		InvokeRepeating("UpdateTerritoryDownloadProgress", 0.1f, 0.1f);  
		m_isResourcesDownloadComplete = false;
        MGDownloader.instance.DownloadAndCacheAssetBundle(downloaderRequestList, new MGDownloader.DownloaderStatusDelegate(OnDownloaderCompleteLoadingBuildingModel), m_territoryDownloadListName);		     
    }
	
	void UpdateTerritoryDownloadProgress() 
	{		
		int totalCount = MGDownloader.instance.getTotalImmediateItemCount(m_territoryDownloadListName);
		int curCount = MGDownloader.instance.getCurrentImmediateItemCount(m_territoryDownloadListName);
		m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000030") + "\n" + 
			(totalCount -  curCount) + "/" + totalCount, true);
    }	
	
    // Hero menu -> Hero house update -> OK	

	private string m_stageID;
	private void OnBattleInviteTerritoryDelegate(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.InviteTeam -= OnBattleInviteTerritoryDelegate;
		MGCInviteTeamEventArgs inviteTeamEventArgs = (MGCInviteTeamEventArgs)bea;
		m_stageID = inviteTeamEventArgs.StageObjectId;
		
		IncrementBottomFunctionMgsCount(1, BottomFunctionEnum.BOTTOM_MAIL);
		
		string stageName = m_wordsTableRef.getWorldText(inviteTeamEventArgs.StageObjectId + "_stage_name");
		string mergeStageName = m_wordsTableRef.getUIText("0301039").Replace("%1d", inviteTeamEventArgs.AvatarName);
		string broadcastString = mergeStageName.Replace("%2d", stageName);
		
		m_broadcast.ShowBroadcast(broadcastString);
		Invoke("DataBroadCastHide", 10.0f);
	}
	
	private bool haveReceiveInviteUpdateTeamInfo = false;
	private void OnBattleInviteUpdateTeamTerritoryDelegate(object sender, MGCBaseEventArgs bea)
    {
		MGCDelegateMgr.instance.UpdateTeam -= OnBattleInviteUpdateTeamTerritoryDelegate;
		MGCUpdateTeamEventArgs updateTeamEventArgs = (MGCUpdateTeamEventArgs)bea;
		//Tell sceneMaster the execute the quest.
        m_commonDataRef.levelData = MGLevelDataFetcher.FetchLevelData(m_stageID, new MGXMLObject((TextAsset)MGDownloader.instance.getObject(BundleType.LOCALIZED_GAMEDATA_BUNDLE, "world")));
		m_commonDataRef.updateTeamArgs = updateTeamEventArgs;
        //Block all input.
        UIManager.instance.blockInput = true;
		haveReceiveInviteUpdateTeamInfo = true;
		
		TryToSwitchScene("Lobby");
		m_commonDataRef.bLevelSelectCompleteAndOpenHeroSelectUI = true;
		TryToSwitchScene("LevelSelect");
	}
	
	private void OnBattleInviteResponseTerritoryDelegate(object sender, MGCBaseEventArgs bea)
	{
		MGCDelegateMgr.instance.InviteTeamResponse -= OnBattleInviteResponseTerritoryDelegate;
		MGCInviteTeamResponseEventArgs inviteTeamResponseEventArgs = (MGCInviteTeamResponseEventArgs)bea;
		if(inviteTeamResponseEventArgs.ErrorCode == 0)//SUCCESS
		{
			InvokeRepeating("OnBattleInviteSwitchScene", 1.0f, 1.0f);
		}
		else
		{
			PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
			switch ((MGCErrorCode.SocialOpErrorCode)inviteTeamResponseEventArgs.ErrorCode)
            {
				case MGCErrorCode.SocialOpErrorCode.Invite_Team_Response_TeamFull:
					pageStyle.message = m_wordsTableRef.getUIText("0301073");
                    break;
				case MGCErrorCode.SocialOpErrorCode.Invite_Team_Response_Failed:
					pageStyle.message = m_wordsTableRef.getUIText("0301073");
                    break;
				default:
					pageStyle.message = "";
					MGDebug.Log("Unknown error");
                    break;
			}
            pageStyle.message = m_wordsTableRef.getUIText("0301073");
            pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
            SetPopUpPage(pageStyle);
		}
	}
	
	private void OnBattleInviteSwitchScene()
	{
		if(haveReceiveInviteUpdateTeamInfo==true)
		{
			TryToSwitchScene("Lobby");
			m_commonDataRef.bLevelSelectCompleteAndOpenHeroSelectUI = true;
			TryToSwitchScene("LevelSelect");
			
			haveReceiveInviteUpdateTeamInfo = false;
			CancelInvoke("OnBattleInviteSwitchScene");
		}
	}

    public string mergeBroadMsg = "";
	private void DataBroadCastShow()
    {
		List<BroadcastPack> BroadcastList = new List<BroadcastPack>();
		BroadcastList = m_commonDataRef.broadcastPackList;
        long NowTick = System.DateTime.Now.Ticks;
		announcementList.Clear();
		AnnouncementData AnnData = new AnnouncementData();
     
		for(int i=0;i<BroadcastList.Count;i++)
		{
			if(BroadcastList[i].Type==(byte)MGCTypeDef.SOCIAL_GROUP_TYPE.SOCIAL_GROUP_TYPE_ALLUSER 
			   || BroadcastList[i].Type==(byte)MGCTypeDef.SOCIAL_GROUP_TYPE.SOCIAL_GROUP_TYPE_MARKET
               || BroadcastList[i].Type == (byte)MGCTypeDef.SOCIAL_GROUP_TYPE.SOCIAL_GROUP_TYPE_BULLETIN)
			{
				if(BroadcastList[i].StartBroadTime<NowTick && BroadcastList[i].EndBroadTime>NowTick)  //In Order to show broadcast, time must in range 
                {
                    switch ((MGCTypeDef.MTUMAIL_TYPE)BroadcastList[i].ContentType)
                    {
                        case MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_CLOSE_SERVER: //close server
                            { 
                                MGGossipMgr.Instance.RefreshChatContent(new MGGossipMgr.LogText(MGGossipMgr.LogType.SystemImportant,Color.red + BroadcastList[i].Content, 1));
                                Invoke("ServerclosedCountdown", 0.0f);
                            }
                            break;
                        case MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_MTU: 
                            if (BroadcastList[i].MessageType == "0801008") //important system broadcast
                            { MGGossipMgr.Instance.RefreshChatContent(new MGGossipMgr.LogText(MGGossipMgr.LogType.SystemImportant, Color.red + BroadcastList[i].Content, 5)); }
                            else if (BroadcastList[i].MessageType == "0801009") //normal system broadcast
                            { MGGossipMgr.Instance.RefreshChatContent(new MGGossipMgr.LogText(MGGossipMgr.LogType.SystemNormal, Color.yellow + BroadcastList[i].Content, 5)); }
                            break;
                        case MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_GAMEPLAY://Gameplay

                            //RollEgg
                            if (BroadcastList[i].MessageType == "0801001" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.RollEgg, BroadcastList[i].Content, 4); }
                            else if (BroadcastList[i].MessageType == "0801011" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.RollEgg, BroadcastList[i].Content, 4); }
                            //Arena
                            else if ((BroadcastList[i].MessageType == "0801002" ||
                                      BroadcastList[i].MessageType == "0801012" ||
                                      BroadcastList[i].MessageType == "0801013" ||
                                      BroadcastList[i].MessageType == "0801014" ||
                                      BroadcastList[i].MessageType == "0801015" ||
                                      BroadcastList[i].MessageType == "0801016" ||
                                      BroadcastList[i].MessageType == "0801017" ||
                                      BroadcastList[i].MessageType == "0801025" ||
                                      BroadcastList[i].MessageType == "0801026" ) && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Arena, BroadcastList[i].Content, 4); }
                            

                            //Vip
                            else if (BroadcastList[i].MessageType == "0801003" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Vip, Color.yellow + BroadcastList[i].Content, 4); }

                            //Metempsychosis
                            else if (BroadcastList[i].MessageType == "0801010" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Metempsychosis, Color.yellow + BroadcastList[i].Content , 4); }
                            else if (BroadcastList[i].MessageType == "0801004" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Metempsychosis, Color.yellow + BroadcastList[i].Content, 4); }
                            else if (BroadcastList[i].MessageType == "0801005" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Metempsychosis, Color.yellow + BroadcastList[i].Content, 4); }

                            //Equip Get
                            else if (BroadcastList[i].MessageType == "0801018" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Equip, Color.yellow + BroadcastList[i].Content, 4); }

                            //Level Upgrade
                            else if (BroadcastList[i].MessageType == "0801019" && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.LevelUpgrade, Color.yellow + BroadcastList[i].Content, 4); }

                            //Mine
                            else if ((BroadcastList[i].MessageType == "0801020" ||
                                      BroadcastList[i].MessageType == "0801021" ) && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.Mine, Color.yellow + BroadcastList[i].Content, 4); }

                            //Equip Upgrade
                            else if ((BroadcastList[i].MessageType == "0801027" ||
                                      BroadcastList[i].MessageType == "0801022" ||
                                      BroadcastList[i].MessageType == "0801023" ||
                                      BroadcastList[i].MessageType == "0801024" ) && BroadcastList[i].IsPlayed == false)
                            { MGGossipMgr.Instance.AddBroadCastMessage(MGGossipMgr.LogType.EquipUpgrade, Color.yellow + BroadcastList[i].Content, 4); }

                            break;
                        case MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_OPERATION:
                            if (BroadcastList[i].MessageType == "0801008") //important broadcast
                            { MGGossipMgr.Instance.RefreshChatContent(new MGGossipMgr.LogText(MGGossipMgr.LogType.SystemImportant, Color.red + BroadcastList[i].Content, 2)); }
                            else if (BroadcastList[i].MessageType == "0801009") //normal broadcast
                            { MGGossipMgr.Instance.RefreshChatContent(new MGGossipMgr.LogText(MGGossipMgr.LogType.SystemNormal, Color.yellow + BroadcastList[i].Content, 2)); }
                            break;
                    }
                    
                    //Show Text in BULLETIN
                    if (BroadcastList[i].ContentType == (byte)MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_BULLETIN)
                    {
                        AnnData.Content = BroadcastList[i].Content;
                        AnnData.MsgID = BroadcastList[i].ID;
                        AnnData.Title = BroadcastList[i].Title;
                        announcementList.Add(AnnData);
                    }
                }	
			}
            BroadcastPack BP = BroadcastList[i];
            BP.IsPlayed = true;
            BroadcastList[i] = BP;
		}

        // Show Next BroadCast Text 
        MGGossipMgr.LogText text = MGGossipMgr.Instance.GetNextBroadCastMsg();
        if (text != null)
        {
            mergeBroadMsg = text.GetText();
            text.SetShowed();
        }

        if (mergeBroadMsg != "")
        {
            m_broadcast.ShowBroadcast(mergeBroadMsg);
            Invoke("DataBroadCastHide", 10.0f);
        }

	}
	
	private void DataBroadCastHide()
	{
		m_broadcast.Hide();
        mergeBroadMsg = "";
	}
	
	private void SendGetMTU()
	{
		m_clientSDKRef.GetMTUMails();
	}
	
	private void UpdateBroadCastCycle()
	{
		int NowHour = System.DateTime.Now.Hour;
		int NowMinute = System.DateTime.Now.Minute;
		if(NowHour==0&&NowMinute==0)
		{
			m_clientSDKRef.GetMTUMails();//update broadcast data at 00:00 everyday
		}
	}
	
	public void RealTimeBroadCastShow(string msg)
    {
		m_broadcast.ShowBroadcast(msg);
	}
	
	public void UpdateBroadcast(MGCGetMTUMailsEventArgs getMTUMailsEventArgs)
	{
		CancelInvoke("SendGetMTU");
		int BroadcastMaxNum = 40;
		if(getMTUMailsEventArgs.getMTUMails()==null)
		{
			MGDebug.Log("broadcast data null");
			return;
		}
		if(getMTUMailsEventArgs.getMTUMails().Length>BroadcastMaxNum)
		{
			MGDebug.Log("broadcast out of max num");
			return;
		}
		List<BroadcastPack> BroadcastList = new List<BroadcastPack>();
		BroadcastList = m_commonDataRef.broadcastPackList;
		List<BroadcastAttrPack> BroadcastAttrList = new List<BroadcastAttrPack>();
		BroadcastAttrList = m_commonDataRef.broadcastAttrPackList;
		BroadcastPack SingleBroadData;
		long NowTick = System.DateTime.Now.Ticks;
		CancelInvoke("DataBroadCastShow");
		CancelInvoke("DataBroadCastHide");
		BroadcastAttrPack BroadcastAttrCache;
		int BroadcastAttrListIndex = 0;

		foreach(MTUMail mtuMail in getMTUMailsEventArgs.getMTUMails())
		{
			BroadcastAttrCache.ID = mtuMail.getId();
			BroadcastAttrCache.MessageType = mtuMail.getMessageType();
			DateTime dateTime = DateTime.Parse(mtuMail.getStartTime());
			BroadcastAttrCache.StartTime = dateTime.Ticks;
            BroadcastAttrCache.EndTime = BroadcastAttrCache.StartTime + mtuMail.getDuration() * 864000000000; //1 Day = 864000000000 Ticks
            BroadcastAttrCache.Status = mtuMail.getMTUStatus(); // Status of mtuMail (Refer to thefollowing enum -- MGCTypeDef.MAIL_STATUS)
            BroadcastAttrCache.Type = mtuMail.getSendScope(); // Type of send range (Refer the following enum -- MGCTypeDef.SOCIAL_GROUP_TYPE) 
            BroadcastAttrCache.ContiunedTime = (mtuMail.getLifeCycle()) * 10000000;//1 Second = 10000000 Ticks
			BroadcastAttrCache.MessageParams = mtuMail.getMessageParams(); //Message text
            BroadcastAttrCache.Title = mtuMail.getTitle();//Message Title
            BroadcastAttrCache.ContentType = mtuMail.getType();//Typr of Content (Refer the followiing enum -- MGCTypeDef.MTUMAIL_TYPE)

            if ((MGCTypeDef.MTUMAIL_TYPE)BroadcastAttrCache.ContentType == MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_CLOSE_SERVER)
            {
                if ((BroadcastAttrCache.StartTime < System.DateTime.Now.Ticks) && (BroadcastAttrCache.EndTime > System.DateTime.Now.Ticks))
                {
                    //how many time left before server closed
                    long _long = 0;
                    if (long.TryParse(BroadcastAttrCache.MessageParams[0], out _long))
                    {
                        MGGameGod.ServerCloseCountDownStartTick = BroadcastAttrCache.StartTime;
                        MGGameGod.ServerCloseCountDownEndTick = BroadcastAttrCache.EndTime;

                        float timeleft = (float)((BroadcastAttrCache.StartTime + long.Parse(BroadcastAttrCache.MessageParams[0]) * 60.0f * 10000000.0f - System.DateTime.Now.Ticks) / (60.0f * 10000000f));
                        MGGameGod.TimeLeftOfCloseServer = UnityEngine.Mathf.CeilToInt(timeleft);
                        BroadcastAttrCache.MessageParams = new string[] { MGGameGod.TimeLeftOfCloseServer.ToString() };
                    }
                }
            }

            //Switch Hero ID String to Hero Name String(Server only know Hero Object ID)
            if ((MGCTypeDef.MTUMAIL_TYPE)BroadcastAttrCache.ContentType == MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_GAMEPLAY)
            {
                for (int i = 0; i < BroadcastAttrCache.MessageParams.Length; ++i)
                {
                    BroadcastAttrCache.MessageParams[i] = ParseIDToString(BroadcastAttrCache.MessageParams[i]);
                    BroadcastAttrCache.MessageParams[i] = ParseArenaIDToString(BroadcastAttrCache.MessageParams[i]);
                }

                //For RollingEggs Machine
                if (BroadcastAttrCache.MessageType == "0801001" ||
                    BroadcastAttrCache.MessageType == "0801011")
                {
                    if (BroadcastAttrCache.MessageParams.Length < 2 || BroadcastAttrCache.MessageParams[1] == "")
                        continue;

                    switch (BroadcastAttrCache.MessageParams[1])
                    {
                        case "1":
                            BroadcastAttrCache.MessageParams[1] = m_wordsTableRef.getUIText("0201094");
                            break;
                        case "2":
                            BroadcastAttrCache.MessageParams[1] = m_wordsTableRef.getUIText("0900004") + m_wordsTableRef.getUIText("0500008");
                            break;
                        case "3":
                            BroadcastAttrCache.MessageParams[1] = m_wordsTableRef.getUIText("0201093");
                            break;
                        case "4":
                            BroadcastAttrCache.MessageParams[1] = m_wordsTableRef.getUIText("0201101");
                            break;
                    }
                }
            }

            BroadcastAttrCache.Links = mtuMail.getLinks();	

			switch (mtuMail.getOccurrneces())
            {
				case 0://weekly
					BroadcastAttrCache.InvokeTime = 6048000000000;
                    break;
				case 1://monthly
					BroadcastAttrCache.InvokeTime = 25920000000000;
                    break;
				case 2://dialy
					BroadcastAttrCache.InvokeTime = 864000000000;
                    break;
				default:
					BroadcastAttrCache.InvokeTime = 0;
					MGDebug.Log("MTU Occurrnece not exist");
                    break;
			}
			if(BroadcastAttrList.Count>=BroadcastMaxNum)
			{
				if(BroadcastAttrCache.ContentType==(byte)MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_BULLETIN)
				{
					string announcementIDPerfKey = "announcement" + BroadcastAttrList[BroadcastAttrListIndex].ID;
					MGSaveManager.DeleteKey(announcementIDPerfKey);
				}
				BroadcastAttrList.Remove(BroadcastAttrList[BroadcastAttrListIndex]);
			}
			BroadcastAttrList.Add(BroadcastAttrCache);
			BroadcastAttrListIndex++;
//			switch (BroadcastAttrCache.Status)
//            {
//                case 0:
//					BroadcastAttrList.Add(BroadcastAttrCache);
//                    break;
//				case 1:
//					for(int j=0;j<BroadcastAttrList.Count;j++)
//					{
//						if(BroadcastAttrList[j].ID == BroadcastAttrCache.ID)
//						{
//							BroadcastAttrList[j] = BroadcastAttrCache;
//							break;
//						}
//					}
//                    break;
//				case 2:
//					for(int j=0;j<BroadcastAttrList.Count;j++)
//					{
//						if(BroadcastAttrList[j].ID == BroadcastAttrCache.ID)
//						{
//							BroadcastAttrList.Remove(BroadcastAttrList[j]);
//							break;
//						}
//					}
//                    break;
//            }
		}

		m_commonDataRef.broadcastAttrPackList = BroadcastAttrList;
		BroadcastList.Clear();
		for(int i=0;i<BroadcastAttrList.Count;i++)
		{
			long startTime = BroadcastAttrList[i].StartTime;
			long endTime = BroadcastAttrList[i].EndTime;
			long broadTime = startTime;
			long invokeTime = BroadcastAttrList[i].InvokeTime;
			if(NowTick<=endTime)
			{
				while(broadTime + BroadcastAttrList[i].ContiunedTime < NowTick)
				{
					broadTime += invokeTime;
				}
				SingleBroadData.Content = m_wordsTableRef.getUIText(BroadcastAttrList[i].MessageType);
				for(int j=0;j<BroadcastAttrList[i].MessageParams.Length;j++)
				{
					string strIndex = "%" + (j+1) + "d";
					SingleBroadData.Content = SingleBroadData.Content.Replace(strIndex, BroadcastAttrList[i].MessageParams[j]);
				}
				SingleBroadData.ID = BroadcastAttrList[i].ID;
                SingleBroadData.MessageType = BroadcastAttrList[i].MessageType;
				SingleBroadData.Type = BroadcastAttrList[i].Type;
				SingleBroadData.StartBroadTime = broadTime;
				SingleBroadData.EndBroadTime = SingleBroadData.StartBroadTime + BroadcastAttrList[i].ContiunedTime;
				SingleBroadData.Title = BroadcastAttrList[i].Title;
				SingleBroadData.ContentType = BroadcastAttrList[i].ContentType;
				SingleBroadData.Links = BroadcastAttrList[i].Links;
                SingleBroadData.IsPlayed = false;
				BroadcastList.Add(SingleBroadData);
			}
		}
		m_commonDataRef.broadcastPackList = BroadcastList;
        InvokeRepeating("DataBroadCastShow", 1.0f, MGGameGod.BroadCastfrequency);
	}

    private void ServerclosedCountdown()
    {
        for (int i = 0; i < m_commonDataRef.broadcastPackList.Count; ++i)
        {
            if ((MGCTypeDef.MTUMAIL_TYPE)m_commonDataRef.broadcastPackList[i].ContentType == MGCTypeDef.MTUMAIL_TYPE.MTUMAIL_TYPE_CLOSE_SERVER)
            {
                if ((System.DateTime.Now.Ticks > m_commonDataRef.broadcastPackList[i].StartBroadTime) && (System.DateTime.Now.Ticks < m_commonDataRef.broadcastPackList[i].EndBroadTime))
                {
                    MGDebug.Log("MGGameGod.TimeLeftOfCloseServer before = " + MGGameGod.TimeLeftOfCloseServer);
                    BroadcastPack Bp = m_commonDataRef.broadcastPackList[i];
                    int T = UnityEngine.Mathf.FloorToInt((m_commonDataRef.broadcastPackList[i].EndBroadTime - System.DateTime.Now.Ticks) / 600000000f);
                    Bp.Content = Bp.Content.Replace(MGGameGod.TimeLeftOfCloseServer.ToString(), T.ToString());
                    MGGameGod.TimeLeftOfCloseServer = T;
                    MGDebug.Log("MGGameGod.TimeLeftOfCloseServer after = " + MGGameGod.TimeLeftOfCloseServer);
                    if (MGGameGod.TimeLeftOfCloseServer <= 0)
                    {
                        Bp.Content = "";
                        m_commonDataRef.broadcastPackList[i] = Bp;
                        CancelInvoke("ServerclosedCountdown");
                    }
                    else
                    { m_commonDataRef.broadcastPackList[i] = Bp; }
                }
                else
                {
                    CancelInvoke("ServerclosedCountdown");
                }
            }
        }
    }

    private void OnGamePause(MGEventArgs eventArgs)
    {
		//iTween.Pause();		
        //MGPauseGameEventArgs pea = (MGPauseGameEventArgs)eventArgs;
    }

    private List<string> invokeRepeatingMethodNameList = new List<string>();
    private int pauseDuration = 0;
    private void OnGameResume(MGEventArgs eventArgs)
    {		
		
        MGPauseGameEventArgs pea = (MGPauseGameEventArgs)eventArgs;
        
        // stop all invokeRepeating method
        pauseDuration = Mathf.FloorToInt(pea.PauseTimeSinceStartup);
        float secToInvoke = pea.PauseTimeSinceStartup - pauseDuration;
        //Debug.Log("pauseDuration = " + pauseDuration + ", " + secToInvoke);

        foreach (string method in invokeRepeatingMethodNameList)
        {
            if (!IsInvoking(method))
                continue;

            CancelInvoke(method);
            Invoke(method + "_Resume", 0);
            InvokeRepeating(method, secToInvoke, 1.0f);
            
        }
        //pauseDuration = 0;
    }

    private void OnServerUpdateInfo(object sender, MGCBaseEventArgs ea)
    {
        MGCUpdateUIEventArgs uiea = (MGCUpdateUIEventArgs)ea;
        //Debug.Log("Server want you to update info of type " + uiea.getUIType.ToString());

        int updateType = (int)uiea.getUIType;

        if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.SET_PALACE_LV && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.SET_LIBRARY_LV)
        {         
            SendCityInfoREQ();
            SendGeneralInfoREQ();
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.ADD_SILVER && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.ADD_GOLD)
        {         
            SendGeneralInfoREQ();
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.GIVE_ITEM && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.ADD_EQUIP_LV)
        {
            SendBackpackInfo();
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.SET_HERO_LV && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.DROP_HERO)
        {
            SendHeroListInfoREQ();
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.CLEAR_ALL_COOLDOWN && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.CLEAR_COOLDOWN)
        {
            SendCityCDInfoREQ();            
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.OPEN_QUEST && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.FINISH_QUEST)
        {
            SendQuestListReq();
        }
        else if (updateType == (int)MGCTypeDef.UPDATE_UI_LIST.OPEN_JEWELRY_MINE)
        {
            m_clientSDKRef.StoneMine("0", "0", "1");
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.UNLOCK_ALL_TECHNOLOGY && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.CLOSE_LOCK_TECHNOLOGY)
        {
            SendTechListREQ();
            SendGeneralInfoREQ();
        }
        else if (updateType >= (int)MGCTypeDef.UPDATE_UI_LIST.GET_ALL_MARK && updateType <= (int)MGCTypeDef.UPDATE_UI_LIST.GET_MARK)
        {
            m_clientSDKRef.getGeneralInfo((ushort)MGCTypeDef.BUILDING_INFO_TYPE.BUILDING_INFO_PORTRAIT_LIST, "");
            m_clientSDKRef.getGeneralInfo((ushort)MGCTypeDef.BUILDING_INFO_TYPE.BUILDING_INFO_MARK_LIST, "");
        }
        else if (updateType == (int)MGCTypeDef.UPDATE_UI_LIST.KICK)
        {
            PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
            pageStyle.message = m_wordsTableRef.getUIText("0000019");
            pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
            SetPopUpPage(pageStyle);

            Invoke("KickedBackToLogin", 2.0f);
        }
        else if (updateType == (int)MGCTypeDef.UPDATE_UI_LIST.SERVER_CLOSE )
        {
            PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
            pageStyle.message = m_wordsTableRef.getUIText("0203145");
            pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
            SetPopUpPage(pageStyle);

            Invoke("KickedBackToLogin", 2.0f);
        }		
    }

    private void KickedBackToLogin()
    {
		if (m_switchSceneDelegate != null)
		{
			UnregisterAllHandler();
			m_switchSceneDelegate("Login");
		}
    }


    private int villagerPathCount = -1;
    public void TestCreateVillager(GameObject caller)
    {
        // test villager
        if (m_villagerMgr.GetVillager("Test") == null)
        {
            for (int i = 0; i < 10; i++)
                m_villagerMgr.CreateVillager("Test" + i);
            //m_villagerMgr.CreateVillager("Test");
            string forceChar = "";
            switch (playerForce)
            {
                case 1:
                    forceChar = "W";
                    break;
                case 2:
                    forceChar = "E";
                    break;
                case 3:
                    forceChar = "ME";
                    break;         
                default:
                    return;
            }

            for (villagerPathCount = 1; ; villagerPathCount++)
                if (!m_villagerMgr.CreatePath("VillagerPath_" + forceChar + "_" + villagerPathCount))
                    break;
            m_villagerMgr.CreateBuildinColliders("VillagerPath_" + forceChar + "_CollisionArea");
            //VillagerPath_E_CollisionArea
        }
        else
        {
            m_villagerMgr.SetCameraDirection(EVilliagerDirection.E_VILLIAGER_DIRECTION_DR);
        }
    }

    int currentVillagerToMove = -1;
    public void TestMoveVillager(GameObject caller)
    {
        // test villager   
        currentVillagerToMove = 0;
        InvokeRepeating("MoveVillagerSeperately", 0, 1.0f);
        //m_villagerMgr.GetVillager("Test" + currentVillagerToMove).Move(m_villagerMgr.GetVillagerPath("VillagerPath_E_1"), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
        /*switch (playerForce)
        {
            case 1:
                m_villagerMgr.GetVillager("Test").Move(m_villagerMgr.GetVillagerPath("VillagerPath_W_1"), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                break;
            case 2:
                m_villagerMgr.GetVillager("Test").Move(m_villagerMgr.GetVillagerPath("VillagerPath_E_1"), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                break;
            case 3:
                m_villagerMgr.GetVillager("Test").Move(m_villagerMgr.GetVillagerPath("VillagerPath_ME_1"), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                break;
        } */       
    }

    public void MoveVillagerSeperately()
    {
        if (currentVillagerToMove < 10)
        {
            int pathIdx = UnityEngine.Random.Range(1, villagerPathCount);
            switch (playerForce)
            {
                case 1:
                    m_villagerMgr.GetVillager("Test" + currentVillagerToMove).Move(m_villagerMgr.GetVillagerPath("VillagerPath_W_" + pathIdx), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                    break;
                case 2:
                    m_villagerMgr.GetVillager("Test" + currentVillagerToMove).Move(m_villagerMgr.GetVillagerPath("VillagerPath_E_" + pathIdx), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                    break;
                case 3:
                    m_villagerMgr.GetVillager("Test" + currentVillagerToMove).Move(m_villagerMgr.GetVillagerPath("VillagerPath_ME_" + pathIdx), iTween.Hash("time", 5.0f, "easetype", "linear", "movetopath", false));
                    break;
            }  
            currentVillagerToMove++;
        }
        else
        {
            CancelInvoke("MoveVillagerSeperately");
            currentVillagerToMove = -1;
        }
    }

    public void TestIconTextAssign()
    {
        nameText.transform.parent.FindChild("TestText").gameObject.GetComponent<SpriteText>().Text
            = "Get 100 \u3400 \u3401 \u3402 \u3403 \u3404 \u3405 \u3406 \u3407 \u3408 \u3409 \u340a \u340b \u340c ";
    }


    WWW _m_www;
    GameObject _m_newUIObj;

    PackedSprite targetSprt;

    List<string> _m_imageFileList = new List<string>();
    List<string> _m_imageFileAssetPathList = new List<string>();
    List<Texture2D> _m_imageTextureList = new List<Texture2D>();
    int _m_currentTextureCount = 0;
    int LAYER_GUI = 8;
    int LAYER_CashFlow = 9;
	int LAYER_IGNORE_RAYCAST = 2;
	//int LAYER_GUI_GOLDMARKET = 10;
	int LAYER_GUI_BG = 29;

    int pressCounter = 0;
    public void TestCreateUIObject()
    {
        // Create a new GameObject in Unity Hierarchy
        if (pressCounter == 0)
        {
            _m_newUIObj = new GameObject("newUIObj1");
            _m_newUIObj.transform.parent = systemButtonPanel.transform;
            TestCreatePackedSprite("file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0052.png");
        }
        else if (pressCounter == 1)
        {
            _m_newUIObj = new GameObject("newUIObj2");
            _m_newUIObj.transform.parent = systemButtonPanel.transform;
            TestCreatePackedSprite("file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0053.png");
        }
        else if (pressCounter == 2)
        {
            _m_newUIObj = new GameObject("newUIObj3");
            _m_newUIObj.transform.parent = systemButtonPanel.transform;
            TestCreatePackedSprite("file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0054.png");
        }
        else
        {
            _m_newUIObj = new GameObject("newUIObj4");
            _m_newUIObj.transform.parent = systemButtonPanel.transform;
            TestCreateUIButton2();
        }
        pressCounter++;
    }

    public void TestCreateUIButton()
    {
        _m_imageFileList.Clear();
        _m_imageFileAssetPathList.Clear();
        _m_imageTextureList.Clear();
        _m_currentTextureCount = 0;

        // Add image file names for different states of a button
        _m_imageFileList.Add( "file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0052.png");
        _m_imageFileList.Add( "file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0053.png");
        _m_imageFileList.Add( "file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0054.png");
        _m_imageFileList.Add( "file://" + Application.dataPath + "/ArtAssets/Images/Button/BB0054.png");
        _m_imageFileAssetPathList.Add( "/ArtAssets/Images/Button/BB0052.png");
        _m_imageFileAssetPathList.Add( "/ArtAssets/Images/Button/BB0053.png");
        _m_imageFileAssetPathList.Add( "/ArtAssets/Images/Button/BB0054.png");
        _m_imageFileAssetPathList.Add( "/ArtAssets/Images/Button/BB0054.png");
        _m_www = new WWW(_m_imageFileList[0]);

        InvokeRepeating("CheckImageListLoading", 0, 1.0f);
    }

    public void TestCreateUIButton2()
    {
        UIButton btn = _m_newUIObj.GetComponent<UIButton>();
        if (btn == null)
        {
            btn = _m_newUIObj.AddComponent<UIButton>();
        
        }
        
        for (int i = 0; i < 3; i++)
        {
            btn.States[i].frameGUIDs = new string[1];
            btn.States[i].frameGUIDs[0] = _m_newUIObj.transform.parent.FindChild("newUIObj" + (i+1).ToString()).gameObject.GetComponent<PackedSprite>().staticTexGUID;
        }

        btn.States[3].frameGUIDs = new string[1];
        btn.States[3].frameGUIDs[0] = _m_newUIObj.transform.parent.FindChild("newUIObj1").gameObject.GetComponent<PackedSprite>().staticTexGUID;
    }

    public void CheckImageListLoading()
    {
        if (!_m_www.isDone)
            return;

        CancelInvoke("CheckImageListLoading");

        UIButton btn = _m_newUIObj.GetComponent<UIButton>();
        if (btn == null)
        {
            btn = _m_newUIObj.AddComponent<UIButton>();            
        }
                       
        btn.renderer.material.shader = Shader.Find("Sprite/Vertex Colored, Fast");
        Texture2D tex = new Texture2D(100, 100);
        _m_imageTextureList.Add(tex);
        _m_www.LoadImageIntoTexture((Texture2D)_m_imageTextureList[_m_currentTextureCount]);

        if (_m_currentTextureCount == 0)
        {
            btn.renderer.material.mainTexture = _m_imageTextureList[_m_currentTextureCount];     
            
            ++_m_currentTextureCount;
            _m_www = new WWW(_m_imageFileList[_m_currentTextureCount]);
            InvokeRepeating("CheckImageListLoading", 0, 1.0f);
        }
        else
        {
            //btn.renderer.material.SetTexture("tex" + _m_currentTextureCount, _m_imageTextureList[_m_currentTextureCount]);            

            if (++_m_currentTextureCount < _m_imageFileList.Count)
            {
                //Debug.Log("texture incoming");
                _m_www = new WWW(_m_imageFileList[_m_currentTextureCount]);
                InvokeRepeating("CheckImageListLoading", 0, 1.0f);
            }
            else
            {
                //Debug.Log("End recv");
                for(int i=0; i< _m_imageFileList.Count; ++i)
                {
                    btn.States[i].framePaths = new string[1];
                    btn.States[i].framePaths[0] = _m_imageFileAssetPathList[i];
                    //btn.States[i].frameGUIDs = new string[1];
                    //btn.States[i].frameGUIDs[0] = _m_imageTextureList[i].GetNativeTextureID().ToString();
                }

                CSpriteFrame[][] frames = new CSpriteFrame[3][];
                for(int i=0; i<3; ++i)
                  frames[i] = new CSpriteFrame[1];

                for(int i=0; i<frames.Length; ++i)
                {
                    frames[i][0] = new CSpriteFrame();
                    frames[i][0].uvs = new Rect(0, 0, 1.0f, 1.0f);                   
                    SPRITE_FRAME[] sfArray = new SPRITE_FRAME[1];
                    sfArray[0] = frames[i][0].ToStruct();
                    btn.animations[i].SetAnim(sfArray);
                }
                btn.CalcSize();
            }
        }
        
    }

    public void TestCreatePackedSprite(string targetFile)
    {
        // Start to load image file from target file path
        //string targetFile = "file://" + Application.dataPath + "/ArtAssets/Images/PackedSprites/100x100/H011000102.png";
        _m_www = new WWW(targetFile);
        // Invoke coroutine to check loading status
        InvokeRepeating("CheckSingleImageLoading", 0, 1.0f);  
    }

    public void CheckSingleImageLoading()
    {
        if (!_m_www.isDone)
            return;

        CancelInvoke("CheckSingleImageLoading");

        // Add EZGUI UI component
        //PackedSprite sprt = _m_newUIObj.AddComponent<PackedSprite>();        
        targetSprt = _m_newUIObj.AddComponent<PackedSprite>();

        // Set render camera
        targetSprt.RenderCamera = UIManager.instance.uiCameras[0].camera;

        // Set position
        targetSprt.transform.localPosition = Vector3.zero;

        // Create new texture 
        _m_newUIObj.renderer.material.mainTexture = new Texture2D(100, 100);

        // Select shader for the 2D sprite
        _m_newUIObj.renderer.material.shader = Shader.Find("Sprite/Vertex Colored, Fast");

        // Set layer to gui layer
        _m_newUIObj.layer = LAYER_GUI;

        // Set image to ui
        _m_www.LoadImageIntoTexture((Texture2D)_m_newUIObj.renderer.material.mainTexture);

        targetSprt.SetUVs(new Rect(0, 0, 1.0f, 1.0f));
        targetSprt.SetTexture((Texture2D)_m_newUIObj.renderer.material.mainTexture);
        targetSprt.pixelPerfect = true;
        targetSprt.CalcSize();
    }

	public void BuyGoldInputDelegate(ref POINTER_INFO ptr)
	{
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
		{
			popUpPanelOKPressed();
			BuyGold();
		}
	}

	public void BuyGold(GameObject caller)
	{		
		BuyGold();
	}

    
    /// <summary>
    /// Instantiate payment UI and request product ID 
    /// </summary>
    public void BuyGold()
    {		
		MGDebug.Log("Buy gold");

#if UNITY_WEBPLAYER
        if (MGGameConfig.GetValue<bool>("TW_Web_WebCashFlow"))
        {
            MGGameGod o = (MGGameGod) GameObject.FindObjectOfType(typeof(MGGameGod));
            MGExternalAccountManager account = o.GetComponent<MGExternalAccountManager>();
            Application.ExternalCall(MGCTerritoryTypeDef.Constants.BUY_GOD_EXTERNAL_CALL_NAME, account.UniqueID, "0");		
            return;
        }
         else
        { ShowPaymentIsNotOpen(); }
#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (MGGameConfig.GetValue<bool>("TW_Web_WebCashFlow"))
        {
            //make payment unopened in Iphone,Android,Webplayer platform
            Application.OpenURL(MGCTerritoryTypeDef.Constants.BUY_GOLD_URL);
            return;
        }
#endif

#if UNITY_IPHONE
        //Pay with IOS 
        if (MGGameConfig.GetValue<bool>("TW_IOS_StoreKit"))
        {
            //Check if the appale device can make payment
            if (StoreKitBinding.canMakePayments() == false)
            {
                Debug.Log("Can't Make Payment,Please check !!!");
                return;
            }

            //ask server sent Product Infomation again
            m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
            MGCClientSDK.instance.SyncProductCategory();
        }
        else if(MGGameConfig.GetValue<bool>("CN_IOS_CoolGw"))
        {
            if (MGTBuyCurrency == null)
            { MGTBuyCurrency = InitBuyCurrencyPanel(); }
			
			MGTBuyCurrency.SetPayID(MGTerritoryOperator_BuyCurrency.payment_id.ChinaUnionPay);
            MGTBuyCurrency.RequestCNProductInfo(MGTerritoryOperator_BuyCurrency.payment_id.ChinaUnionPay, MGGameConfig.GetValue<int>("DEFINE_REGOIN_ID"));
        }
        else
        { ShowPaymentIsNotOpen();}
#endif

#if UNITY_ANDROID
#if GOOGLEPLAY
        // Pay With GooglePlay
        if (MGGameConfig.GetValue<bool>("TW_Android_InAppBilling"))
        { 
           //register event of In App billing support check
           GoogleIABManager.billingSupportedEvent -= OnbillingSupportedEvent;
           GoogleIABManager.billingSupportedEvent += OnbillingSupportedEvent;

           GoogleIABManager.billingNotSupportedEvent -= OnbillingNotSupportedEvent;
           GoogleIABManager.billingNotSupportedEvent += OnbillingNotSupportedEvent;

           m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
           GoogleIAB.init(publickey);
        }
#endif
        //Pay With CN_Android_CoolGw
         if (MGGameConfig.GetValue<bool>("CN_Android_CoolGw"))
        {
            if (MGTBuyCurrency == null)
            { MGTBuyCurrency = InitBuyCurrencyPanel(); }
			
			MGTBuyCurrency.SetPayID(MGTerritoryOperator_BuyCurrency.payment_id.ChinaUnionPay);
            MGTBuyCurrency.RequestCNProductInfo(MGTerritoryOperator_BuyCurrency.payment_id.ChinaUnionPay, MGGameConfig.GetValue<int>("DEFINE_REGOIN_ID"));
        }
        //Pay With UC PlatForm
        else if (MGGameConfig.GetValue<int>("CHINAL_PLATFORM") == (int)MGExternalAccountManager.CHINAL_THIRD_PLATFORM.UC)
        {
            if (MGTBuyCurrency == null)
            { MGTBuyCurrency = InitBuyCurrencyPanel(); }
			
			MGTBuyCurrency.SetPayID(MGTerritoryOperator_BuyCurrency.payment_id.UC);
            MGTBuyCurrency.RequestCNProductInfo(MGTerritoryOperator_BuyCurrency.payment_id.UC, MGGameConfig.GetValue<int>("DEFINE_REGOIN_ID"));
        }
		//Pay With Downjoy PlatForm
		else if(MGGameConfig.GetValue<int>("CHINAL_PLATFORM") == (int)MGExternalAccountManager.CHINAL_THIRD_PLATFORM.DOWNJOY)
		{
			 if (MGTBuyCurrency == null)
            { MGTBuyCurrency = InitBuyCurrencyPanel(); }
			
			MGTBuyCurrency.SetPayID(MGTerritoryOperator_BuyCurrency.payment_id.Downjoy);
            MGTBuyCurrency.RequestCNProductInfo(MGTerritoryOperator_BuyCurrency.payment_id.Downjoy, MGGameConfig.GetValue<int>("DEFINE_REGOIN_ID"));
		}
        else
        { ShowPaymentIsNotOpen(); }
#endif
        MGTBuyCurrency.BringinBtnPanel();
    }

  
    public void BringInBuyGoldPanel(MGTerritoryOperator_BuyCurrency.CashFlowType PayType)
    {
        moneyPanel.Dismiss();
        experiencePanel.Dismiss();
        commonMenuPanelSet.transform.FindChild("MenuButtonPanel").gameObject.GetComponent<UIPanel>().BringIn();

        ShowMaskCube(true);

        SetObjectToCashFlowLayer();
        SetObjectToCashFlowPos();

        BuyCurrencyPanelObj.GetComponent<UIPanel>().BringIn();
        BuyCurrencyPanelObj.transform.parent.parent.FindChild("BuycurrencyRightUpperPlacement").FindChild("LVPanel").GetComponent<UIPanel>().BringIn();
        CancelInvoke("RotateBuildingInfoBG");
        InvokeRepeating("RotateBuildingInfoBG", 0, 0.05f);

        if (MGTBuyCurrency != null)
        { MGTBuyCurrency.SwitchMenuByType(PayType); }
        else
        { Debug.LogError("MGTBuyCurrency is Null !!"); }
    }

    public void DisMissBuyGoldPanel()
    {
        if (lastMenuPanel == null)
        {
            commonMenuPanelSet.transform.FindChild("MenuButtonPanel").gameObject.GetComponent<UIPanel>().Dismiss();
            ShowMaskCube(false);
            moneyPanel.BringIn();
            experiencePanel.BringIn();
            CancelInvoke("RotateBuildingInfoBG");
        }
       
        SetObjectToGUILayer();
        SetObjectToGUIPos();
        
        //BuyCurrencyPanelObj.transform.FindChild("BuyGoldMenuPanelSet").transform.FindChild("LVPanel").GetComponent<UIPanel>().Dismiss();
        BuyCurrencyPanelObj.transform.parent.parent.FindChild("BuycurrencyRightUpperPlacement").FindChild("LVPanel").GetComponent<UIPanel>().Dismiss();
    }

    /// <summary>
    /// Initial BuyCurrencyPanel
    /// </summary>
	/// 	
    private MGTerritoryOperator_BuyCurrency InitBuyCurrencyPanel()
    {
        if (BuyCurrencyPanelObj == null)
        {
            GameObject Prefab = (GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, "BaseResource/GUI/Territory/BuycurrencyPanel");
            BuyCurrencyPanelObj = GameObject.Instantiate(Prefab) as GameObject;
            MGTerritoryOperator_BuyCurrency buycurrency = BuyCurrencyPanelObj.GetComponent<MGTerritoryOperator_BuyCurrency>();

            buycurrency.popUpMaskPanel = popUpMaskPanel;
            buycurrency.MGTOperator = this.gameObject.GetComponent<MGTerritoryOperator>();
            buycurrency.MGWTable = m_wordsTableRef;
            buycurrency.Barrier = m_barrier;
            buycurrency.WordsTable = m_wordsTableRef;
			
			buycurrency.SetSceneType(0);
            buycurrency.Init();
            MGUITextAdjuster.DoIt(BuyCurrencyPanelObj);
            GameObject TerritoryOperator = GameObject.Find("TerritoryUI");
            GameObject BuycurrencyPlacement = TerritoryOperator.transform.FindChild("BuyCurrencyUISet").FindChild("BuycurrencyPlacement").gameObject;
            BuyCurrencyPanelObj.transform.parent = BuycurrencyPlacement.transform;
            BuyCurrencyPanelObj.transform.FindChild("CloseBtn").transform.parent = BuycurrencyPlacement.transform.parent.FindChild("BuycurrencyCloseBtnPlacement");
            

            //Initial player's name and gold number 
            buycurrency.SetPlayerName(m_commonDataRef.avatarEventArgs.Avatar.Name);
            buycurrency.SetPlayerGold(m_commonDataRef.playerDetailedInfo.ntd.ToString());

            return buycurrency;
        }
        else
        { return BuyCurrencyPanelObj.GetComponent<MGTerritoryOperator_BuyCurrency>();}
    }

    /// <summary>
    /// Receive event of product ID from server 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    private void OnSyncProductCategory(object sender, MGCBaseEventArgs ea)
    {
        MGCSyncProductCategoryEventArgs args = (MGCSyncProductCategoryEventArgs)ea;
        ProductInfoList.Clear();

        if (MGTBuyCurrency == null)
        { MGTBuyCurrency = InitBuyCurrencyPanel(); }

        // except price($0.99,$1.99,...from apple server),product ID,premium,gold exchange are from hou server 
        foreach (KeyValuePair<string, Dictionary<string, object>> pair in args.Data)
        {
            MGTerritoryOperator_BuyCurrency.ProductInfo productinfo = new MGTerritoryOperator_BuyCurrency.ProductInfo();
            productinfo.Product_Identifier = pair.Key;
			productinfo.category = (MGCTypeDef.PRODUCT_CATEGORY)((short)(pair.Value[MGCProperties.ATTR_CATEGORY]));
            productinfo.PremiumText = pair.Value[MGCProperties.ATTR_DISCOUNT].ToString();
            productinfo.GoldNumber = pair.Value[MGCProperties.ATTR_NUMBER].ToString();

#if UNITY_ANDROID
            // Fill in the Product Price,google
            productinfo.Price = pair.Value[MGCProperties.ATTR_PRICE].ToString();
#endif
            ProductInfoList.Add(productinfo);
        }

        if (ProductInfoList.Count == 0)
            return;

#if UNITY_IPHONE
        string[] productid = new string[ProductInfoList.Count];
        for (int i = 0; i < productid.Length; ++i)
        { productid[i] = ProductInfoList[i].Product_Identifier; }

        //register event of receive product Id from apple successfully
        StoreKitManager.productListReceivedEvent -= OnproductListReceivedEvent;
        StoreKitManager.productListReceivedEvent += OnproductListReceivedEvent;

        //register event of receive product Id fail from apple 
        StoreKitManager.productListRequestFailedEvent -= OnproductListRequestFailedEvent;
        StoreKitManager.productListRequestFailedEvent += OnproductListRequestFailedEvent;

        //send request product information to apple server
        StoreKitBinding.requestProductData(productid);
#endif

#if UNITY_ANDROID
        string[] productid = new string[ProductInfoList.Count];
        for (int i = 0; i < productid.Length; ++i)
        { productid[i] = ProductInfoList[i].Product_Identifier; }

        //send request product information to google play server
       
#if GOOGLEPLAY 
        GoogleIABManager.queryInventorySucceededEvent -= OnqueryInventorySucceededEvent;
        GoogleIABManager.queryInventorySucceededEvent += OnqueryInventorySucceededEvent;
        GoogleIABManager.queryInventoryFailedEvent -= OnqueryInventoryFailedEvent;
        GoogleIABManager.queryInventoryFailedEvent += OnqueryInventoryFailedEvent;
        GoogleIAB.queryInventory(productid);
#endif
        //MGTerritoryOperator_BuyCurrency buycurrency = BuyCurrencyPanel.GetComponent<MGTerritoryOperator_BuyCurrency>();
        //buycurrency.SetProductInfo(ProductInfoList);
        //popUpMaskPanel.BringIn();
        //BuyCurrencyPanel.SetActiveRecursively(true);

        //m_barrier.Hide();
#endif
    }

#if UNITY_IPHONE
    /// <summary>
    /// receive event of receiving product information successfully from apple
    /// </summary>
    private void OnproductListReceivedEvent(List<StoreKitProduct> storekitproductinfo)
    {
        StoreKitManager.productListReceivedEvent -= OnproductListReceivedEvent;
        StoreKitManager.productListRequestFailedEvent -= OnproductListRequestFailedEvent;
         
        if (storekitproductinfo.Count == 0)
            return;

        for (int i = 0; i < storekitproductinfo.Count; ++i)
        {
           for (int j = 0; j < ProductInfoList.Count; ++j)
            {
                if (storekitproductinfo[i].productIdentifier == ProductInfoList[j].Product_Identifier)
                { ProductInfoList[j].Price = storekitproductinfo[i].formattedPrice; }
            }
        }

        if (MGTBuyCurrency != null)
        {
            MGTBuyCurrency.SetProductInfo(ProductInfoList);
            BringInBuyGoldPanel(MGTerritoryOperator_BuyCurrency.CashFlowType.Apple);
        }
        else
        { MGDebug.LogError("MGTBuyCurrency is null!!"); }
        //MGTerritoryOperator_BuyCurrency buycurrency = BuyCurrencyPanel.GetComponent<MGTerritoryOperator_BuyCurrency>();
        //buycurrency.SetProductInfo(ProductInfoList);
        //BringInBuyGoldPanel(MGTerritoryOperator_BuyCurrency.CashFlowType.Apple);
        //popUpMaskPanel.BringIn();
        //BuyCurrencyPanel.SetActiveRecursively(true);

        m_barrier.Hide();
       }
#endif

#if UNITY_ANDROID
    /// <summary>
    /// receive event of receiving product information successfully from google play
    /// </summary>
    /// 
#if GOOGLEPLAY
    private void OnqueryInventorySucceededEvent(List<GooglePurchase> GP,List<GoogleSkuInfo> GS)
    {
        if (GS.Count == 0)
            return;

        for (int i = 0; i < GS.Count; ++i)
        {
            for (int j = 0; j < ProductInfoList.Count; ++j)
            {
                if (GS[i].productId == ProductInfoList[j].Product_Identifier)
                {
                    Debug.Log(" GS[" + i + "].price = " + GS[i].price);
                    ProductInfoList[j].Price = GS[i].price;
                }
            }
        }

        if (MGTBuyCurrency != null)
        {
            MGTBuyCurrency.SetProductInfo(ProductInfoList);
            BringInBuyGoldPanel(MGTerritoryOperator_BuyCurrency.CashFlowType.Google);
            //popUpMaskPanel.BringIn();
            //BuyCurrencyPanel.SetActiveRecursively(true);
            m_barrier.Hide();
        }
        else
        { MGDebug.LogError("MGTBuyCurrency is null !!");}
    }
#endif
    private void OnqueryInventoryFailedEvent(string msg)
    {
        MGDebug.LogError("OnqueryInventoryFailedEvent msg = " + msg);
        m_barrier.Hide();
    }
#endif

#if UNITY_IPHONE
    /// <summary>
    /// receive event of receiving product information fail
    /// </summary>
    /// <param name="errormsg"></param>
    private void OnproductListRequestFailedEvent(string errormsg)
    {
        StoreKitManager.productListReceivedEvent -= OnproductListReceivedEvent;
        StoreKitManager.productListRequestFailedEvent -= OnproductListRequestFailedEvent;

        MGDebug.Log("OnproductListRequestFailedEvent message = " + errormsg);
    }
#endif

    /// <summary>
    /// Receive event of Login Reward,Initial the Login Reward UI and  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    /// 
    
	private MGCGetLoginRewardEventArgs m_baseEventArgs = null;
    private void OnLoginReward(object sender, MGCBaseEventArgs ea)
    {
		MGGameGod.EventUnit EU = new MGGameGod.EventUnit();
		EU.type = MGGameGod.Event_type.server;
		EU.Args = ea;
		
		MGGameGod.Instance.EventQueue.Enqueue(EU);
		
		if(MGGameGod.Instance.EventQueue.Count != 0 && MGGameGod.Instance.IsEventUIShowing == false)
		{
			HandleQueueEvent((MGGameGod.EventUnit)MGGameGod.Instance.EventQueue.Dequeue());
		}
    }
	
	private int m_eventType = 0;
	public void HandleQueueEvent(MGGameGod.EventUnit EU)
	{
		//Another UI is Showing,return it
		if(MGGameGod.Instance.IsEventUIShowing == true)
		{
			return;
		}
		//Set the UI Showing flag to be true
		MGGameGod.Instance.IsEventUIShowing = true;
		
		if(EU.type == MGGameGod.Event_type.client)
		{
			int i = UnityEngine.Random.Range(1,10);
			if(i <= EventProperty)
			{
				Invoke("PlayEventAni",2.0f);
				m_eventType = 1;
			}
			else
			{
				EventMovePanel.Dismiss();
				StopEventAni();
			}
		}
		else
		{
			if(EU.Args == null)
			{
				if(m_eventType != 1)
				{
					Invoke("PlayOtherPlayerEventAni",2.0f);
					m_eventType = 2;
				}
				return;
			}
			switch(EU.Args.EventArgsType)
			{
		   	 	//Login Reward Event
				case MGCNetMsg.EOpType.GetLoginReward:
					MGCGetLoginRewardEventArgs args = (MGCGetLoginRewardEventArgs)EU.Args;
			    	m_baseEventArgs = args;
				
					// Can't Receive Login Reward,return and do nothing
		        	if ((MGCErrorCode.LoginRewardErrorCode)args.errorCode != MGCErrorCode.LoginRewardErrorCode.ERR_SUCCESS)
		        	{
						MGDebug.LogWarning((MGCErrorCode.LoginRewardErrorCode)args.errorCode);
						MGGameGod.Instance.IsEventUIShowing = false;
		           		return;
		        	}
					
					//Do nothing if Server send empty reward list
		        	if (args.ItemSize == 0)
		        	{
		            	MGDebug.LogWarning("Login Reward list data from server is empty , please check !!!");
						MGGameGod.Instance.IsEventUIShowing = false;
		            	return;
		        	}
					//Init LoginReward after downloader download complete.
					InvokeRepeating("InitLoginReward", 0.1f, 0.1f);
					break;
				
				//Building Level Up Event
				case MGCNetMsg.EOpType.Building_OP:
					LoadSelectedBuildingModel();
				break;
			}
		}
		if(m_eventType == 1)
		{
			this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").GetComponent<UIButton>().methodToInvoke = "PPVEBtnPressed";
		}
		else if(m_eventType == 2)
		{
			this.transform.parent.FindChild("OtherPlayerEventBtnPlacement").FindChild("OtherPlayerEventBtn").GetComponent<UIButton>().methodToInvoke = "OpenOtherPlayerEvent";
		}
	}
	
	private void InitLoginReward()
	{
		if(IsResourcesDownloadComplete() && MGSceneMasterTerritory.Instance.IsResourcesDownloadComplete())
		{		
			CancelInvoke("InitLoginReward");			
	        popUpMaskPanel.BringIn();
	        //Instantiate Login Reward UI & Init the Component
	        if (LoginRewardPanelObj == null)
	        {
	            GameObject LoginRewardPrefab = (GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, "BaseResource/GUI/Territory/LoginRewardPanel");
	            LoginRewardPanelObj = GameObject.Instantiate(LoginRewardPrefab) as GameObject;
	            //Adjust Text Font Size 
	            MGUITextAdjuster.DoIt(LoginRewardPanelObj);
	            //
	            MGLoginReward LoginrewardComponent = LoginRewardPanelObj.GetComponent<MGLoginReward>();
	            LoginrewardComponent.MGTOperator = this.gameObject.GetComponent<MGTerritoryOperator>();
	            LoginrewardComponent.barrier = m_barrier;
	            LoginrewardComponent.MaskPanel = popUpMaskPanel;
	            LoginrewardComponent.WordsTable = m_wordsTableRef;
	            //Init the Component
	            LoginrewardComponent.Init(m_baseEventArgs);
	
				RemoveMainCameraRaycast();
	            //Set Login Reward Data
	            List<MGLoginReward.LoginRewardData> rewardlist = new List<MGLoginReward.LoginRewardData>();
	            for (int i = 0; i < m_baseEventArgs.ItemSize; ++i)
	            {
	                MGLoginReward.LoginRewardData rewarddata = new MGLoginReward.LoginRewardData();
	                rewarddata.Item_ID = m_baseEventArgs.ItemIDs[i];
	                rewarddata.Item_Number = m_baseEventArgs.ItemVaules[i];
	                rewardlist.Add(rewarddata);
	            }
	            LoginrewardComponent.SetRewardContent(rewardlist, m_baseEventArgs.LoginCount);
	
	            //Get Secretary Event Picture and Reassign Picture
	            //GameObject SecretaryPrefab = (GameObject)MGDownloader.instance.getObject(BundleType.LOCALIZED_EVENT_BUNDLE, "Secretary");
	            //PackedSprite Secretarypic = SecretaryPrefab.GetComponent<PackedSprite>();
	            Transform speakTranform = LoginRewardPanelObj.transform.FindChild("LoginReward_Narrator");
				
				//Get Secretary Event Picture and Reassign Picture
				if( speakTranform != null )
				{
			        UnityEngine.Object SecretaryPrefab = MGDownloader.instance.getObject(BundleType.LOCALIZED_EVENT_BUNDLE, "Secretary");
					GameObject secretaryObj = (GameObject)GameObject.Instantiate( SecretaryPrefab );
					if( secretaryObj != null )
					{
						secretaryObj.transform.parent = speakTranform;
						secretaryObj.transform.localPosition = new Vector3(34.86719f, 0.05369568f, 0f);//Vector3.zero;
						secretaryObj.transform.eulerAngles = new Vector3( 0f, 180f, 0f );
						secretaryObj.transform.localScale = Vector3.one * 7f;
						secretaryObj.name = secretaryObj.name.Replace( "(Clone)", "" );
						m_speakerControl = secretaryObj.GetComponent<MGRealTimeAnimationPlayer>();
						if( m_speakerControl != null )
						{
							m_speakerControl.PlayAnimation( MGSystemAnimationString.Stand );
						}
					}
				}
	
	            LoginRewardPanelObj.transform.parent = GameObject.Find("TerritoryUI").transform.FindChild("EventUISet").FindChild("LoginRewardPlacement").transform;
//				LoginRewardPanelObj.transform.localPosition = new Vector3(14.59277f, 1.012939f, 1f);
	        }
	
	        // When open Loginreward UI hide chat panel in back of mask panel
	        MGGossipMgr.Instance.BringInChatPanel();	
			
			m_baseEventArgs = null;
		}
	}

    /// <summary>
    /// Receive event of if In App Billing supported from google play 
    /// </summary>
    /// <param name="isSupported"></param>
    private void OnbillingSupportedEvent()
    {
#if UNITY_ANDROID
#if GOOGLEPLAY
        GoogleIABManager.billingSupportedEvent -= OnbillingSupportedEvent;
        GoogleIABManager.billingNotSupportedEvent -= OnbillingNotSupportedEvent;
#endif
#endif

        //MGTBuyCurrency = InitBuyCurrencyPanel();
        //BuyCurrencyPanelObj.SetActiveRecursively(false);

        //cask server sent Product Infomation again
        MGCClientSDK.instance.SyncProductCategory();
    }

    private void OnbillingNotSupportedEvent(string msg)
    {
#if UNITY_ANDROID
#if GOOGLEPLAY
        GoogleIABManager.billingSupportedEvent -= OnbillingSupportedEvent;
        GoogleIABManager.billingNotSupportedEvent -= OnbillingNotSupportedEvent;
#endif
#endif
        MGDebug.LogError("In app billing is not support msg = " + msg);
    }

    public void ReDataBroadcastShow()
    { DataBroadCastShow(); }

    private void OnCashFlow_Purchase(object sender, MGCBaseEventArgs ea)
    {
        MGCCashFlow_PurchaseEventArgs args = (MGCCashFlow_PurchaseEventArgs)ea;
 
        if ((MGCErrorCode.CashFlowErrorCode)args.ErrorCode == MGCErrorCode.CashFlowErrorCode.ERR_CASHFLOW_SUCCESS)
            MGDebug.Log(args.ProductId + " Purchase Succeed !!! Many Thanks :)");
        else
            MGDebug.Log(args.ProductId + " Purchase Fail !!! Error Code = " + (MGCErrorCode.CashFlowErrorCode)args.ErrorCode + " :(");
    }

    /// <summary>
    /// Input Hero ID , Equip ID or Stage ID, return the Hero Name , Equip Name or Stage Name 
    /// if the ID is not Hero ID , Equip ID or  Stage Name 
    /// </summary>
    /// <param name="ID">Hero,Equip or Stage ID</param>
    /// <returns>Hero,Equip or Stage Name</returns>
    public string ParseIDToString(string ID)
    {
        //The length of Hero ID or Item ID = 9
        if (ID.Length != 11)
        { return ID; }

        string IDKind = ID.Substring(0, 2);

        //ID is Equip ID
        if (IDKind == "95")
        {
            string quality = ID.Substring(7, 1);
            Color color = Color.white;

            switch (quality)
            {
                case "0":
                    color = Color.white;
                    break;
                case "1":
                    color = Color.green;
                    break;
                case "2":
                    color = Color.blue;
                    break;
                case "3":
                    color = Color.magenta;
                    break;
                case "4":
                    color = Color.yellow;
                    break;
            }

            return color + m_wordsTableRef.getEquipmentText(ID + "_name"); 
        }
        //ID is Hero ID
        else if (IDKind == "25")
        { return m_wordsTableRef.getHeroText(ID + "_name"); }
        //ID is Stage ID
        else if (IDKind == "11")
        {
            string temp_stage_name = m_wordsTableRef.getWorldText(ID + "_stage_name");
            int idx = temp_stage_name.IndexOf("¡Ð");

            if (idx >= 0)
            {
                string stage_name = temp_stage_name.Substring(0, idx);
                return stage_name;
            }
            else
            { return temp_stage_name; }
        }
        else
        { return ID; }
    }

    public string ParseArenaIDToString(string ID)
    {
        if(ID.Length != 12)
        { return ID; }

        string ArenaID = ID.Substring(0, 3);
        MGXMLObject arenaXMLObject = new MGXMLObject((TextAsset)MGDownloader.instance.getObject(BundleType.LOCALIZED_GAMEDATA_BUNDLE, "arena_data"));

        if (ArenaID == "231")
        { return MGArenaDataFetcher.FetchArenaXMLData(ID, arenaXMLObject).m_arenaName; }
        else
        { return ID; }
    }

    /// <summary>
    /// Get Friend List
    /// </summary>
    public void GetFriendList()
    {
        short[] requestType = new short[1];
        requestType[0] = (byte)MGCTypeDef.AVATAR_RELATION_TYPE.AVATAR_RELATION_FRIEND;
        m_clientSDKRef.GetRelationRequestList(requestType, (byte)MGCTypeDef.AVATAR_RELATION_LIST_TYPE.AVATAR_RELATION_LIST_RELATION, true);
    }

    public void OnGetFriendList(object sender, MGCBaseEventArgs ea)
    {
        MGCGetRelationRequestListEventArgs area = (MGCGetRelationRequestListEventArgs)ea;

        if (area.getListType() == (byte)MGCTypeDef.AVATAR_RELATION_LIST_TYPE.AVATAR_RELATION_LIST_REQUEST)
            return;

        List<string> FriendOnLineList = new List<string>();

        List<MGCStructDef.AvatarIndicator> avatarIndicatorArray = new List<MGCStructDef.AvatarIndicator>();
		
		RelationRequestData[] relationData = area.getData((short)MGCTypeDef.AVATAR_RELATION_TYPE.AVATAR_RELATION_FRIEND);
		
		for(int i=0;i<relationData.Length;i++)
		{
			if (relationData[i].OnlineStatus == (byte)MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_WEB ||
                    relationData[i].OnlineStatus == (byte)MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_MOBILE)
                { FriendOnLineList.Add(relationData[i].AvatarName); }
		}
		
//        for (int j = 0; j < area.getAvatarRelationTypeCodes().Length; j++)
//        {
//            for (int i = 0; i < area.getOnlineStatus()[j].Length; i++)
//            {
//                if (area.getOnlineStatus()[j][i] == (byte)MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_WEB ||
//                    area.getOnlineStatus()[j][i] == (byte)MGCTypeDef.AVATAR_ONLINE_STATUS.AVATAR_ONLINE_STATUS_ONLINE_MOBILE)
//                { FriendOnLineList.Add(area.getAvatarNames()[j][i]); }
//            }
//        }
        
        ShowFriendOnLineMsg(FriendOnLineList);
        MGGameGod.bNewFriendLogin = false;
    }

    public void ShowFriendOnLineMsg(List<string> list)
    {
        if (list.Count <= 0)
            return;

        string text = "";
   
        if (list.Count == 1)
        {
            text = m_wordsTableRef.getUIText("0803001");
            text = text.Replace("%1d", list[0]);
        }
        else
        {
            text = m_wordsTableRef.getUIText("0803002");
            text = text.Replace("%1d", list[0]);
            text = text.Replace("%2d", (list.Count - 1).ToString());
        }

        FriendOnLineNotifyBtn1.GetComponentInChildren<SpriteText>().Text = text;
        FriendOnLineNotifyPanel.BringIn();
        MGAudioManager.PlaySoundByKey("SOUND_FRIENDONLINE");
        CancelInvoke("DismissFriendOnLineNotifyPanel");
        Invoke("DismissFriendOnLineNotifyPanel", 3.0f);
    }
    
    public void OnFriendOnLineNotifyBtnPressed(GameObject caller)
    {
        CancelInvoke("DismissFriendOnLineNotifyPanel");
        FriendOnLineNotifyPanel.Dismiss();
    }

    public void DismissFriendOnLineNotifyPanel()
    { FriendOnLineNotifyPanel.Dismiss(); }
	
	//mj 2013/05/29 
	private GameObject m_AntiAddictedIdentifyBtn;
	public void OnAntiAddictedStatus()
	{
		m_AntiAddictedIdentifyBtn = GameObject.Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, "BaseResource/GUI/Territory/AntiAddicatedIdentifyBtn")) as GameObject;
		Vector3 origin = m_AntiAddictedIdentifyBtn.transform.localPosition;
		m_AntiAddictedIdentifyBtn.transform.parent = leftMainMenuPanel.transform.FindChild("MenuBG");
		m_AntiAddictedIdentifyBtn.transform.localPosition = origin;
		
		UIButton btn = m_AntiAddictedIdentifyBtn.GetComponent<UIButton>();
		btn.scriptWithMethodToInvoke = this;
		btn.methodToInvoke = "ShowAntiAddictedIdentifyPanel";
	}
	
	private GameObject m_AntiAddictedIdentifyPanel = null;
	private void ShowAntiAddictedIdentifyPanel()
	{
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (!GetHotKeyLock())
			return;
#endif
		if(m_AntiAddictedIdentifyPanel == null)
		{
			m_AntiAddictedIdentifyPanel = GameObject.Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, "BaseResource/GUI/Territory/AntiAddicatedIdentifyPanel")) as GameObject;
			MGUITextAdjuster.DoIt(m_AntiAddictedIdentifyPanel);
			Vector3 origin = m_AntiAddictedIdentifyPanel.transform.localPosition;
			UIPanel panel = m_AntiAddictedIdentifyPanel.transform.FindChild("AntiAddicatedIdentifyPanel").GetComponent<UIPanel>();
			
			UIButton btn = panel.transform.FindChild("ForgetPasswordBtn").GetComponent<UIButton>();
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "HideAntiAddicatedPanel";
			
			btn = panel.transform.FindChild("IdentifyBtn").GetComponent<UIButton>();
			btn.scriptWithMethodToInvoke = this;
			btn.methodToInvoke = "RequestAntiAddicatedIdentify";
			
			m_AntiAddictedIdentifyPanel.transform.parent = this.transform.parent.FindChild("DynamicUISet");
			m_AntiAddictedIdentifyPanel.transform.localPosition = origin;
		
			panel.BringIn();
		}
		else
		{
			m_AntiAddictedIdentifyPanel.SetActiveRecursively(true);
			m_AntiAddictedIdentifyPanel.transform.FindChild("AntiAddicatedIdentifyPanel").GetComponent<UIPanel>().BringIn();
		}
	}
	
	private void HideAntiAddicatedPanel()
	{
		m_AntiAddictedIdentifyPanel.SetActiveRecursively(false);
		m_AntiAddictedIdentifyPanel.transform.FindChild("AntiAddicatedIdentifyPanel").localPosition = new Vector3(0,-500.0f,0);
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		InvokeRepeating("CheckUILock", 0, 1.0f);			
#endif
	}
	
	private void RequestAntiAddicatedIdentify()
	{
		m_AntiAddictedIdentifyPanel.SetActiveRecursively(false);
		m_barrier.ShowWithText("");
		Transform panel = m_AntiAddictedIdentifyPanel.transform.FindChild("AntiAddicatedIdentifyPanel");
		string name = panel.FindChild("AccountTextField").GetComponent<UITextField>().Text;
		string idNum = panel.FindChild("PasswordTextField").GetComponent<UITextField>().Text;
		string checksum = MGExternalAccountManager.GetMD5Sum(MGCClientSDK.instance.UserId.ToString()+"|"+name+"|"+idNum);
		
		string urlPath = (string)MGGameConfig.GetValue("anti_addicated_identify");
		
		WWWForm postform = new WWWForm();
		postform.AddField("action", "idCheckin");
		postform.AddField("member_sn",MGCClientSDK.instance.UserId.ToString());
		postform.AddField("name",name);
		postform.AddField("person_id",idNum);
		postform.AddField("product_id",MGExternalAccountManager.Product_Number);
		postform.AddField("confirm_code",checksum);
		
		
		WWW www = new WWW(urlPath, postform);
		Debug.Log(www.url.ToString());
		StartCoroutine(WaitForAntiAddicatedIdentifyResult(www));
#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
		InvokeRepeating("CheckUILock", 0, 1.0f);			
#endif
//		MGCClientSDK.instance.UserId
	}
	
	private IEnumerator WaitForAntiAddicatedIdentifyResult(WWW www)
	{
		yield return www;
		
		PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
		pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
		
		if(www.error != null)
			m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000019"), 3.0f);
		else
		{
			JsonData jd = JsonMapper.ToObject(www.text);
			int status = (int)jd["status"];
			string errorMSG = (string)jd["msg"];
			if(status == 1)
			{
				MGGameGod.AntiAddiction(false);
				pageStyle.message = m_wordsTableRef.getUIText( "0003013" );
				SetPopUpPage(pageStyle);
				GameObject.Destroy(m_AntiAddictedIdentifyBtn);
				GameObject.Destroy(m_AntiAddictedIdentifyPanel);
				m_AntiAddictedIdentifyBtn = null;
				m_AntiAddictedIdentifyPanel = null;
				MGGameGod.ClearAntiAddiction();
			}
			else
			{
				Debug.Log(errorMSG);
				string msgID = "";
				switch(errorMSG)
				{
					case "001":
						msgID = "0003003";
						break;
					case "002":
						msgID = "0003009";
						break;
					case "003":
						msgID = "0003010";
						break;
					case "004":
						msgID = "0001026";
						break;
					case "005":
						msgID = "0001027";
						break;
					case "006":
						msgID = "0003014";
						break;
					case "007":
						msgID = "0003015";
						break;
					case "008":
						msgID = "0203999";
						break;
					case "010":
						msgID = "0002002";
						break;
					case "111":
						msgID = "0003016";
						break;
				}
					
				pageStyle.message = m_wordsTableRef.getUIText(msgID);
				SetPopUpPage(pageStyle);
			}
		}
	}

    private void SetObjectToCashFlowLayer()
    {
        moneyPanel.gameObject.layer = LAYER_CashFlow;
        SetLayerRecursively(moneyPanel.transform, LAYER_CashFlow);
        //SetLayerRecursively(functionMenuBGPanel.transform, LAYER_CashFlow);
        SetLayerRecursively(goldEdge.transform, LAYER_CashFlow);
        SetLayerRecursively(goldCircle.transform, LAYER_CashFlow);

        //Turn off GUI Layer and turn on CashFlow layer
        this.transform.parent.gameObject.GetComponent<Camera>().cullingMask &= ~(1 << LAYER_GUI);
        this.transform.parent.gameObject.GetComponent<Camera>().cullingMask |= (1 << LAYER_CashFlow);

        SetLayerRecursively(m_barrier.gameObject.transform, LAYER_CashFlow);
		SetLayerRecursively(popUpMaskPanel.transform.parent, LAYER_CashFlow);
    }

    private void SetObjectToCashFlowPos()
    {
        //Set MoneyPanel Z axis Pos
        Vector3 tmp = Vector3.zero;
        tmp.x = moneyPanel.transform.localPosition.x;
        tmp.y = moneyPanel.transform.localPosition.y;
        tmp.z = -80.0f;
        moneyPanel.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = goldEdge.transform.localPosition.x;
        tmp.y = goldEdge.transform.localPosition.y;
        tmp.z = -77.0f;
        goldEdge.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = goldCircle.transform.localPosition.x;
        tmp.y = goldCircle.transform.localPosition.y;
        tmp.z = -77.0f;
        goldCircle.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);


        tmp = Vector3.zero;
        tmp.x = functionMenuBGPanel.transform.localPosition.x;
        tmp.y = functionMenuBGPanel.transform.localPosition.y;
        tmp.z = -70.0f;
        functionMenuBGPanel.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = m_barrier.gameObject.transform.localPosition.x;
        tmp.y = m_barrier.gameObject.transform.localPosition.y;
        tmp.z = 90.0f;
        m_barrier.gameObject.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);
		
		tmp = Vector3.zero;
        tmp.x = popUpMaskPanel.transform.parent.localPosition.x;
        tmp.y = popUpMaskPanel.transform.parent.localPosition.y;
        tmp.z = -110.0f;
		popUpMaskPanel.transform.parent.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);
    }

    private void SetObjectToGUILayer()
    {
        moneyPanel.gameObject.layer = LAYER_GUI;
        SetLayerRecursively(moneyPanel.transform, LAYER_GUI);
        //SetLayerRecursively(functionMenuBGPanel.transform, LAYER_GUI);
        SetLayerRecursively(goldEdge.transform, LAYER_GUI);
        SetLayerRecursively(goldCircle.transform, LAYER_GUI);

        //Turn on GUI Layer and turn off CashFlow layer
        this.transform.parent.gameObject.GetComponent<Camera>().cullingMask |= (1 << LAYER_GUI);
        this.transform.parent.gameObject.GetComponent<Camera>().cullingMask &= ~(1 << LAYER_CashFlow);

        SetLayerRecursively(m_barrier.gameObject.transform, LAYER_GUI);
		SetLayerRecursively(popUpMaskPanel.transform.parent, LAYER_GUI);
    }

    private void SetObjectToGUIPos()
    {
        //Set MoneyPanel Z axis Pos
        Vector3 tmp = Vector3.zero;
        tmp.x = moneyPanel.transform.localPosition.x;
        tmp.y = moneyPanel.transform.localPosition.y;
        tmp.z = 0.0f;
        moneyPanel.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = goldEdge.transform.localPosition.x;
        tmp.y = goldEdge.transform.localPosition.y;
        tmp.z = 51.0f;
        goldEdge.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = goldCircle.transform.localPosition.x;
        tmp.y = goldCircle.transform.localPosition.y;
        tmp.z = 50.5f;
        goldCircle.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = functionMenuBGPanel.transform.localPosition.x;
        tmp.y = functionMenuBGPanel.transform.localPosition.y;
        tmp.z = 300.0f;
        functionMenuBGPanel.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);

        tmp = Vector3.zero;
        tmp.x = m_barrier.gameObject.transform.localPosition.x;
        tmp.y = m_barrier.gameObject.transform.localPosition.y;
        tmp.z = 160.0f;
        m_barrier.gameObject.transform.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);
		
		tmp = Vector3.zero;
        tmp.x = popUpMaskPanel.transform.parent.localPosition.x;
        tmp.y = popUpMaskPanel.transform.parent.localPosition.y;
        tmp.z = -100.0f;
		popUpMaskPanel.transform.parent.localPosition = new Vector3(tmp.x, tmp.y, tmp.z);
    }

    /// <summary>
    /// Set Layer recursively
    /// </summary>
    /// <param name="root"></param>
    /// <param name="layer"></param>
    private void SetLayerRecursively(Transform root, int layer)
    {
        Stack<Transform> moveTargets = new Stack<Transform>();
        moveTargets.Push(root);
        Transform currentTarget;
        while (moveTargets.Count != 0)
        {
            currentTarget = moveTargets.Pop();
            currentTarget.gameObject.layer = layer;
            foreach (Transform child in currentTarget)
                moveTargets.Push(child);
        }
    }

    /// <summary>
    /// If the payment is set closed in MGConfig then call this function to info player this payment is not supported yet 
    /// </summary>
    private void ShowPaymentIsNotOpen()
    {
        functionErrorHandleMsgBoxStyle_Double = new PopUpPage_DoubleStyle();
        functionErrorHandleMsgBoxStyle_Double.popUpType = (byte)PopUpPage_DoubleStyle.PopUpType.SINGLE_BUTTON;
        functionErrorHandleMsgBoxStyle_Double.message = m_wordsTableRef.getUIText("0600024");
        functionErrorHandleMsgBoxStyle_Double.functionButtonText = m_wordsTableRef.getUIText("0202003");
        SetPopUpPage(functionErrorHandleMsgBoxStyle_Double);
        return;
    }


    public void SetPlayerLvNumImg(int Lv)
    {
		stBtn_Ones.GetComponent<BoxCollider>().enabled = false;
		stBtn_Tens.GetComponent<BoxCollider>().enabled = false;

        if (Lv <= 0 || Lv > 99)
        { Lv = 0; }

        int iOnes = Lv % 10;
        double dTens = Math.Floor((double)(Lv / 10));
        int Tens = int.Parse(Math.Round(dTens, 0).ToString());

        stBtn_Ones.SetState(iOnes);

        if (Tens > 0)
        {
            stBtn_Tens.Hide(false);
            stBtn_Tens.SetState(Tens);
        }
    }

    //Update Experience Bar
    public void UpdateExperienceBar(MGCStructDef.BuildingInfo buildinginfo)
    {
        float NowExperience = (float)m_commonDataRef.playerDetailedInfo.techPoint;

        //If the Buildingcost is out of the limit, m_wordsTableRef.getBuildingCost() will return null string
        //then return
        float levelupExperience = 0.0f;
        if (!float.TryParse(m_wordsTableRef.getBuildingCost(buildinginfo.buildingObjectId + "_" + buildinginfo.buildingLevel), out levelupExperience))
        { return; }
        
        experiencePanel.GetComponentInChildren<UIProgressBar>().Value = (NowExperience / levelupExperience);
        experiencePanel.GetComponentInChildren<UIProgressBar>().Text = NowExperience + "/" + levelupExperience;
		
		
        if (NowExperience >= levelupExperience)
        {
            m_barrier.ShowWithText(m_wordsTableRef.getUIText("0000029"));
            SendBuildingUpdateREQ((byte)MGCTypeDef.BUILDING_ID.MAIN_TOWN);
        }
    }
	
	#region Buy Resource
	public void OnResourcePurchaseUIBtnPressed(GameObject caller)
	{
		
		string BtnName = caller.name;
		/*
		BuyResourecePanel.transform.FindChild("GoldNumberSelect").FindChild("AmountText").GetComponent<SpriteText>().Text = "0";
		BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("ResourceGetAmount").GetComponent<SpriteText>().Text = "0";
		GoldCost = 0;
		*/
		//m_CurrentProductTypeTab = goldMarketTabMenuPanel.transform.GetChild(int.Parse(index) - 1).gameObject;
		switch(BtnName)
		{
		case "LirraBG":
			BuyCurrency(null, MGCTerritoryTypeDef.Constants.BUY_CURRENCY_HEROPOINT_GUID);
			/*
			ResourceType = ResourcePurchaseType.SilverCoin;
			BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("IconBonus").GetComponent<PackedSprite>().Hide(true);
			BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("IconSilverCoin").GetComponent<PackedSprite>().Hide(false);
			BuyResourecePanel.transform.FindChild("TitleBG").FindChild("TitleText").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0202056")+m_wordsTableRef.getUIText("0900002");
			*/
			m_CurrentProductTypeTab = goldMarketTabMenuPanel.transform.GetChild(0).gameObject;
			break;
		case "HeroPointBG":
			BuyCurrency(null, MGCTerritoryTypeDef.Constants.ROLLING_EGG_COUPON_ID);
			/*
			ResourceType = ResourcePurchaseType.Bonus;
			BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("IconBonus").GetComponent<PackedSprite>().Hide(false);
			BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("IconSilverCoin").GetComponent<PackedSprite>().Hide(true);
			BuyResourecePanel.transform.FindChild("TitleBG").FindChild("TitleText").GetComponent<SpriteText>().Text = m_wordsTableRef.getUIText("0202056")+m_wordsTableRef.getUIText("0200005");
			*/
			m_CurrentProductTypeTab = goldMarketTabMenuPanel.transform.GetChild(0).gameObject;
			break;
		case "CloseBtn":
			break;
		default:
			Debug.LogError("No This Button Name !!! Please Check");
			break;
		}
		/*
		BuyResourecePanel.BringIn();
		popUpMaskPanel.BringIn();
		*/
	}
	
	public void OnCloseResourcePurchaseBtnPressed(GameObject caller)
	{
		ResourceType = ResourcePurchaseType.None;
		popUpMaskPanel.Dismiss();
		BuyResourecePanel.Dismiss();
	}
	
	public void OnResourceAmountChangeBtnPressed(GameObject caller)
	{
		string BtnName = caller.name;
		int ExchangeRatio = 0;
		
		//Parser the Gold Number Text
		string StrAmount = BuyResourecePanel.transform.FindChild("GoldNumberSelect").FindChild("AmountText").GetComponent<SpriteText>().Text;
		if(!int.TryParse(StrAmount,out GoldCost))
		{
			Debug.LogError("Parse Error !!! Please Check");
			return;
		}
		
		//Get Resource Item Data 
		MGItemData ItemData = null;
		switch(ResourceType)
		{
		case ResourcePurchaseType.None:
			break;
		case ResourcePurchaseType.SilverCoin:
			ItemData = MGDataManager.instance.GetItemData("2010101603");
			//reference Items.XML , ItemData.m_serverCallback[2] represent the ratio of Gold and Bonus or Silver Coin
		    //If You have any question about it,just ask austom
			if(!int.TryParse(ItemData.m_serverCallback[2],out ExchangeRatio))
			{
				Debug.LogError("Parse Error !!! Please Check");
			}
			break;
		case ResourcePurchaseType.Bonus:
			ItemData = MGDataManager.instance.GetItemData("2010101604");
			if(!int.TryParse(ItemData.m_serverCallback[2],out ExchangeRatio))
			{
				Debug.LogError("Parse Error !!! Please Check");
			}
			break;
		default:
			Debug.LogError("Type Error !!! Please Check");
			break;
		}
		
		if(ItemData == null || ItemData.m_serverCallback[2] == "" || ItemData.m_serverCallback[2] == null)
		{return;}
		
		
		
		//Plus or Minus Quantity
		switch(BtnName)
		{
		case "PlusBtn":
			GoldCost += 1;
			break;
		case "MinusBtn":
			GoldCost -= 1;
			break;
		default:
			Debug.LogError("Button Name Error !!! Please Check");
			break;
		}
		
		if(GoldCost < 0)
		{
			GoldCost = 0;
		}
		
		BuyResourecePanel.transform.FindChild("GoldNumberSelect").FindChild("AmountText").GetComponent<SpriteText>().Text = GoldCost.ToString();
		//reference Items.XML , ItemData.m_serverCallback[2] represent the ratio of Gold and Bonus or Silver Coin
		//If You have any question about it,just ask austom
		//Debug.Log("@@@@ ItemData.m_sellPriceGameCoin = " + ItemData.m_serverCallback[2].ToString());
		//Debug.Log("@@@@ ItemData.m_buyPriceGameCoin = " + ItemData.m_buyPriceGameCoin);
		//Debug.Log("@@@@ ItemData.m_sellPriceGameCoin = " + ItemData.m_sellPriceGameCoin);

		BuyResourecePanel.transform.FindChild("ResourceGetBG").FindChild("ResourceGetAmount").GetComponent<SpriteText>().Text = 
				(ExchangeRatio * GoldCost).ToString();
	}
	
	public void OnResourcePurchaseBtnPressed(GameObject caller)
	{
		string ItemID = "";
		int Ratio = 0;
			
		if(ResourceType == ResourcePurchaseType.SilverCoin)
		{
			MGItemData ItemData = MGDataManager.instance.GetItemData("2010101603");
			ItemID =  "78000400001";
			
			if(!int.TryParse(ItemData.m_serverCallback[2],out Ratio))
			{Debug.LogError("Parse Error !!! Please Check");}
		}
		else if(ResourceType == ResourcePurchaseType.Bonus)
		{
			MGItemData ItemData = MGDataManager.instance.GetItemData("2010101604");
			ItemID = "78000400002";
			
			if(!int.TryParse(ItemData.m_serverCallback[2],out Ratio))
			{Debug.LogError("Parse Error !!! Please Check");}
		}
		
		m_clientSDKRef.BuyMallItem(ItemID, (GoldCost*Ratio));
		BuyResourecePanel.Dismiss();
	}
	
	private enum ResourcePurchaseType
	{
		None = 0,
		Bonus = 1,
		SilverCoin = 2
	}
	#endregion
	
	public void CheckSynthesisQuestList()
	{
		if(m_questCompleteConditionStatus.ContainsKey("synthesisCount"))
		{
			if(m_questCompleteConditionStatus["synthesisCount"] == true)
			{
				MGCDelegateMgr.instance.GetQuestStatusCount -= SetQuestCountMissionPage;
        		MGCDelegateMgr.instance.GetQuestStatusCount += SetQuestCountMissionPage;
        		m_clientSDKRef.GetQuestStatusCount();
			
				//Send Refresh mission list
				SetQuestListHandler(OnGetUpdateQuestList_Main);
				MGCClientSDK.instance.GetQuestList();
			}
		}
	}
	
	private void OnGetRankRewardBtnDown()
	{
		GameObject rankRewardPanel = Instantiate((GameObject)MGDownloader.instance.getObject(BundleType.BASERESOURCE_BUNDLE, MGResourcePaths.m_UI + "Territory/ArenaAndPVPRankRewardPanel")) as GameObject;
		MGUITextAdjuster.DoIt(rankRewardPanel);
		rankRewardPanel.transform.parent = GameObject.Find("TerritoryUI").transform;
		rankRewardPanel.GetComponent<MGCArenaAndPVPRankRewardPanel>().Initialize(m_wordsTableRef ,MGSceneMasterTerritory.Instance.m_ArenaRankRewardItem, MGSceneMasterTerritory.Instance.m_PVPRankRewardItem);
		
		rankRewardPanel.GetComponent<UIPanel>().BringIn();
		if(MGSceneMasterTerritory.Instance.m_ArenaRankRewardItem != null)
		{
			m_clientSDKRef.SetArenaRankReward();
			MGSceneMasterTerritory.Instance.m_ArenaRankRewardItem = null;
		}
		
		if(MGSceneMasterTerritory.Instance.m_PVPRankRewardItem != null)
		{
			m_clientSDKRef.SetPVPRankReward();
			MGSceneMasterTerritory.Instance.m_PVPRankRewardItem = null;
		}
		SendGeneralInfoREQ();
	}
	
	private void SetRankRewardBtnVisible(bool enable)
	{		
		Transform rankrewardpanel = GameObject.Find("TerritoryUI/UpperUISet/UpperUIPanel/UpperLeftPlacement/PortraitPanel/PortraitBG/RankRewardPanel").transform;
			
		if(enable && (MGSceneMasterTerritory.Instance.m_ArenaRankRewardItem != null || MGSceneMasterTerritory.Instance.m_PVPRankRewardItem != null))
			rankrewardpanel.localPosition = new Vector3(61.1748f, -27.62361f, 0f);
		else
			rankrewardpanel.localPosition = new Vector3(5000f, 0f, 0f);
	}
	
	public void OnLevelUnqualifiedPopUp()
	{
		PopUpPageStyleBase pageStyle = new PopUpPageStyleBase();
		pageStyle.message = m_wordsTableRef.getUIText("0301232").Replace("%d", MGGameConfig.GetValue<int>("Unlock_Explore_Homelevel").ToString());
		pageStyle.popUpType = (byte)PopUpPageStyleBase.PopUpType.SINGLE_BUTTON;
		SetPopUpPage(pageStyle);
	}
}
