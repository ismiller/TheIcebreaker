using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {

    public enum TriggerEventType {
        Enter, Stay, Exit
    }

    public delegate void MediatorCallback<T>(T _c);

    public abstract class Message { /* */ }

        public abstract class M_Trigger : Message {
            public TriggerEventType eventType;
            protected M_Trigger(TriggerEventType _type) => eventType = _type;
        }

            public class M_TriggerInteract : M_Trigger {
                public RemoteControlActor actor;
                public M_TriggerInteract(RemoteControlActor _actor, TriggerEventType _type) : base (_type) {
                    actor = _actor;
                }
            }  

            public class M_TriggerSelection : M_Trigger {
                public SelectionItemsActor actor;
                public M_TriggerSelection(SelectionItemsActor _actor, TriggerEventType _type) : base (_type) {
                    actor = _actor;
                }
            }  

            public class M_TriggerZone : M_Trigger {
                public ImpactZoneActor actor;
                public M_TriggerZone(ImpactZoneActor _actor, TriggerEventType _type) : base (_type) {
                    actor = _actor;
                }
            }

    public abstract class M_Raycast : Message { /* */ }

        public abstract class M_Surface : M_Raycast { /* */ }

            public class M_DefoltSurface : M_Surface {
                public DefoltSurfaceActor actor;
                public M_DefoltSurface(DefoltSurfaceActor _actor) => actor = _actor;
            }

            public class M_SlopeSurface : M_Surface {
                public SlopeSurfaceActor actor;
                public M_SlopeSurface(SlopeSurfaceActor _actor) => actor = _actor;
            }

            public class M_MovePlatform : M_Surface {
                public MovePlatformActor actor;
                public M_MovePlatform(MovePlatformActor _actor) => actor = _actor;
            }

        public class M_Obstacle : M_Raycast {
            public ObstacleActor actor;
            public M_Obstacle(ObstacleActor _actor) => actor = _actor;   
        }

        public class M_Nullable : M_Raycast {
            public NullableObstacleActor actor;
            public M_Nullable(NullableObstacleActor _actor) => actor = _actor;
        } 
}
