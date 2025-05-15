using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Chests;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using UnityEngine;

public class Chest : MonoBehaviour, IUseable
{
    [ColorUsage(false, true)]
    public Color materializeColor;
    public float materializeTime = 3f;
    public Transform itemSpawnPoint;

    [HideInInspector] public int healthPercent;
    [HideInInspector] public WeaponDetailsSO weaponDetails;
    [HideInInspector] public int ammoPercent;

    private bool isEnabled = false;

    public GameObject chestItemGameObject {  get; private set; }
    public ChestItem chestItem { get; private set; }
    public TextMeshPro messageTextTMP {  get; private set; }

    public Animator Animator { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }  
    public MaterializeEffect materializeEffect { get; private set; }

    public StateManager StateManager { get; private set; }
    public ChessCloseState ChessCloseState { get; private set; }
    public ChestOpenState ChestOpenState { get; private set; }
    public ChestHealthItemState ChestHealthItemState { get; private set; }
    public ChestAmmoItemState ChestAmmoItemState { get; private set; }
    public ChestWeaponItemState ChestWeaponItemState { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();

        StateManager = new StateManager();
        ChessCloseState = new ChessCloseState(this, StateManager, 0);
        ChestOpenState = new ChestOpenState(this, StateManager, Settings.use);
        ChestHealthItemState = new ChestHealthItemState(this, StateManager, 0);
        ChestAmmoItemState = new ChestAmmoItemState(this, StateManager, 0);
        ChestWeaponItemState = new ChestWeaponItemState(this, StateManager, 0);
    }
    private void Start()
    {
        StateManager.Initialize(ChessCloseState);
    }

    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;

        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { SpriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(
            GameResources.Instance.materializeShader,
            materializeColor,
            materializeTime,
            spriteRendererArray,
            GameResources.Instance.litMaterial));

        EnableChest();
    }

    private void EnableChest()
    {
        isEnabled = true;
    }

    public void UseItem()
    {
        if (!isEnabled) return;
        StateManager.CurrentChestState.HandleUse();
    }
    public void UpdateChestState()
    {
        if (healthPercent != 0)
        {
            StateManager.ChangeState(ChestHealthItemState);
            ChestHealthItemState.InstantiateHealthItem();
        }
        else if (ammoPercent != 0)
        {
            StateManager.ChangeState(ChestAmmoItemState);
            ChestAmmoItemState.InstatiateAmmoItem();
        }
        else if (weaponDetails != null)
        {
            StateManager.ChangeState(ChestWeaponItemState);
            ChestWeaponItemState.InstatiateWeaponItem();
        }
    }
    public void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }
    public void PlayOpenSound()
    {
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);
    }
    public void DestroyItem()
    {
        Destroy(chestItemGameObject);
    }
    public void DebugText(string text)
    {
        Debug.Log(text);
    }
}
