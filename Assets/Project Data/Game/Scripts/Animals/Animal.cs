using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class Animal
    {
        [SerializeField] Type animalType;
        public Type AnimalType => animalType;

        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;

        [SerializeField] float carryingHeight;
        public float CarryingHeight => carryingHeight;

        [Space]
        [SerializeField] CurrencyType rewardType;
        public CurrencyType RewardType => rewardType;

        [SerializeField] int rewardAmount;
        public int RewardAmount => rewardAmount;

        private Pool pool;
        public Pool Pool => pool;

        public void Initialise()
        {
            pool = new Pool(new PoolSettings(prefab.name, prefab, 0, true));
        }

        public enum Type
        {
            
            Rabbit_01 = 0,
            Chicken_01 = 1,
            Chicken_02 = 2,
            Chicken_03 = 3,
            Chicken_04 = 4,
            Chicken_05 = 5,
            Cat_01 = 6,
            Cat_02 = 7,
            Cat_03 = 8,
            Cat_04 = 9,
            Cat_05 = 10,
            Cow_01 = 11,
            Cow_02 = 12,
            Cow_03 = 13,
            Cow_04 = 14,
            Cow_05 = 15,
            Dog_01 = 16,
            Dog_02 = 17,
            Dog_03 = 18,
            Dog_04 = 19,
            Dog_05 = 20,
            Duck_01 = 21,
            Duck_02 = 22,
            Duck_03 = 23,
            Duck_04 = 24,
            Duck_05 = 25,
            Goat_01 = 26,
            Goat_02 = 27,
            Goat_03 = 28,
            Goat_04 = 29,
            Goat_05 = 30,
            Pig_01 = 31,
            Pig_02 = 32,
            Pig_03 = 33,
            Pig_04 = 34,
            Pig_05 = 35,
            Pony_01 =36,
            Pony_02 =37,
            Pony_03 =38,
            Pony_04 =39,
            Pony_05 =   40,
            Puppe_01 = 41,
            Puppe_02 = 42,
            Puppe_03 = 43,
            Puppe_04 = 44,
            Puppe_05 = 45,
            Sheep_01 = 46,
            Sheep_02 = 47,
            Sheep_03 = 48,
            Sheep_04 = 49,
            Sheep_05 = 50,
            Penguin_01 = 51,
            Qutecat_02 = 52,
            sheepoter_03 = 53,

        }
    }
}