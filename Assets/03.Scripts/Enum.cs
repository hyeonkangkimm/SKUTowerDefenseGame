using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAddressableType
{
    PREFAB,
    DATA,
    AUDIO,
    IMAGE,
    UI
}

public enum ECurrencyType
{
    ELIXER,
    WOOD,
    STONE,
    STEEL,
    GOLD
}

public enum EStatChangeType
{
    ADD,
    GRADE,
    STARS,
    MULTIPLE,
    OVERRIDE
}

public enum ERarityType
{
    COMMON = 1,
    RARE = 2,
    EPIC = 3,
    LEGEND = 4,
}

public enum EEntityType
{
    Player,
    MONSTER
}

public enum EStatType
{
    ATK = 0,
    HEALTH = 1,
    DEFENSE = 2,
    ATKSPEED = 3,

    CRITRATE = 4,
    CRITMULTIPLIER = 5,
    SKILLMULTIPLIER = 6,
    DAMAGEMULTIPLIER = 7,
    HEALMULTIPLIER = 8
}

public enum ECombatConditionType
{
    START,
    END,
    READY
}

public enum EAudioMixerType
{
    Master,
    BGM,
    SFX
}

public enum ESkillType
{
    PROJECTILE,
    AOE, //Area of Effect
    BUFF,
    Heal
}
public enum EAlertType
{
    DENY,
    WARNING,

}

public enum ESkillMotion
{
    MOTION1,
    MOTION2,
}





