using System;

namespace LineageServer.Server.Server.Templates
{
    [Serializable]
    public class L1Armor : L1Item
    {
        /// 
        private const long serialVersionUID = 1L;

        public L1Armor()
        {
        }

        private int _ac = 0; // ● ＡＣ

        public override int get_ac()
        {
            return _ac;
        }

        public virtual void set_ac(int i)
        {
            this._ac = i;
        }

        private int _damageReduction = 0; // ● ダメージ軽減

        public override int DamageReduction
        {
            get
            {
                return _damageReduction;
            }
            set
            {
                _damageReduction = value;
            }
        }


        private int _weightReduction = 0; // ● 重量軽減

        public override int WeightReduction
        {
            get
            {
                return _weightReduction;
            }
            set
            {
                _weightReduction = value;
            }
        }


        private int _hitModifierByArmor = 0; // ● 命中率補正

        public override int HitModifierByArmor
        {
            get
            {
                return _hitModifierByArmor;
            }
            set
            {
                _hitModifierByArmor = value;
            }
        }


        private int _dmgModifierByArmor = 0; // ● ダメージ補正

        public override int DmgModifierByArmor
        {
            get
            {
                return _dmgModifierByArmor;
            }
            set
            {
                _dmgModifierByArmor = value;
            }
        }


        private int _bowHitModifierByArmor = 0; // ● 弓の命中率補正

        public override int BowHitModifierByArmor
        {
            get
            {
                return _bowHitModifierByArmor;
            }
            set
            {
                _bowHitModifierByArmor = value;
            }
        }


        private int _bowDmgModifierByArmor = 0; // ● 弓のダメージ補正

        public override int BowDmgModifierByArmor
        {
            get
            {
                return _bowDmgModifierByArmor;
            }
            set
            {
                _bowDmgModifierByArmor = value;
            }
        }


        private int _defense_water = 0; // ● 水の属性防御

        public virtual void set_defense_water(int i)
        {
            _defense_water = i;
        }

        public override int get_defense_water()
        {
            return this._defense_water;
        }

        private int _defense_wind = 0; // ● 風の属性防御

        public virtual void set_defense_wind(int i)
        {
            _defense_wind = i;
        }

        public override int get_defense_wind()
        {
            return this._defense_wind;
        }

        private int _defense_fire = 0; // ● 火の属性防御

        public virtual void set_defense_fire(int i)
        {
            _defense_fire = i;
        }

        public override int get_defense_fire()
        {
            return this._defense_fire;
        }

        private int _defense_earth = 0; // ● 土の属性防御

        public virtual void set_defense_earth(int i)
        {
            _defense_earth = i;
        }

        public override int get_defense_earth()
        {
            return this._defense_earth;
        }

        private int _regist_stun = 0; // ● スタン耐性

        public virtual void set_regist_stun(int i)
        {
            _regist_stun = i;
        }

        public override int get_regist_stun()
        {
            return this._regist_stun;
        }

        private int _regist_stone = 0; // ● 石化耐性

        public virtual void set_regist_stone(int i)
        {
            _regist_stone = i;
        }

        public override int get_regist_stone()
        {
            return this._regist_stone;
        }

        private int _regist_sleep = 0; // ● 睡眠耐性

        public virtual void set_regist_sleep(int i)
        {
            _regist_sleep = i;
        }

        public override int get_regist_sleep()
        {
            return this._regist_sleep;
        }

        private int _regist_freeze = 0; // ● 凍結耐性

        public virtual void set_regist_freeze(int i)
        {
            _regist_freeze = i;
        }

        public override int get_regist_freeze()
        {
            return this._regist_freeze;
        }

        private int _regist_sustain = 0; // ● ホールド耐性

        public virtual void set_regist_sustain(int i)
        {
            _regist_sustain = i;
        }

        public override int get_regist_sustain()
        {
            return this._regist_sustain;
        }

        private int _regist_blind = 0; // ● 暗闇耐性

        public virtual void set_regist_blind(int i)
        {
            _regist_blind = i;
        }

        public override int get_regist_blind()
        {
            return this._regist_blind;
        }

        private int _grade = 0; // 飾品級別

        public virtual int Grade
        {
            set
            {
                _grade = value;
            }
            get
            {
                return this._grade;
            }
        }


    }
}