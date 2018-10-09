﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client.MirNetwork;
using Client.MirScenes;
using S = ServerPackets;
using C = ClientPackets;

namespace Client.MirObjects
{

    public class ObserverObject : MapObject, ICamera
    {
        public string Name { get; set; }

        public bool LockedOn { get; set; }
        public ObserverObject(uint objectID) : base(objectID)
        {
            FreeMovement();
        }

        public void Load(UserObject user)
        {
            CurrentLocation = user.CurrentLocation;
            Name = user.Name;
            GameScene.Scene.MapControl.AddObject(this);
        }

        public void LockOnObject(uint objectID)
        {
            LockedOn = true;
            Network.Enqueue(new C.ObserveLock { ObjectID = objectID });

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != objectID) continue;
                GameScene.Camera = ob as ICamera;
                
                return;
            }
        }

        public void FreeMovement()
        {
            if (LockedOn)
            {
                LockedOn = false;
                Network.Enqueue(new C.ObserveLock { ObjectID = 0 });
                CurrentLocation = GameScene.Camera.CurrentLocation;
                Name = GameScene.Camera.Name;
            }
            
            GameScene.Camera = this;
        }

        public override ObjectType Race => ObjectType.Observer;

        public override bool Blocking => false;

        public override void Draw()
        {
            
        }

        public override void DrawBehindEffects(bool effectsEnabled)
        {
            
        }

        public override void DrawEffects(bool effectsEnabled)
        {
           
        }

        public override bool MouseOver(Point p)
        {
            return false;
        }

        public override void Process()
        {
            Movement = CurrentLocation;
        }
    }
}
