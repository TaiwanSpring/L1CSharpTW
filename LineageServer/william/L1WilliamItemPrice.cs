/*
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
 * 02111-1307, USA.
 *
 * http://www.gnu.org/copyleft/gpl.html
 */
namespace LineageServer.william
{

	public class L1WilliamItemPrice
	{

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") private static java.util.logging.Logger _log = java.util.logging.Logger.getLogger(L1WilliamItemPrice.class.getName());
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1WilliamItemPrice).FullName);

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