﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    public delegate GameObject EmitHandler(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0);

    class EventCallerEmitter : IEmitter
    {
        public string ID { get; set; }
        public EmitHandler OnEmit;

        public EventCallerEmitter(EmitHandler emitHandler)
        {
            OnEmit = emitHandler;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            return OnEmit.Invoke(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
        }
    }
}
