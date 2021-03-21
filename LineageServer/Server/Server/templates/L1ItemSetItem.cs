/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.Templates
{
	public class L1ItemSetItem
	{
		private readonly int id;
		private readonly int amount;
		private readonly int enchant;

		public L1ItemSetItem(int id, int amount, int enchant) : base()
		{
			this.id = id;
			this.amount = amount;
			this.enchant = enchant;
		}

		public virtual int Id
		{
			get
			{
				return id;
			}
		}

		public virtual int Amount
		{
			get
			{
				return amount;
			}
		}

		public virtual int Enchant
		{
			get
			{
				return enchant;
			}
		}
	}
}