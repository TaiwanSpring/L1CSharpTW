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
namespace LineageServer.Server.Server.Model
{
	public class L1TaxCalculator
	{
		/// <summary>
		/// 戦争税は15%固定
		/// </summary>
		private const int WAR_TAX_RATES = 15;

		/// <summary>
		/// 国税は10%固定（地域税に対する割合）
		/// </summary>
		private const int NATIONAL_TAX_RATES = 10;

		/// <summary>
		/// ディアド税は10%固定（戦争税に対する割合）
		/// </summary>
		private const int DIAD_TAX_RATES = 10;

		private readonly int _taxRatesCastle;
		private readonly int _taxRatesTown;
		private readonly int _taxRatesWar = WAR_TAX_RATES;

		/// <param name="merchantNpcId">
		///            計算対象商店のNPCID </param>
		public L1TaxCalculator(int merchantNpcId)
		{
			_taxRatesCastle = L1CastleLocation.getCastleTaxRateByNpcId(merchantNpcId);
			_taxRatesTown = L1TownLocation.getTownTaxRateByNpcid(merchantNpcId);
		}

		public virtual int calcTotalTaxPrice(int price)
		{
			int taxCastle = price * _taxRatesCastle;
			int taxTown = price * _taxRatesTown;
			int taxWar = price * WAR_TAX_RATES;
			return (taxCastle + taxTown + taxWar) / 100;
		}

		// XXX 個別に計算する為、丸め誤差が出る。
		public virtual int calcCastleTaxPrice(int price)
		{
			return (price * _taxRatesCastle) / 100 - calcNationalTaxPrice(price);
		}

		public virtual int calcNationalTaxPrice(int price)
		{
			return (price * _taxRatesCastle) / 100 / (100 / NATIONAL_TAX_RATES);
		}

		public virtual int calcTownTaxPrice(int price)
		{
			return (price * _taxRatesTown) / 100;
		}

		public virtual int calcWarTaxPrice(int price)
		{
			return (price * _taxRatesWar) / 100;
		}

		public virtual int calcDiadTaxPrice(int price)
		{
			return (price * _taxRatesWar) / 100 / (100 / DIAD_TAX_RATES);
		}

		/// <summary>
		/// 課税後の価格を求める。
		/// </summary>
		/// <param name="price">
		///            課税前の価格 </param>
		/// <returns> 課税後の価格 </returns>
		public virtual int layTax(int price)
		{
			return price + calcTotalTaxPrice(price);
		}
	}

}