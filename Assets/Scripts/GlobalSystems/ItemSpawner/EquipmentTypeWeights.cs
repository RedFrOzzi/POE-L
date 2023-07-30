using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "EquipmentTypeWeights", menuName = "ScriptableObjects/Weights/EquipmentTypeWeights")]
public class EquipmentTypeWeights : ScriptableObject
{
	[SerializeField, Header("Pistols")] private float Pistol_MK_1;
	[SerializeField] private float Pistol_MK_2;
	[SerializeField] private float Revolver;
	[SerializeField] private float Revolver_MK_2;
	[SerializeField] private float Pistol_MK_5;
	[SerializeField] private float Pistol_MK_6;

	[SerializeField, Header("Assault rifles"), Space(10)] private float AssaultRifle_MK_1;
	[SerializeField] private float AssaultRifle_MK_2;
	[SerializeField] private float AssaultRifle_MK_3;
	[SerializeField] private float AssaultRifle_MK_4;
	[SerializeField] private float AssaultRifle_MK_5;
	[SerializeField] private float AssaultRifle_MK_6;

	[SerializeField, Header("Shotgun"), Space(10)] private float Shotgun_MK_1;
	[SerializeField] private float Shotgun_MK_2;
	[SerializeField] private float Shotgun_MK_3;
	[SerializeField] private float Shotgun_MK_4;
	[SerializeField] private float Shotgun_MK_5;
	[SerializeField] private float Shotgun_MK_6;

	[SerializeField, Header("Rocket launcher"), Space(10)] private float RocketLauncher_MK_1;
	[SerializeField] private float RocketLauncher_MK_2;
	[SerializeField] private float RocketLauncher_MK_3;
	[SerializeField] private float RocketLauncher_MK_4;
	[SerializeField] private float RocketLauncher_MK_5;
	[SerializeField] private float RocketLauncher_MK_6;

	[SerializeField, Header("Grenade launcher"), Space(10)] private float GrenadeLauncher_MK_1;
	[SerializeField] private float GrenadeLauncher_MK_2;
	[SerializeField] private float GrenadeLauncher_MK_3;
	[SerializeField] private float GrenadeLauncher_MK_4;
	[SerializeField] private float GrenadeLauncher_MK_5;
	[SerializeField] private float GrenadeLauncher_MK_6;

	[SerializeField, Header("Minigun"), Space(10)] private float Minigun_MK_1;
	[SerializeField] private float Minigun_MK_2;
	[SerializeField] private float Minigun_MK_3;
	[SerializeField] private float Minigun_MK_4;
	[SerializeField] private float Minigun_MK_5;
	[SerializeField] private float Minigun_MK_6;

	[SerializeField, Header("Charging Gun"), Space(10)] private float GargingGun_MK_1;
	[SerializeField] private float GargingGun_MK_2;
	[SerializeField] private float GargingGun_MK_3;
	[SerializeField] private float GargingGun_MK_4;
	[SerializeField] private float GargingGun_MK_5;
	[SerializeField] private float GargingGun_MK_6;

	[SerializeField, Header("Helmet"), Space(10)] private float Helmet_MK_Armor;
	[SerializeField] private float Helmet_MK_MResist;
	[SerializeField] private float Helmet_MK_Health;
	[SerializeField] private float Helmet_MK_HealthArmor;
	[SerializeField] private float Helmet_MK_HealthResist;
	[SerializeField] private float Helmet_MK_ArmorMResist;

	[SerializeField, Header("Body Armor"), Space(10)] private float BodyArmor_Armor;
	[SerializeField] private float BodyArmor_MResist;
	[SerializeField] private float BodyArmor_Health;
	[SerializeField] private float BodyArmor_HealthArmor;
	[SerializeField] private float BodyArmor_HealthResist;
	[SerializeField] private float BodyArmor_ArmorMResist;

	[SerializeField, Header("Gloves"), Space(10)] private float Gloves_MK_Armor;
	[SerializeField] private float Gloves_MK_MResist;
	[SerializeField] private float Gloves_MK_Health;
	[SerializeField] private float Gloves_MK_HealthArmor;
	[SerializeField] private float Gloves_MK_HealthResist;
	[SerializeField] private float Gloves_MK_ArmorMResist;

	[SerializeField, Header("Boots"), Space(10)] private float Boots_MK_Armor;
	[SerializeField] private float Boots_MK_MResist;
	[SerializeField] private float Boots_MK_Health;
	[SerializeField] private float Boots_MK_HealthArmor;
	[SerializeField] private float Boots_MK_HealthResist;
	[SerializeField] private float Boots_MK_ArmorMResist;

	[SerializeField, Header("Books"), Space(10)] private float Book_MK_1;
	[SerializeField] private float Book_MK_2;
	[SerializeField] private float Book_MK_3;
	[SerializeField] private float Book_MK_4;
	[SerializeField] private float Book_MK_5;
	[SerializeField] private float Book_MK_6;

	[SerializeField, Header("Ammo packs"), Space(10)] private float AmmoPack_MK_1;
	[SerializeField] private float AmmoPack_MK_2;
	[SerializeField] private float AmmoPack_MK_3;
	[SerializeField] private float AmmoPack_MK_4;
	[SerializeField] private float AmmoPack_MK_5;
	[SerializeField] private float AmmoPack_MK_6;

	[SerializeField, Header("Ability gem"), Space(20)] private float AbilityGem_Tier_1;
	[SerializeField] private float AbilityGem_Tier_2;
	[SerializeField] private float AbilityGem_Tier_3;
	[SerializeField] private float AbilityGem_Tier_4;
	[SerializeField] private float AbilityGem_Tier_5;
	[SerializeField] private float AbilityGem_Tier_6;

	[SerializeField, Header("Super ability gem")] private float SuperAbilityGem_Tier_1;
	[SerializeField] private float SuperAbilityGem_Tier_2;
	[SerializeField] private float SuperAbilityGem_Tier_3;
	[SerializeField] private float SuperAbilityGem_Tier_4;
	[SerializeField] private float SuperAbilityGem_Tier_5;
	[SerializeField] private float SuperAbilityGem_Tier_6;

    private float overallWeaponTypesWeight;
    private float overallHelmetTypesWeight;
    private float overallBodyArmorTypesWeight;
    private float overallGlovesTypesWeight;
    private float overallBootsTypesWeight;
    private float overallLeftHandTypesWeight;
    private float overallAbilityGemTypesWeight;
    private float overallSuperAbilityGemTypesWeight;

    private float[] weaponTypeWeights;
    private float[] weaponTypeChances;

	private float[] helmetTypeWeights;
	private float[] helmetTypeChances;

    private float[] bodyArmorTypeWeights;
    private float[] bodyArmorTypeChances;

    private float[] glovesTypeWeights;
    private float[] glovesTypeChances;

    private float[] bootsTypeWeights;
    private float[] bootsTypeChances;

    private float[] leftHandTypeWeights;
    private float[] leftHandTypeChances;

    private float[] abilityGemTypeWeights;
	private float[] abilityGemTypeChances;

	private float[] superAbilityGemTypeWeights;
	private float[] superAbilityGemTypeChances;

    public void SetUpChances()
    {
		SetUpWeaponChances();
		SetUpHelmetChances();
		SetUpBodyArmorChances();
		SetUpGlovesChances();
		SetUpBootsChances();
		SetUpLeftHandChances();
		SetUpAbilityGemChances();
		SetUpSuperAbilityGemChances();
    }

	public EquipmentType GetWeightedWeaponEquipmentType()
    {
		float chance = Random.value;

		float accum = 0f;
		for (int i = 0; i < weaponTypeChances.Length; i++)
		{
			accum += weaponTypeChances[i];
			if (chance < accum)
			{
				return i switch
				{
					0 => EquipmentType.Pistol_MK_1,
					1 => EquipmentType.Revolver_MK_1,
					2 => EquipmentType.Pistol_MK_2,
					3 => EquipmentType.Revolver_MK_2,
					4 => EquipmentType.Pistol_MK_5,
					5 => EquipmentType.Pistol_MK_6,

					6 => EquipmentType.AssaultRifle_MK_1,
					7 => EquipmentType.AssaultRifle_MK_2,
					8 => EquipmentType.AssaultRifle_MK_3,
					9 => EquipmentType.AssaultRifle_MK_4,
					10 => EquipmentType.AssaultRifle_MK_5,
					11 => EquipmentType.AssaultRifle_MK_6,

					12 => EquipmentType.Shotgun_MK_1,
					13 => EquipmentType.Shotgun_MK_2,
					14 => EquipmentType.Shotgun_MK_3,
					15 => EquipmentType.Shotgun_MK_4,
					16 => EquipmentType.Shotgun_MK_5,
					17 => EquipmentType.Shotgun_MK_6,

					18 => EquipmentType.RocketLauncher_MK_1,
					19 => EquipmentType.RocketLauncher_MK_2,
					20 => EquipmentType.RocketLauncher_MK_3,
					21 => EquipmentType.RocketLauncher_MK_4,
					22 => EquipmentType.RocketLauncher_MK_5,
					23 => EquipmentType.RocketLauncher_MK_6,

					24 => EquipmentType.GrenadeLauncher_MK_1,
					25 => EquipmentType.GrenadeLauncher_MK_2,
					26 => EquipmentType.GrenadeLauncher_MK_3,
					27 => EquipmentType.GrenadeLauncher_MK_4,
					28 => EquipmentType.GrenadeLauncher_MK_5,
					29 => EquipmentType.GrenadeLauncher_MK_6,

					30 => EquipmentType.Minigun_MK_1,
					31 => EquipmentType.Minigun_MK_2,
					32 => EquipmentType.Minigun_MK_3,
					33 => EquipmentType.Minigun_MK_4,
					34 => EquipmentType.Minigun_MK_5,
					35 => EquipmentType.Minigun_MK_6,

					36 => EquipmentType.ChargingGun_MK_1,
					37 => EquipmentType.ChargingGun_MK_2,
					38 => EquipmentType.ChargingGun_MK_3,
					39 => EquipmentType.ChargingGun_MK_4,
					40 => EquipmentType.ChargingGun_MK_5,
					41 => EquipmentType.ChargingGun_MK_6,

					_ => EquipmentType.Pistol_MK_1
				};
			}
		}

		return EquipmentType.Pistol_MK_1;
	}

	public EquipmentType GetWeightedHelmetEquipmentType()
	{
		float chance = Random.value;

		float accum = 0f;
		for (int i = 0; i < helmetTypeChances.Length; i++)
		{
			accum += helmetTypeChances[i];
			if (chance < accum)
			{
				return i switch
				{
					0 => EquipmentType.Helmet_Armor,
					1 => EquipmentType.Helmet_MResist,
					2 => EquipmentType.Helmet_Health,
					3 => EquipmentType.Helmet_HealthArmor,
					4 => EquipmentType.Helmet_HealthResist,
					5 => EquipmentType.Helmet_ArmorMResist,

					_ => EquipmentType.Helmet_Armor
                };
			}
		}

		return EquipmentType.Helmet_Armor;
	}

	public EquipmentType GetWeightedLeftHandEquipmentType()
	{
        float chance = Random.value;

        float accum = 0f;
		for (int i = 0; i < leftHandTypeChances.Length; i++)
		{
			accum += leftHandTypeChances[i];
			if (chance < accum)
			{
				return i switch
				{
					0 => EquipmentType.Book_MK_1,
					1 => EquipmentType.Book_MK_2,
					2 => EquipmentType.Book_MK_3,
					3 => EquipmentType.Book_MK_4,
					4 => EquipmentType.Book_MK_5,
					5 => EquipmentType.Book_MK_6,

					6 => EquipmentType.AmmoPack_MK_1,
					7 => EquipmentType.AmmoPack_MK_2,
					8 => EquipmentType.AmmoPack_MK_3,
					9 => EquipmentType.AmmoPack_MK_4,
					10 => EquipmentType.AmmoPack_MK_5,
					11 => EquipmentType.AmmoPack_MK_6,

                    _ => EquipmentType.AmmoPack_MK_1
                };
			}
		}

        return EquipmentType.AmmoPack_MK_1;
    }

    public EquipmentType GetWeightedBodyArmorEquipmentType()
    {
        float chance = Random.value;

        float accum = 0f;
        for (int i = 0; i < bodyArmorTypeChances.Length; i++)
        {
            accum += bodyArmorTypeChances[i];
            if (chance < accum)
            {
                return i switch
                {
                    0 => EquipmentType.BodyArmor_Armor,
                    1 => EquipmentType.BodyArmor_MResist,
                    2 => EquipmentType.BodyArmor_Health,
                    3 => EquipmentType.BodyArmor_HealthArmor,
                    4 => EquipmentType.BodyArmor_HealthResist,
                    5 => EquipmentType.BodyArmor_ArmorMResist,

                    _ => EquipmentType.BodyArmor_Armor
                };
            }
        }

        return EquipmentType.BodyArmor_Armor;
    }

    public EquipmentType GetWeightedGlovesEquipmentType()
    {
        float chance = Random.value;

        float accum = 0f;
        for (int i = 0; i < glovesTypeChances.Length; i++)
        {
            accum += glovesTypeChances[i];
            if (chance < accum)
            {
                return i switch
                {
                    0 => EquipmentType.Gloves_MK_Armor,
                    1 => EquipmentType.Gloves_MK_MResist,
                    2 => EquipmentType.Gloves_MK_Health,
                    3 => EquipmentType.Gloves_MK_HealthArmor,
                    4 => EquipmentType.Gloves_MK_HealthResist,
                    5 => EquipmentType.Gloves_MK_ArmorMResist,

                    _ => EquipmentType.Gloves_MK_Armor
                };
            }
        }

        return EquipmentType.Gloves_MK_Armor;
    }

    public EquipmentType GetWeightedBootsEquipmentType()
    {
        float chance = Random.value;

        float accum = 0f;
        for (int i = 0; i < bootsTypeChances.Length; i++)
        {
            accum += bootsTypeChances[i];
            if (chance < accum)
            {
                return i switch
                {
                    0 => EquipmentType.Boots_MK_Armor,
                    1 => EquipmentType.Boots_MK_MResist,
                    2 => EquipmentType.Boots_MK_Health,
                    3 => EquipmentType.Boots_MK_HealthArmor,
                    4 => EquipmentType.Boots_MK_HealthResist,
                    5 => EquipmentType.Boots_MK_ArmorMResist,

                    _ => EquipmentType.Boots_MK_Armor
                };
            }
        }

        return EquipmentType.Boots_MK_Armor;
    }

    public EquipmentType GetWeightedAbilityGemEquipmentType()
    {
		float chance = Random.value;

		float accum = 0f;
		for (int i = 0; i < abilityGemTypeChances.Length; i++)
		{
			accum += abilityGemTypeChances[i];
			if (chance < accum)
			{
				return i switch
				{
					0 => EquipmentType.AbilityGem_Tier_1,
					1 => EquipmentType.AbilityGem_Tier_2,
					2 => EquipmentType.AbilityGem_Tier_3,
					3 => EquipmentType.AbilityGem_Tier_4,
					4 => EquipmentType.AbilityGem_Tier_5,
					5 => EquipmentType.AbilityGem_Tier_6,

					_ => EquipmentType.AbilityGem_Tier_1
				};
			}
		}

		return EquipmentType.AbilityGem_Tier_1;
	}

    public EquipmentType GetWeightedSuperAbilityGemEquipmentType()
    {
        float chance = Random.value;

        float accum = 0f;
        for (int i = 0; i < abilityGemTypeChances.Length; i++)
        {
            accum += abilityGemTypeChances[i];
            if (chance < accum)
            {
                return i switch
                {
                    0 => EquipmentType.SuperAbilityGem_Tier_1,
                    1 => EquipmentType.SuperAbilityGem_Tier_2,
                    2 => EquipmentType.SuperAbilityGem_Tier_3,
                    3 => EquipmentType.SuperAbilityGem_Tier_4,
                    4 => EquipmentType.SuperAbilityGem_Tier_5,
                    5 => EquipmentType.SuperAbilityGem_Tier_6,

                    _ => EquipmentType.SuperAbilityGem_Tier_1
                };
            }
        }

        return EquipmentType.SuperAbilityGem_Tier_1;
    }

    private void SetUpWeaponChances()
    {
		weaponTypeWeights = new float[] { Pistol_MK_1, Revolver, Pistol_MK_2, Revolver_MK_2, Pistol_MK_5, Pistol_MK_6,
			AssaultRifle_MK_1, AssaultRifle_MK_2, AssaultRifle_MK_3, AssaultRifle_MK_4, AssaultRifle_MK_5, AssaultRifle_MK_6,
			Shotgun_MK_1, Shotgun_MK_2, Shotgun_MK_3, Shotgun_MK_4, Shotgun_MK_5, Shotgun_MK_6,
			RocketLauncher_MK_1, RocketLauncher_MK_2, RocketLauncher_MK_3, RocketLauncher_MK_4, RocketLauncher_MK_5, RocketLauncher_MK_6,
			GrenadeLauncher_MK_1, GrenadeLauncher_MK_2, GrenadeLauncher_MK_3, GrenadeLauncher_MK_4, GrenadeLauncher_MK_5, GrenadeLauncher_MK_6,
			Minigun_MK_1, Minigun_MK_2, Minigun_MK_3, Minigun_MK_4, Minigun_MK_5, Minigun_MK_6,
			GargingGun_MK_1, GargingGun_MK_2, GargingGun_MK_3, GargingGun_MK_4, GargingGun_MK_5, GargingGun_MK_6
		};

		overallWeaponTypesWeight = weaponTypeWeights.Sum();

		weaponTypeChances = new float[weaponTypeWeights.Length];

		for (int i = 0; i < weaponTypeWeights.Length; i++)
		{
			weaponTypeChances[i] = weaponTypeWeights[i] / overallWeaponTypesWeight;
		}
	}

	private void SetUpHelmetChances()
    {
		helmetTypeWeights = new float[] { Helmet_MK_Armor, Helmet_MK_MResist, Helmet_MK_Health, Helmet_MK_HealthArmor, Helmet_MK_HealthResist, Helmet_MK_ArmorMResist };

		overallHelmetTypesWeight = helmetTypeWeights.Sum();

		helmetTypeChances = new float[helmetTypeWeights.Length];

		for (int i = 0; i < helmetTypeWeights.Length; i++)
		{
            helmetTypeChances[i] = helmetTypeWeights[i] / overallHelmetTypesWeight;
		}
	}

    private void SetUpBodyArmorChances()
	{
        bodyArmorTypeWeights = new float[] { BodyArmor_Armor, BodyArmor_MResist, BodyArmor_Health, BodyArmor_HealthArmor, BodyArmor_HealthResist, BodyArmor_ArmorMResist };

        overallBodyArmorTypesWeight = bodyArmorTypeWeights.Sum();

        bodyArmorTypeChances = new float[bodyArmorTypeWeights.Length];

        for (int i = 0; i < bodyArmorTypeWeights.Length; i++)
        {
            bodyArmorTypeChances[i] = bodyArmorTypeWeights[i] / overallBodyArmorTypesWeight;
        }
    }
    private void SetUpGlovesChances()
	{
        glovesTypeWeights = new float[] { Gloves_MK_Armor, Gloves_MK_MResist, Gloves_MK_Health, Gloves_MK_HealthArmor, Gloves_MK_HealthResist, Gloves_MK_ArmorMResist };

        overallGlovesTypesWeight = glovesTypeWeights.Sum();

        glovesTypeChances = new float[glovesTypeWeights.Length];

        for (int i = 0; i < glovesTypeWeights.Length; i++)
        {
            glovesTypeChances[i] = glovesTypeWeights[i] / overallGlovesTypesWeight;
        }
    }
    private void SetUpBootsChances()
	{
        bootsTypeWeights = new float[] { Boots_MK_Armor, Boots_MK_MResist, Boots_MK_Health, Boots_MK_HealthArmor, Boots_MK_HealthResist, Boots_MK_ArmorMResist };

        overallBootsTypesWeight = bootsTypeWeights.Sum();

        bootsTypeChances = new float[bootsTypeWeights.Length];

        for (int i = 0; i < bootsTypeWeights.Length; i++)
        {
            bootsTypeChances[i] = bootsTypeWeights[i] / overallBootsTypesWeight;
        }
    }
    private void SetUpLeftHandChances()
	{
        leftHandTypeWeights = new float[] { Book_MK_1, Book_MK_2, Book_MK_3, Book_MK_4, Book_MK_5, Book_MK_6,
			AmmoPack_MK_1, AmmoPack_MK_2, AmmoPack_MK_3, AmmoPack_MK_4, AmmoPack_MK_5, AmmoPack_MK_6 };

        overallLeftHandTypesWeight = leftHandTypeWeights.Sum();

        leftHandTypeChances = new float[leftHandTypeWeights.Length];

        for (int i = 0; i < leftHandTypeWeights.Length; i++)
        {
            leftHandTypeChances[i] = leftHandTypeWeights[i] / overallLeftHandTypesWeight;
        }
    }

    private void SetUpAbilityGemChances()
    {
		abilityGemTypeWeights = new float[] { AbilityGem_Tier_1, AbilityGem_Tier_2, AbilityGem_Tier_3, AbilityGem_Tier_4, AbilityGem_Tier_5, AbilityGem_Tier_6 };

		overallAbilityGemTypesWeight = abilityGemTypeWeights.Sum();

		abilityGemTypeChances = new float[abilityGemTypeWeights.Length];

		for (int i = 0; i < abilityGemTypeWeights.Length; i++)
		{
			abilityGemTypeChances[i] = abilityGemTypeWeights[i] / overallAbilityGemTypesWeight;
		}
	}

    private void SetUpSuperAbilityGemChances()
    {
        superAbilityGemTypeWeights = new float[] { SuperAbilityGem_Tier_1, SuperAbilityGem_Tier_2, SuperAbilityGem_Tier_3, SuperAbilityGem_Tier_4, SuperAbilityGem_Tier_5, SuperAbilityGem_Tier_6 };

        overallSuperAbilityGemTypesWeight = superAbilityGemTypeWeights.Sum();

        superAbilityGemTypeChances = new float[superAbilityGemTypeWeights.Length];

        for (int i = 0; i < superAbilityGemTypeWeights.Length; i++)
        {
            superAbilityGemTypeChances[i] = superAbilityGemTypeWeights[i] / overallSuperAbilityGemTypesWeight;
        }
    }
}
