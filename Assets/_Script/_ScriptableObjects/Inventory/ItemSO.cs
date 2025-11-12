using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bajru.Inventory
{
    [CreateAssetMenu(menuName = "New Item")]
    public class ItemSO : ScriptableObject
    {
        [Header("Properties")]
        public ItemType itemType;
        public Sprite itemSprite;
    }

    public enum ItemType { RevealCard, CurseCard, BlessingCard, GambleCard, StallCard}
}
