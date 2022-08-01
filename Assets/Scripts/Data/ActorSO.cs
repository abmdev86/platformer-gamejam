using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sluggagames.jumper.Data
{
    [CreateAssetMenu(menuName = "Actor/New Actor")]
    public class ActorSO : ScriptableObject
    {
        public string ActorName;
        public float ActorHealth;
        public GameObject ActorModel;
        public int AttackPower;
        public int Defense;
        public float ActorSpeed = 5.0f;

    }
}
