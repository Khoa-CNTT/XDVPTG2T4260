using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace tuleeeeee.Events
{
    /// <summary>
    ///     An event representing a fire input 
    /// </summary>
    public class AttackEvent : UnityEvent<bool> { }

    /// <summary>
    ///     An event representing a look input (where the character must look) in the direction of its parameter
    /// </summary>
    public class LookEvent : UnityEvent<Vector2> { }

    /// <summary>
    ///     An event representing a scroll input (y axis)
    /// </summary>
    public class ScrollEvent : UnityEvent<Vector2> { }

    public class SelectWeaponEvent : UnityEvent<int> { }
    public class FastSwitchWeaponEvent : UnityEvent<bool> { }

}
