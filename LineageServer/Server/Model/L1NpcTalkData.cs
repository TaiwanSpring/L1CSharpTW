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
namespace LineageServer.Server.Model
{
	public class L1NpcTalkData
	{

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		internal int ID_Conflict;

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods of the current type:
		internal int NpcID_Conflict;

		internal string normalAction;

		internal string caoticAction;

		internal string teleportURL;

		internal string teleportURLA;

		/// <returns> Returns the normalAction. </returns>
		public virtual string NormalAction
		{
			get
			{
				return normalAction;
			}
			set
			{
				this.normalAction = value;
			}
		}


		/// <returns> Returns the caoticAction. </returns>
		public virtual string CaoticAction
		{
			get
			{
				return caoticAction;
			}
			set
			{
				this.caoticAction = value;
			}
		}


		/// <returns> Returns the teleportURL. </returns>
		public virtual string TeleportURL
		{
			get
			{
				return teleportURL;
			}
			set
			{
				this.teleportURL = value;
			}
		}


		/// <returns> Returns the teleportURLA. </returns>
		public virtual string TeleportURLA
		{
			get
			{
				return teleportURLA;
			}
			set
			{
				this.teleportURLA = value;
			}
		}


		/// <returns> Returns the iD. </returns>
		public virtual int ID
		{
			get
			{
				return ID_Conflict;
			}
			set
			{
				ID_Conflict = value;
			}
		}


		/// <returns> Returns the npcID. </returns>
		public virtual int NpcID
		{
			get
			{
				return NpcID_Conflict;
			}
			set
			{
				NpcID_Conflict = value;
			}
		}


	}

}