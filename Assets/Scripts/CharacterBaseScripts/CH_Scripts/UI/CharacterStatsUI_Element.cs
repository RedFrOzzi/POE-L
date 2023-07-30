using UnityEngine;
using TMPro;
using System.Text;

public class CharacterStatsUI_Element : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI statsOffenceText;
	[SerializeField] private TextMeshProUGUI statsDefenceText;

    [SerializeField] private GameObject[] statsTabs;

    private CharacterController2D controller;
    private CH_Stats stats;
    private StringBuilder strBldr = new();
    private bool isInventoryOpen = false;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<CharacterController2D>();
        stats = player.GetComponent<CH_Stats>();

        statsDefenceText.transform.parent.gameObject.SetActive(false);
        statsOffenceText.transform.parent.gameObject.SetActive(false);

        controller.OpenInventory += OnInventoryOpen;
    }

    private void OnDestroy()
    {
        controller.OpenInventory -= OnInventoryOpen;
    }

    public void OnOffenceTabClick()
    {
        foreach (var tab in statsTabs)
        {
            if (tab.name == "PanelStatsOffence")
            {
                tab.SetActive(true);
                continue;
            }

            tab.SetActive(false);
        }
    }

    public void OnDefanceTabClick()
    {
        foreach (var tab in statsTabs)
        {
            if (tab.name == "PanelStatsDefence")
            {
                tab.SetActive(true);
                continue;
            }

            tab.SetActive(false);
        }
    }

    private void OnInventoryOpen()
    {
        isInventoryOpen = !isInventoryOpen;

        statsOffenceText.gameObject.SetActive(isInventoryOpen);

        statsOffenceText.text = BuildOffenceString();
        statsDefenceText.text = BuildDefenceString();
    }

    private string BuildOffenceString()
    {
        strBldr.Clear();

        strBldr.Append($" Attack damage: {stats.CurrentMinDamage} - {stats.CurrentMaxDamage}" +
            $"\n Attack speed: {stats.CurrentAttackSpeed}" +
            $"\n Crit chance: {stats.CurrentCritChance}" +
            $"\n Crit multiplier: {stats.CurrentAttackCritMultiplier}" +
            $"\n Attack range: {stats.CurrentAttackRange}" +
            $"\n Reload speed: {stats.CurrentReloadSpeed}" +
            $"\n Ammo capacity: {stats.CurrentAmmoCapacity}" +
            $"\n Bullet spread angle: {stats.CurrentSpreadAngle}" +
            $"\n Projectiles: {stats.WeaponProjectileAmount}" +
            $"\n Pierce: {stats.WeaponPierceAmount}" +
            $"\n Chains: {stats.WeaponChainsAmount}");

        return strBldr.ToString();
    }

    private string BuildDefenceString()
    {
        strBldr.Clear();

        strBldr.Append($" Armor: {stats.CurrentArmor}" +
            $"\n Percent physical reduction: {stats.PercentPhisicalResistance:F2}" +
            $"\n Magic resist: {stats.CurrentMagicResist}" +
            $"\n Percent magic resist: {stats.PercentMagicResistance:F2}" +
            $"\n Health points: {stats.CurrentHP}" +
            $"\n Health regen: {stats.CurrentHPRegeneration}");

        return strBldr.ToString();
    }
}
