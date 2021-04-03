namespace LineageServer.william
{

    class L1WilliamItemPrice
    {
        private int _itemId;

        private int _price;

        public L1WilliamItemPrice(int itemId, int price)
        {
            _itemId = itemId;
            _price = price;
        }

        public virtual int ItemId
        {
            get
            {
                return _itemId;
            }
        }

        public virtual int Price
        {
            get
            {
                return _price;
            }
        }

        public static int getItemId(int itemId)
        {
            L1WilliamItemPrice item_price = ItemPrice.Instance.getTemplate(itemId);

            if (item_price == null)
            {
                return 0;
            }

            int price = item_price.Price;
            return price;
        }
    }


}