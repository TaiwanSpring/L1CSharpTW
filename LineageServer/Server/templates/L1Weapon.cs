using System;

namespace LineageServer.Server.Templates
{
    [Serializable]
    public class L1Weapon : L1Item
    {
        /// 
        private const long serialVersionUID = 1L;

        public L1Weapon()
        {
        }

        private int _range = 0; // ● 射程範囲

        public override int Range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
            }
        }


        private int _hitModifier = 0; // ● 命中率補正

        public override int HitModifier
        {
            get
            {
                return _hitModifier;
            }
            set
            {
                _hitModifier = value;
            }
        }


        private int _dmgModifier = 0; // ● ダメージ補正

        public override int DmgModifier
        {
            get
            {
                return _dmgModifier;
            }
            set
            {
                _dmgModifier = value;
            }
        }


        private int _doubleDmgChance; // ● DB、クロウの発動確率

        public override int DoubleDmgChance
        {
            get
            {
                return _doubleDmgChance;
            }
            set
            {
                _doubleDmgChance = value;
            }
        }


        private int _magicDmgModifier = 0; // ● 攻撃魔法のダメージ補正

        public override int MagicDmgModifier
        {
            get
            {
                return _magicDmgModifier;
            }
            set
            {
                _magicDmgModifier = value;
            }
        }


        private int _canbedmg = 0; // ● 損傷の有無

        public override int get_canbedmg()
        {
            return _canbedmg;
        }

        public virtual void set_canbedmg(int i)
        {
            _canbedmg = i;
        }

        public override bool TwohandedWeapon
        {
            get
            {
                int weapon_type = Type;

                bool @bool = (weapon_type == 3 || weapon_type == 4 || weapon_type == 5 || weapon_type == 11 || weapon_type == 12 || weapon_type == 15 || weapon_type == 16 || weapon_type == 18 || weapon_type == 19);

                return @bool;
            }
        }
    }

}