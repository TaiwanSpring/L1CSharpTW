using System;
namespace LineageServer.Server.Templates
{

    public class L1AuctionBoard
    {
        public L1AuctionBoard()
        {
        }

        private int _houseId;

        public virtual int HouseId
        {
            get
            {
                return _houseId;
            }
            set
            {
                _houseId = value;
            }
        }


        private string _houseName;

        public virtual string HouseName
        {
            get
            {
                return _houseName;
            }
            set
            {
                _houseName = value;
            }
        }


        private int _houseArea;

        public virtual int HouseArea
        {
            get
            {
                return _houseArea;
            }
            set
            {
                _houseArea = value;
            }
        }


        private DateTime _deadline;

        public virtual DateTime Deadline
        {
            get
            {
                return _deadline;
            }
            set
            {
                _deadline = value;
            }
        }


        private int _price;

        public virtual int Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }


        private string _location;

        public virtual string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }


        private string _oldOwner;

        public virtual string OldOwner
        {
            get
            {
                return _oldOwner;
            }
            set
            {
                _oldOwner = value;
            }
        }


        private int _oldOwnerId;

        public virtual int OldOwnerId
        {
            get
            {
                return _oldOwnerId;
            }
            set
            {
                _oldOwnerId = value;
            }
        }


        private string _bidder;

        public virtual string Bidder
        {
            get
            {
                return _bidder;
            }
            set
            {
                _bidder = value;
            }
        }


        private int _bidderId;

        public virtual int BidderId
        {
            get
            {
                return _bidderId;
            }
            set
            {
                _bidderId = value;
            }
        }


    }
}