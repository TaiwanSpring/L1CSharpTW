/// <summary>
/// License THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). THE WORK IS PROTECTED
/// BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS
/// AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND AGREE TO
/// BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE MAY BE
/// CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Templates
{

	public class L1Drop
	{
		internal int _mobId;

		internal int _itemId;

		internal int _min;

		internal int _max;

		internal int _chance;

		internal int _enchantlvl;

		public L1Drop(int mobId, int itemId, int min, int max, int chance, int enchantlvl)
		{
			_mobId = mobId;
			_itemId = itemId;
			_min = min;
			_max = max;
			_chance = chance;
			_enchantlvl = enchantlvl;
		}

		public virtual int Chance
		{
			get
			{
				return _chance;
			}
		}

		public virtual int Itemid
		{
			get
			{
				return _itemId;
			}
		}

		public virtual int Max
		{
			get
			{
				return _max;
			}
		}

		public virtual int Min
		{
			get
			{
				return _min;
			}
		}

		public virtual int Mobid
		{
			get
			{
				return _mobId;
			}
		}

		public virtual int Enchantlvl
		{
			get
			{
				return _enchantlvl;
			}
		}
	}

}