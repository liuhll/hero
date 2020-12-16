namespace Surging.Hero.BasicData.IApplication
{
    public static class CacheKeyConstant
    {
        public const string GetWordBookById = "GetWordBookBy_Id_{0}";

        public const string GetWordBookByCode = "GetWordBookBy_Code_{0}";

        public const string RemoveGetWordBook = "GetWordBookBy_*";

        public const string GetWordBookItemsById = "GetWordBookItemsBy_Id_{0}";

        public const string GetWordBookItemsByCode = "GetWordBookItemsBy_Code_{0}";

        public const string RemoveGetWordBookItems = "GetWordBookItemBy_*";

        public const string GetWordBookItemById = "GetWordBookItemBy_Id_{0}";

        public const string GetWordBookItemByCode = "GetWordBookItemBy_Code_{0}";

        public const string RemoveGetWordBookItem = "GetWordBookItemBy_*";

        public const string GetWordBookItemByKey = "GetWordBookItemByKey_{0}_{1}";
    }
}