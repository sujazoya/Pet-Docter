using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Item", menuName = "Content/Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] Type itemType;
        public Type ItemType => itemType;

        [Space]
        [SerializeField] Sprite icon;
        public Sprite Icon => icon;

        [SerializeField] GameObject model;
        public GameObject Model => model;

        [SerializeField] float modelHeight;
        public float ModelHeight => modelHeight;

        private Pool pool;
        public Pool Pool => pool;

        public void Initialise()
        {
            pool = new Pool(new PoolSettings(model.name, model, 0, true));
        }

        public enum Type
        {
            None = -1,
            Soap = 0,
            Injection = 1,
            Pill = 2,
            Bandage = 3,
            Disinfictent = 4,
            Enima = 5,
            Pach = 6,
            Pill_2 = 7,
            strech_cream = 8,
            liver_Pill = 9,
            Sunscreen = 10,
            Syrup = 11,
            Mixture = 12,
        }
    }
}