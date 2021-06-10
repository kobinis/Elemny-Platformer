using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    public enum MeterType //TODO can be used to hold the sate of sum systems
    {
        /// <summary>
        /// The Damage inflicted on the agent - Damage value will be reduced from shileds then after armor reduction from hull
        /// </summary>
        Damage,        
        /// <summary>
        /// The hitpoints of the agent hitpoints on the hull 
        /// </summary>
        Hitpoints,
        /// <summary>
        /// The energy of the agent
        /// </summary>
        Energy,
        /// <summary>
        /// The shield of an agent
        /// </summary>
        Shield,
        /// <summary>
        /// The value used to set the max value of the shield meter - to prevent moments when the maxvalue is not set to the correct value
        /// </summary>
        ShieldMaxValue,
        /// <summary>
        /// The value used to set the max energy - to prevent moments when the max value is not set to the correct value
        /// </summary>
        EnergyMaxValue,
        /// <summary>
        /// The time(in frames) the agent will be on stun (zero-no stun)
        /// </summary>
        StunTime,
        /// <summary>
        /// The agent Cloak state (zero no clock) 
        /// </summary>
        Cloak,
        /// <summary>
        /// The armor of the agent - values reduce damage to hitpoints
        /// </summary>
        Armor,
        /// <summary>
        /// Used for mines to enable mining only by spesific projectiles 
        /// </summary>
        MiningLevel,
        /// <summary>
        /// Used for undeploying deployables and to mine specail asteroids
        /// </summary>
        MiningSpeed,
        /// <summary>
        /// The money of the faction/agent   Repair 
        /// </summary>
        Money,
        /// <summary>
        /// Enables you to warp to diffrent destenations in the galaxy
        /// </summary>
        WarpFuel,
        WarpCooldown,

        Lives,
        Kills,
        FactionKills,
        GlobalRepairCooldown,
        GlobalEnergyCool,
       // RepiarCooldown,

        Electricity,
        Light,
        Heat,
        ExtraMass,
     
        Fuel,
        Reputation,
        Ammo1,
        Ammo2,
        Ammo3,
        Score,
        ActivationCount,
        LifetimeTimer,
        ControlPoints,
        MaxControlPoints,
        MiningPoints,
        GoodReputation,
        BadReputation,
        ReviveCooldown,        
        AiState,
        TextState,
        InsititeSate,
        Left,
        Right,
        /// <summary>
        /// This meter can hold max weapon range to help AI
        /// </summary>
        MaxRangeAction1,
        /// <summary>
        /// This meter can hold max weapon range to help AI
        /// </summary>
        MaxRangeAction2,
        /// <summary>
        /// Damage Multiplier 
        /// </summary>
        DamageMultiplier,
        /// <summary>
        /// Used for racing
        /// </summary>
        CheckpointIndex,
         RespawnTimer, None,
        Speed,
        /// <summary>
        /// Used to randomize lighting effects
        /// </summary>
        DisplayOffset,        
        //General State
        //AI states
        //Buffs and debuffs counters
        //Projectile
        AnimationFrame, PositionRadius, PositionAngle, PositionX, PositionY, //TODO: move to projectile meters
        ShipIndex,
        /// <summary>
        /// Used to 
        /// </summary>
        NebulaGas,
        /// <summary>
        /// The index of the Character, if zero will generate a character from hash
        /// </summary>
        Character,
        /// <summary>
        /// Used to store the response Index
        /// </summary>
        CharacterRespnse,       

        ShieldLevel,
        DamageLevel,
        MaxSpeedLevel,
        HullLevel,
        ArmorLevel,

        



    }; 
    //...
    
}
