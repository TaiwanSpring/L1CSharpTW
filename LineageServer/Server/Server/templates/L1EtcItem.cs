using System;
namespace LineageServer.Server.Server.Templates
{
    [Serializable]
    public class L1EtcItem : L1Item
    {
        /// 
        private const long serialVersionUID = 1L;

        public L1EtcItem()
        {
        }

        private bool _stackable;

        private int _locx;

        private int _locy;

        private short _mapid;

        private int _delay_id;

        private int _delay_time;

        private int _delay_effect;

        private int _maxChargeCount;

        private bool _isCanSeal; // ● 封印スクロールで封印可能

        public override bool Stackable
        {
            get
            {
                return _stackable;
            }
        }

        public virtual void set_stackable(bool stackable)
        {
            _stackable = stackable;
        }

        public virtual void set_locx(int locx)
        {
            _locx = locx;
        }

        public override int get_locx()
        {
            return _locx;
        }

        public virtual void set_locy(int locy)
        {
            _locy = locy;
        }

        public override int get_locy()
        {
            return _locy;
        }

        public virtual void set_mapid(short mapid)
        {
            _mapid = mapid;
        }

        public override short get_mapid()
        {
            return _mapid;
        }

        public virtual void set_delayid(int delay_id)
        {
            _delay_id = delay_id;
        }

        public override int get_delayid()
        {
            return _delay_id;
        }

        public virtual void set_delaytime(int delay_time)
        {
            _delay_time = delay_time;
        }

        public override int get_delaytime()
        {
            return _delay_time;
        }

        public virtual void set_delayEffect(int delay_effect)
        {
            _delay_effect = delay_effect;
        }

        public virtual int get_delayEffect()
        {
            return _delay_effect;
        }

        public virtual int MaxChargeCount
        {
            set
            {
                _maxChargeCount = value;
            }
            get
            {
                return _maxChargeCount;
            }
        }


        public override bool CanSeal
        {
            get
            {
                return _isCanSeal;
            }
            set
            {
                _isCanSeal = value;
            }
        }
    }
}